using Sample.Messaging.DTO;

namespace Sample.Messaging.Repository
{
    public interface IMessagingRepository
    {
        SampleDTO CreateDTO(long id);
    }
}
