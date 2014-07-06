using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Odyssey.Content.Shaders
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