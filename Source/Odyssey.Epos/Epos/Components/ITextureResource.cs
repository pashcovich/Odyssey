using Odyssey.Graphics;

namespace Odyssey.Epos.Components
{
    public interface ITextureResource : IContentComponent
    {
        Texture this[string type] { get; }
    }
}
