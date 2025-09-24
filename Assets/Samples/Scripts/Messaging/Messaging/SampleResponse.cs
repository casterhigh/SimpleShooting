using Infrastructure.Messaging;
using Sample.Messaging.DTO;

namespace Sample.Messaging.Messaging
{
    public record SampleResponse : IMessage
    {
        public SampleDTO SampleDTO { get; }

        public SampleResponse(SampleDTO dto)
        {
            SampleDTO = dto;
        }
    }
}
