using Infrastructure.Messaging;
using Infrastructure.ObjectPooling;
using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Request
{
    public record RentEnemyRequest : IMessage
    {
        public EnemyDTO Enemy { get; }

        public RentEnemyRequest(EnemyDTO enemy)
        {
            Enemy = enemy;
        }
    }
}
