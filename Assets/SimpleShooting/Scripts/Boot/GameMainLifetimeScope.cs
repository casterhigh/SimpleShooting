using Infrastructure;
using SimpleShooting.Domain.Repository;
using SimpleShooting.UseCase;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SimpleShooting.Boot
{
    public class GameMainLifetimeScope : LifetimeScope
    {
        [SerializeField]
        string dbName = "Main.db";

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        protected override void Configure(IContainerBuilder builder)
        {
            var dbPath = string.Format("{0}/DB/{1}", Application.streamingAssetsPath, dbName);
            builder.Configure(dbPath);

            builder.Register<EnemyRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerRepository>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<MasterDataLoadUseCase>(Lifetime.Singleton);
            builder.Register<EnemyUseCase>(Lifetime.Singleton);
            builder.Register<PlayerUseCase>(Lifetime.Singleton);

            builder.RegisterBuildCallback(resolver =>
            {
                resolver.Resolve<MasterDataLoadUseCase>();
                resolver.Resolve<EnemyUseCase>();
                resolver.Resolve<PlayerUseCase>();
            });

            builder.RegisterEntryPoint<BootEntryPoint>();
        }
    }
}
