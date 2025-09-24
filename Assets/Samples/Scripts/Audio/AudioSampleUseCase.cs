using Infrastructure.Messaging;
using Infrastructure.Messaging.Request;
using Infrastructure.Messaging.Response;
using Infrastructure.Sound.Domain.Interface;
using MessagePipe;
using R3;

namespace Sample.Audio
{
    public class AudioSampleUseCase
    {
        readonly IPublisher<IMessage> publisher;

        readonly ISubscriber<IMessage> subscriber;

        readonly IAudioRepository audioRepository;

        readonly CompositeDisposable disposable;

        public AudioSampleUseCase(IPublisher<IMessage> publisher,
        ISubscriber<IMessage> subscriber,
        IAudioRepository audioRepository)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.audioRepository = audioRepository;
            disposable = new();

            Subscribe();
        }

        void Subscribe()
        {
            subscriber.Subscribe(msg =>
            {
                if (msg is LoadMasterRequest)
                {
                    audioRepository.Load();

                    publisher.Publish(new LoadMasterResponse());
                }
            }).AddTo(disposable);
        }
    }
}