using Odyssey.Geometry;

namespace Odyssey.UserInterface.Controls.Charts
{
    public abstract class ChartItem : Control
    {
        private float actualValue;

        protected ChartItem(string controlStyleClass) : base(controlStyleClass)
        { }

        public float Value
        {
            get { return actualValue; }
            set
            {
                if (actualValue == value)
                    return;
                var chart = (Chart) Parent;
                actualValue = MathHelper.Clamp(value, chart.MinimumValue, chart.MaximumValue);

                Height = ItemHeight(chart.ClientAreaHeight, chart.MaximumValue, actualValue);
            }
        }

        static float ItemHeight(float chartAreaHeight, float maximumValue, float value)
        {
            return (value/maximumValue)*chartAreaHeight;
        }
    }
}
