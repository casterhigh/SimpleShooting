using Infrastructure.Domain.Interface.Repository;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Request;
using Infrastructure.Messaging.Response;
using Infrastructure.Sound.Domain.Interface;
using MessagePipe;
using R3;
using SimpleShooting.Domain.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.UseCase
{
    public class MasterDataLoadUseCase
    {
        readonly ISubscriber<IMessage> subscriber;

        readonly IPublisher<IMessage> publisher;

        readonly List<ILoadRepository> loadRepositories = new();

        readonly CompositeDisposable disposables;

        readonly ILogger logger;

        public MasterDataLoadUseCase(ISubscriber<IMessage> subscriber,
        IPublisher<IMessage> publisher,
        ILogger logger,
        IPlayerRepository playerRepository,
        IEnemyRepository enemyRepository,
        IAudioRepository audioRepository)
        {
            this.subscriber = subscriber;
            this.publisher = publisher;
            this.logger = logger;
            this.disposables = new CompositeDisposable();

            loadRepositories = new()
            {
                playerRepository,
                enemyRepository,
                audioRepository,
            };

            Subscribe();
        }

        void Subscribe()
        {
            subscriber.Subscribe(val =>
            {
                if (val is LoadMasterRequest request)
                {
                    logger.Log("LoadMasterRequest");
                    foreach (var repo in loadRepositories)
                    {
                        repo.Load();
                    }

                    publisher.Publish(new LoadMasterResponse());
                }
            }).AddTo(disposables);
        }
    }
}
