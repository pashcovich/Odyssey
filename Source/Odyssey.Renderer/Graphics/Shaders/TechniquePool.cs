using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Shaders
{
    public class TechniquePool : Component
    {
        private readonly DirectXDevice device;
        private readonly Dictionary<string, Shader> shaders;
        private readonly Dictionary<string, Technique> techniques;

        public TechniquePool(DirectXDevice device)
        {
            this.device = device;
            shaders = new Dictionary<string, Shader>();
            techniques = new Dictionary<string, Technique>();
        }

        public void RegisterTechnique(Technique technique)
        {
            Contract.Requires<ArgumentNullException>(technique != null, "technique");
            techniques.Add(technique.Name, ToDispose(technique));
        }

        public Technique GetTechnique(string name)
        {
            return techniques[name];
        }

        public bool ContainsTechnique(string technique)
        {
            return techniques.ContainsKey(technique);
        }


        public TShader GetShader<TShader>(string name)
            where TShader : Shader
        {
            return (TShader)shaders[name];
        }

        [Pure]
        public bool ContainsShader(string name)
        {
            return shaders.ContainsKey(name);
        }

        public TShader RegisterShader<TShader>(string name, byte[] byteCode)
            where TShader : Shader
        {
            Type shaderType = typeof (TShader);
            TShader shader;
            if (ContainsShader(name)) shader = GetShader<TShader>(name);
            else
            {
                shader = ToDispose((TShader) Activator.CreateInstance(shaderType, new object[] {device, name, byteCode}));
                shader.Initialize();
                shaders.Add(shader.Name, shader);
            }
            return shader;
        }

        //public TShader GetShader<TShader>(string name)
        //    where TShader : Shader
        //{
        //    Contract.Requires<ArgumentException>(ContainsShader(name),"name");
        //    return (TShader) shaders[name];
        //}

        //public void RegisterShader(ShaderDescription shaderDesc)
        //{
        //    string name = shaderDesc.Name;

        //    Type shaderType;

        //    switch (shaderDesc.ShaderType)
        //    {
        //        case ShaderType.Vertex:
        //            shaderType = typeof(VertexShader);
        //            break;
        //        case ShaderType.Pixel:
        //            shaderType = typeof(PixelShader);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("shaderType");
        //    }
        //    //DirectXDevice device, string name, byte[] byteCode

        //    Shader shader = ToDispose((Shader)Activator.CreateInstance(shaderType, new object[] { device, name, shaderDesc.ByteCode }));
        //    shader.Initialize();

        //    shaders.Add(name, shader);
        //}

    }
}
