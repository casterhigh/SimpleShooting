using Cysharp.Threading.Tasks;

namespace Infrastructure.View.Direction.Interface
{
    public interface IDirection
    {
        UniTask PlayAsync();
    }
}
