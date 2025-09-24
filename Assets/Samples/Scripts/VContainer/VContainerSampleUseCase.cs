using Infrastructure.Messaging;
using MessagePipe;
using R3;
using System;
using UnityEngine;

namespace Sample.VContainerSample
{
    public class VContainerSampleUseCase : IDisposable
    {
        ISubscriber<IMessage> subscriber;

        CompositeDisposable disposable;

        public VContainerSampleUseCase(ISubscriber<IMessage> subscriber)
        {
            this.subscriber = subscriber;
            disposable = new CompositeDisposable();
            Debug.Log("Construct VContainerSampleUseCase");
            Subscribe();
        }

        public void Dispose()
        {
            Debug.Log("Dispose VContainerSampleUseCase");
            disposable.Dispose();
        }

        void Subscribe()
        {
            subscriber.Subscribe(val =>
            {
                if (val is VContainerSampleMessage msg)
                {
                    Debug.Log("Message is VContainerSampleUseCase");
                }
            }).AddTo(disposable);
        }
    }
}
