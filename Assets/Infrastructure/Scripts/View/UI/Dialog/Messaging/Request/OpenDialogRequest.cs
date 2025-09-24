using Infrastructure.Messaging;

namespace Infrastructure.View.Dialog.Messaging.Request
{
    public record OpenDialogRequest : IMessage
    {
        public CommonDialogData DialogData { get; }

        public OpenDialogRequest(CommonDialogData dialogData)
        {
            DialogData = dialogData;
        }
    }
}
