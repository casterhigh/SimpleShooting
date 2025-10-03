using Infrastructure.Messaging;
using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Request
{
    public record PlayerDamageRequest : IMessage
    {
        public EnemyDTO Enemy { get; }

        public PlayerDamageRequest(EnemyDTO enemy)
        {
            Enemy = enemy;
        }
    }
}
