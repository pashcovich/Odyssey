﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SharpDX.Mathematics;
using Real = System.Single;
using Point = SharpDX.Mathematics.Vector3;

namespace Odyssey.Geometry.Primitives
{
    public struct Triangle3D
    {
        public Point Vertex1;
        public Point Vertex2;
        public Point Vertex3;

        public Triangle3D(Point v1, Point v2, Point v3)
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

        public Point CalculateNormal()
        {
            Point p1 = Vertex2 - Vertex1;
            Point p2 = Vertex3 - Vertex1;

            return Point.Normalize(Point.Cross(p1, p2));
        }

        public static bool FindOverlappingEdges(Triangle3D t1, Triangle3D t2, Face face1, Face face2, out UndirectedEdge edge1,
            out UndirectedEdge edge2)
        {
            Contract.Requires<ArgumentNullException>(face1.Edges != null, "face1");
            Contract.Requires<ArgumentNullException>(face2.Edges != null, "face2");
            var tolerance = new Point(MathUtil.ZeroTolerance);

            var segments1 = t1.Segments.ToArray();
            var segments2 = t2.Segments.ToArray();
            for (int i = 0; i < 3; i++)
            {
                Segment s1 = segments1[i];

                for (int j = 0; j < 3; j++)
                {
                    Segment s2 = segments2[j];

                    Point e1v1 = s1.A;
                    Point e1v2 = s1.B;
                    Point e2v1 = s2.A;
                    Point e2v2 = s2.B;

                    bool test1 = Point.NearEqual(e1v1, e2v1, tolerance) || Point.NearEqual(e1v1, e2v2, tolerance);
                    bool test2 = Point.NearEqual(e1v2, e2v1, tolerance) || Point.NearEqual(e1v2, e2v2, tolerance);

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