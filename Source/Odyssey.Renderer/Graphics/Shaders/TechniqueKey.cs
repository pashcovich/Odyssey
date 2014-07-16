using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Organization;
using Odyssey.Utilities;
using Odyssey.Utilities.Reflection;
using SharpDX.Multimedia;
using SharpDX.Serialization;
using System;
using System.Runtime.Serialization;

namespace Odyssey.Graphics.Shaders
{
    public struct TechniqueKey : IEquatable<TechniqueKey>, IDataSerializable
    {
        private ShaderModel shaderModel;
        private PixelShaderFlags pixelShader;
        private VertexShaderFlags vertexShader;
        private PreferredRasterizerState rasterizerState;
        private PreferredBlendState blendState;
        private PreferredDepthStencilState depthStencilState;

        public TechniqueKey(VertexShaderFlags vs = VertexShaderFlags.None, PixelShaderFlags ps = PixelShaderFlags.None, ShaderModel sm = ShaderModel.SM_4_0_Level_9_1,
            PreferredRasterizerState rasterizerState = PreferredRasterizerState.CullBack, PreferredBlendState blendState = PreferredBlendState.Opaque, PreferredDepthStencilState depthStencilState = PreferredDepthStencilState.Enabled)
            : this()
        {
            VertexShader = vs;
            PixelShader = ps;
            ShaderModel = sm;
            RasterizerState = rasterizerState;
            BlendState = blendState;
            DepthStencilState = depthStencilState;
        }

        #region

        [DataMember]
        public PixelShaderFlags PixelShader
        {
            get { return pixelShader; }
            private set { pixelShader = value; }
        }

        [DataMember]
        public ShaderModel ShaderModel
        {
            get { return shaderModel; }
            private set { shaderModel = value; }
        }

        [DataMember]
        public VertexShaderFlags VertexShader
        {
            get { return vertexShader; }
            internal set { vertexShader = value; }
        }

        [DataMember]
        public PreferredRasterizerState RasterizerState
        {
            get { return rasterizerState; }
            private set { rasterizerState = value; }
        }

        [DataMember]
        public PreferredBlendState BlendState
        {
            get { return blendState; }
            private set { blendState = value; }
        }

        [DataMember]
        public PreferredDepthStencilState DepthStencilState
        {
            get { return depthStencilState; }
            private set { depthStencilState = value; }
        }

        #endregion

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
            return (int)ShaderModel >= (int)shaderModel || ShaderModel == ShaderModel.Any;
        }

        public bool Supports(TechniqueKey key)
        {
            return Supports(key.VertexShader) && Supports(key.PixelShader) && Supports(key.ShaderModel);
        }

        public int Rank(TechniqueKey key)
        {
            int rank = 0;

            foreach (var vsFlag in key.VertexShader.GetUniqueFlags())
                if (Supports((VertexShaderFlags)vsFlag))
                    rank++;
            foreach (var psFlag in key.PixelShader.GetUniqueFlags())
                if (Supports((PixelShaderFlags)psFlag))
                    rank++;
            return rank;
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
            return VertexShader == other.VertexShader && PixelShader == other.PixelShader && ShaderModel == other.ShaderModel
                && RasterizerState == other.RasterizerState && BlendState == other.BlendState && DepthStencilState == other.DepthStencilState;
        }

        public override bool Equals(object obj)
        {
            if (obj is TechniqueKey)
            {
                TechniqueKey key = (TechniqueKey)obj;
                return Equals(key);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return VertexShader.GetHashCode() ^ PixelShader.GetHashCode() ^ ShaderModel.GetHashCode()
                ^ RasterizerState.GetHashCode() ^ BlendState.GetHashCode() ^ DepthStencilState.GetHashCode();
        }

        #endregion Equality

        public void Serialize(BinarySerializer serializer)
        {
            serializer.SerializeEnum(ref shaderModel);
            serializer.SerializeEnum(ref vertexShader);
            serializer.SerializeEnum(ref pixelShader);
            serializer.SerializeEnum(ref rasterizerState);
            serializer.SerializeEnum(ref blendState);
            serializer.SerializeEnum(ref depthStencilState);
        }
    }
}