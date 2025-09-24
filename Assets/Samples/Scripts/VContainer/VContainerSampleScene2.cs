using Cysharp.Threading.Tasks;
using Infrastructure.Messaging;
using Infrastructure.Scenes;
using MessagePipe;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Sample.VContainerSample
{
    public class VContainerSampleScene2 : MonoBehaviour
    {
        [SerializeField]
        Button reloadButton;

        SceneChanger sceneChanger;

        ModelSingleton singleton;

        ModelTransient transient;

        IPublisher<IMessage> publisher;

        [Inject]
        public void Construct(SceneChanger sceneChanger, ModelSingleton singleton, ModelTransient transient, IPublisher<IMessage> publisher)
        {
            Debug.Log("Construct");
            this.sceneChanger = sceneChanger;
            this.singleton = singleton;
            this.transient = transient;
            this.publisher = publisher;

            reloadButton.onClick.AddListener(() => OnReloadButtonClick());
        }

        void OnReloadButtonClick()
        {
            publisher.Publish(new VContainerSampleMessage());
            WaitForReloadScene().Forget();
        }

        async UniTask WaitForReloadScene()
        {
            await sceneChanger.LoadScene("Samples/Scenes/VContainerSample2");
        }
    }
}
