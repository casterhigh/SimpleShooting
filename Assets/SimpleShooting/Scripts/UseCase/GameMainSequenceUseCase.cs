using Infrastructure.Messaging;
using MessagePipe;
using R3;
using SimpleShooting.Messaging.Request;
using SimpleShooting.Messaging.Response;
using System;

namespace SimpleShooting.UseCase
{
    public class GameMainSequenceUseCase : IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly CompositeDisposable disposable;

        public GameMainSequenceUseCase(IPublisher<IMessage> publisher,
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
                if (msg is StartGameRequest)
                {
                    publisher.Publish(new StartGameResponse());
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is PauseGameRequest)
                {
                    publisher.Publish(new PauseGameResponse());
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is ResumeGameRequest)
                {
                    publisher.Publish(new ResumeGameResponse());
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is FinishGameRequest request)
                {
                    publisher.Publish(new FinishGameResponse(request.FinishKind));
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is RentBulletRequest request)
                {
                    publisher.Publish(new RentBulletResponse(request.Id, request.IsEnemy, request.Position, request.Direction));
                }
            }).AddTo(disposable);
        }
    }
}
