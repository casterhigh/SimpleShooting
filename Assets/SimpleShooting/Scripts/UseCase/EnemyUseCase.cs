using Infrastructure.Messaging;
using MessagePipe;
using SimpleShooting.Model.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.UseCase
{
    public class EnemyUseCase : IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly IEnemyModel model;

        public EnemyUseCase(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        IEnemyModel model)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.model = model;

            Subscribe();
        }

        public void Dispose()
        {
        }

        void Subscribe()
        {
        }
    }
}
