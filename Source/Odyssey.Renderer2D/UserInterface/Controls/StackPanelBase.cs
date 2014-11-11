#region Using Directives

using System;
using Odyssey.UserInterface.Style;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public abstract class StackPanelBase : ItemsControl
    {
        private const string ControlTag = "StackPanel";
        private Orientation orientation;

        protected StackPanelBase()
            : base("Panel")
        {
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    OnLayoutUpdated(EventArgs.Empty);
                }
            }
        }

        protected override void Arrange()
        {
            if (Controls.IsEmpty)
                return;

            if (Orientation == Orientation.Horizontal)
                LayoutManager.DistributeHorizontally(this, Controls);
            else
                LayoutManager.DistributeVertically(this, Controls);
        }
    }
}