using Infrastructure.Sound.Domain.DTO;
using System.Collections.Generic;

namespace Infrastructure.Messaging.Sound.Response
{
    public record LoadBgmResponse : IMessage
    {
        public List<BgmDTO> BgmList { get; }

        public LoadBgmResponse(List<BgmDTO> bgmList)
        {
            BgmList = bgmList;
        }
    }
}
