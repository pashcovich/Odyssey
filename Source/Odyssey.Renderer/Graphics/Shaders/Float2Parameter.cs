using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public class Float2Parameter : Parameter<Vector2>
    {
        public Float2Parameter(int index, string handle, RetrieveParameter<Vector2> method)
            : base(index, handle)
        {
            Method = method;
        }

        public override int Size
        {
            get { return 8; }
        }

        public override Vector4[] ToArray()
        {
            return new[] { new Vector4(Method(), 0, 0) };
        }
    }
}