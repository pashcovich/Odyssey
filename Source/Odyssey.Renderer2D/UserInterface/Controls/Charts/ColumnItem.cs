using Odyssey.Geometry;

namespace Odyssey.UserInterface.Controls.Charts
{
    public class ColumnItem : ChartItem
    {
        private float actualValue;

        public ColumnItem() : base("ColumnItem")
        {
        }

        public override float Value
        {
            get { return actualValue; }
            set
            {
                if (actualValue == value)
                    return;
                actualValue = MathHelper.Clamp(value, Chart.MinimumValue, Chart.MaximumValue);

                Height = ItemHeight(Chart.ChartArea.Y, Chart.MaximumValue, Value);
            }
        }

        static float ItemHeight(float chartAreaHeight, float maximumValue, float value)
        {
            return (value / maximumValue) * chartAreaHeight;
        }
    }
}
