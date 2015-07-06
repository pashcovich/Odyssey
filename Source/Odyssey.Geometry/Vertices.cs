using System.Collections.Generic;
using System.Linq;
using Odyssey.Geometry.Primitives;
using Real = System.Single;
using Point = SharpDX.Mathematics.Vector2;

namespace Odyssey.Geometry
{
    public class Vertices : IList<Point>
    {
        private readonly List<Point> vertices;

        public Vertices()
        {
            vertices = new List<Point>();
        }

        public Vertices(IEnumerable<Point> vertices)
            : this()
        {
            this.vertices.AddRange(vertices);
        }

        #region IList<Point> Members

        #region ICollection<Point> Members

        public void Add(Point item)
        {
            vertices.Add(item);
        }

        public void AddRange(IEnumerable<Point> points)
        {
            vertices.AddRange(points);
        }

        public void Clear()
        {
            vertices.Clear();
        }

        public bool Contains(Point item)
        {
            return vertices.Contains(item);
        }

        void ICollection<Point>.CopyTo(Point[] array, int arrayIndex)
        {
            vertices.CopyTo(array, arrayIndex);
        }

        bool ICollection<Point>.IsReadOnly
        {
            get { return false; }
        }

        public int Count
        {
            get { return vertices.Count; }
        }

        public bool Remove(Point item)
        {
            if (!vertices.Contains(item))
                return false;

            vertices.Remove(item);
            return true;
        }

        #endregion ICollection<Point> Members

        #region IEnumerable<Point> Members

        public IEnumerator<Point> GetEnumerator()
        {
            return vertices.GetEnumerator();
        }

        #endregion IEnumerable<Point> Members

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return vertices.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region IList<Point> Members

        int IList<Point>.IndexOf(Point item)
        {
            return vertices.IndexOf(item);
        }

        public void Insert(int index, Point item)
        {
            vertices.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            vertices.RemoveAt(index);
        }

        public int IndexOf(Point index)
        {
            return vertices.IndexOf(index);
        }

        public Point this[int index]
        {
            get
            {
                return vertices[index];
            }
            set
            {
                vertices[index] = value;
            }
        }

        #endregion IList<Point> Members

        #endregion IList<Point> Members

        /// <summary>
        /// Advances the index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public int NextIndex(int index)
        {
            if (index == Count - 1)
            {
                return 0;
            }
            return index + 1;
        }

        /// <summary>
        /// Gets the previous index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public int PreviousIndex(int index)
        {
            if (index == 0)
            {
                return Count - 1;
            }
            return index - 1;
        }

        public Point NextVertex(int index)
        {
            return this[NextIndex(index)];
        }

        public void Reverse()
        {
            vertices.Reverse();
        }

        public static explicit operator Polygon(Vertices vertices)
        {
            return new Polygon(vertices);
        }

        public static implicit operator Point[](Vertices vertices)
        {
            return vertices.ToArray();
        }
    }
}