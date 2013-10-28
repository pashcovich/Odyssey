using Odyssey.Engine;
using Odyssey.Graphics.Rendering;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Constants;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System.Runtime.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    [DataContract]
    public class ColorShader : Shader
    {
        public ColorShader()
        {
            Name = "SpritePS";
            Type = ShaderType.Pixel;
            FeatureLevel = Odyssey.Graphics.Materials.FeatureLevel.PS_4_0_Level_9_1;
            EnableSeparators = true;
            InputStruct = Struct.VertexPositionNormalTextureOut;
            OutputStruct = Struct.PixelShaderOutput;
            Struct cbMaterial = ConstantBuffer.CBMaterial;
            Struct material = (Struct)cbMaterial[Param.Struct.Material];
            Variable tDiffuse = Texture.Diffuse;
            Variable sDiffuse = Sampler.MinMagMipLinearWrap;

            Add(cbMaterial);
            Add(sDiffuse);
            Add(tDiffuse);

            tDiffuse.SetMarkup("Key", "SpriteDiffuse");

            TextureCoordinateNode nTexCoord = new TextureCoordinateNode
            {
                Coordinates = InputStruct[Param.SemanticVariables.TextureUV],
            };
            TextureSampleNode nTexSample = new TextureSampleNode
            {
                Coordinates = nTexCoord,
                Texture = tDiffuse,
                Sampler = sDiffuse,
            };

            ColorConstantNode nColorConst = new ColorConstantNode()
            {
                Color = material[Param.Material.Diffuse],
            };

            MultiplyNode nMultiply = new MultiplyNode()
            {
                Input1 = nTexSample,
                Input2 = nColorConst,
                Output = new Variable { Name = "cFinal", Type = Shaders.Type.Float4 }
            };

            Result = new PSOutputNode()
            {
                FinalColor = nMultiply,
                Output = OutputStruct
            };
        }


    }
}
