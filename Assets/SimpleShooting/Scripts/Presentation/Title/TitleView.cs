using Generated;
using Infrastructure.Messaging;
using Infrastructure.Scenes.Interface;
using Infrastructure.View.Logger.Interface;
using Infrastructure.View.UI;
using MessagePipe;
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

namespace SimpleShooting.Presentation.Title
{
    public class TitleView : PageViewBase
    {
        [SerializeField]
        Button starButton;

        IPublisher<IMessage> publisher;

        ISceneChanger sceneChanger;

        ILogger logger;

        public override string PageName => nameof(TitleView);

        [Inject]
        public void Construct(IPublisher<IMessage> publisher,
        ISceneChanger sceneChanger,
        ILogger logger)
        {
            this.publisher = publisher;
            this.sceneChanger = sceneChanger;
            this.logger = logger;
        }

        public override void Initialize()
        {
            starButton.OnClickAsObservable().Subscribe(_ => OnStartButtonClick()).AddTo(this);
        }

        void OnStartButtonClick()
        {
            logger.Log($"Scene Change to {ScenePaths.Main}");
            sceneChanger.LoadSceneWithUI(ScenePaths.Main, "");
        }
    }
}
