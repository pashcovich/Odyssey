namespace Odyssey.Talos.DataStructures
{
    public interface IConnection
    {
        IGraphNode From { get; }
        IGraphNode To { get; }
    }
}
