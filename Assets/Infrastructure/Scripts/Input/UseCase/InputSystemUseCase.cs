using MessagePipe;
using R3;
using Infrastructure.Input.Messaging.Request;
using Infrastructure.Input.Messaging.Response;
using Infrastructure.Input.Model.Interface;
using Infrastructure.Messaging;
using System;

namespace Infrastructure.Input.UseCase
{
    public class InputSystemUseCase : IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly IInputModel model;

        readonly CompositeDisposable disposable;

        public InputSystemUseCase(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        IInputModel model)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.model = model;
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
                if (msg is OnCanceledRequest request)
                {
                    model.CreateDTO(request.Context, request.PlayerActions, request.UIActions, request.MapKind, Enums.ActionKind.OnCanceled);
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is OnPerformedRequest request)
                {
                    model.CreateDTO(request.Context, request.PlayerActions, request.UIActions, request.MapKind, Enums.ActionKind.OnPerformed);
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is OnStartedRequest request)
                {
                    model.CreateDTO(request.Context, request.PlayerActions, request.UIActions, request.MapKind, Enums.ActionKind.OnStarted);
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is ChangeMapKindRequest request)
                {
                    publisher.Publish(new ChangeMapKindResponse(request.MapKind));
                }
            }).AddTo(disposable);
        }
    }
}
