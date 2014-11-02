using Odyssey.Geometry;

namespace Odyssey.UserInterface.Controls.Charts
{
    public abstract class ChartItem : Control
    {
        private float actualValue;
        internal Chart Chart { get; set; }

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

                Height = ItemHeight(Chart.ClientAreaHeight, Chart.MaximumValue, actualValue);
            }
        }

        static float ItemHeight(float chartAreaHeight, float maximumValue, float value)
        {
            return (value/maximumValue)*chartAreaHeight;
        }
    }
}
