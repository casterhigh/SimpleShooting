using UnityEngine.InputSystem;
using Infrastructure.Input.Generated.Enum;
using Infrastructure.Messaging;
using Infrastructure.Enums;

namespace Infrastructure.Input.Messaging.Request
{
    public record OnPerformedRequest : IMessage
    {
        public InputAction.CallbackContext Context { get; }

        public PlayerActions? PlayerActions { get; }

        public UIActions? UIActions { get; }

        public MapKind MapKind { get; }

        public ActionKind ActionKind => ActionKind.OnPerformed;

        public OnPerformedRequest(InputAction.CallbackContext context, PlayerActions? playerActions, UIActions? uIActions, MapKind mapKind)
        {
            Context = context;
            PlayerActions = playerActions;
            UIActions = uIActions;
            MapKind = mapKind;
        }
    }
}
