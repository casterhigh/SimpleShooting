using Infrastructure.Messaging;
using Infrastructure.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Request
{
    public record ReturnBulletRequest : IMessage
    {
        public IPoolable Poolable { get; }

        public ReturnBulletRequest(IPoolable poolable)
        {
            Poolable = poolable;
        }
    }
}
