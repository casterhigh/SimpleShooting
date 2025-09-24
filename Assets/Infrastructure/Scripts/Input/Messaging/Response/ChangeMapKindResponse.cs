using Infrastructure.Enums;
using Infrastructure.Messaging;

namespace Infrastructure.Input.Messaging.Response
{
    public record ChangeMapKindResponse : IMessage
    {
        public MapKind MapKind { get; }

        public ChangeMapKindResponse(MapKind mapKind)
        {
            MapKind = mapKind;
        }
    }
}
