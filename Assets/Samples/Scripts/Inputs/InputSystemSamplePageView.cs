using Generated.Enums;
using Infrastructure.Enums;
using Infrastructure.Input.Generated.Enum;
using Infrastructure.Input.Messaging.Request;
using Infrastructure.Input.Messaging.Response;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Sound.Request;
using Infrastructure.View.Dialog;
using Infrastructure.View.Dialog.Messaging.Request;
using Infrastructure.View.Dialog.Messaging.Response;
using Infrastructure.View.UI;
using Infrastructure.View.UI.Messaging;
using MessagePipe;
using R3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

namespace Sample.Inputs
{
    public class InputSystemSamplePageView : PageViewBase
    {
        [SerializeField]
        Button closeButton;

        [SerializeField]
        RectTransform contentRoot;

        [SerializeField]
        InputSystemSampleContentView prefab;

        [SerializeField]
        int createCount = 30;

        int row = 5;

        int currentIndex = 0;

        Dictionary<int, Button> contents = new();

        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        IObjectResolver resolver;

        ILogger logger;

        Dictionary<DialogKind, string> closeButtonNames;

        public override string PageName => nameof(InputSystemSamplePageView);

        [Inject]
        public void Construct(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        IObjectResolver resolver,
        ILogger logger)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.resolver = resolver;
            this.logger = logger;

            closeButton.OnClickAsObservable().Where(_ => !ShowingDialog).Subscribe(_ => OnClosButtonClick()).AddTo(this);

            Subscribe();
        }

        public override void Initialize()
        {
            base.Initialize();
            prefab.gameObject.SetActive(false);

            row = Mathf.FloorToInt(contentRoot.sizeDelta.y / prefab.Height);

            for (var i = 0; i < createCount; i++)
            {
                var index = i;
                var obj = Instantiate(prefab, contentRoot);
                resolver.InjectGameObject(obj.gameObject);
                obj.gameObject.SetActive(true);
                obj.Initialize(index);
                obj.SubmitButton.OnClickAsObservable().Where(_ => !ShowingDialog).Subscribe(_ => OnContentButtonClick(index)).AddTo(obj);
                contents.Add(index, obj.SubmitButton);
            }

            var selectObj = contents[currentIndex];
            EventSystem.current.SetSelectedGameObject(selectObj.gameObject);
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (ShowingDialog) return;
                if (msg is OnStartedResponse response)
                {
                    if (response.DTO.MapKind != MapKind.UI) return;
                    if (response.DTO.UIActions.Value == UIActions.Left)
                    {
                        OnStartedLeft();
                        return;
                    }

                    if (response.DTO.UIActions.Value == UIActions.Right)
                    {
                        OnStartedRight();
                        return;
                    }

                    if (response.DTO.UIActions.Value == UIActions.Up)
                    {
                        OnStartedUp();
                        return;
                    }

                    if (response.DTO.UIActions.Value == UIActions.Down)
                    {
                        OnStartedDown();
                        return;
                    }
                }
            }).AddTo(this);

            subscriber.Subscribe(msg =>
            {
                if (ShowingDialog) return;
                if (msg is OnCanceledResponse response)
                {
                    if (response.DTO.MapKind != MapKind.UI) return;
                    if (response.DTO.UIActions.Value == UIActions.Submit)
                    {
                        var btn = contents[currentIndex];
                        btn.onClick.Invoke();
                    }

                    if (response.DTO.UIActions.Value == UIActions.Cancel)
                    {
                        closeButton.onClick.Invoke();
                    }
                }
            }).AddTo(this);

            subscriber.Subscribe(msg =>
            {
                if (msg is CloseDialogResponse response)
                {
                    if (response.ContinueDialog)
                    {
                        OnDialogClosed(response.Result);
                        return;
                    }

                    ShowingDialog = false;
                    var btn = contents[currentIndex];
                    EventSystem.current.SetSelectedGameObject(btn.gameObject);
                }
            }).AddTo(this);

            subscriber.Subscribe(msg =>
            {
                if (ShowingDialog) return;
                if (msg is OnPerformedResponse response)
                {
                    if (response.DTO.MapKind != MapKind.UI) return;
                    if (response.DTO.UIActions.Value == UIActions.Click
                    || response.DTO.UIActions.Value == UIActions.RightClick
                    || response.DTO.UIActions.Value == UIActions.MiddleClick)
                    {
                        if (EventSystem.current.currentSelectedGameObject != null) return;

                        var btn = contents[currentIndex];
                        EventSystem.current.SetSelectedGameObject(btn.gameObject);
                    }
                }
            }).AddTo(this);
        }

        void OnContentButtonClick(int index)
        {
            var dialogData = new CommonDialogData("サンプル", $"サンプルメッセージ From Button{index}", DialogKind.YesNo, AddressableAssetAddress.SAMPLE_DIALOG, true);
            ShowingDialog = true;
            publisher.Publish(new OpenDialogRequest(dialogData));
        }

        void OnDialogClosed(bool result)
        {
            var resultMessage = result ? "OKボタンが押されました" : "キャンセルボタンが押されました";
            var dialogData = new CommonDialogData("リザルト", resultMessage, DialogKind.Close, AddressableAssetAddress.SAMPLE_DIALOG, false);
            ShowingDialog = true;
            publisher.Publish(new OpenDialogRequest(dialogData));
        }

        void OnClosButtonClick()
        {
            publisher.Publish(new ChangeMapKindRequest(MapKind.Player));
            publisher.Publish(new CloseAllPage());
            publisher.Publish(new StartSeRequest(Se.Cancel));
        }

        void OnStartedLeft()
        {
            MoveCursor(row * -1);
            logger.Log($" OnStartedLeft {currentIndex}");
        }

        void OnStartedRight()
        {
            MoveCursor(row);
            logger.Log($" OnStartedRight {currentIndex}");
        }

        void OnStartedUp()
        {
            MoveCursor(-1);
            logger.Log($" OnStartedUp {currentIndex}");
        }

        void OnStartedDown()
        {
            MoveCursor(1);
            logger.Log($" OnStartedDown {currentIndex}");
        }

        void MoveCursor(int moveCount)
        {
            var index = currentIndex + moveCount;
            if (index < 0) index = 0;
            if (index >= contents.Count) index = contents.Count - 1;
            var view = contents[index];
            EventSystem.current.SetSelectedGameObject(view.gameObject);
            currentIndex = index;
            publisher.Publish(new StartSeRequest(Se.Cursor));
        }
    }
}
