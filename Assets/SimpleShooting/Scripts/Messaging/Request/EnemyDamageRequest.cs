using Infrastructure.Domain;
using Infrastructure.Messaging;
using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Request
{
    public record EnemyDamageRequest : IMessage
    {
        public ID EnemyId { get; }

        public PlayerDTO Player { get; }

        public EnemyDamageRequest(ID enemyId, PlayerDTO player)
        {
            EnemyId = enemyId;
            Player = player;
        }
    }
}
