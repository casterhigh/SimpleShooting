using MessagePipe;
using R3;
using Sample.Messaging.Messaging;
using Sample.Messaging.Model;
using Cysharp.Threading.Tasks;
using Infrastructure.Messaging;
using System;

namespace Sample.Messaging.UseCase
{
    public class MessagingUseCase : IDisposable
    {
        ISubscriber<IMessage> subscriber;

        IMessagingModel model;

        CompositeDisposable disposables;

        public MessagingUseCase(ISubscriber<IMessage> subscriber, IMessagingModel model)
        {
            this.model = model;
            this.subscriber = subscriber;
            disposables = new();
            SubscribeMessage();
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        void SubscribeMessage()
        {
            subscriber.Subscribe(val =>
            {
                if (val is SampleRequest msg)
                {
                    model.CreateDTO(msg.MonsterId);
                }
            }).AddTo(disposables);
        }
    }
}