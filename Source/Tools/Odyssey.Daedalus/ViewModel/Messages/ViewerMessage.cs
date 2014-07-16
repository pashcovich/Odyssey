namespace Odyssey.Daedalus.ViewModel.Messages
{
    public enum ViewerAction
    {
        None,
        CaptureFrame
    }

    public class ViewerMessage
    {
        public ViewerMessage(ViewerAction action)
        {
            Action = action;
        }

        public ViewerAction Action { get; private set; }
    }
}