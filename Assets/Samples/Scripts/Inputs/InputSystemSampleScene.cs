using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using R3;
using UnityEngine.SceneManagement;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;
using Infrastructure.Messaging;
using Infrastructure.Interface.View.UI;
using Infrastructure.View.UI.Messaging;
using Infrastructure.Messaging.Sound.response;
using Infrastructure.Messaging.Sound.Request;
using Infrastructure.Input.Messaging.Response;
using Infrastructure.Enums;
using Infrastructure.Input.Generated.Enum;
using Infrastructure.Input.Messaging.Request;
using Generated.Enums;
using Infrastructure.Initialization;
using Infrastructure.Messaging.Request;
using Infrastructure.Messaging.Response;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Inputs
{
    public class InputSystemSampleScene : MonoBehaviour
    {
        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        ILogger logger;

        IObjectResolver resolver;

        CompositeDisposable disposable = new();

        Dictionary<string, IPageView> pages;

        [Inject]
        public void Construct(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        IObjectResolver resolver,
        ILogger logger)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.resolver = resolver;
            this.logger = logger;

            Subscribe();
        }

        void Awake()
        {
            WaitForResponses().Forget();
            publisher.Publish(new LoadMasterRequest());
            publisher.Publish(new LoadUserDataRequest());
            Application.targetFrameRate = 60;

            var scene = SceneManager.GetActiveScene();

            foreach (var obj in scene.GetRootGameObjects().Where(go => go.TryGetComponent<IPageView>(out var _)))
            {
                resolver.InjectGameObject(obj);
            }

            pages = scene.GetRootGameObjects()
            .SelectMany(go => go.GetComponentsInChildren<IPageView>(true))
            .Where(p => !string.IsNullOrEmpty(p.PageName))
            .GroupBy(p => p.PageName)
            .ToDictionary(g => g.Key, g => g.First());

            foreach (var kvp in pages)
            {
                var page = kvp.Value;
                page.Initialize();
            }
        }

        void Start()
        {
            publisher.Publish(new SendPages(pages, ""));
        }

        void OnDestroy()
        {
            disposable.Dispose();
        }

        async UniTask WaitForResponses()
        {
            var masterTcs = new UniTaskCompletionSource<LoadMasterResponse>();
            var audioTcs = new UniTaskCompletionSource<AudioLoadFinishedResponse>();

            subscriber.Subscribe(val =>
            {
                if (val is LoadMasterResponse res)
                    masterTcs.TrySetResult(res);
            }).AddTo(disposable);

            await UniTask.WhenAll(masterTcs.Task);

            logger.Log("✅ Master と UserData の両方を受信しました！");

            subscriber.Subscribe(val =>
            {
                if (val is AudioLoadFinishedResponse res)
                    audioTcs.TrySetResult(res);
            }).AddTo(disposable);

            publisher.Publish(new LoadBgmRequest());
            publisher.Publish(new LoadSeRequest());

            await UniTask.WhenAll(audioTcs.Task);

            publisher.Publish(new InitializeFinishedRequest());
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is OnCanceledResponse response)
                {
                    if (response.DTO.MapKind == MapKind.Player
                    && response.DTO.PlayerActions.Value == PlayerActions.UI)
                    {
                        ShowUI();
                    }
                }
            }).AddTo(disposable);
        }

        void ShowUI()
        {
            publisher.Publish(new ChangeMapKindRequest(MapKind.UI));
            publisher.Publish(new OpenPage(nameof(InputSystemSamplePageView)));
            publisher.Publish(new StartSeRequest(Se.Submit));
        }
    }
}
