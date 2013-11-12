using Odyssey.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public enum Type
    {
        None,
        Float,
        Float2,
        Float3,
        Float3x3,
        Float4,
        Matrix,
        Struct,
        ConstantBuffer,
        Texture2D,
        Texture3D,
        TextureCube,
        Sampler,
        SamplerComparisonState,
    }

    public enum CustomType
    {
        None,
        VSIn,
        VSOut,
        PSOut,
        Material,
        PointLight,
    }

    internal static class HLSLTypes
    {
        internal const string Float = "float";
        internal const string Float2 = "float2";
        internal const string Float3 = "float3";
        internal const string Float3x3 = "float3x3";
        internal const string Float4 = "float4";
        internal const string Matrix = "matrix";
        internal const string Struct = "struct";
        internal const string Texture2D = "Texture2D";
        internal const string Texture3D = "Texture3D";
        internal const string TextureCube = "TextureCube";
        internal const string Sampler = "sampler";
        internal const string SamplerComparisonState = "SamplerComparisonState";
        internal const string ConstantBuffer = "cbuffer";

        internal const string Material = "Material";
        internal const string PointLight = "PointLight";

        internal const string VSIn = "VSIn";
        internal const string VSOut = "VSOut";
        internal const string PSOut = "PSOut";

        static Dictionary<Type, string> types;
        static Dictionary<CustomType, string> customTypes;

        static HLSLTypes()
        {
            types = new Dictionary<Type, string>();
            customTypes = new Dictionary<CustomType, string>();
            types.Add(Type.Float, Float);
            types.Add(Type.Float2, Float2);
            types.Add(Type.Float3, Float3);
            types.Add(Type.Float3x3, Float3x3);
            types.Add(Type.Float4, Float4);
            types.Add(Type.Matrix, Matrix);
            types.Add(Type.Struct, Struct);
            types.Add(Type.Texture2D, Texture2D);
            types.Add(Type.Texture3D, Texture3D);
            types.Add(Type.TextureCube, TextureCube);
            types.Add(Type.Sampler, Sampler);
            types.Add(Type.SamplerComparisonState, SamplerComparisonState);
            types.Add(Type.ConstantBuffer, ConstantBuffer);

            customTypes.Add(CustomType.Material, Material);
            customTypes.Add(CustomType.VSIn, VSIn);
            customTypes.Add(CustomType.VSOut, VSOut);
            customTypes.Add(CustomType.PSOut, PSOut);
            customTypes.Add(CustomType.PointLight, PointLight);
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

        internal static string Map(CustomType type)
        {
            try
            {
                return customTypes[type];
            }
            catch (KeyNotFoundException ex)
            {
                Log.Daedalus.Warning("Key '{0}' not in dictionary.", type);
                return type.ToString();
            }
        }
    }
}
