using Infrastructure.View.UI.Management;
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
            builder.Register<MainUIProvider>(Lifetime.Transient);

            builder.Register<PageManager>(Lifetime.Transient);

            builder.Register<EnemyModel>(Lifetime.Transient).AsImplementedInterfaces();
            builder.Register<PlayerModel>(Lifetime.Transient).AsImplementedInterfaces();

            builder.Register<EnemyUseCase>(Lifetime.Transient);
            builder.Register<PlayerUseCase>(Lifetime.Transient);
            builder.RegisterComponentInHierarchy<MainCameraComponent>();

            builder.RegisterBuildCallback(resolver =>
            {
                resolver.Resolve<MainUIProvider>();
                resolver.Resolve<PageManager>();
                resolver.Resolve<EnemyUseCase>();
                resolver.Resolve<PlayerUseCase>();
            });

            builder.RegisterEntryPoint<MainEntryPoint>();
        }
    }
}
