using Infrastructure.Messaging;
using Infrastructure.View.UI.Loading.Messaging.Request;
using Infrastructure.View.UI.Loading.Messaging.Response;
using MessagePipe;
using R3;
using System;

namespace Infrastructure.UseCase.View
{
    public class LoadingUseCase : IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly CompositeDisposable disposable;

        public LoadingUseCase(IPublisher<IMessage> publisher, ISubscriber<IMessage> subscriber)
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
                if (msg is StartLoadingRequest)
                {
                    publisher.Publish(new StartLoadingResponse());
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is FinishLoadingRequest)
                {
                    publisher.Publish(new FinishLoadingResponse());
                }
            }).AddTo(disposable);
        }
    }
}
