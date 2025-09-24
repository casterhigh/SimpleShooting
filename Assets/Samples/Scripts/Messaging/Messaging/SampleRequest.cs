using Infrastructure.Messaging;

namespace Sample.Messaging.Messaging
{
    public record SampleRequest : IMessage
    {
        public long MonsterId { get; }

        public SampleRequest(long monsterId)
        {
            MonsterId = monsterId;
        }
    }
}
