using SharpDX;

namespace Odyssey.Graphics.Meshes
{
    public interface IPositionVertex
    {
        /// <summary>
        /// Gets or sets the position of the vertex.
        /// </summary>
        Vector3 Position { get; set; }
    }

    public interface IRenderableVertex : IPositionVertex
    {
        Vector3 Normal { get; set; }
    }
}