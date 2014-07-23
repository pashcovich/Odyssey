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
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using SharpDX;
using SharpDX.Direct3D11;

#endregion

namespace Odyssey.Graphics.Shaders
{
    public class Technique : Component, IEnumerable<Shader>
    {
        //private readonly Effect effect;
        private readonly InputLayout inputLayout;
        private readonly TechniqueMapping mapping;
        private readonly Dictionary<ShaderType, Shader> shaders;
        private readonly TechniqueMapping techniqueMapping;

        public Technique(DirectXDevice device, string techniqueName, TechniqueMapping mapping)
        {
            Contract.Requires<ArgumentNullException>(device != null, "device");
            Contract.Requires<ArgumentNullException>(mapping != null, "mapping");
            Name = techniqueName;
            this.mapping = mapping;

            ShaderDescription vsDesc;
            ShaderDescription psDesc;
            techniqueMapping = mapping;

            shaders = new Dictionary<ShaderType, Shader>();

            if (mapping.TryGetValue(ShaderType.Vertex, out vsDesc))
            {
                VertexShader vertexShader = device.TechniquePool.RegisterShader<VertexShader>(vsDesc.Name, vsDesc.ByteCode);
                var vertexInputLayout = mapping.GenerateVertexInputLayout();
                inputLayout = ToDispose(new InputLayout(device, vsDesc.ByteCode, vertexInputLayout.InputElements));
                shaders.Add(ShaderType.Vertex, vertexShader);
            }
            if (mapping.TryGetValue(ShaderType.Pixel, out psDesc))
            {
                PixelShader pixelShader = device.TechniquePool.RegisterShader<PixelShader>(psDesc.Name, psDesc.ByteCode);
                shaders.Add(ShaderType.Pixel, pixelShader);
            }
        }

        public TechniqueMapping Mapping
        {
            get { return mapping; }
        }

        public bool IsInited { get; private set; }

        public bool UsesProceduralTextures { get; set; }

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

        public InputLayout InputLayout
        {
            get { return inputLayout; }
        }

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
            foreach (
                ConstantBuffer cb in
                    shaders.Values.SelectMany(shader => shader.Buffers.Where(cb => cb.Description.UpdateFrequency == updateType)))
                cb.Update();
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

        #region Technique Feature Methods

        public bool GetFeatureStatus(VertexShaderFlags feature)
        {
            return Mapping.Key.Supports(feature);
        }

        public bool GetFeatureStatus(PixelShaderFlags feature)
        {
            return Mapping.Key.Supports(feature);
        }

        #endregion Technique Feature Methods
    }
}