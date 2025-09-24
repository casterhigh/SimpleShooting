using Cysharp.Threading.Tasks;
using System.Threading;

namespace Infrastructure
{
    public static class SimpleAnimationExtension
    {
        public static async UniTask PlayAsync(this SimpleAnimation animation, string stateName = "Default", CancellationToken token = default)
        {
            if (animation == null) return;

            var state = animation.GetState(stateName);
            if (state == null) return;

            animation.Stop(stateName);
            animation.Play(stateName);

            await UniTask.Yield(PlayerLoopTiming.Update);

            await UniTask.WaitWhile(
                () =>
                {
                    if (animation == null) return false;
                    return state != null && state.normalizedTime < 1;
                },
                PlayerLoopTiming.LastUpdate,
                token
            ).SuppressCancellationThrow();

            if (animation != null)
                animation.Stop(stateName);
        }


        public static void PlayNotSame(this SimpleAnimation animation, string stateName)
        {
            if (animation.IsPlaying(stateName)) return;
            animation.Stop();
            animation.Play(stateName);
        }

        public static void PlayDefault(this SimpleAnimation animation)
        {
            if (animation.isPlaying) animation.Stop();
            animation.Play("Default");
        }
    }
}
