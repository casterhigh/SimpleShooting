using Cysharp.Threading.Tasks;
using Infrastructure.Interface.View.UI;
using Infrastructure.Messaging;
using Infrastructure.View.Dialog;
using Infrastructure.View.Dialog.Messaging.Request;
using Infrastructure.View.Dialog.Messaging.Response;
using Infrastructure.View.Logger.Interface;
using Infrastructure.View.UI.Interface;
using Infrastructure.View.UI.Messaging;
using MessagePipe;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

namespace Infrastructure.View.UI.Management
{
    public class PageManager : IDisposable
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly IObjectResolver resolver;

        readonly CompositeDisposable disposable;

        readonly ILogger logger;

        Dictionary<string, IPageView> pages;

        IPageView currentPage;

        IDialogPageView dialogPageView = null;

        public PageManager(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        IObjectResolver resolver,
        ILogger logger)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.resolver = resolver;
            this.logger = logger;
            disposable = new();
            Subscribe();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        void Subscribe()
        {
            subscriber.Subscribe(val =>
            {
                if (val is SendPages msg)
                {
                    pages = msg.Pages;
                    CloseAllPage();
                    if (string.IsNullOrEmpty(msg.FirstPageName)) return;

                    Show(msg.FirstPageName);
                }
            }).AddTo(disposable);

            subscriber.Subscribe(val =>
            {
                if (val is OpenPage msg)
                {
                    Show(msg.PageName);
                }
            }).AddTo(disposable);

            subscriber.Subscribe(val =>
           {
               if (val is CloseAllPage msg)
               {
                   CloseAllPage();
               }
           }).AddTo(disposable);

            subscriber.Subscribe(async val =>
            {
                if (val is OpenDialogRequest msg)
                {
                    await OpenDialog(msg.DialogData);
                }
            }).AddTo(disposable);
        }

        void Show(string pageName)
        {
            if (!pages.ContainsKey(pageName))
            {
                throw new InvalidOperationException($"{pageName}は存在していません");
            }

            if (currentPage != null)
            {
                currentPage.Hide();
            }

            var page = pages[pageName];
            logger.Log($"Show {page.PageName}");
            page.Open();
            currentPage = page;
        }

        void CloseAllPage()
        {
            foreach (var page in pages)
            {
                logger.Log($"Hide {page.Key}");
                page.Value.Hide();
            }
        }

        async UniTask OpenDialog(CommonDialogData dialogData)
        {
            var dialog = await Addressables.InstantiateAsync(dialogData.DialogPrefabName);
            WaitForCloseDialogRequest(dialog, dialogData.ContinueDialog, dialogData.RequirePage).Forget();
            dialogPageView = dialog.GetComponent<IDialogPageView>();
            resolver.InjectGameObject(dialogPageView.GameObject);
            dialogPageView.SetData(dialogData);
            await dialogPageView.Open();
            publisher.Publish(new OpenDialogResponse());
        }

        async UniTask WaitForCloseDialogRequest(GameObject dialog, bool continueDialog, string requirePage)
        {
            var tcs = new UniTaskCompletionSource<CloseDialogRequest>();
            var result = false;

            subscriber.Subscribe(val =>
            {
                if (val is CloseDialogRequest req)
                {
                    tcs.TrySetResult(req);
                    result = req.Result;
                }
            }).AddTo(disposable);

            await UniTask.WhenAll(tcs.Task);

            await dialogPageView.Hide();
            dialogPageView = null;
            Addressables.Release(dialog);

            publisher.Publish(new CloseDialogResponse(result, continueDialog, requirePage));
        }
    }
}
