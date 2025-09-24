using UnityEngine;
using VContainer;
using VContainer.Unity;
using Infrastructure.View.UI.Management;
using Infrastructure;

namespace Sample.Inputs
{
    public class InputSystemSampleLifetimeScope : LifetimeScope
    {
        [SerializeField]
        string dbName = "Main.db";

        protected override void Configure(IContainerBuilder builder)
        {
            var dbPath = string.Format("{0}/DB/{1}", Application.streamingAssetsPath, dbName);
            builder.Configure(dbPath);

            builder.Register<PageManager>(Lifetime.Transient);

            builder.Register<InputSystemSampleUseCase>(Lifetime.Singleton);

            builder.RegisterBuildCallback(resolver =>
            {
                resolver.Resolve<InputSystemSampleUseCase>();
                resolver.Resolve<PageManager>();
            });
        }
    }
}
