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

using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.UserInterface.Style;

#endregion Using Directives

namespace Odyssey.Graphics.Drawing
{
    public class PathGeometry : Geometry
    {
        protected new readonly SharpDX.Direct2D1.PathGeometry Resource;

        protected PathGeometry(string name, Direct2DDevice device)
            : base(name, device)
        {
            Resource = new SharpDX.Direct2D1.PathGeometry(device);
        }

        /// <summary>
        /// <see cref="SharpDX.Direct2D1.PathGeometry"/> casting operator.
        /// </summary>
        /// <param name="from">From the PathGeometry.</param>
        public static implicit operator SharpDX.Direct2D1.PathGeometry(PathGeometry from)
        {
            // Don't bother with multithreading here
            return from == null ? null : from.Resource ?? null;
        }

        public static PathGeometry New(string name, Direct2DDevice device)
        {
            return new PathGeometry(name, device);
        }

        public override void Initialize()
        {
            Initialize(Resource);
        }

        public void RunCommands(IEnumerable<VectorCommand> commands)
        {
            var sink = new GeometrySink(Resource.Open());
            var designer = new FigureParser(sink);
            designer.Execute(commands);
            if (designer.IsFigureOpen)
                sink.EndFigure(FigureEnd.Open);
            sink.Close();
        }

    }
}