
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Infrastructure.Animations.Editor
{
    public class AnimatorTransitionSetupTool : EditorWindow
    {
        AnimatorController animatorController;
        string parameterName = "AnimationStateHash";

        [MenuItem("Infrastructure/Tools/Animator/Setup Transition Blocker")]
        static void ShowWindow()
        {
            GetWindow<AnimatorTransitionSetupTool>("Animator Setup");
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Animator Transition Setup", EditorStyles.boldLabel);

            animatorController = (AnimatorController)EditorGUILayout.ObjectField("Animator Controller", animatorController, typeof(AnimatorController), false);
            parameterName = EditorGUILayout.TextField("Parameter Name", parameterName);

            if (GUILayout.Button("Setup Animator"))
            {
                if (animatorController == null)
                {
                    EditorUtility.DisplayDialog("Error", "AnimatorControllerを選択してください。", "OK");
                    return;
                }

                SetupAnimator(animatorController, parameterName);
            }
        }

        void SetupAnimator(AnimatorController controller, string parameterName)
        {
            Undo.RecordObject(controller, "Setup Animator Transition Blocker");

            // パラメータが存在しない場合は作成
            if (!controller.parameters.Any(p => p.name == parameterName))
            {
                controller.AddParameter(parameterName, AnimatorControllerParameterType.Int);
                Debug.Log($"[AnimatorSetup] パラメータ '{parameterName}' を追加しました。");
            }

            foreach (var layer in controller.layers)
            {
                foreach (var state in layer.stateMachine.states)
                {
                    // StateMachineBehaviour をアタッチ
                    if (!state.state.behaviours.OfType<AnimationTransitionBlocker>().Any())
                    {
                        state.state.AddStateMachineBehaviour<AnimationTransitionBlocker>();
                    }

                    var stateHash = state.state.nameHash;

                    // AnyState からこのステートへの遷移を探す
                    var anyTransitions = layer.stateMachine.anyStateTransitions
                        .Where(t => t.destinationState == state.state);

                    foreach (var transition in anyTransitions)
                    {
                        // 条件に "AnimationStateHash != stateHash" を追加
                        if (!transition.conditions.Any(c => c.parameter == parameterName))
                        {
                            transition.AddCondition(AnimatorConditionMode.NotEqual, stateHash, parameterName);
                            Debug.Log($"[AnimatorSetup] {state.state.name} に条件を追加: {parameterName} != {stateHash}");
                        }
                    }
                }
            }

            EditorUtility.SetDirty(controller);
            AssetDatabase.SaveAssets();
            Debug.Log($"[AnimatorSetup] セットアップ完了: {controller.name}");
        }
    }
#endif
}
