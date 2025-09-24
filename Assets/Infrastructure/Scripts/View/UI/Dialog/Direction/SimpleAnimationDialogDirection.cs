using Cysharp.Threading.Tasks;
using UnityEngine;
using Infrastructure.View.Direction.Interface;
using System.Threading;

namespace Infrastructure.View.Dialog.Direction
{
    [RequireComponent(typeof(SimpleAnimation))]
    public class SimpleAnimationDialogDirection : MonoBehaviour, IDialogDirection
    {
        [SerializeField]
        SimpleAnimation open;

        [SerializeField]
        SimpleAnimation close;

        CancellationToken token;

        void Awake()
        {
            token = this.GetCancellationTokenOnDestroy();
        }

        public async UniTask Open()
        {
            await open.PlayAsync(token: token).SuppressCancellationThrow();
        }

        public async UniTask Close()
        {
            await close.PlayAsync(token: token).SuppressCancellationThrow();
        }

        void OnDestroy()
        {
            open.Stop();
            close.Stop();
        }
    }
}
