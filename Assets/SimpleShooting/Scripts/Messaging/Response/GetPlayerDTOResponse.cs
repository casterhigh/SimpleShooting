using Infrastructure.Messaging;
using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Response
{
    public record GetPlayerDTOResponse : IMessage
    {
        public PlayerDTO Player { get; }

        public GetPlayerDTOResponse(PlayerDTO player)
        {
            Player = player;
        }
    }
}
