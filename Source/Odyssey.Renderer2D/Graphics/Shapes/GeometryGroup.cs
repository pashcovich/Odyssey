using Odyssey.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

#if !WP8

namespace Odyssey.Graphics.Shapes
{
    public class GeometryGroup : Geometry
    {
        protected new readonly SharpDX.Direct2D1.GeometryGroup Resource;

        protected GeometryGroup(Direct2DDevice device, FillMode fillMode, SharpDX.Direct2D1.Geometry[] geometries)
            : base(device)
        {
            Contract.Requires<InvalidOperationException>(geometries.Any(), "At least one geometry needed.");
            Resource = new SharpDX.Direct2D1.GeometryGroup(Device, GetD2DFlag(fillMode), geometries.ToArray());
        }

        private static SharpDX.Direct2D1.FillMode GetD2DFlag(FillMode fillMode)
        {
            switch (fillMode)
            {
                case FillMode.Alternate:
                    return SharpDX.Direct2D1.FillMode.Alternate;

                case FillMode.Winding:
                    return SharpDX.Direct2D1.FillMode.Winding;

                default:
                    throw new ArgumentOutOfRangeException("fillMode");
            }
        }

        /// <summary>
        /// <see cref="SharpDX.Direct2D1.GeometryGroup"/> casting operator.
        /// </summary>
        /// <param name="from">From the Texture1D.</param>
        public static implicit operator SharpDX.Direct2D1.GeometryGroup(GeometryGroup from)
        {
            // Don't bother with multithreading here
            return from == null ? null : from.Resource ?? null;
        }

        public static GeometryGroup New(Direct2DDevice device, FillMode fillMode, IEnumerable<Geometry> geometries)
        {
            var nativeGeometries = geometries.Select(geometry => (SharpDX.Direct2D1.Geometry)geometry).ToArray();
            return new GeometryGroup(device, fillMode, nativeGeometries);
        }
    }
}

#endif