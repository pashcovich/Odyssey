using Odyssey.Engine;
using SharpDX.Direct2D1;

namespace Odyssey.Graphics.Shapes
{
    public abstract class PathGeometryBase : Geometry
    {
        protected readonly new PathGeometry Resource;

        protected PathGeometryBase(Direct2DDevice device)
            : base(device)
        {
            Resource = new PathGeometry(device);
        }

        protected GeometrySink DefineFigure()
        {
            return new GeometrySink(Resource.Open());
        }

        /// <summary>
        /// <see cref="SharpDX.Direct2D1.PathGeometry"/> casting operator.
        /// </summary>
        /// <param name="from">From the PathGeometry.</param>
        public static implicit operator SharpDX.Direct2D1.PathGeometry(PathGeometryBase from)
        {
            // Don't bother with multithreading here
            return from == null ? null : from.Resource ?? null;
        }
    }
}