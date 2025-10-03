using Generated;
using Infrastructure.Messaging;
using Infrastructure.View.UI.Messaging;
using MessagePipe;
using R3;
using SimpleShooting.Enums;
using SimpleShooting.Messaging.Request;
using SimpleShooting.Messaging.Response;
using SimpleShooting.Presentation.Main;
using System;
using UnityEngine;
using VContainer.Unity;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

namespace SimpleShooting.Main
{
    public class GameMainSequence : IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly CompositeDisposable disposable;

        int enemyKillCount;

        const int ClearKillCount = 100;

        public GameMainSequence(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            disposable = new();
            enemyKillCount = 0;

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

            subscriber.Subscribe(msg =>
            {
                if (msg is EnemyDamageResponse response)
                {
                    if (!response.Enemy.IsDead) return;

                    enemyKillCount++;
                    if (ClearKillCount <= enemyKillCount)
                    {
                        publisher.Publish(new FinishGameRequest(GameFinishKind.Win));
                    }
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is PlayerDamageResponse response)
                {
                    if (response.Player.IsDead)
                    {
                        publisher.Publish(new FinishGameRequest(GameFinishKind.Lose));
                    }
                }
            }).AddTo(disposable);
        }
    }
}
