using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public class EffectPool : Component
    {
        private readonly DirectXDevice device;
        private readonly Dictionary<string, Effect> effects;
        private readonly Dictionary<string, Shader> shaders;

        public EffectPool(DirectXDevice device)
        {
            this.device = device;
            shaders = new Dictionary<string, Shader>();
            effects = new Dictionary<string, Effect>();
        }

        public bool ContainsShader(string name)
        {
            return shaders.ContainsKey(name);
        }

        public bool ContainsEffect(string name)
        {
            return effects.ContainsKey(name);
        }

        public TShader GetShader<TShader>(string name)
            where TShader : Shader
        {
            Contract.Requires<ArgumentException>(ContainsShader(name),"name");
            return (TShader) shaders[name];
        }

        public Effect GetEffect(string name)
        {
            return effects[name];
        }

        public void RegisterEffect(Effect effect)
        {
            effects.Add(effect.Name, effect);
        }

        public void RegisterShader(ShaderDescription shaderDesc)
        {
            string name = shaderDesc.Name;

            Type shaderType;

            switch (shaderDesc.ShaderType)
            {
                case ShaderType.Vertex:
                    shaderType = typeof(VertexShader);
                    break;
                case ShaderType.Pixel:
                    shaderType = typeof(PixelShader);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("shaderType");
            }
            //DirectXDevice device, string name, byte[] byteCode

            Shader shader = ToDispose((Shader)Activator.CreateInstance(shaderType, new object[] { device, name, shaderDesc.ByteCode }));
            shader.Initialize();

            shaders.Add(name, shader);
        }

    }
}
