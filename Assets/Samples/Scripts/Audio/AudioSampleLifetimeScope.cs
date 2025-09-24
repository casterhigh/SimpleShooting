using UnityEngine;
using VContainer;
using VContainer.Unity;
using Infrastructure;

namespace Sample.Audio
{
    public class AudioSampleLifetimeScope : LifetimeScope
    {
        [SerializeField]
        string dbName = "Main.db";

        protected override void Configure(IContainerBuilder builder)
        {
            var dbPath = string.Format("{0}/DB/{1}", Application.streamingAssetsPath, dbName);

            builder.Configure(dbPath);

            builder.Register<AudioSampleUseCase>(Lifetime.Singleton);

            builder.RegisterBuildCallback(resolver =>
            {
                resolver.Resolve<AudioSampleUseCase>();
            });

            builder.RegisterEntryPoint<AudioSampleScene>();
        }
    }
}
