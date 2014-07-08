using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System.Runtime.Serialization;
using ConstantBuffer = Odyssey.Tools.ShaderGenerator.Shaders.Structs.ConstantBuffer;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    [DataContract]
    public class PhongShadowsPS : PhongPS
    {
        public PhongShadowsPS()
        {
            Name = "PhongShadowsPS";
            FeatureLevel = FeatureLevel.PS_5_0;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.Specular | PixelShaderFlags.Shadows | PixelShaderFlags.ShadowMap, sm: FromFeatureLevel(FeatureLevel));
            Clear();
            PhongLightingNode nPhongLighting = (PhongLightingNode)((PSOutputNode)Result).FinalColor;
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

            TextureSampleNode nShadowMapSampler = new TextureSampleNode
            {
                Coordinates = new ReferenceNode { Value = inputStruct[Param.SemanticVariables.ShadowProjection] },
                Texture = tShadow,
                Sampler = sShadowSampler,
                IsVerbose = false,
            };
            nPhongLighting.ShadowMapSample = nShadowMapSampler;
            nPhongLighting.Shadows = true;
            
        }

        public static ConstantBuffer CBStatic
        {
            get
            {
                ConstantBuffer cbStatic = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.Static,
                    UpdateType = UpdateType.SceneStatic,
                };

                cbStatic.Add(Struct.Material);
                cbStatic.Add(Struct.PointLight);
                cbStatic.Add(Float.ShadowBias);
                return cbStatic;
            }
        }

        public static new ConstantBuffer CBFrame
        {
            get
            {
                ConstantBuffer cbFrame = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.PerFrame,
                    UpdateType = UpdateType.SceneFrame,
                };

                cbFrame.Add(Vector.CameraPosition);
                return cbFrame;
            }
        }
    }
}
