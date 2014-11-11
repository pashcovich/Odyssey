using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Graphics;
using SharpDX.DirectWrite;

namespace Odyssey.UserInterface.Style
{
    public interface IStyleService : IResourceProvider
    {
        FontCollection FontCollection { get; }
        Theme GetTheme(string themeName);
        void AddResource(IResource resource, bool shared = true);
        Brush GetBrushResource(ColorResource colorResource, bool shared = true);
        Brush GetBrushResource(string name, Theme theme, bool shared = true);
        TextFormat GetTextResource(TextStyle textStyle, bool shared = true);
    }
}