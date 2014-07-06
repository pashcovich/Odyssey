using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public class Float3Parameter : Parameter<Vector3>
    {
        public Float3Parameter(int index, string handle, RetrieveParameter<Vector3> method)
            : base(index, handle)
        {
            Method = method;
        }

        public override int Size
        {
            get { return 12; }
        }

        public override Vector4[] ToArray()
        {
            return new[] { new Vector4(Method(), 0) };
        }
    }
}