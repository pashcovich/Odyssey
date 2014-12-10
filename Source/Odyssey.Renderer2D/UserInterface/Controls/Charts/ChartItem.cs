namespace Odyssey.UserInterface.Controls.Charts
{
    public abstract class ChartItem : VisualElement
    {
        private Chart chart;

        protected ChartItem(string controlStyleClass) : base(controlStyleClass)
        {
        }

        protected Chart Chart
        {
            get { return chart ?? (chart = FindAncestor<Chart>()); }
        }

        public abstract float Value { get; set; }
    }
}