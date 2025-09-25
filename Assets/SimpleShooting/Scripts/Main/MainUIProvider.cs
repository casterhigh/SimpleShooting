using Infrastructure.Messaging;
using Infrastructure.View.UI.Messaging;
using MessagePipe;
using R3;
using SimpleShooting.Messaging.Response;
using SimpleShooting.Presentation.Main;
using System;
using UnityEngine;
using VContainer.Unity;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

namespace SimpleShooting.Main
{
    public class MainUIProvider : IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly CompositeDisposable disposable;

        public MainUIProvider(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            disposable = new();

            Subscribe();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is PauseGameResponse)
                {
                    publisher.Publish(new OpenPage(nameof(GamePauseView)));
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is ResumeGameResponse)
                {
                    publisher.Publish(new CloseAllPage());
                }
            }).AddTo(disposable);
        }
    }
}
