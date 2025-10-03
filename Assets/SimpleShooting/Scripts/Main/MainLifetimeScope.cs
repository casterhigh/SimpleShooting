using Infrastructure.View.UI.Management;
using SimpleShooting.Boot;
using SimpleShooting.Model;
using SimpleShooting.Presentation.Main;
using SimpleShooting.UseCase;
using VContainer;
using VContainer.Unity;

namespace SimpleShooting.Main
{
    public class MainLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameMainSequence>(Lifetime.Transient);

            builder.Register<PageManager>(Lifetime.Transient);

            builder.Register<EnemyModel>(Lifetime.Transient).AsImplementedInterfaces();
            builder.Register<PlayerModel>(Lifetime.Transient).AsImplementedInterfaces();

            builder.Register<EnemyUseCase>(Lifetime.Transient);
            builder.Register<PlayerUseCase>(Lifetime.Transient);
            builder.Register<GameMainSequenceUseCase>(Lifetime.Transient);
            builder.RegisterComponentInHierarchy<MainCameraComponent>();

            builder.RegisterBuildCallback(resolver =>
            {
                resolver.Resolve<GameMainSequence>();
                resolver.Resolve<PageManager>();
                resolver.Resolve<EnemyUseCase>();
                resolver.Resolve<PlayerUseCase>();
                resolver.Resolve<GameMainSequenceUseCase>();
            });

            builder.RegisterEntryPoint<MainEntryPoint>();
        }

        protected override void Awake()
        {
            var bootScope = FindObjectOfType<GameMainLifetimeScope>();
            if (bootScope != null)
            {
                parentReference.Object = bootScope;
            }

            base.Awake();
        }
    }
}
