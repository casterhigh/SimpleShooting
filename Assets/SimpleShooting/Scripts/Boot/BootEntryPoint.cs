using Cysharp.Threading.Tasks;
using Generated;
using Infrastructure.Initialization;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Request;
using Infrastructure.Messaging.Response;
using Infrastructure.Messaging.Sound.Request;
using Infrastructure.Messaging.Sound.response;
using Infrastructure.Scenes.Interface;
using Infrastructure.View.Logger.Interface;
using Infrastructure.View.Sound;
using MessagePipe;
using R3;
using SimpleShooting.Messaging.Request;
using SimpleShooting.Messaging.Response;
using SimpleShooting.Presentation.Title;
using SimpleShooting.UseCase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

namespace SimpleShooting.Boot
{
    public class BootEntryPoint : IInitializable, IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly ILogger logger;

        readonly ISceneChanger sceneChanger;

        readonly AudioManager audioManager;

        readonly CompositeDisposable disposable = new();

        public BootEntryPoint(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        ILogger logger,
        ISceneChanger sceneChanger,
        AudioManager audioManager)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.logger = logger;
            this.sceneChanger = sceneChanger;
            this.audioManager = audioManager;
        }

        public void Initialize()
        {
            UnityEngine.Debug.Log("Boot Initialize");
            WaitForResponses().Forget();
            publisher.Publish(new LoadMasterRequest());
            Application.targetFrameRate = 60;
        }

        public void Dispose()
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

            subscriber.Subscribe(val =>
            {
                if (val is AudioLoadFinishedResponse res)
                    audioTcs.TrySetResult(res);
            }).AddTo(disposable);

            publisher.Publish(new LoadBgmRequest());
            publisher.Publish(new LoadSeRequest());

            await UniTask.WhenAll(audioTcs.Task);
            publisher.Publish(new InitializeFinishedRequest());

            await ChangeScene();
        }

        async UniTask ChangeScene()
        {
            await sceneChanger.LoadSceneWithUI(ScenePaths.Title, nameof(TitleView));
        }
    }
}
