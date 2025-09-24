using MessagePipe;
using R3;
using Infrastructure.Messaging.Sound.Request;
using Infrastructure.Messaging.Sound.response;
using Infrastructure.Messaging.Sound.Response;
using Infrastructure.Sound.Domain.Interface;
using Infrastructure.Messaging;
using System;

namespace Infrastructure.UseCase.Sound
{
    public class AudioUseCase : IDisposable
    {
        readonly ISubscriber<IMessage> subscriber;

        readonly IPublisher<IMessage> publisher;

        readonly IAudioRepository audioRepository;

        readonly CompositeDisposable disposable;

        public AudioUseCase(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        IAudioRepository audioRepository)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.audioRepository = audioRepository;
            disposable = new();

            Subscribe();
        }
        public void Dispose()
        {
            disposable.Dispose();
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is LoadBgmRequest)
                {
                    audioRepository.CreateBgmDto();
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is LoadSeRequest)
                {
                    audioRepository.CreateSeDto();
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is AudioSettingRequest request)
                {
                    audioRepository.UpdateAudioSetting(request.BgmVolume, request.SeVolume);
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is StartBgmRequest request)
                {
                    publisher.Publish(new StartBgmResponse(request.Id));
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is StartSeRequest request)
                {
                    publisher.Publish(new StartSeResponse(request.Id));
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is StopAllRequest)
                {
                    publisher.Publish(new StopAllResponse());
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is StopBgmRequest)
                {
                    publisher.Publish(new StopBgmResponse());
                }
            }).AddTo(disposable);

            subscriber.Subscribe(msg =>
            {
                if (msg is AudioLoadFinishedRequest)
                {
                    publisher.Publish(new AudioLoadFinishedResponse());
                }
            }).AddTo(disposable);
        }
    }
}
