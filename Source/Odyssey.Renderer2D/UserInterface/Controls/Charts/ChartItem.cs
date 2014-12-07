using System;
using Odyssey.Geometry;

namespace Odyssey.UserInterface.Controls.Charts
{
    public abstract class ChartItem : Control
    {
        private Chart chart;

        protected Chart Chart
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

        public abstract float Value { get; set; }

    }
}
