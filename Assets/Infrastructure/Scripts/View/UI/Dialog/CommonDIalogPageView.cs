using Cysharp.Threading.Tasks;
using Generated;
using Generated.Enums;
using Infrastructure.Enums;
using Infrastructure.Input.Generated.Enum;
using Infrastructure.Input.Messaging.Response;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Sound.Request;
using Infrastructure.View.Dialog.Messaging.Request;
using Infrastructure.View.Direction.Interface;
using Infrastructure.View.UI.Interface;
using MessagePipe;
using R3;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

namespace Infrastructure.View.Dialog
{
    public class CommonDialogData
    {
        public string Title { get; } = "";

        public string Message { get; }

        public DialogKind DialogKind { get; }

        public string DialogPrefabName { get; }

        public bool ContinueDialog { get; }

        public string RequirePage { get; }

        public CommonDialogData(string title, string message, DialogKind dialogKind, string dialogPrefabName, bool continueDialog, string requirePage)
        {
            Title = title;
            Message = message;
            DialogKind = dialogKind;
            DialogPrefabName = dialogPrefabName;
            ContinueDialog = continueDialog;
            RequirePage = requirePage;
        }
    }

    public class CommonDIalogPageView : MonoBehaviour, IDialogPageView
    {
        [SerializeField]
        TextMeshProUGUI title;

        [SerializeField]
        TextMeshProUGUI message;

        [SerializeField]
        Button okButton;

        [SerializeField]
        Button cancelButton;

        [SerializeField]
        Button closeButton;

        [SerializeField]
        GameObject directionObj;

        [SerializeField]
        TextMeshProUGUI okButtonText;

        [SerializeField]
        TextMeshProUGUI cancelButtonText;

        [SerializeField]
        TextMeshProUGUI closeButtonText;

        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        ILogger logger;

        IDialogDirection direction;

        int currentIndex;

        List<Button> contents;

        public GameObject GameObject => gameObject;

        [Inject]
        public void Construct(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        ILogger logger)
        {
            if (!directionObj.TryGetComponent(out direction))
            {
                throw new InvalidOperationException($"Open用の演出がありません");
            }

            this.publisher = publisher;
            this.subscriber = subscriber;
            this.logger = logger;

            okButton.OnClickAsObservable().Subscribe(_ => OnOkButtonClick()).AddTo(this);
            closeButton.OnClickAsObservable().Subscribe(_ => OnCloseButtonClick()).AddTo(this);
            cancelButton.OnClickAsObservable().Subscribe(_ => OnCloseButtonClick()).AddTo(this);

            okButtonText.text = MessageText.OKButton;
            cancelButtonText.text = MessageText.CancelButton;
            closeButtonText.text = MessageText.CloseButton;

            Subscribe();
        }

        public void SetData(CommonDialogData data)
        {
            title.gameObject.SetActive(!string.IsNullOrEmpty(data.Title));
            title.text = data.Title;
            message.text = data.Message;
            CreateButtonList(data.DialogKind);
            var content = contents[currentIndex];
            EventSystem.current.SetSelectedGameObject(content.gameObject);
        }

        public async UniTask Open()
        {
            await direction.Open();
        }

        public async UniTask Hide()
        {
            await direction.Close();
            Destroy(gameObject);
        }

        void OnOkButtonClick() => publisher.Publish(new CloseDialogRequest(true));

        void OnCloseButtonClick() => publisher.Publish(new CloseDialogRequest(false));

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is OnStartedResponse response)
                {
                    if (response.DTO.MapKind != MapKind.UI) return;
                    if (response.DTO.UIActions.Value == UIActions.Right)
                    {
                        MoveCursor(1);
                        return;
                    }

                    if (response.DTO.UIActions.Value == UIActions.Left)
                    {
                        MoveCursor(-1);
                        return;
                    }
                }
            }).AddTo(this);

            subscriber.Subscribe(msg =>
            {
                if (msg is OnCanceledResponse response)
                {
                    if (response.DTO.MapKind != MapKind.UI) return;
                    if (response.DTO.UIActions.Value == UIActions.Submit)
                    {
                        var content = contents[currentIndex];
                        content.onClick.Invoke();
                        return;
                    }

                    if (response.DTO.UIActions.Value == UIActions.Cancel)
                    {
                        closeButton.onClick.Invoke();
                        return;
                    }
                }
            }).AddTo(this);
        }

        void CreateButtonList(DialogKind dialogKind)
        {
            if (dialogKind == DialogKind.YesNo)
            {
                contents = new List<Button>
                {
                    okButton,
                    cancelButton,
                };

                closeButton.gameObject.SetActive(false);

                return;
            }
            if (dialogKind == DialogKind.Close)
            {
                contents = new List<Button>
                {
                    closeButton,
                };

                okButton.gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(false);


                return;
            }
        }

        void MoveCursor(int moveCount)
        {
            var index = currentIndex + moveCount;
            if (index < 0) index = contents.Count - 1;
            if (index >= contents.Count) index = 0;
            var view = contents[index];
            EventSystem.current.SetSelectedGameObject(view.gameObject);
            currentIndex = index;
            publisher.Publish(new StartSeRequest(Se.Cursor));
        }
    }
}
