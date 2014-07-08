namespace Odyssey.Tools.ShaderGenerator.ViewModel.Messages
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