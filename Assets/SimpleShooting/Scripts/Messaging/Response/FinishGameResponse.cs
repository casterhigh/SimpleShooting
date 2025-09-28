using Infrastructure.Messaging;
using SimpleShooting.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Messaging.Response
{
    public record FinishGameResponse : IMessage
    {
        public GameFinishKind FinishKind { get; }

        public FinishGameResponse(GameFinishKind finishKind)
        {
            FinishKind = finishKind;
        }
    }
}
