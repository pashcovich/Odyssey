using Odyssey.Graphics;
using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Controls
{
    public interface IControl : IUIElement, IRenderable
    {
        ControlDescription Description { get; set; }
    }
}