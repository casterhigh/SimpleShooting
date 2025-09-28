using Infrastructure.Domain;
using Infrastructure.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Response
{
    public record RentBulletResponse : IMessage
    {
        public ID Id { get; }

        public bool IsEnemy { get; }

        public Vector3 Position { get; }

        public Vector3 Direction { get; }

        public RentBulletResponse(ID id, bool isEnemy, Vector3 position, Vector3 direction)
        {
            Id = id;
            IsEnemy = isEnemy;
            Position = position;
            Direction = direction;
        }
    }
}
