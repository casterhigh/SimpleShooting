using Cysharp.Threading.Tasks;
using Infrastructure.Messaging;
using MessagePipe;
using SimpleShooting.Messaging.Request;
using SimpleShooting.Messaging.Response;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

namespace SimpleShooting.Presentation.Main
{
    public class EnemySpawner : MonoBehaviour
    {
        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        MainCameraComponent mainCamera;

        CancellationToken token;

        [Inject]
        public void Construct(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        MainCameraComponent mainCamera)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.mainCamera = mainCamera;
            token = this.GetCancellationTokenOnDestroy();

            Subscribe();
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is StartGameResponse)
                {
                    SpawnEnemy().Forget();
                }
            }).AddTo(token);
        }

        async UniTask SpawnEnemy()
        {
            await UniTask.CompletedTask;
            publisher.Publish(new SpawnEnemyRequest()); // ここで出現するVector3の座標を渡してあげる
        }
    }
}
