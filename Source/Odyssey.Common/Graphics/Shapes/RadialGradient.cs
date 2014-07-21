using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public class RadialGradient : GradientBase
    {
        private readonly Vector2 center;
        private readonly Vector2 originOffset;
        private readonly float radiusX;
        private readonly float radiusY;

        public Vector2 Center
        {
            get { return center; }
        }

        public Vector2 OriginOffset
        {
            get { return originOffset; }
        }

        public float RadiusX
        {
            get { return radiusX; }
        }

        public float RadiusY
        {
            get { return radiusY; }
        }

        public RadialGradient(string name, Vector2 center, Vector2 originOffset, float radiusX, float radiusY, GradientStopCollection gradientStops) : base(name, gradientStops, GradientType.Radial)
        {
            this.center = center;
            this.originOffset = originOffset;
            this.radiusX = radiusX;
            this.radiusY = radiusY;
        }

        public static RadialGradient New(string name, Vector2 center, Vector2 originOffset, float radiusX, float radiusY, IEnumerable<GradientStop> gradientStops)
        {
            return new RadialGradient(name, center, originOffset, radiusX, radiusY, new GradientStopCollection(gradientStops));
        }

        public static RadialGradient New(string name, IEnumerable<GradientStop> gradientStops)
        {
            return new RadialGradient(name, Vector2.Zero, Vector2.Zero, 1, 1, new GradientStopCollection(gradientStops));
        }
    }
}
