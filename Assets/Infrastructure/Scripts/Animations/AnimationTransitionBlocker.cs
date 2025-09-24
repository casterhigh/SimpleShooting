#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Infrastructure.Animations
{

    public class AnimationTransitionBlocker : StateMachineBehaviour
    {
        [SerializeField] string parameterName = "AnimationStateHash";

#if UNITY_EDITOR
        [SerializeField, HideInInspector] int cachedHash;

        void OnValidate()
        {
            // エディタでステート名からハッシュを自動計算
            if (!string.IsNullOrEmpty(name))
            {
                cachedHash = Animator.StringToHash(name);
            }
        }
#endif

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger(parameterName, stateInfo.shortNameHash);
        }
    }
}
