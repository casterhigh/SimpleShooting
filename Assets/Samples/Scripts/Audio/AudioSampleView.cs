using Infrastructure.Domain;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Sound.Request;
using Infrastructure.Messaging.Sound.Response;
using Infrastructure.View.Sound;
using MessagePipe;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Sample.Audio
{
    public class AudioSampleView : MonoBehaviour
    {
        [SerializeField]
        Button playBgm01;

        [SerializeField]
        Button playBgm02;

        [SerializeField]
        Button stopBgm;

        [SerializeField]
        Button stopAll;

        [SerializeField]
        Slider bgmVolumeSlider;

        [SerializeField]
        Slider seVolumeSlider;

        [SerializeField]
        Button volumeChangeSubmit;

        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        [Inject]
        public void Construct(IPublisher<IMessage> publisher, ISubscriber<IMessage> subscriber, IObjectResolver resolver)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            playBgm01.OnClickAsObservable().Subscribe(_ => OnPlayBgm01ButtonClick()).AddTo(this);
            playBgm02.OnClickAsObservable().Subscribe(_ => OnPlayBgm02ButtonClick()).AddTo(this);
            stopBgm.OnClickAsObservable().Subscribe(_ => OnStopBgmButtonClick()).AddTo(this);
            stopAll.OnClickAsObservable().Subscribe(_ => OnStopAllButtonClick()).AddTo(this);
            volumeChangeSubmit.OnClickAsObservable().Subscribe(_ => OnVolumeChangeSubmit()).AddTo(this);


            var sePublishers = GetComponentsInChildren<ButtonSePublisher>(includeInactive: true);
            foreach (var sePublisher in sePublishers)
            {
                resolver.InjectGameObject(sePublisher.gameObject);
            }

            SubScribe();
        }

        void SubScribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is AudioSettingResponse response)
                {
                    bgmVolumeSlider.normalizedValue = response.BgmVolume;
                    seVolumeSlider.normalizedValue = response.SeVolume;
                }
            }).AddTo(this);
        }

        void OnPlayBgm01ButtonClick() => publisher.Publish(new StartBgmRequest(new ID(60001)));

        void OnPlayBgm02ButtonClick() => publisher.Publish(new StartBgmRequest(new ID(60002)));

        void OnStopBgmButtonClick() => publisher.Publish(new StopBgmRequest());

        void OnStopAllButtonClick() => publisher.Publish(new StopAllRequest());

        void OnVolumeChangeSubmit() => publisher.Publish(new AudioSettingRequest(bgmVolumeSlider.value, seVolumeSlider.value));
    }
}
