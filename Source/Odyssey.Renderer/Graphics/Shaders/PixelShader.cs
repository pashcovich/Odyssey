using Odyssey.Engine;
using Odyssey.Graphics.Effects;

namespace Odyssey.Graphics.Shaders
{
    public class PixelShader : Shader
    {
        protected readonly new SharpDX.Direct3D11.PixelShader Resource;

        public PixelShader(DirectXDevice device, string name, byte[] byteCode)
            : base(device, ShaderType.Pixel, name)
        {
            Resource = new SharpDX.Direct3D11.PixelShader(device, byteCode);
        }

        public override void Initialize()
        {
            Initialize(Resource);
            Resource.DebugName = Name;
        }

        public override void Apply(string technique, CBUpdateType cbUpdateType)
        {
            foreach (ConstantBuffer cb in SelectBuffers(technique, cbUpdateType))
                Device.SetPixelShaderConstantBuffer(cb.Index, cb.Buffer);
        }

        public override void Apply(string technique, long id, CBUpdateType cbUpdateType)
        {
            foreach (ConstantBuffer cb in SelectBuffers(technique, id, cbUpdateType))
                Device.SetPixelShaderConstantBuffer(cb.Index, cb.Buffer);
        }

        /// <summary>
        /// <see cref="SharpDX.Direct3D11.PixelShader"/> casting operator.
        /// </summary>
        /// <param name="from">From the PixelShader.</param>
        public static implicit operator SharpDX.Direct3D11.PixelShader(PixelShader from)
        {
            return from.Resource;
        }
    }
}
