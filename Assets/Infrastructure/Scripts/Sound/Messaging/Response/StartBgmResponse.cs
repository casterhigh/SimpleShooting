using Infrastructure.Domain;

namespace Infrastructure.Messaging.Sound.Response
{
    public record StartBgmResponse : IMessage
    {
        public ID Id { get; }

        public StartBgmResponse(ID id)
        {
            Id = id;
        }
    }
}
