using Odyssey.Geometry.Extensions;
using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public class MatrixParameter : Parameter<Matrix>
    {

        public MatrixParameter(int index, string handle, RetrieveParameter<Matrix> method)
            : base(index, handle)
        {
            Method = method;
        }


        public override int Size
        {
            get { return 64; }
        }

        public override Vector4[] ToArray()
        {
            Matrix matrix = Matrix.Transpose(Method());
            return matrix.ToVector4Array();
        }
    }
}