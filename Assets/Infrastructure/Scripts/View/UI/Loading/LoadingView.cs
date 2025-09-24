using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;
using Infrastructure.View.UI.Loading.Messaging.Response;
using Infrastructure.Messaging;
using System.Threading;

namespace Infrastructure.View.UI.Loading
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;

        ISubscriber<IMessage> subscriber;

        CancellationToken token;

        [Inject]
        public void Construct(ISubscriber<IMessage> subscriber)
        {
            this.subscriber = subscriber;
            token = this.GetCancellationTokenOnDestroy();
            Subscribe();
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is StartLoadingResponse)
                {
                    Open();
                }
            }).AddTo(token);

            subscriber.Subscribe(msg =>
            {
                if (msg is FinishLoadingResponse)
                {
                    Hide();
                }
            }).AddTo(token);
        }

        void Open()
        {
            gameObject.SetActive(true);
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        void Hide()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
