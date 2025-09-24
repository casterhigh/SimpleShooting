using Cysharp.Threading.Tasks;
using MessagePipe;
using VContainer;
using Infrastructure.Messaging.Sound.Response;
using Infrastructure.Sound.Domain.DTO;
using Infrastructure.Messaging;
using Infrastructure.Domain;
using Infrastructure.View.Logger.Interface;
using UnityEngine;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;
using System.Threading;
using System.Collections.Generic;
using System;

namespace Infrastructure.View.Sound
{
    public partial class AudioManager : MonoBehaviour
    {
        [SerializeField]
        AudioSource bgmSource;

        [SerializeField]
        AudioSource seSource;

        [SerializeField]
        float audioFadeRate;

        [SerializeField]
        AudioListener audioListener;

        IPublisher<IMessage> publisher;

        ISubscriber<IMessage> subscriber;

        ILogger logger;

        CancellationTokenSource cts;

        CancellationToken token;

        Dictionary<ID, BgmDTO> bgmList = new();

        Dictionary<ID, SeDTO> seList = new();

        ID currentBgmClipId = null;

        float defaultBgmVolume;

        [Inject]
        public void Construct(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        ILogger logger)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.logger = logger;
            token = this.GetCancellationTokenOnDestroy();
            cts = new CancellationTokenSource();

            Subscribe();
        }

        void SetAudioSetting(AudioSettingResponse response)
        {
            bgmSource.volume = (float)response.BgmVolume;
            seSource.volume = (float)response.SeVolume;
            defaultBgmVolume = bgmSource.volume;

            logger.Log($"BGMのボリューム変更 : {bgmSource.volume} SEのボリューム変更 : {seSource.volume}");
        }

        void PlaySe(StartSeResponse response)
        {
            if (!seList.TryGetValue(response.Id, out var se))
            {
                throw new InvalidOperationException($"{response.Id}に対するAudioClipがありません");
            }

            seSource.clip = se.Clip;
            seSource.Play();
        }

        async UniTask FadeToNextBgm(StartBgmResponse response)
        {
            if (cts == null) cts = new();
            await FadeOutBgm();
            if (cts == null) return;
            await PlayBgm(response);
        }

        async UniTask PlayBgm(StartBgmResponse response)
        {
            if (!bgmList.TryGetValue(response.Id, out var bgm))
            {
                throw new InvalidOperationException($"{response.Id}に対するAudioClipがありません");
            }

            if (cts == null) cts = new();

            currentBgmClipId = bgm.Id;
            bgmSource.clip = bgm.Clip;
            bgmSource.pitch = bgm.Pitch;
            bgmSource.loop = true;
            bgmSource.Play();
            var isCanceled = await UniTask.DelayFrame(1, cancellationToken: cts.Token).SuppressCancellationThrow();
            if (isCanceled) return;
            bgmSource.loop = bgm.IsLoop;

            bgmSource.volume = 0;

            while (bgmSource.volume < defaultBgmVolume)
            {
                bgmSource.volume += audioFadeRate;
                isCanceled = await UniTask.DelayFrame(1, cancellationToken: cts.Token).SuppressCancellationThrow();
                if (isCanceled) return;
            }

            bgmSource.volume = defaultBgmVolume;
        }

        async UniTask FadeOutBgm()
        {
            while (bgmSource.volume > 0f)
            {
                bgmSource.volume -= audioFadeRate;
                var isCanceled = await UniTask.DelayFrame(1, cancellationToken: cts.Token).SuppressCancellationThrow();
                if (isCanceled) return;
            }

            bgmSource.volume = 0;
            bgmSource.Stop();
        }

        void StopBgm()
        {
            bgmSource.Stop();
            currentBgmClipId = null;
            cts?.Cancel();
            cts = null;
        }

        void StopAll()
        {
            seSource.Stop();
            StopBgm();
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is StartBgmResponse response)
                {
                    if (currentBgmClipId == response.Id)
                    {
                        logger.Log("再生中と同じクリップが指定されています");
                        return;
                    }

                    if (currentBgmClipId == null)
                    {
                        PlayBgm(response).Forget();
                    }
                    else
                    {
                        FadeToNextBgm(response).Forget();
                    }
                }
            }).AddTo(token);

            subscriber.Subscribe(msg =>
            {
                if (msg is StartSeResponse response)
                {
                    PlaySe(response);
                }
            }).AddTo(token);

            subscriber.Subscribe(msg =>
            {
                if (msg is StopBgmResponse response)
                {
                    StopBgm();
                }
            }).AddTo(token);

            subscriber.Subscribe(msg =>
            {
                if (msg is AudioSettingResponse response)
                {
                    SetAudioSetting(response);
                }
            }).AddTo(token);

            subscriber.Subscribe(msg =>
            {
                if (msg is StopAllResponse response)
                {
                    StopAll();
                }
            }).AddTo(token);
        }

        void Reset()
        {
            var sources = GetComponents<AudioSource>();
            foreach (var source in sources)
            {
                DestroyImmediate(source);
            }

            var audioListeners = GetComponents<AudioListener>();
            foreach (var listener in audioListeners)
            {
                DestroyImmediate(listener);
            }

            bgmSource = gameObject.AddComponent<AudioSource>();
            seSource = gameObject.AddComponent<AudioSource>();
            audioListener = gameObject.AddComponent<AudioListener>();

            bgmSource.playOnAwake = false;
            seSource.playOnAwake = false;
            audioFadeRate = .03f;
        }
    }
}
