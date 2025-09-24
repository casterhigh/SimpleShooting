using Infrastructure.Messaging;

namespace Infrastructure.View.Dialog.Messaging.Request
{
    public record CloseDialogRequest : IMessage
    {
        public bool Result { get; }

        public CloseDialogRequest(bool result)
        {
            Result = result;
        }
    }
}
