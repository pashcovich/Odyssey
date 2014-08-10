using Odyssey.Graphics;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;

namespace Odyssey.UserInterface.Style
{
    public interface IStyleService : IResourceProvider
    {
        ControlStyle GetControlStyle(string themenName, string id);
        TextDescription GetTextStyle(string themenName, string id);
        FontCollection FontCollection { get; }
        Theme GetTheme(string themeName);
        void AddResource(IResource resource);
    }
}