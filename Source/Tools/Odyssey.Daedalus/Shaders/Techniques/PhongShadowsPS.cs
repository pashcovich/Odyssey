using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Functions;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using System.Runtime.Serialization;
using ConstantBuffer = Odyssey.Daedalus.Shaders.Structs.ConstantBuffer;

namespace Odyssey.Daedalus.Shaders.Techniques
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
            var nPhongLighting = (PhongLightingNode)((PSOutputNode)Result).FinalColor;
            var inputStruct = PhongShadowsVS.VSOut;
            inputStruct.Name = "input";
            InputStruct = inputStruct;

            var cbStatic = CBStatic;
            var cbFrame = CBFrame;
            var tShadow = Texture.ShadowMap;
            var sShadowSampler = Sampler.MinMagMiLinearMirrorLessEqual;
            sShadowSampler.Name = "sShadowMap";

            Add(tShadow);
            Add(sShadowSampler);
            Add(cbStatic);
            Add(cbFrame);

            var nShadowMapSampler = new TextureSampleNode
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
                var cbStatic = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.Static,
                    CbUpdateType = CBUpdateType.SceneStatic,
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
                var cbFrame = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.PerFrame,
                    CbUpdateType = CBUpdateType.SceneFrame,
                };

                cbFrame.Add(Vector.CameraPosition);
                return cbFrame;
            }
        }
    }
}
