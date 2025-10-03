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
    public class EnemyUseCase : IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly IEnemyModel model;

        readonly CompositeDisposable disposable;

        public EnemyUseCase(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        IEnemyModel model)
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
                if (msg is GetEnemyDTORequest request)
                {
                    var enemy = model.GetEnemyDTO(request.EnemyId);
                    publisher.Publish(new GetEnemyDTOResponse(enemy));
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is SpawnEnemyRequest request)
                {
                    var enemy = model.CrateDto(request.Position, request.IsBoss);
                    publisher.Publish(new SpawnEnemyResponse(enemy));
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is RentEnemyRequest request)
                {
                    publisher.Publish(new RentEnemyResponse(request.Enemy));
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is EnemyDamageRequest request)
                {
                    var enemy = model.ReceiveDamage(request.EnemyId, request.Player);
                    publisher.Publish(new EnemyDamageResponse(enemy));
                }
            }).AddTo(disposable);
        }
    }
}
