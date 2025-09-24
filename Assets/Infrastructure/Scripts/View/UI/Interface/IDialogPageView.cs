using Cysharp.Threading.Tasks;
using Infrastructure.View.Dialog;
using UnityEngine;

namespace Infrastructure.View.UI.Interface
{
    public interface IDialogPageView
    {
        GameObject GameObject { get; }

        void SetData(CommonDialogData data);

        UniTask Open();

        UniTask Hide();
    }
}
