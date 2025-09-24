using Infrastructure.Messaging;
using MessagePipe;
using R3;
using System;

namespace Infrastructure.Initialization
{
    public class InitializeUseCase : IDisposable
    {
        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        CompositeDisposable disposable;

        public InitializeUseCase(IPublisher<IMessage> publisher, ISubscriber<IMessage> subscriber)
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
                if (msg is InitializeFinishedRequest)
                {
                    publisher.Publish(new InitializeFinishedResponse());
                }
            }).AddTo(disposable);
        }
    }
}
