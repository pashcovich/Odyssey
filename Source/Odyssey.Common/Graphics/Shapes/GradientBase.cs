namespace Odyssey.Graphics.Shapes
{
    public abstract class GradientBase : IGradient
    {
        private readonly string name;
        private readonly GradientType type;

        public string Name
        {
            get { return name; }
        }

        public GradientType Type { get { return type; } }
        public GradientStopCollection GradientStops { get; private set; }

        protected GradientBase(string name, GradientStopCollection gradientStops, GradientType type)
        {
            this.name = name;
            GradientStops = gradientStops;
            this.type = type;
        }
    }
}