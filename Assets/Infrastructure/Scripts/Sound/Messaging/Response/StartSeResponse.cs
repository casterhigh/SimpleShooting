using Infrastructure.Domain;

namespace Infrastructure.Messaging.Sound.Response
{
    public record StartSeResponse : IMessage
    {
        public ID Id { get; }

        public StartSeResponse(ID id)
        {
            Id = id;
        }
    }
}
