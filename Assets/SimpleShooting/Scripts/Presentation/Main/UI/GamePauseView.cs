using Generated;
using Infrastructure.Enums;
using Infrastructure.Messaging;
using Infrastructure.View.Dialog;
using Infrastructure.View.Dialog.Messaging.Request;
using Infrastructure.View.Dialog.Messaging.Response;
using Infrastructure.View.UI;
using Infrastructure.View.UI.Management;
using Infrastructure.View.UI.Messaging;
using MessagePipe;
using R3;
using SimpleShooting.Messaging.Request;
using SimpleShooting.Messaging.Response;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace SimpleShooting.Presentation.Main
{
    public class GamePauseView : PageViewBase
    {
        [SerializeField]
        Button closeButton;

        [SerializeField]
        Button exitButton;

        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        public override string PageName => nameof(GamePauseView);

        [Inject]
        public void Construct(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;

            closeButton.OnClickAsObservable().Subscribe(_ => OnCloseButtonClick()).AddTo(this);
            exitButton.OnClickAsObservable().Subscribe(_ => OnExitButtonClick()).AddTo(this);

            Subscribe();
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is CloseDialogResponse response)
                {
                    if (!response.RequirePage.Equals(PageName)) return;
                    OnCloseDialog(response.Result);
                }
            }).AddTo(this);
        }

        void OnCloseButtonClick() => publisher.Publish(new ResumeGameRequest());

        void OnExitButtonClick()
        {
            var dialogData = new CommonDialogData("", MessageText.GameExit, DialogKind.YesNo, AddressableAssetAddress.COMMON_DIALOG, false, PageName);
            publisher.Publish(new OpenDialogRequest(dialogData));
        }

        void OnCloseDialog(bool result)
        {
            if (!result) return;
            Application.Quit();
        }
    }
}