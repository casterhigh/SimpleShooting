using Cysharp.Threading.Tasks;
using UnityEngine;
using Infrastructure.Messaging.Sound.Response;
using MessagePipe;
using VContainer;
using UnityEngine.AddressableAssets;
using Infrastructure.Sound.Domain.DTO;
using Infrastructure.Messaging.Sound.Request;
using System.Collections.Generic;

namespace Infrastructure.View.Sound
{
    public partial class AudioManager : MonoBehaviour
    {
        [Inject]
        public void Construct()
        {
            WaitForResponses().Forget();
        }

        async UniTask WaitForResponses()
        {
            var bgmTcs = new UniTaskCompletionSource<LoadBgmResponse>();
            var seTcs = new UniTaskCompletionSource<LoadSeResponse>();
            var taskList = new List<UniTask>();
            subscriber.Subscribe(val =>
            {
                if (val is LoadBgmResponse res)
                {
                    bgmTcs.TrySetResult(res);
                    foreach (var bgm in res.BgmList)
                    {
                        taskList.Add(LoadAudioClip(bgm));
                    }
                }
            }).AddTo(token);

            subscriber.Subscribe(val =>
            {
                if (val is LoadSeResponse res)
                {
                    seTcs.TrySetResult(res);
                    foreach (var se in res.SeList)
                    {
                        taskList.Add(LoadAudioClip(se));
                    }
                }
            }).AddTo(token);

            await UniTask.WhenAll(bgmTcs.Task, seTcs.Task);
            await UniTask.WhenAll(taskList);

            publisher.Publish(new AudioLoadFinishedRequest());

            logger.Log("サウンドデータ読み込み完了");
        }

        async UniTask LoadAudioClip<T>(T data)
        {
            if (data is BgmDTO bgm)
            {
                var clip = await Addressables.LoadAssetAsync<AudioClip>(bgm.ClipName).ToUniTask();
                bgm.SetAudioClip(clip);
                bgmList.Add(bgm.Id, bgm);
                return;
            }

            if (data is SeDTO se)
            {
                var clip = await Addressables.LoadAssetAsync<AudioClip>(se.ClipName).ToUniTask();
                se.SetAudioClip(clip);
                seList.Add(se.Id, se);
                return;
            }
        }
    }
}
