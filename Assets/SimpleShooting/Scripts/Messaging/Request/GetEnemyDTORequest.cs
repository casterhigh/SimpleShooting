using Infrastructure.Domain;
using Infrastructure.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Request
{
    public record GetEnemyDTORequest : IMessage
    {
        public ID EnemyId { get; }

        public GetEnemyDTORequest(ID enemyId)
        {
            EnemyId = enemyId;
        }
    }
}
