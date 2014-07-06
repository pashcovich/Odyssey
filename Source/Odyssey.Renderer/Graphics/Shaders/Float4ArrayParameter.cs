using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public class Float4ArrayParameter : FloatArrayParameterBase<Vector4>
    {
        public Float4ArrayParameter(int index, int length, string handle, RetrieveParameter<Vector4[]> method)
            : base(index, length, handle, method)
        {
        }

        public override int Size
        {
            get { return 16; }
        }

        public override Vector4[] ToArray()
        {
            return Method();
        }
    }
}