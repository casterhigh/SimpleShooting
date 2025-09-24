using Cysharp.Threading.Tasks;

namespace Infrastructure.View.Direction.Interface
{
    public interface ILitMotionDirection
    {
        UniTask PlayAsync();

        void SetDirection(UniTask task);
    }
}
