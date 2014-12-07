#region Using Directives

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public class StackPanel : Panel
    {
        private Orientation orientation;

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

        protected internal override UIElement Copy()
        {
            var sp =(StackPanel)base.Copy();
            sp.orientation = Orientation;
            return sp;
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            int sumIndex = Orientation == Orientation.Horizontal ? 0 : 1;
            int maximizeIndex = Orientation == Orientation.Horizontal ? 1 : 0;
            var desiredSize = Vector3.Zero;

            foreach (var control in Children.Visual)
            {
                control.Measure(availableSizeWithoutMargins);
                desiredSize[sumIndex] += control.DesiredSizeWithMargins[sumIndex];
                desiredSize[maximizeIndex] = Math.Max(desiredSize[maximizeIndex], control.DesiredSizeWithMargins[maximizeIndex]);
            }
            return desiredSize;
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            if (Orientation == Orientation.Horizontal)
            {
                LayoutManager.DistributeHorizontally(availableSizeWithoutMargins, this);
            }
            else
            {
                LayoutManager.DistributeVertically(availableSizeWithoutMargins, this);
            }

            return availableSizeWithoutMargins;
        }

    }
}