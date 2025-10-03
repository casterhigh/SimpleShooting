using Infrastructure.Messaging;
using MessagePipe;
using R3;
using SimpleShooting.Messaging.Request;
using SimpleShooting.Messaging.Response;
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

        readonly CompositeDisposable disposable;

        public PlayerUseCase(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        IPlayerModel model)
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
                if (msg is GetPlayerDTORequest)
                {
                    var player = model.GetPlayer();
                    publisher.Publish(new GetPlayerDTOResponse(player));
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is PlayerDamageRequest request)
                {
                    var player = model.ReceiveDamage(request.Enemy);
                    publisher.Publish(new PlayerDamageResponse(player));
                }
            }).AddTo(disposable);
        }
    }
}
