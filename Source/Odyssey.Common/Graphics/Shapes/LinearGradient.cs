using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public class LinearGradient : GradientBase
    {
        private readonly Vector2 startPoint;
        private readonly Vector2 endPoint;

        public Vector2 StartPoint
        {
            get { return startPoint; }
        }

        public Vector2 EndPoint
        {
            get { return endPoint; }
        }

        public LinearGradient(string name, Vector2 startPoint, Vector2 endPoint, GradientStopCollection gradientStops) : base(name, gradientStops, GradientType.Linear)
        {
            Contract.Requires<ArgumentNullException>(gradientStops != null, "gradientStops");
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public static LinearGradient CreateUniform(Color4 color)
        {
            return CreateVertical("DefaultUniform", new[] { new GradientStop(color, 0), new GradientStop(color, 1.0f) });
        }

        public static LinearGradient CreateVertical(string name, IEnumerable<GradientStop> gradientStops)
        {
            return new LinearGradient(name, new Vector2(0.5f, 0), new Vector2(0.5f, 1),new GradientStopCollection(gradientStops));
        }

        public static LinearGradient Horizontal(string name, IEnumerable<GradientStop> gradientStops)
        {
            return new LinearGradient(name, new Vector2(0f, 0.5f), new Vector2(1.0f, 0.5f), new GradientStopCollection(gradientStops));
        }
      
    }
}
