using UnityEngine;
using Infrastructure.Input.Generated.Enum;
using Infrastructure.Enums;

namespace Infrastructure.Input.Domain.DTO
{
    public class InputDTO
    {
        public Vector2 Vector2 { get; }

        public PlayerActions? PlayerActions { get; } = null;

        public UIActions? UIActions { get; } = null;

        public MapKind MapKind { get; }

        public InputDTO(Vector2 vector2, PlayerActions playerActions)
        {
            Vector2 = vector2;
            PlayerActions = playerActions;
            UIActions = null;
            MapKind = MapKind.Player;
        }

        public InputDTO(Vector2 vector2, UIActions uIActions)
        {
            Vector2 = vector2;
            PlayerActions = null;
            UIActions = uIActions;
            MapKind = MapKind.UI;
        }
    }
}
