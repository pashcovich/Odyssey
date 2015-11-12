#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
//
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
//
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion License

#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Core;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX;
using SharpDX.Mathematics.Interop;

#endregion

namespace Odyssey.Graphics.Drawing
{
    public class GeometrySink : Component
    {
        private readonly SharpDX.Direct2D1.GeometrySink geometrySink;
        private bool isFigureOpened;

        internal GeometrySink(SharpDX.Direct2D1.GeometrySink sink)
        {
            geometrySink = ToDispose(sink);
        }

        public void AddLine(Vector2 point)
        {
            geometrySink.AddLine(point);
        }

        public void AddLines(IEnumerable<Vector2> points)
        {
            geometrySink.AddLines(points.Select(p => new RawVector2 {X = p.X, Y = p.Y}).ToArray());
        }

        public void AddArc(float width, float height, float angleDegrees, bool isLargeArc, bool sweepDirection,
            Vector2 endPoint)
        {
            geometrySink.AddArc(new ArcSegment
            {
                Size = new Size2F(width, height),
                RotationAngle = angleDegrees,
                ArcSize = isLargeArc ? ArcSize.Large : ArcSize.Small,
                SweepDirection = sweepDirection ? SweepDirection.Clockwise : SweepDirection.CounterClockwise,
                Point = endPoint
            });
        }

        public void AddCubicBezierCurve(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            geometrySink.AddBezier(new BezierSegment
            {
                Point1 = p1,
                Point2 = p2,
                Point3 = p3
            });
        }

        public void BeginFigure(Vector2 point, FigureBegin figureBegin)
        {
            isFigureOpened = true;
            geometrySink.BeginFigure(point, GetD2DFigureFlag(figureBegin));
        }

        public void Close()
        {
            geometrySink.Close();
            geometrySink.Dispose();
        }

        public void EndFigure(FigureEnd figureEnd)
        {
            if (!isFigureOpened)
                throw new InvalidOperationException("Figure was not opened.");
            geometrySink.EndFigure(GetD2DFigureFlag(figureEnd));
            isFigureOpened = false;
        }

        private static SharpDX.Direct2D1.FigureBegin GetD2DFigureFlag(FigureBegin figureBegin)
        {
            switch (figureBegin)
            {
                case FigureBegin.Filled:
                    return SharpDX.Direct2D1.FigureBegin.Filled;

                case FigureBegin.Hollow:
                    return SharpDX.Direct2D1.FigureBegin.Hollow;

                default:
                    throw new ArgumentOutOfRangeException("figureBegin");
            }
        }

        private static SharpDX.Direct2D1.FigureEnd GetD2DFigureFlag(FigureEnd figureEnd)
        {
            switch (figureEnd)
            {
                case FigureEnd.Open:
                    return SharpDX.Direct2D1.FigureEnd.Open;

                case FigureEnd.Closed:
                    return SharpDX.Direct2D1.FigureEnd.Closed;

                default:
                    throw new ArgumentOutOfRangeException("figureEnd");
            }
        }
    }
}