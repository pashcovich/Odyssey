using System;
using System.Linq;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls
{
    public class UniformStackPanel : StackPanelBase
    {
        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            int count = Controls.Public.Count();

            var size = Orientation == Orientation.Horizontal
                ? new Vector3(availableSizeWithoutMargins.X/count, availableSizeWithoutMargins.Y, availableSizeWithoutMargins.Z)
                : new Vector3(availableSizeWithoutMargins.X, availableSizeWithoutMargins.Y/count, availableSizeWithoutMargins.Z);

            int sumIndex = Orientation == Orientation.Horizontal ? 0 : 1;
            int maximizeIndex = Orientation == Orientation.Horizontal ? 1 : 0;
            var desiredSize = Vector3.Zero;

            foreach (var control in Controls.Public)
            {
                control.Measure(size);
                desiredSize[sumIndex] += control.DesiredSizeWithMargins[sumIndex];
                desiredSize[maximizeIndex] = Math.Max(desiredSize[maximizeIndex], control.DesiredSizeWithMargins[maximizeIndex]);
            }
            return availableSizeWithoutMargins;
        }
    }
}
