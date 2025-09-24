using Infrastructure.Messaging;

namespace Infrastructure.View.UI.Messaging
{
    public record OpenPage : IMessage
    {
        public string PageName { get; }

        public OpenPage(string pageName)
        {
            PageName = pageName;
        }
    }
}
