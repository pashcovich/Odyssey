using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SharpDX;

namespace Odyssey.Graphics
{
    public class GradientStopCollection : IEnumerable<GradientStop>, IEquatable<GradientStopCollection>
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

        public void Add(GradientStop gradientStop)
        {
            Contract.Requires<ArgumentNullException>(gradientStop != null, "gradientStop");
            gradientStops.Add(gradientStop);
            gradientStop.Index = gradientStops.Count - 1;
        }
        
        public void AddRange(IEnumerable<GradientStop> gradientStops)
        {
            Contract.Requires<ArgumentNullException>(gradientStops != null, "gradientStops");
            foreach (var gradientStop in gradientStops)
            {
                Add(gradientStop);
            }
        }

        public void Clear()
        {
            gradientStops.Clear();
        }

        public Color4 Evaluate(float offset)
        {
            GradientStop gsFrom;
            GradientStop gsTo;
            FindStops(offset, out gsFrom, out gsTo);
            
            return Color4.Lerp(gsFrom.Color, gsTo.Color, offset * (gsTo.Offset -gsFrom.Offset));
        }

        internal GradientStopCollection Copy()
        {
            GradientStopCollection collection = new GradientStopCollection();
            foreach (var gradientStop in gradientStops)
            {
                collection.Add(new GradientStop(gradientStop.Color, gradientStop.Offset));
            }
            return collection;
        }


        #region IEnumerable<GradientStop>
        public IEnumerator<GradientStop> GetEnumerator()
        {
            return gradientStops.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        } 
        #endregion

        public GradientStop this[int index]
        {
            get { return gradientStops[index]; }
        }


        #region IEquatable<GradientStopCollection>
        public bool Equals(GradientStopCollection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(gradientStops, other.gradientStops) && ExtendMode == other.ExtendMode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GradientStopCollection)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((gradientStops != null ? gradientStops.GetHashCode() : 0) * 397);
            }
        }

        public static bool operator ==(GradientStopCollection left, GradientStopCollection right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GradientStopCollection left, GradientStopCollection right)
        {
            return !Equals(left, right);
        } 
        #endregion
    }
}
