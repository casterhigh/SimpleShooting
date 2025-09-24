using Infrastructure.DB;
using Infrastructure.Messaging;
using VContainer;
using MessagePipe;
using Infrastructure.Scenes;
using Infrastructure.View.UI.Loading;
using VContainer.Unity;
using Infrastructure.Input;
using Infrastructure.UseCase.Sound;
using Infrastructure.Input.UseCase;
using Infrastructure.UseCase.View;
using Infrastructure.Input.Model;
using Infrastructure.View.Sound;
using Infrastructure.Input.Domain.Repository;
using Infrastructure.Sound.Domain.Repository;
using Infrastructure.Initialization;

namespace Infrastructure
{
    public static class DIExtension
    {
        public static void Configure(this IContainerBuilder builder, string dbPath)
        {
            builder.Register<LiteDBConnecter>(Lifetime.Singleton).WithParameter(dbPath).AsImplementedInterfaces();
            builder.Configure();
        }

        static void Configure(this IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<IMessage>(options);

            builder.Register<SceneChanger>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AudioRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputRepository>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<LoadingUseCase>(Lifetime.Singleton);
            builder.Register<AudioUseCase>(Lifetime.Singleton);
            builder.Register<InputSystemUseCase>(Lifetime.Singleton);
            builder.Register<InitializeUseCase>(Lifetime.Singleton);

            builder.Register<InputSystem>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<LoadingView>();
            builder.RegisterComponentInHierarchy<AudioManager>();

            builder.Register<InputModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<Logger>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterBuildCallback(resolver =>
            {
                resolver.Resolve<LoadingUseCase>();
                resolver.Resolve<AudioUseCase>();
                resolver.Resolve<InputSystemUseCase>();
                resolver.Resolve<InputSystem>();
                resolver.Resolve<InitializeUseCase>();
            });
        }
    }
}
