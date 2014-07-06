using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;

namespace Odyssey.Graphics.Shaders
{
    public class VertexShader : Shader
    {
        protected readonly new SharpDX.Direct3D11.VertexShader Resource;

        public VertexShader(DirectXDevice device, string name, byte[] byteCode) : base(device, ShaderType.Vertex, name)
        {
            Resource = new SharpDX.Direct3D11.VertexShader(device, byteCode);
        }

        public override void Initialize()
        {
            Initialize(Resource);
            Resource.DebugName = Name;
        }

        /// <summary>
        /// <see cref="SharpDX.Direct3D11.VertexShader"/> casting operator.
        /// </summary>
        /// <param name="from">From the VertexShader.</param>
        public static implicit operator SharpDX.Direct3D11.VertexShader(VertexShader from)
        {
            return from.Resource;
        }

        public override void Apply(string technique, UpdateType updateType)
        {
            foreach (ConstantBuffer cb in SelectBuffers(technique, updateType))
                Device.SetVertexShaderConstantBuffer(cb.Index, cb.Buffer);
            Device.SetShader(this);
        }

        public override void Apply(string technique, long id, UpdateType updateType)
        {
            foreach (ConstantBuffer cb in SelectBuffers(technique,id, updateType))
                Device.SetVertexShaderConstantBuffer(cb.Index, cb.Buffer);
            Device.SetShader(this);
        }
    }
}
