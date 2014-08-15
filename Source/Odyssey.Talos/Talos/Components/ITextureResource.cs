using Odyssey.Graphics;

namespace Odyssey.Talos.Components
{
    public interface ITextureResource : IContentComponent
    {
        Texture this[string type] { get; }
    }
}
