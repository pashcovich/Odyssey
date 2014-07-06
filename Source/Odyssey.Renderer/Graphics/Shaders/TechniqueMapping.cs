using Odyssey.Graphics.Effects;
using Odyssey.Utilities;
using Odyssey.Utilities.Reflection;
using SharpDX.DXGI;
using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Odyssey.Graphics.Shaders
{
    [DataContract]
    [DebuggerDisplay("{Name}")]
    public class TechniqueMapping : IDataSerializable
    {
        [DataMember]
        private readonly Dictionary<ShaderType, ShaderDescription> techniqueMap;

        private string name;
        private TechniqueKey key;

        [DataMember]
        public TechniqueKey Key
        {
            get { return key; }
            internal set { key = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public IEnumerable<ShaderDescription> Shaders { get { return techniqueMap.Values; } }

        public TechniqueMapping(string name)
        {
            techniqueMap = new Dictionary<ShaderType, ShaderDescription>();
            Name = name;
        }

        public void Set(ShaderDescription shader)
        {
            techniqueMap[shader.ShaderType] = shader;
        }

        public void Remove(ShaderType shaderType)
        {
            techniqueMap.Remove(shaderType);
        }

        public bool Contains(ShaderType type)
        {
            return techniqueMap.ContainsKey(type);
        }

        public bool Contains(string shaderName)
        {
            return techniqueMap.Values.Count(s => s.Name == shaderName) > 0;
        }

        public bool TryGetValue(ShaderType type, out ShaderDescription shader)
        {
            shader = null;
            if (techniqueMap.ContainsKey(type))
            {
                shader = techniqueMap[type];
                return true;
            }
            return false;
        }

        public bool Validate()
        {
            return Shaders.Aggregate(true, (current, shaderDesc) => current & shaderDesc.Validate());
        }

        public ShaderDescription this[ShaderType type]
        {
            get
            {
                return techniqueMap[type];
            }
        }

        public VertexInputLayout GenerateVertexInputLayout()
        {
            VertexShaderFlags vsFlags = Key.VertexShader;

            return VertexInputLayout.New(0, vsFlags.GetUniqueFlags().Cast<VertexShaderFlags>().SelectMany(CreateElement).ToArray());
        }

        private static VertexElement[] CreateElement(VertexShaderFlags flag)
        {
            switch (flag)
            {
                case VertexShaderFlags.Position:
                    return new[] { VertexElement.Position(Format.R32G32B32_Float) };

                case VertexShaderFlags.Normal:
                    return new[] { VertexElement.Normal(Format.R32G32B32_Float) };

                case VertexShaderFlags.TextureUV:
                    return new[] { VertexElement.TextureCoordinate(Format.R32G32_Float) };

                case VertexShaderFlags.TextureUVW:
                    return new[] { VertexElement.TextureCoordinate(Format.R32G32B32_Float) };

                case VertexShaderFlags.Tangent:
                    return new[] { VertexElement.Tangent(Format.R32G32B32A32_Float) };

                case VertexShaderFlags.Barycentric:
                    return new[] { new VertexElement("BARYCENTRIC", Format.R32G32B32_Float) };

                case VertexShaderFlags.InstanceWorld:
                    return new[]
                    {
                        new VertexElement("INSTANCEWORLD", 0, Format.R32G32B32A32_Float),
                        new VertexElement("INSTANCEWORLD", 1, Format.R32G32B32A32_Float),
                        new VertexElement("INSTANCEWORLD", 2, Format.R32G32B32A32_Float),
                        new VertexElement("INSTANCEWORLD", 3, Format.R32G32B32A32_Float),
                    };

                default:
                    throw new ArgumentOutOfRangeException(string.Format("[{0}]: VertexShaderFlag not valid", flag));
            }
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref name);
            serializer.Serialize(ref key);
            //serializer.Serialize(ref techniqueMap);
        }
    }
}