using Cysharp.Threading.Tasks;
using Infrastructure.Initialization;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Request;
using Infrastructure.Messaging.Response;
using Infrastructure.Messaging.Sound.Request;
using Infrastructure.Messaging.Sound.response;
using MessagePipe;
using R3;
using System;
using UnityEngine;
using VContainer.Unity;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

namespace Sample.Audio
{
    public class AudioSampleScene : IInitializable, IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly ILogger logger;

        readonly CompositeDisposable disposable = new();

        public AudioSampleScene(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        ILogger logger)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.logger = logger;
        }

        public void Initialize()
        {
            logger.Log("AudioSampleScene Initialize");
            WaitForResponses().Forget();
            publisher.Publish(new LoadMasterRequest());
            publisher.Publish(new LoadUserDataRequest());
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
    }
}
