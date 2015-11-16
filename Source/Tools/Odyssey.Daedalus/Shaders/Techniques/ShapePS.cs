using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Daedalus.Shaders.Techniques
{
    public class ShapePS : Shader
    {
        public ShapePS()
        {
            Name = "ShapePS";
            Type = ShaderType.Pixel;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse);
            FeatureLevel = FeatureLevel.PS_4_0_Level_9_1;
            EnableSeparators = true;
            var inputStruct = ShapeVS.VSOut;
            inputStruct.Name = "input";
            InputStruct = inputStruct;
            OutputStruct = Struct.PixelShaderOutput;

            Result = new PSOutputNode
            {
                FinalColor = new ReferenceNode() { Value = InputStruct[Param.SemanticVariables.Color]},
                Output = OutputStruct
            };
        }
    }
}
