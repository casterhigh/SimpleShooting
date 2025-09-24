using Infrastructure.Input.Domain.DTO;
using Infrastructure.Messaging;

namespace Infrastructure.Input.Messaging.Response
{
    public record OnCanceledResponse : IMessage
    {
        public InputDTO DTO { get; }

        public OnCanceledResponse(InputDTO dto)
        {
            DTO = dto;
        }
    }
}
