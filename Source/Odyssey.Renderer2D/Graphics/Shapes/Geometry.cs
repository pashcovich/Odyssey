using Odyssey.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Shapes
{
    public abstract class Geometry : Direct2DResource
    {
        protected Geometry(Direct2DDevice device)
            : base(device)
        {
        }

        /// <summary>
        /// <see cref="SharpDX.Direct2D1.Geometry"/> casting operator.
        /// </summary>
        /// <param name="from">From the Texture1D.</param>
        public static implicit operator SharpDX.Direct2D1.Geometry(Geometry from)
        {
            // Don't bother with multithreading here
            return from == null ? null : (SharpDX.Direct2D1.Geometry)from.Resource ?? null;
        }
    }
}