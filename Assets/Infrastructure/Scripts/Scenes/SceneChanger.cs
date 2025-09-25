using Cysharp.Threading.Tasks;
using Infrastructure.Interface.View.UI;
using Infrastructure.Messaging;
using Infrastructure.Presentation.Interface;
using Infrastructure.Scenes.Interface;
using Infrastructure.View.UI.Loading.Messaging.Request;
using Infrastructure.View.UI.Messaging;
using MessagePipe;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Scenes
{
    /// <summary>
    /// todo:PageManagerのようにインターフェース参照させずにMessageを購読してから処理を走るようにする
    /// </summary>
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

            var roots = scene.GetRootGameObjects();
            var pages = roots
            .SelectMany(go => go.GetComponentsInChildren<IPageView>(true))
            .Where(p => !string.IsNullOrEmpty(p.PageName))
            .GroupBy(p => p.PageName)
            .ToDictionary(g => g.Key, g => g.First());

            // todo:ここはマスターと同じようにMessageを飛ばしてLoadFinishのレスポンスを待つようにしたい
            var loaderTasks = roots
            .SelectMany(root => root.GetComponentsInChildren<IResourceLoader>(true))
            .Select(loader => loader.Load())
            .ToList();

            await UniTask.WhenAll(loaderTasks);

            if (!string.IsNullOrEmpty(firstPageName) && !pages.ContainsKey(firstPageName))
            {
                throw new InvalidOperationException($"{firstPageName}は存在していません");
            }

            // todo:ここはマスターと同じようにMessageを飛ばしてInitializeFinishのレスポンスを待つようにしたい
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
