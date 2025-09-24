using Infrastructure.View.UI.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sample.Inputs
{
    public class InputSystemSampleContentView : MonoBehaviour, IContentView
    {
        [SerializeField]
        Vector2 size;

        [SerializeField]
        Vector2 padding;

        [SerializeField]
        Button submitButton;

        [SerializeField]
        TextMeshProUGUI buttonName;

        public Button SubmitButton => submitButton;

        public int Index { get; private set; }

        public string Name => buttonName.text;

        public float Width => size.x + padding.x;

        public float Height => size.y + padding.y;

        public void Initialize(int index)
        {
            buttonName.text = $"Button {index}";
            Index = index;
        }
    }
}
