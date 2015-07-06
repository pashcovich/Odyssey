namespace Odyssey.Epos
{

    public enum UpdateAction
    {
        None,
        Register,
        Initialize,
    }

    public enum UpdateType
    {
        Undefined,
        Add,
        Remove,
    }

    public enum MessagePriority
    {
        Asynchronous,
        Synchronous
    }

}
