using Infrastructure.Messaging;
using SimpleShooting.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Request
{
    public record FinishGameRequest : IMessage
    {
        public GameFinishKind FinishKind { get; }

        public FinishGameRequest(GameFinishKind finishKind)
        {
            FinishKind = finishKind;
        }
    }
}
