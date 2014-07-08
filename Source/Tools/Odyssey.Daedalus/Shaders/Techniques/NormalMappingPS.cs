using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    public class NormalMappingPS : PhongDiffuseMapPS
    {
        public NormalMappingPS()
        {
            Name = "NormalMappingPS";
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.DiffuseMap | PixelShaderFlags.Specular | PixelShaderFlags.NormalMap);

            Struct inputStruct = Struct.VertexPositionNormalTextureTangentOut;
            inputStruct.Name = "input";
            InputStruct = inputStruct;

            PhongLightingNode nPhongLighting = (PhongLightingNode)((PSOutputNode)Result).FinalColor;
            Texture tNormal = Texture.NormalMap;
            Sampler sDiffuseSampler = Get<Sampler>(Param.Samplers.MinMagMipLinearWrap);
            Add(tNormal);

            TextureSampleNode nNormalMapSample = new TextureSampleNode
            {
                Coordinates = new ReferenceNode { Value = InputStruct[Param.SemanticVariables.Texture] },
                Texture = tNormal,
                Sampler = sDiffuseSampler,
                IsVerbose = true
            };

            TrinaryFunctionNode nNormalMapping = new TrinaryFunctionNode
            {
                Input1 = nPhongLighting.Normal,
                Input2 = new ReferenceNode {Value= InputStruct[Param.SemanticVariables.Tangent]},
                Input3 = nNormalMapSample,
                Function = new NormalMappingMethod(),
                Output = new Vector {Type = Shaders.Type.Float3, Name = "vNormalTS"},
                IsVerbose = true
            };

            nPhongLighting.Normal = nNormalMapping;

        }
    }
}
