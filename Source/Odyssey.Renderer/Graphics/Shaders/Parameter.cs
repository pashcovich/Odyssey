using System.Diagnostics;
using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public delegate T RetrieveParameter<out T>();

    [DebuggerDisplay("[{Index}] Name = {ParamHandle}")]
    public abstract class Parameter<T> : IParameter
    {
        protected internal RetrieveParameter<T> Method { get; set; }
        readonly string paramHandle;
        readonly int index;

        public string ParamHandle { get { return paramHandle; } }
        public T Value { get { return Method(); } }
        public int Index { get { return index; } }
        public abstract int Size { get; }

        protected Parameter(int index, string handle)
        {
            paramHandle = handle;
            this.index = index;
        }

        public abstract Vector4[] ToArray();
    }
}