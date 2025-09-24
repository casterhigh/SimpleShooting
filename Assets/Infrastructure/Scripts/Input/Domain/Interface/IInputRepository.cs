using Infrastructure.Domain.Interface.Repository;

namespace Infrastructure.Input.Domain.Interface
{
    public interface IInputRepository : ILoadRepository, IUpsertRepository
    {
    }
}
