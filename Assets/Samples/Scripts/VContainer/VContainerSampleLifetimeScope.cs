using VContainer;
using VContainer.Unity;
using MessagePipe;
using Sample.VContainerSample;
using Infrastructure.Messaging;
using Infrastructure.Scenes;

public class VContainerSampleLifetimeScope : LifetimeScope
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    protected override void Configure(IContainerBuilder builder)
    {
        var options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<IMessage>(options);

        builder.Register<SceneChanger>(Lifetime.Singleton);
        builder.Register<ModelSingleton>(Lifetime.Singleton);
        builder.Register<ModelTransient>(Lifetime.Transient);
        builder.RegisterEntryPoint<VContainerSampleScene1>();
        builder.Register<VContainerSampleUseCase>(Lifetime.Transient);

        builder.RegisterBuildCallback(resolver =>
        {
            resolver.Resolve<VContainerSampleUseCase>();
        });
    }
}
