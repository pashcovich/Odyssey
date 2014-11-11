using SharpDX.Mathematics;

namespace Odyssey.Graphics.Shaders
{
    public class Float4Parameter : Parameter<Vector4>
    {
        public Float4Parameter(int index, string handle, RetrieveParameter<Vector4> method)
            : base(index, handle)
        {
            Method = method;
        }

        public override int Size
        {
            get { return 16; }
        }

        public override Vector4[] ToArray()
        {
            return new[] { Method() };
        }
    }
}