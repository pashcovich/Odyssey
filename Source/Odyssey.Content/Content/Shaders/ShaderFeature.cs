
using Odyssey.Graphics.Materials;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Odyssey.Content.Shaders
{
    [DataContract]
    public class ShaderFeature
    {
        [DataMember]
        public FeatureType Type { get; private set; }
        [DataMember]
        public object Value { get; private set; }

        public ShaderFeature(FeatureType type, object value)
        {
            Type = type;
            Value = value;
        }

        public bool Supports(VertexShaderFlags feature)
        {
            if (Type != FeatureType.VertexShader)
                return false;
            else
                return ((VertexShaderFlags)Value & feature) == feature;
        }

        public bool Matches(VertexShaderFlags feature)
        {
            if (Type != FeatureType.VertexShader)
                return false;
            else
                return ((VertexShaderFlags)Value) == feature;
        }

        public bool Matches(ShaderFeature feature)
        {
            if (Type != FeatureType.VertexShader)
                return false;
            else
                return Matches((VertexShaderFlags)feature.Value);
        }

        public void SetStatus(VertexShaderFlags feature, bool status)
        {
            Contract.Requires<ArgumentException>(Type == FeatureType.VertexShader);
            VertexShaderFlags fRendering = ((VertexShaderFlags)Value);
            if (status)
                Value = fRendering | feature;
            else
                Value = fRendering & ~feature;
        }
    }
}
