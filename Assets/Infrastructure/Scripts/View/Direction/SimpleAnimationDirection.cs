using Cysharp.Threading.Tasks;
using Infrastructure.View.Direction.Interface;
using System.Threading;
using UnityEngine;

namespace Infrastructure.View.Direction
{
    public class SimpleAnimationDirection : MonoBehaviour, IDirection
    {
        [SerializeField]
        SimpleAnimation simpleAnimation;

        CancellationToken token;

        void Awake()
        {
            token = this.GetCancellationTokenOnDestroy();
        }

        public async UniTask PlayAsync()
        {
            await simpleAnimation.PlayAsync(token: token);
        }
    }
}
