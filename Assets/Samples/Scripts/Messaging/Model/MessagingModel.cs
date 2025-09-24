using Infrastructure.Messaging;
using MessagePipe;
using Sample.Messaging.Messaging;
using Sample.Messaging.Repository;

namespace Sample.Messaging.Model
{
    public class MessagingModel : IMessagingModel
    {
        IPublisher<IMessage> publisher;

        IMessagingRepository repository;

        public MessagingModel(IPublisher<IMessage> publisher, IMessagingRepository repository)
        {
            this.publisher = publisher;
            this.repository = repository;
        }

        public void CreateDTO(long id)
        {
            var dto = repository.CreateDTO(id);
            publisher.Publish(new SampleResponse(dto));
        }
    }
}
