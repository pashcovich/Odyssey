#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
// 
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
// 
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion

#region Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Organization;
using SharpDX;
using SharpDX.Direct3D11;

#endregion

namespace Odyssey.Graphics.Shaders
{
    public class Effect : Component, IByteSize, IEnumerable<Shader>
    {
        internal const long SceneEntityID = -1;

        private readonly int byteSize;
        private readonly DirectXDevice device;
        private readonly InputLayout inputLayout;
        private readonly Dictionary<ShaderType, Shader> shaders;
        private readonly TechniqueMapping techniqueMapping;

        public Effect(DirectXDevice device, string techniqueName, TechniqueMapping mapping)
            : this(device, string.Format("{0}.{1}", techniqueName, mapping.Name))
        {
            ShaderDescription vsDesc;
            ShaderDescription psDesc;
            techniqueMapping = mapping;

            shaders = new Dictionary<ShaderType, Shader>();

            if (mapping.TryGetValue(ShaderType.Vertex, out vsDesc))
            {
                if (!device.EffectPool.ContainsShader(vsDesc.Name))
                    device.EffectPool.RegisterShader(vsDesc);
                VertexShader vertexShader = device.EffectPool.GetShader<VertexShader>(vsDesc.Name);

                var vertexInputLayout = mapping.GenerateVertexInputLayout();
                inputLayout = ToDispose(new InputLayout(device, vsDesc.ByteCode, vertexInputLayout.InputElements));

                byteSize += vsDesc.ByteCode.Length;
                shaders.Add(ShaderType.Vertex, vertexShader);
            }
            if (mapping.TryGetValue(ShaderType.Pixel, out psDesc))
            {
                if (!device.EffectPool.ContainsShader(psDesc.Name))
                    device.EffectPool.RegisterShader(psDesc);
                PixelShader pixelShader = device.EffectPool.GetShader<PixelShader>(psDesc.Name);
                byteSize += psDesc.ByteCode.Length;
                shaders.Add(ShaderType.Pixel, pixelShader);
            }
        }

        private Effect(DirectXDevice device, string name)
            : base(name)
        {
            this.device = device;
        }

        public InputLayout InputLayout
        {
            get { return inputLayout; }
        }


        public TechniqueKey TechniqueKey
        {
            get { return techniqueMapping.Key; }
        }

        public bool UsesProceduralTextures { get; set; }

        public IEnumerable<Shader> Shaders
        {
            get { return shaders.Values; }
        }

        public IEnumerable<KeyValuePair<string, string>> MetaData
        {
            get
            {
                return (from shader in techniqueMapping.Shaders
                    let cBuffers = shader.ConstantBuffers
                    from cb in cBuffers
                    from data in cb.Metadata
                    select data);
            }
        }

        public Shader this[ShaderType type]
        {
            get
            {
                Contract.Requires<ArgumentException>(ContainsShader(type));
                return shaders[type];
            }
        }

        public int ByteSize
        {
            get { return byteSize; }
        }

        #region IEnumerable

        public IEnumerator<Shader> GetEnumerator()
        {
            return ((IEnumerable<Shader>) shaders.Values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable

        [Pure]
        public bool ContainsShader(ShaderType type)
        {
            return shaders.ContainsKey(type);
        }

        [Pure]
        public bool ContainsShader(string shaderName)
        {
            return shaders.Values.Any(s => s.Name == shaderName);
        }

        public void AssembleBuffers()
        {
            foreach (ConstantBuffer cb in shaders.Values.SelectMany(shader => shader.Buffers.Where(cb => !cb.IsInited)))
                cb.Assemble();
        }

        public void UpdateBuffers(UpdateType updateType)
        {
            foreach (ConstantBuffer cb in shaders.Values.SelectMany(shader => shader.Buffers.Where(cb => cb.Description.UpdateFrequency == updateType)))
                cb.Update();
        }
    }
}