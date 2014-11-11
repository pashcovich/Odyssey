using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Controls
{
    public abstract class ListBoxBase : ItemsControl
    {
        private const string ControlTag = "ListBox";

        protected ListBoxBase()
            : base("Panel")
        {
        }

        protected override void Arrange()
        {
            base.Arrange();
            if (Controls.IsEmpty)
                return;

            LayoutManager.DistributeHorizontally(this, Controls);
        }
    }
}