using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Constants;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System.Runtime.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    [DataContract]
    [PixelShader(PixelShaderFlags.Diffuse)]
    [PixelShader(PixelShaderFlags.Specular)]
    [PixelShader(PixelShaderFlags.Shadows)]
    [PixelShader(PixelShaderFlags.ShadowMap)]
    public class PhongShadowsPS : PhongPS
    {
        public PhongShadowsPS()
        {
            Name = "PhongShadowsPS";
            FeatureLevel = Graphics.Materials.FeatureLevel.PS_5_0;
            Clear();
            PhongLightingNode nPhong = (PhongLightingNode)((PSOutputNode)Result).FinalColor;
            var inputStruct = PhongShadowsVS.VSOut;
            inputStruct.Name = "input";
            InputStruct = inputStruct;

            ConstantBuffer cbStatic = CBStatic;
            ConstantBuffer cbFrame = CBFrame;
            Texture tShadow = Texture.ShadowMap;
            Sampler sShadowSampler = Sampler.MinMagMiLinearMirrorLessEqual;
            sShadowSampler.Name = "sShadowMap";

            Add(tShadow);
            Add(sShadowSampler);
            Add(cbStatic);
            Add(cbFrame);

            //CastNode nV3toV2 = new CastNode
            //{
            //    Input = new ConstantNode { Value = inputStruct[Param.SemanticVariables.ShadowProjection] },
            //    Output = new Vector { Type = Shaders.Type.Float2, Name = "tProjection", Swizzle = new[] { Swizzle.X, Swizzle.Y,} },
            //};

            TextureSampleNode nShadowMapSampler = new TextureSampleNode
            {
                Coordinates = new ConstantNode { Value = inputStruct[Param.SemanticVariables.ShadowProjection] },
                Texture = tShadow,
                Sampler = sShadowSampler,
                IsVerbose = false,
            };
            nPhong.ShadowMapSamplerNode = nShadowMapSampler;
            nPhong.Shadows = true;
            
        }

        public static ConstantBuffer CBStatic
        {
            get
            {
                ConstantBuffer cbStatic = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.Static,
                    UpdateFrequency = UpdateFrequency.Static,
                };

                cbStatic.Add(Struct.Material);
                cbStatic.Add(Struct.PointLight0);
                cbStatic.Add(Float.ShadowBias);
                return cbStatic;
            }
        }

        public static ConstantBuffer CBFrame
        {
            get
            {
                ConstantBuffer cbFrame = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.PerFrame,
                    UpdateFrequency = UpdateFrequency.PerFrame,
                };

                cbFrame.Add(Vector.CameraPosition);
                return cbFrame;
            }
        }
    }
}
