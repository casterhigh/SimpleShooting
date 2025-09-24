using VContainer;
using VContainer.Unity;
using UnityEngine;
using MessagePipe;
using Sample.Messaging.UseCase;
using Sample.Messaging.Model;
using Sample.Messaging.Repository;
using Infrastructure.Messaging;
using Infrastructure.DB;

public class MessagingLifetimeScope : LifetimeScope
{
    [SerializeField]
    string dbName;

    protected override void Configure(IContainerBuilder builder)
    {
        var dbPath = string.Format("{0}/DB/{1}", Application.streamingAssetsPath, dbName);

        var options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<IMessage>(options);

        builder.Register<MessagingUseCase>(Lifetime.Transient);

        builder.Register<MessagingModel>(Lifetime.Transient).AsImplementedInterfaces();

        builder.Register<MessagingRepository>(Lifetime.Singleton).AsImplementedInterfaces();

        builder.Register<LiteDBConnecter>(Lifetime.Singleton).WithParameter(dbPath).AsImplementedInterfaces();

        builder.RegisterBuildCallback(resolver =>
        {
            var useCase = resolver.Resolve<MessagingUseCase>();
        });
    }
}
