using Infrastructure.Messaging;
using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Response
{
    public record GetEnemyDTOResponse : IMessage
    {
        public EnemyDTO Enemy { get; }

        public GetEnemyDTOResponse(EnemyDTO enemy)
        {
            Enemy = enemy;
        }
    }
}
