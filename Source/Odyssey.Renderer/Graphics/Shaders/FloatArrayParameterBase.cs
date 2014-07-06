using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public abstract class FloatArrayParameterBase<T> :Parameter<T[]>
    {
        private readonly int length;

        protected int Length { get { return length; } }

        protected FloatArrayParameterBase(int index, int length, string handle, RetrieveParameter<T[]> method) : base(index, handle)
        {
            Contract.Requires<ArgumentException>(length >0, "length");
            this.length = length;
            Method = method;
        }


    }
}
