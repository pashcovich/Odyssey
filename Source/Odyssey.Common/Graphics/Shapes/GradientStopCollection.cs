using System.Collections;
using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.Geometry;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public class GradientStopCollection : IEnumerable<GradientStop>
    {
        private readonly List<GradientStop> gradientStops;

        public GradientStopCollection()
        {
            gradientStops = new List<GradientStop>();
        }

        public GradientStopCollection(IEnumerable<GradientStop> gradientStops)
        {
            this.gradientStops = new List<GradientStop>(gradientStops);
        }

        public ExtendMode ExtendMode { get; set; }

        public int Count { get { return gradientStops.Count; } }

        void FindStops(float offset, out GradientStop from, out GradientStop to)
        {
            from = gradientStops.FindLast(gs => gs.Offset <= offset);
            to = gradientStops.Find(gs => gs.Offset > offset) ?? gradientStops.Find(gs=> MathUtil.NearEqual(gs.Offset,offset));
        }

        public Color4 Evaluate(float offset)
        {
            GradientStop gsFrom;
            GradientStop gsTo;
            FindStops(offset, out gsFrom, out gsTo);
            
            return Color4.Lerp(gsFrom.Color, gsTo.Color, offset * (gsTo.Offset -gsFrom.Offset));
        }


        public IEnumerator<GradientStop> GetEnumerator()
        {
            return gradientStops.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public GradientStop this[int index]
        {
            get { return gradientStops[index]; }
        }


    }
}
