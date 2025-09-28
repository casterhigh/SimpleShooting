using Infrastructure.Messaging;
using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Response
{
    public record PlayerDamageResponse : IMessage
    {
        public PlayerDTO Player { get; }

        public PlayerDamageResponse(PlayerDTO player)
        {
            Player = player;
        }
    }
}
