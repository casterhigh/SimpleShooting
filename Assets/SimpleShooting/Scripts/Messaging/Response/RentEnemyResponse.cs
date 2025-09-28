using Infrastructure.Messaging;
using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Response
{
    public record RentEnemyResponse : IMessage
    {
        public EnemyDTO Enemy { get; }

        public RentEnemyResponse(EnemyDTO enemy)
        {
            Enemy = enemy;
        }
    }
}
