using SharpDX.DirectWrite;

namespace Odyssey.UserInterface.Style
{
    public interface IStyleService
    {
        ControlDescription GetControlDescription(string themenName, string id);
        TextDescription GetTextDescription(string themenName, string id);
        FontCollection FontCollection { get; }
    }
}