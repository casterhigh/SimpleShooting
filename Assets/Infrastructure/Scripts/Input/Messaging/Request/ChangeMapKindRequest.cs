using Infrastructure.Enums;
using Infrastructure.Messaging;

namespace Infrastructure.Input.Messaging.Request
{
    public record ChangeMapKindRequest : IMessage
    {
        public MapKind MapKind { get; }

        public ChangeMapKindRequest(MapKind mapKind)
        {
            MapKind = mapKind;
        }
    }
}
