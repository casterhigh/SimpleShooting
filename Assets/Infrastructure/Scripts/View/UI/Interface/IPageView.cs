namespace Infrastructure.Interface.View.UI
{
    public interface IPageView
    {
        string PageName { get; }

        void Open();

        void Hide();

        void Initialize();
    }
}
