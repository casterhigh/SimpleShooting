using Infrastructure.Input.Domain.Interface;

namespace Infrastructure.Input.Domain.Repository
{
    /// <summary>
    /// キーコンフィグの時とかに使う予定
    /// </summary>
    public class InputRepository : IInputRepository
    {
        public void Load()
        {
            throw new System.NotImplementedException();
        }

        public void Upsert<T>(T data)
        {
            throw new System.NotImplementedException();
        }
    }
}
