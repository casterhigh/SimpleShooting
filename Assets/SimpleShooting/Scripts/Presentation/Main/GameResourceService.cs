using Cysharp.Threading.Tasks;
using Infrastructure.Messaging;
using Infrastructure.Presentation.Interface;
using Infrastructure.View.Logger.Interface;
using MessagePipe;
using R3;
using SimpleShooting.Messaging.Response;
using UnityEngine;
using VContainer;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

namespace SimpleShooting.Presentation.Main
{
    public class GameResourceService : MonoBehaviour, IResourceLoader
    {
        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        IObjectResolver resolver;

        ILogger logger;

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

        public async UniTask Load()
        {
            await UniTask.CompletedTask;
            logger.Log($"Load Resource");
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is RentEnemyResponse response)
                {
                    // EnemyDTO、ポジションを与えてObjectPoolからRentする
                }
            }).AddTo(this);

            subscriber.Subscribe(msg =>
            {
                if (msg is ReturnEnemyResponse response)
                {
                }
            }).AddTo(this);

            subscriber.Subscribe(msg =>
            {
                if (msg is RentBulletResponse response)
                {
                    // Responseのポジションを与えてObjectPoolからRentする
                }
            }).AddTo(this);

            subscriber.Subscribe(msg =>
            {
                if (msg is ReturnBulletResponse response)
                {
                }
            }).AddTo(this);
        }
    }
}
