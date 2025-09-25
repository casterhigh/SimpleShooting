using Infrastructure.Messaging;
using MessagePipe;
using SimpleShooting.Model.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.UseCase
{
    public class PlayerUseCase : IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly IPlayerModel model;

        public PlayerUseCase(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        IPlayerModel model)
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
