using Odyssey.UserInterface;

namespace Odyssey.Epos.Components
{
    public class UIElementComponent : Component
    {
        public UIElement Element { get; set; }

        public UIElementComponent() : base(ComponentTypeManager.GetType<UIElementComponent>())
        {
        }
    }
}
