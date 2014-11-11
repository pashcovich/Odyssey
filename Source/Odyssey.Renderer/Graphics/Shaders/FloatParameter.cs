using SharpDX.Mathematics;

namespace Odyssey.Graphics.Shaders
{
    public class FloatParameter : Parameter<float>
    {

        public FloatParameter(int index, string handle, RetrieveParameter<float> method)
            : base(index, handle)
        {
            Method = method;
        }

        public override int Size
        {
            get { return 4; }
        }

        public override Vector4[] ToArray()
        {
            return new[] { new Vector4(Method(), 0, 0, 0) };
        }
    }
}