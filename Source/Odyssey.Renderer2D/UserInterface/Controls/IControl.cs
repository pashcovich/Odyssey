
using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Controls
{
    public interface IControl : IUIElement
    {
        ControlDescription Description { get; }
    }
}