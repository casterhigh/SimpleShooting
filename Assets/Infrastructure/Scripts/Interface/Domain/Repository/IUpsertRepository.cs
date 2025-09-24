namespace Infrastructure.Domain.Interface.Repository
{
    public interface IUpsertRepository
    {
        void Upsert<T>(T data);
    }
}
