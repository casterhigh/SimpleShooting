using System.Collections;
using System.Collections.Generic;
using Infrastructure.Messaging;
using UnityEngine;

namespace SimpleShooting.Messaging.Request
{
    public record PauseGameRequest : IMessage
    {
    }
}
