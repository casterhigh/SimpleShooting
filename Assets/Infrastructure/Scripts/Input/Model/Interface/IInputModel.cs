using UnityEngine.InputSystem;
using Infrastructure.Input.Generated.Enum;
using Infrastructure.Enums;

namespace Infrastructure.Input.Model.Interface
{
    public interface IInputModel
    {
        void CreateDTO(InputAction.CallbackContext context, PlayerActions? playerActions, UIActions? uIActions, MapKind mapKind, ActionKind actionKind);
    }
}
