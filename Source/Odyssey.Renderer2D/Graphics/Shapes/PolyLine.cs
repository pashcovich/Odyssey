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

using Odyssey.Engine;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion Using Directives

namespace Odyssey.Graphics.Shapes
{
    public class PolyLine : PathGeometry
    {
        internal PolyLine(Direct2DDevice device)
            : base(device)
        {
            Initialize(Resource);
        }

        public static PolyLine New(Direct2DDevice device, IEnumerable<Vector2> points, FigureBegin figureBegin,
            FigureEnd figureEnd)
        {
            Contract.Requires<ArgumentNullException>(points != null);
            Contract.Requires<ArgumentException>(points.Count() >= 2, "Figure needs at least two points.");

            var array = points as Vector2[] ?? points.ToArray();
            PolyLine polyLine = new PolyLine(device);
            var sink = polyLine.DefineFigure();
            sink.BeginFigure(array[0], figureBegin);
            sink.AddLines(array.Skip(1));
            sink.EndFigure(figureEnd);
            sink.Close();
            return polyLine;
        }
    }
}