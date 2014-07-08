using System.Collections.Generic;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public enum Type
    {
        None,
        Int,
        Vector,
        Float,
        FloatArray,
        Float2,
        Float3,
        Float3x3,
        Float4,
        Float4x4,
        Matrix,
        Struct,
        ConstantBuffer,
        Texture2D,
        Texture3D,
        TextureCube,
        Sampler,
        SamplerComparisonState,
    }

    public static class CustomType
    {
        internal const string None = "None";
        internal const string VSIn = "VSIn";
        internal const string VSOut = "VSOut";
        internal const string PSOut = "PSOut";
        internal const string Material = "Material";
        internal const string PointLight = "PointLight";
    }

    internal static class HLSLTypes
    {
        internal const string Int = "int";
        internal const string Float = "float";
        internal const string Float2 = "float2";
        internal const string Float3 = "float3";
        internal const string Float3x3 = "float3x3";
        internal const string Float4 = "float4";
        internal const string Float4x4 = "float4x4";
        internal const string Vector = "vector";
        internal const string Matrix = "matrix";
        internal const string Struct = "struct";
        internal const string Texture2D = "Texture2D";
        internal const string Texture3D = "Texture3D";
        internal const string TextureCube = "TextureCube";
        internal const string Sampler = "sampler";
        internal const string SamplerComparisonState = "SamplerComparisonState";
        internal const string ConstantBuffer = "cbuffer";


        static readonly Dictionary<Type, string> types;

        static HLSLTypes()
        {
            types = new Dictionary<Type, string>
            {
                {Type.Int, Int},
                {Type.Float, Float},
                {Type.Float2, Float2},
                {Type.Float3, Float3},
                {Type.Float3x3, Float3x3},
                {Type.Float4, Float4},
                {Type.Float4x4, Float4x4},
                {Type.FloatArray, Float},
                {Type.Matrix, Matrix},
                {Type.Struct, Struct},
                {Type.Texture2D, Texture2D},
                {Type.Texture3D, Texture3D},
                {Type.TextureCube, TextureCube},
                {Type.Sampler, Sampler},
                {Type.SamplerComparisonState, SamplerComparisonState},
                {Type.ConstantBuffer, ConstantBuffer}
            };

        }

        internal static string Map(Type type)
        {
            try
            {
                return types[type];
            }
            catch (KeyNotFoundException ex)
            {
                Log.Daedalus.Warning("Key '{0}' not in dictionary.", type);
                return type.ToString();
            }
        }

    }
}
