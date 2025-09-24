using Infrastructure.Sound.Domain.DTO;
using System.Collections.Generic;

namespace Infrastructure.Messaging.Sound.Response
{
    public record LoadSeResponse : IMessage
    {
        public List<SeDTO> SeList { get; }

        public LoadSeResponse(List<SeDTO> seList)
        {
            SeList = seList;
        }
    }
}
