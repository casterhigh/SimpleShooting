using Cysharp.Threading.Tasks;

namespace Infrastructure.Presentation.Interface
{
    /// <summary>
    /// MonoBehaviourを継承したクラスにのみ継承させる
    /// </summary>
    public interface IResourceLoader
    {
        UniTask Load();
    }
}
