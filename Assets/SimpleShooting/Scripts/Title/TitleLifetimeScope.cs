using Infrastructure.View.UI.Management;
using SimpleShooting.Boot;
using VContainer;
using VContainer.Unity;

namespace SimpleShooting.Title
{
    public class TitleLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PageManager>(Lifetime.Transient);

            builder.RegisterBuildCallback(resolver =>
            {
                resolver.Resolve<PageManager>();
            });

            builder.RegisterEntryPoint<TitleEntryPoint>();
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
