using Cysharp.Threading.Tasks;
using Infrastructure.Messaging;
using MessagePipe;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Sample.Messaging.Messaging;
using System.Threading;
using System;

namespace Sample.Messaging.View
{
    public class MessagingView : MonoBehaviour
    {
        [SerializeField]
        TMP_InputField monsterIdInput;

        [SerializeField]
        TextMeshProUGUI monsterName;

        [SerializeField]
        Button requestButton;

        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        CancellationToken token;

        [Inject]
        public void Construct(IPublisher<IMessage> publisher, ISubscriber<IMessage> subscriber)
        {
            token = this.GetCancellationTokenOnDestroy();
            this.publisher = publisher;
            this.subscriber = subscriber;
            requestButton.onClick.AddListener(() => OnMonsterInfoRequestButtonClick());
            SubscribeMessage();
        }

        void SubscribeMessage()
        {
            subscriber.Subscribe(val =>
            {
                if (val is SampleResponse msg)
                {
                    monsterName.text = msg.SampleDTO.MonsterName;
                }
            }).AddTo(token);
        }

        void OnMonsterInfoRequestButtonClick()
        {
            var idText = monsterIdInput.text;
            if (string.IsNullOrEmpty(idText))
            {
                throw new InvalidOperationException($"{idText}の文字が空です。");
            }

            if (!int.TryParse(idText, out var id))
            {
                throw new InvalidOperationException($"{idText}はintへ変換できません。");
            }

            publisher.Publish(new SampleRequest(id));
        }
    }
}
