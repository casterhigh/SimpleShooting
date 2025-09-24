using Infrastructure.Input.Domain.DTO;
using Infrastructure.Messaging;

namespace Infrastructure.Input.Messaging.Response
{
    public record OnStartedResponse : IMessage
    {
        public InputDTO DTO { get; }

        public OnStartedResponse(InputDTO dto)
        {
            DTO = dto;
        }
    }
}
