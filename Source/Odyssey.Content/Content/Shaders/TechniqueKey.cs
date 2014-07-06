using Odyssey.Graphics.Materials;
using System;
using System.Runtime.Serialization;

namespace Odyssey.Graphics.Materials
{
    [DataContract]
    public struct TechniqueKey : IEquatable<TechniqueKey>
    {
        public TechniqueKey(VertexShaderFlags vs = VertexShaderFlags.None, PixelShaderFlags ps = PixelShaderFlags.None, ShaderModel sm = ShaderModel.SM_4_0_Level_9_1)
            : this()
        {
            VertexShader = vs;
            PixelShader = ps;
            ShaderModel = sm;
        }

        [DataMember]
        public PixelShaderFlags PixelShader { get; private set; }

        [DataMember]
        public ShaderModel ShaderModel { get; private set; }

        [DataMember]
        public VertexShaderFlags VertexShader { get; private set; }

        public bool Supports(VertexShaderFlags feature)
        {
            return (VertexShader & feature) == feature;
        }

        public bool Supports(PixelShaderFlags feature)
        {
            return (PixelShader & feature) == feature;
        }

        public bool Supports(ShaderModel shaderModel)
        {
            return (int)ShaderModel >= (int)shaderModel || ShaderModel == Materials.ShaderModel.Any;
        }

        public bool Supports(TechniqueKey key)
        {
            return Supports(key.VertexShader) && Supports(key.PixelShader) && Supports(key.ShaderModel);
        }

        #region Equality

        public static bool operator !=(TechniqueKey a, TechniqueKey b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(TechniqueKey a, TechniqueKey b)
        {
            return a.Equals(b);
        }

        public bool Equals(TechniqueKey other)
        {
            return VertexShader == other.VertexShader && PixelShader == other.PixelShader && ShaderModel == other.ShaderModel;
        }

        public override bool Equals(object obj)
        {
            if (obj is TechniqueKey)
            {
                TechniqueKey key = (TechniqueKey)obj;
                return this.Equals(key);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return VertexShader.GetHashCode() ^ PixelShader.GetHashCode() ^ ShaderModel.GetHashCode();
        }

        #endregion Equality
    }
}