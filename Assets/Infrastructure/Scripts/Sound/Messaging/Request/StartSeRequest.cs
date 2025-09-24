using Generated.Enums;
using Infrastructure.Domain;

namespace Infrastructure.Messaging.Sound.Request
{
    public record StartSeRequest : IMessage
    {
        public ID Id { get; }

        public StartSeRequest(Se id)
        {
            Id = new ID((long)id);
        }
    }
}
