using Infrastructure.Messaging;

namespace Infrastructure.View.Dialog.Messaging.Response
{
    public record CloseDialogResponse : IMessage
    {
        public bool Result { get; }

        public bool ContinueDialog { get; }

        public CloseDialogResponse(bool result, bool continueDialog)
        {
            Result = result;
            ContinueDialog = continueDialog;
        }
    }
}
