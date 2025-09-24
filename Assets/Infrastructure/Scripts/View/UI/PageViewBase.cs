using R3;
using UnityEngine;
using Infrastructure.Interface.View.UI;

namespace Infrastructure.View.UI
{
    public abstract class PageViewBase : MonoBehaviour, IPageView
    {
        [SerializeField] CanvasGroup canvasGroup;

        protected virtual bool ShowingDialog { get; set; }

        public abstract string PageName { get; }

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
