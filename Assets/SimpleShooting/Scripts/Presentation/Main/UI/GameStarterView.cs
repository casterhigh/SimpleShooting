using Cysharp.Threading.Tasks;
using Infrastructure.Messaging;
using Infrastructure.View.Direction.Interface;
using Infrastructure.View.UI;
using Infrastructure.View.UI.Loading.Messaging.Response;
using MessagePipe;
using R3;
using SimpleShooting.Messaging.Request;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

namespace SimpleShooting.Presentation.Main
{
    public class GameStarterView : PageViewBase
    {
        [SerializeField]
        GameObject countDownDirection;

        IDirection direction;

        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        CancellationToken token;

        public override string PageName => nameof(GameStarterView);

        [Inject]
        public void Construct(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            token = this.GetCancellationTokenOnDestroy();
            direction = countDownDirection.GetComponent<IDirection>();
            Subscribe();
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is FinishLoadingResponse)
                {
                    StartCountDown().Forget();
                }
            }).AddTo(token);
        }

        async UniTask StartCountDown()
        {
            var isCanceled = await direction.PlayAsync().SuppressCancellationThrow();
            if (isCanceled) return;
            publisher.Publish(new StartGameRequest());
        }
    }
}
