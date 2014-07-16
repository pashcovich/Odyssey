using System.Collections.Generic;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;

namespace Odyssey.Talos.Components
{
    public interface ITextureResource : IContentComponent
    {
        Texture this[string type] { get; }
    }
}
