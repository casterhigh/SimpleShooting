using Infrastructure.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Request
{
    public record SpawnEnemyRequest : IMessage
    {
        public Vector3 Position { get; }

        public bool IsBoss { get; }

        public SpawnEnemyRequest(Vector3 position, bool isBoss)
        {
            Position = position;
            IsBoss = isBoss;
        }
    }
}
