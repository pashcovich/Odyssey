using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml;
using Odyssey.Serialization;
using Odyssey.Utilities.Text;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public class LinearGradient : Gradient
    {
        private Vector2 startPoint;
        private Vector2 endPoint;

        public Vector2 StartPoint
        {
            get { return startPoint; }
        }

        public Vector2 EndPoint
        {
            get { return endPoint; }
        }

        public LinearGradient()
        { }

        public LinearGradient(string name, Vector2 startPoint, Vector2 endPoint, GradientStopCollection gradientStops) : base(name, gradientStops, GradientType.Linear)
        {
            Contract.Requires<ArgumentNullException>(gradientStops != null, "gradientStops");
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public static LinearGradient Vertical(string name, IEnumerable<GradientStop> gradientStops)
        {
            return new LinearGradient(name, new Vector2(0.5f, 0), new Vector2(0.5f, 1),new GradientStopCollection(gradientStops));
        }

        public static LinearGradient Horizontal(string name, IEnumerable<GradientStop> gradientStops)
        {
            return new LinearGradient(name, new Vector2(0f, 0.5f), new Vector2(1.0f, 0.5f), new GradientStopCollection(gradientStops));
        }

        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            var reader = e.XmlReader;
            Type = GradientType.Linear;
            string sStart = reader.GetAttribute("StartPoint");
            string sEnd = reader.GetAttribute("EndPoint");
            startPoint = string.IsNullOrEmpty(sStart) ? Vector2.Zero : Text.DecodeFloatVector2(sStart);
            endPoint = string.IsNullOrEmpty(sEnd) ? Vector2.Zero : Text.DecodeFloatVector2(sEnd);
            base.OnReadXml(e);
        }

        internal override Gradient Copy()
        {
            return new LinearGradient(Name, startPoint, endPoint, GradientStops);
        }
    }
}
