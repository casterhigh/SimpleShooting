using Infrastructure.Input.Domain.DTO;
using Infrastructure.Messaging;

namespace Infrastructure.Input.Messaging.Response
{
    public record OnPerformedResponse : IMessage
    {
        public InputDTO DTO { get; }

        public OnPerformedResponse(InputDTO dto)
        {
            DTO = dto;
        }
    }
}
