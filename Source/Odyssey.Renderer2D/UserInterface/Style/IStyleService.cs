using Odyssey.Graphics.Shapes;
using SharpDX.DirectWrite;

namespace Odyssey.UserInterface.Style
{
    public interface IStyleService
    {
        ControlStyle GetControlStyle(string themenName, string id);
        TextDescription GetTextStyle(string themenName, string id);
        FontCollection FontCollection { get; }
        Gradient GetGradient(string themeName, string resourceName);
    }
}