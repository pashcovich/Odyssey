using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Controls.Charts
{
    public abstract class Chart : ItemsControl
    {
        internal const string IdXAxisTitle = "XAxisTitle";
        internal const string IdChartArea = "ChartArea";

        protected internal UIElement XAxisTitle { get; internal set; }
        protected internal UIElement ChartArea { get; internal set; }
        
        protected Chart(string controlStyleClass, string textStyleClass = TextStyle.Default)
            : base(controlStyleClass, textStyleClass)
        {
        }

        public float MinimumValue { get; set; }
        public float MaximumValue { get; set; }
    }
}