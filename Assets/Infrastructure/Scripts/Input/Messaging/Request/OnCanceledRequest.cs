using UnityEngine.InputSystem;
using Infrastructure.Input.Generated.Enum;
using Infrastructure.Messaging;
using Infrastructure.Enums;

namespace Infrastructure.Input.Messaging.Request
{
    public record OnCanceledRequest : IMessage
    {
        public InputAction.CallbackContext Context { get; }

        public PlayerActions? PlayerActions { get; }

        public UIActions? UIActions { get; }

        public Enums.MapKind MapKind { get; }

        public ActionKind ActionKind => ActionKind.OnCanceled;

        public OnCanceledRequest(InputAction.CallbackContext context, PlayerActions? playerActions, UIActions? uIActions, MapKind mapKind)
        {
            Context = context;
            PlayerActions = playerActions;
            UIActions = uIActions;
            MapKind = mapKind;
        }
    }
}
