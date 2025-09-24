using MessagePipe;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Infrastructure.Messaging.Sound.Request;
using Infrastructure.Messaging;
using Generated.Enums;
using System;

namespace Infrastructure.View.Sound
{
    public class ButtonSePublisher : MonoBehaviour
    {
        [SerializeField]
        Button button;

        [SerializeField]
        Se se;

        IPublisher<IMessage> publisher;

        [Inject]
        public void Construct(IPublisher<IMessage> publisher)
        {
            this.publisher = publisher;
            if (button == null)
            {
                Reset();
            }

            if (button == null)
            {
                throw new InvalidOperationException("Buttonがnullです。");
            }

            button.OnClickAsObservable().Subscribe(_ => PlaySe()).AddTo(this);
        }

        void PlaySe()
        {
            publisher.Publish(new StartSeRequest(se));
        }

        void Reset()
        {
            if (TryGetComponent<Button>(out var button))
            {
                this.button = button;
            }
        }
    }
}
