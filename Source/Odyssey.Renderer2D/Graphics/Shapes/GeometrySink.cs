using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
#if !WP8


namespace Odyssey.Graphics.Shapes
{
    public class GeometrySink : Component
    {
        private bool isFigureOpened;
        private bool isFigureClosed;
        private readonly SharpDX.Direct2D1.GeometrySink geometrySink;

        internal GeometrySink(SharpDX.Direct2D1.GeometrySink sink)
        {
            geometrySink = ToDispose(sink);
        }

        

        public void BeginFigure(Vector2 point, FigureBegin figureBegin)
        {
            geometrySink.BeginFigure(point, GetD2DFigureFlag(figureBegin));
            isFigureOpened = true;
        }

        public void AddLine(Vector2 point)
        {
            geometrySink.AddLine(point);
        }

        public void AddLines(IEnumerable<Vector2> points)
        {
            geometrySink.AddLines(points.ToArray());
        }

        public void CloseFigure(FigureEnd figureEnd)
        {
            if (!isFigureOpened)
                throw new InvalidOperationException("Figure was not opened.");
            geometrySink.EndFigure(GetD2DFigureFlag(figureEnd));
            geometrySink.Close();
            geometrySink.Dispose();
            isFigureClosed = true;
        }


        static SharpDX.Direct2D1.FigureBegin GetD2DFigureFlag(FigureBegin figureBegin)
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

        static SharpDX.Direct2D1.FigureEnd GetD2DFigureFlag(FigureEnd figureEnd)
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
#endif
