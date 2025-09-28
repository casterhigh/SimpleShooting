using Infrastructure.Messaging;
using Infrastructure.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Response
{
    public record ReturnBulletResponse : IMessage
    {
        public IPoolable Poolable { get; }

        public ReturnBulletResponse(IPoolable poolable)
        {
            Poolable = poolable;
        }
    }
}
