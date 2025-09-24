using Cysharp.Threading.Tasks;

namespace Infrastructure.Scenes.Interface
{
    public interface ISceneChanger
    {
        UniTask LoadScene(string sceneName, bool useLoading);

        UniTask LoadSceneWithUI(string sceneName, string firstPageName);
    }
}
