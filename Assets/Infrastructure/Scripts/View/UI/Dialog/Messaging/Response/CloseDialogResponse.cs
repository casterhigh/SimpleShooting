using Infrastructure.Messaging;

namespace Infrastructure.View.Dialog.Messaging.Response
{
    public record CloseDialogResponse : IMessage
    {
        public bool Result { get; }

        public bool ContinueDialog { get; }

        public string RequirePage { get; }

        public CloseDialogResponse(bool result, bool continueDialog, string requirePage)
        {
            Result = result;
            ContinueDialog = continueDialog;
            RequirePage = requirePage;
        }
    }
}
