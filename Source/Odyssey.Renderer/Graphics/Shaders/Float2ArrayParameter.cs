using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public class Float2ArrayParameter : FloatArrayParameterBase<Vector2>
    {
        public Float2ArrayParameter(int index, int length, string handle, RetrieveParameter<Vector2[]> method) : base(index, length, handle, method)
        {
        }

        public override int Size
        {
            get { return 4 * 16; }
        }

        public override Vector4[] ToArray()
        {
            var data = Method().ToArray();
            var vectorList = new List<Vector4>();
            for (int i = 0; i < data.Length; i ++)
            {
                vectorList.Add(new Vector4(data[i], 0, 0));
            }
            vectorList[14] = new Vector4(vectorList[14].X, vectorList[14].Y, 100, 100);
            return vectorList.ToArray();
        }
    }
}
