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

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            LayoutManager.DistributeHorizontally(availableSizeWithoutMargins, Controls);

            return availableSizeWithoutMargins;
        }            
    }
}