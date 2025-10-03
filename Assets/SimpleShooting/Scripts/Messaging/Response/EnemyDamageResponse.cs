using Infrastructure.Messaging;
using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Response
{
    public record EnemyDamageResponse : IMessage
    {
        public EnemyDTO Enemy { get; }

        public EnemyDamageResponse(EnemyDTO enemy)
        {
            Enemy = enemy;
        }
    }
}
