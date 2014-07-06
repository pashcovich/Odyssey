#region Using Directives

using System;

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
            if (Children.IsEmpty)
                return;

            if (Orientation == Orientation.Horizontal)
                Style.Layout.UpdateLayoutHorizontal(this, Children);
            else
                Style.Layout.UpdateLayoutVertical(this, Children);
        }
    }
}