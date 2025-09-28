using Infrastructure.Messaging;
using Infrastructure.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Request
{
    public record ReturnEnemyRequest : IMessage
    {
        public IPoolable Poolable { get; }

        public ReturnEnemyRequest(IPoolable poolable)
        {
            Poolable = poolable;
        }
    }
}
