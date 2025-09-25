using Infrastructure.Messaging;
using MessagePipe;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace SimpleShooting.Title
{
    public class TitleEntryPoint : IInitializable, IDisposable
    {
        IPublisher<IMessage> publisher;

        CompositeDisposable disposable;

        public TitleEntryPoint(IPublisher<IMessage> publisher)
        {
            this.publisher = publisher;
            disposable = new();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        public void Initialize()
        {
        }
    }
}
