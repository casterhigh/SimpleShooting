using Infrastructure.Domain;

namespace Infrastructure.Messaging.Sound.Request
{
    public record StartBgmRequest : IMessage
    {
        public ID Id { get; }

        public StartBgmRequest(ID id)
        {
            Id = id;
        }
    }
}
