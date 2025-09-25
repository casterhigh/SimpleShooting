using Infrastructure.Interface.View.UI;
using R3;
using UnityEngine;

namespace Infrastructure.View.UI
{
    public abstract class PageViewBase : MonoBehaviour, IPageView
    {
        [SerializeField] CanvasGroup canvasGroup;

        protected virtual bool ShowingDialog { get; set; }

        /// <summary>
        /// nameof()で継承クラスを指定する
        /// </summary>
        public abstract string PageName { get; }

        /// <summary>
        /// 親クラスの処理はなし
        /// </summary>
        public virtual void Initialize() { }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public virtual void Hide()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
