using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using MessagePipe;
using Infrastructure.Messaging;
using Infrastructure.Scenes.Interface;
using Infrastructure.View.UI.Loading.Messaging.Request;
using Infrastructure.Interface.View.UI;
using Infrastructure.View.UI.Messaging;
using System.Linq;
using System;

namespace Infrastructure.Scenes
{
    public class SceneChanger : ISceneChanger
    {
        IObjectResolver resolver;

        IPublisher<IMessage> publisher;

        public SceneChanger(IObjectResolver resolver, IPublisher<IMessage> publisher)
        {
            this.resolver = resolver;
            this.publisher = publisher;
        }

        public async UniTask LoadScene(string sceneName, bool useLoading = true)
        {
            if (useLoading)
            {
                publisher.Publish(new StartLoadingRequest());
            }

            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            var scene = SceneManager.GetSceneByName(sceneName);
            foreach (var obj in scene.GetRootGameObjects())
            {
                resolver.InjectGameObject(obj);
            }

            if (useLoading)
            {
                publisher.Publish(new FinishLoadingRequest());
            }
        }

        public async UniTask LoadSceneWithUI(string sceneName, string firstPageName)
        {
            publisher.Publish(new StartLoadingRequest());
            await LoadScene(sceneName, false);
            var scene = SceneManager.GetSceneByName(sceneName);
            await UniTask.WaitWhile(() => !scene.isLoaded);

            var pages = scene.GetRootGameObjects()
            .SelectMany(go => go.GetComponentsInChildren<IPageView>(true))
            .Where(p => !string.IsNullOrEmpty(p.PageName))
            .GroupBy(p => p.PageName)
            .ToDictionary(g => g.Key, g => g.First());

            if (!string.IsNullOrEmpty(firstPageName) && !pages.ContainsKey(firstPageName))
            {
                throw new InvalidOperationException($"{firstPageName}は存在していません");
            }

            foreach (var kvp in pages)
            {
                var page = kvp.Value;
                page.Initialize();
            }

            publisher.Publish(new SendPages(pages, firstPageName));
            publisher.Publish(new FinishLoadingRequest());
        }
    }
}
