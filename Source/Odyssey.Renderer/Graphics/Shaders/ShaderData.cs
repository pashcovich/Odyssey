using System.Runtime.InteropServices;
using SharpDX;

namespace Odyssey.Graphics.Shaders
{

    [StructLayout(LayoutKind.Sequential)]
    public struct ShaderData
    {
        public Vector4[] Array { get; private set; }

        public ShaderData(params Vector4[] fields)
            : this()
        {
            Array = fields;
        }

    }
}