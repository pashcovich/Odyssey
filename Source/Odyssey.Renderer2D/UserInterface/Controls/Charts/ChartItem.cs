using System;
using Odyssey.Geometry;

namespace Odyssey.UserInterface.Controls.Charts
{
    public abstract class ChartItem : Control
    {
        private float actualValue;
        private Chart chart;

        private Chart Chart
        {
            get
            {
                if (chart == null)
                    chart = FindAncestor<Chart>();
                return chart;
            }
        }

        protected ChartItem(string controlStyleClass) : base(controlStyleClass)
        { }

        public float Value
        {
            get { return actualValue; }
            set
            {
                if (actualValue == value)
                    return;
                actualValue = MathHelper.Clamp(value, Chart.MinimumValue, Chart.MaximumValue);

                Height = ItemHeight(Chart.ChartArea.Y, Chart.MaximumValue, actualValue);
            }
        }


        static float ItemHeight(float chartAreaHeight, float maximumValue, float value)
        {
            return (value/maximumValue)*chartAreaHeight;
        }
    }
}
