using MessagePipe;
using R3;
using UnityEngine.InputSystem;
using Infrastructure.Input.Generated.Enum;
using Infrastructure.Input.Messaging.Request;
using Infrastructure.Input.Messaging.Response;
using Infrastructure.Messaging;
using Infrastructure.Enums;
using Infrastructure.View.Logger.Interface;
using System;
using System.Collections.Generic;

namespace Infrastructure.Input
{
    public class InputSystem : IDisposable
    {
        readonly MainInputAction inputAction;

        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly CompositeDisposable disposable;

        readonly List<Action> unsubscribeActions = new();

        readonly ILogger logger;

        MapKind currentMapKind;

        public InputSystem(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        ILogger logger)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.logger = logger;
            inputAction = new();
            inputAction.Enable();
            inputAction.UI.Disable();
            inputAction.Player.Disable();
            disposable = new();

            Subscribe();
            Initialize();
            publisher.Publish(new ChangeMapKindRequest(MapKind.Player));
        }

        public void Dispose()
        {
            foreach (var unsubscribe in unsubscribeActions)
            {
                unsubscribe?.Invoke();
            }

            unsubscribeActions.Clear();
            inputAction.Disable();
            disposable.Dispose();
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is ChangeMapKindResponse response)
                {
                    SwitchAction(response.MapKind);
                }
            }).AddTo(disposable);
        }

        void Initialize()
        {
            InitializeAction<PlayerActions>(MapKind.Player);
            InitializeAction<UIActions>(MapKind.UI);
        }

        void InitializeAction<TEnum>(MapKind mapKind)
        {
            foreach (TEnum @enum in Enum.GetValues(typeof(TEnum)))
            {
                var action = inputAction.FindAction(@enum.ToString());
                if (action == null)
                {
                    logger.Log($"{@enum}に対応するアクションがありません。");
                    continue;
                }

                Action<InputAction.CallbackContext> onCanceled = context => OnCanceled(context, @enum, mapKind);
                Action<InputAction.CallbackContext> onPerformed = context => OnPerformed(context, @enum, mapKind);
                Action<InputAction.CallbackContext> onStarted = context => OnStarted(context, @enum, mapKind);

                action.canceled += onCanceled;
                action.performed += onPerformed;
                action.started += onStarted;

                unsubscribeActions.Add(() =>
                {
                    action.canceled -= onCanceled;
                    action.performed -= onPerformed;
                    action.started -= onStarted;
                });
            }
        }

        void SwitchAction(MapKind mapKind)
        {
            currentMapKind = mapKind;
            switch (mapKind)
            {
                case MapKind.Player:
                    inputAction.UI.Disable();
                    inputAction.Player.Enable();
                    break;
                case MapKind.UI:
                    inputAction.UI.Enable();
                    inputAction.Player.Disable();
                    break;
            }
        }

        void OnCanceled<TEnum>(InputAction.CallbackContext context, TEnum @enum, MapKind mapKind)
        {
            if (mapKind != currentMapKind) return;
            var convertedEnum = ConvertEnum(@enum);
            publisher.Publish(new OnCanceledRequest(context, convertedEnum.playerActions, convertedEnum.uIActions, mapKind));
        }

        void OnPerformed<TEnum>(InputAction.CallbackContext context, TEnum @enum, MapKind mapKind)
        {
            if (mapKind != currentMapKind) return;
            var convertedEnum = ConvertEnum(@enum);
            publisher.Publish(new OnPerformedRequest(context, convertedEnum.playerActions, convertedEnum.uIActions, mapKind));
        }

        void OnStarted<TEnum>(InputAction.CallbackContext context, TEnum @enum, MapKind mapKind)
        {
            if (mapKind != currentMapKind) return;
            var convertedEnum = ConvertEnum(@enum);
            publisher.Publish(new OnStartedRequest(context, convertedEnum.playerActions, convertedEnum.uIActions, mapKind));
        }

        (PlayerActions? playerActions, UIActions? uIActions) ConvertEnum<TEnum>(TEnum @enum)
        {
            if (@enum is PlayerActions playerActions)
            {
                return (playerActions, null);
            }
            else if (@enum is UIActions uIActions)
            {
                return (null, uIActions);
            }

            throw new InvalidOperationException($"{typeof(TEnum).Name}に対応するアクションが見当たりません");
        }
    }
}
