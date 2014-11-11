using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Shaders
{
    public class FloatArrayParameter : FloatArrayParameterBase<float>
    {
        public FloatArrayParameter(int index, int length, string handle, RetrieveParameter<float[]> method) : base(index, length, handle, method)
        {
        }

        public override int Size
        {
            get { return 16; }
        }

        public override Vector4[] ToArray()
        {
            return Method().Select(f => new Vector4(f, 1, 2, 3)).ToArray();
            //float x =0, y=0, z=0, w=0;
            //float[] values = Method();
            //int count = 0;
            //for (int i = 0; i < Length; i ++)
            //{
            //    if (i%4 == 0)
            //        x = values[i];
            //    else if (i%4 == 1)
            //        y = values[i];
            //    else if (i%4 == 2)
            //        z = values[i];
            //    else w = values[i];

            //    count++;
            //    if (count%4 == 0)
            //    {
            //        vectorList.Add(new Vector4(x,y,z,w));
            //        count = 0;
            //        x = y = z = w = 0;
            //    }
            //}
            //if (count > 0)
            //    vectorList.Add(new Vector4(x,y,z,w));

            //return vectorList.ToArray();
        }
    }
}
