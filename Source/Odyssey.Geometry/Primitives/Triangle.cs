using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics;
using SharpDX.Mathematics;

namespace Odyssey.Geometry.Primitives
{
    public struct Triangle
    {
        public Vector3 Vertex1;
        public Vector3 Vertex2;
        public Vector3 Vertex3;

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vertex1 = v1;
            Vertex2 = v2;
            Vertex3 = v3;
        }

        public IEnumerable<Segment> Segments
        {
            get
            {
                return new[]
                {
                    new Segment(Vertex1, Vertex2),
                    new Segment(Vertex2, Vertex3),
                    new Segment(Vertex3, Vertex1),
                };
            }
        }

        public Vector3 CalculateNormal()
        {
            Vector3 p1 = Vertex2 - Vertex1;
            Vector3 p2 = Vertex3 - Vertex1;

            return Vector3.Normalize(Vector3.Cross(p1, p2));
        }

        public static bool FindOverlappingEdges(Triangle t1, Triangle t2, Face face1, Face face2, out UndirectedEdge edge1,
            out UndirectedEdge edge2)
        {
            Contract.Requires<ArgumentNullException>(face1.Edges != null, "face1");
            Contract.Requires<ArgumentNullException>(face2.Edges != null, "face2");
            Vector3 tolerance = new Vector3(MathUtil.ZeroTolerance);

            var segments1 = t1.Segments.ToArray();
            var segments2 = t2.Segments.ToArray();
            for (int i = 0; i < 3; i++)
            {
                Segment s1 = segments1[i];

                for (int j = 0; j < 3; j++)
                {
                    Segment s2 = segments2[j];

                    Vector3 e1v1 = s1.A;
                    Vector3 e1v2 = s1.B;
                    Vector3 e2v1 = s2.A;
                    Vector3 e2v2 = s2.B;

                    bool test1 = Vector3.NearEqual(e1v1, e2v1, tolerance) || Vector3.NearEqual(e1v1, e2v2, tolerance);
                    bool test2 = Vector3.NearEqual(e1v2, e2v1, tolerance) || Vector3.NearEqual(e1v2, e2v2, tolerance);

                    if (test1 && test2)
                    {
                        var edges1 = face1.Edges.ToArray();
                        var edges2 = face2.Edges.ToArray();
                        edge1 = edges1[i];
                        edge2 = edges2[j];
                        return true;
                    }
                }
            }
            edge1 = edge2 = default(UndirectedEdge);
            return false;
        }

    }
    
}
