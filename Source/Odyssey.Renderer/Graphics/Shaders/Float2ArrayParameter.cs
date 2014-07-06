using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            int debug;
            for (int i = 0; i < data.Length; i ++)
            {
                vectorList.Add(new Vector4(data[i], 0, 0));
                //vectorList.Add(new Vector4(data[i].X, 11, 12,13));
                //vectorList.Add(new Vector4(data[i].Y, 21,22, 23));
                //vectorList.Add(new Vector4(30, 31, 32, 33));
                //vectorList.Add(new Vector4(40,41,42,43));
                //vectorList.Add(new Vector4(data[i].X, 0, 0, 0));
                //vectorList.Add(new Vector4(data[i].Y, 0, 0, 0));
                //vectorList.Add(Vector4.Zero);
                //vectorList.Add(Vector4.Zero);
            }
            vectorList[14] = new Vector4(vectorList[14].X, vectorList[14].Y, 100, 100);
            return vectorList.ToArray();
            //var vectorList = new List<Vector4>();
            //float x = 0, y = 0, z = 0, w = 0;
            //Vector2[] values = Method();
            //int count = 0;
            //for (int i = 0; i < Length; i ++)
            //{
            //    if (i%4 == 0)
            //        x = values[i].X;
            //    else if (i%4 == 1)
            //        y = values[i].Y;
            //    else if (i%4 == 2)
            //        z = values[i].X;
            //    else w = values[i].Y;

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
