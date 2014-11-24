using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls
{
    public abstract class ListBoxBase : ItemsControl
    {
        private const string ControlTag = "ListBox";

        protected ListBoxBase()
            : base("Panel")
        {
        }

        protected override Vector2 ArrangeOverride(Vector2 availableSizeWithoutMargins)
        {
            LayoutManager.DistributeHorizontally(availableSizeWithoutMargins, Controls);

            return availableSizeWithoutMargins;
        }            
    }
}