using MessagePipe;
using UnityEngine.InputSystem;
using Infrastructure.Input.Domain.DTO;
using Infrastructure.Input.Domain.Interface;
using Infrastructure.Input.Generated.Enum;
using Infrastructure.Input.Messaging.Response;
using Infrastructure.Input.Model.Interface;
using Infrastructure.Messaging;
using Infrastructure.Enums;
using Infrastructure.View.Logger.Interface;
using UnityEngine;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;
using System;

namespace Infrastructure.Input.Model
{
    public class InputModel : IInputModel
    {
        readonly IPublisher<IMessage> publisher;

        readonly IInputRepository inputRepository; // キーコンフィグとかで使う予定

        readonly ILogger logger;

        public InputModel(IPublisher<IMessage> publisher,
        IInputRepository inputRepository,
        ILogger logger)
        {
            this.publisher = publisher;
            this.inputRepository = inputRepository;
            this.logger = logger;
        }

        public void CreateDTO(InputAction.CallbackContext context, PlayerActions? playerActions, UIActions? uIActions, MapKind mapKind, ActionKind actionKind)
        {
            var dto = null as InputDTO;
            var action = "";
            if (mapKind == MapKind.Player)
            {
                dto = CreatePlayerInputActionDTO(context, playerActions.Value);
                action = playerActions.Value.ToString();
            }
            else if (mapKind == MapKind.UI)
            {
                dto = CreateUIInputActionDTO(context, uIActions.Value);
                action = uIActions.Value.ToString();
            }

            if (dto == null)
            {
                throw new InvalidOperationException($"InputDTOがnullです");
            }

            switch (actionKind)
            {
                case ActionKind.OnCanceled:
                    publisher.Publish(new OnCanceledResponse(dto));
                    break;
                case ActionKind.OnPerformed:
                    publisher.Publish(new OnPerformedResponse(dto));
                    break;
                case ActionKind.OnStarted:
                    publisher.Publish(new OnStartedResponse(dto));
                    break;
                default:
                    throw new InvalidOperationException($"{actionKind}に対応するInputActionが存在しません");
            }

            logger.Log($"{mapKind} {actionKind} {action}");
        }

        InputDTO CreatePlayerInputActionDTO(InputAction.CallbackContext context, PlayerActions playerActions)
        {
            var vector2 = Vector2.zero;
            if (context.valueType == typeof(Vector2))
            {
                vector2 = context.ReadValue<Vector2>();
            }

            return new InputDTO(vector2, playerActions);
        }

        InputDTO CreateUIInputActionDTO(InputAction.CallbackContext context, UIActions uIActions)
        {
            var vector2 = Vector2.zero;
            if (context.valueType == typeof(Vector2))
            {
                vector2 = context.ReadValue<Vector2>();
            }

            return new InputDTO(vector2, uIActions);
        }
    }
}
