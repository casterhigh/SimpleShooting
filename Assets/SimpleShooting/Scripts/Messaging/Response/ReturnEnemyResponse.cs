using Infrastructure.Messaging;
using Infrastructure.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Response
{
    public record ReturnEnemyResponse : IMessage
    {
        public IPoolable Poolable { get; }

        public ReturnEnemyResponse(IPoolable poolable)
        {
            Poolable = poolable;
        }
    }
}
