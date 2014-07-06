using SharpDX;

#if !WP8

using Odyssey.Engine;

namespace Odyssey.Graphics.Shapes
{
    public abstract class Brush : Direct2DResource
    {
        protected new readonly SharpDX.Direct2D1.Brush Resource;

        protected Brush(Direct2DDevice device, SharpDX.Direct2D1.Brush brush)
            : base(device)
        {
            Resource = brush;
        }

        /// <summary>
        /// <see cref="SharpDX.Direct2D1.Brush"/> casting operator.
        /// </summary>
        /// <param name="from">From the Texture1D.</param>
        public static implicit operator SharpDX.Direct2D1.Brush(Brush from)
        {
            // Don't bother with multithreading here
            return from == null ? null : from.Resource ?? null;
        }
    }
}

#endif