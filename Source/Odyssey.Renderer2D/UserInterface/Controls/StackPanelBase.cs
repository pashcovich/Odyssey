#region Using Directives

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

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

        protected override Vector2 MeasureOverride(Vector2 availableSizeWithoutMargins)
        {
            int sumIndex = Orientation == Orientation.Horizontal ? 0 : 1;
            int maximizeIndex = Orientation == Orientation.Horizontal ? 1 : 0;
            var desiredSize = Vector2.Zero;

            foreach (var control in Controls.Public)
            {
                control.Measure(availableSizeWithoutMargins);
                desiredSize[sumIndex] += control.DesiredSizeWithMargins[sumIndex];
                desiredSize[maximizeIndex] = Math.Max(desiredSize[maximizeIndex], control.DesiredSizeWithMargins[maximizeIndex]);
            }
            return desiredSize;
        }

        protected override Vector2 ArrangeOverride(Vector2 availableSizeWithoutMargins)
        {
            if (Orientation == Orientation.Horizontal)
                LayoutManager.DistributeHorizontally(availableSizeWithoutMargins, Controls);
            else
                LayoutManager.DistributeVertically(availableSizeWithoutMargins, Controls);

            return availableSizeWithoutMargins;
        }

    }
}