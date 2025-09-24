using Infrastructure.Interface.View.UI;
using Infrastructure.Messaging;
using System.Collections.Generic;

namespace Infrastructure.View.UI.Messaging
{
    public record SendPages : IMessage
    {
        public Dictionary<string, IPageView> Pages { get; }

        public string FirstPageName { get; }

        public SendPages(Dictionary<string, IPageView> pages, string firstPageName)
        {
            Pages = pages;
            FirstPageName = firstPageName;
        }
    }
}
