using Cysharp.Threading.Tasks;
using Infrastructure.Scenes;
using UnityEngine;
using VContainer.Unity;

namespace Sample.VContainerSample
{
    public class VContainerSampleScene1 : IInitializable
    {
        SceneChanger sceneChanger;

        public VContainerSampleScene1(SceneChanger sceneChanger)
        {
            this.sceneChanger = sceneChanger;
        }

        public void Initialize()
        {
            Debug.Log("Initialize");
            WaitForLoadScene().Forget();
        }

        async UniTask WaitForLoadScene()
        {
            await sceneChanger.LoadScene("Samples/Scenes/VContainerSample2");
        }
    }
}
