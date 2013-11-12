using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    #region Operator Nodes
    [YamlTag("Cast")]
    [YamlMapping(typeof(CastNode))]
    public class YamlCastNode : YamlNode
    {
        public YamlCastNode(){}
        public YamlCastNode(INode node) : base(node) { }
    }

    [YamlTag("Constant")]
    [YamlMapping(typeof(ConstantNode))]
    public class YamlConstantNode : YamlNode
    {
        public YamlConstantNode(){}
        public YamlConstantNode(INode node) : base(node) { }
    } 

    [YamlTag("TextureSample")]
    [YamlMapping(typeof(TextureSampleNode))]
    public class YamlTextureSampleNode : YamlNode
    {
        public YamlTextureSampleNode() { }
        public YamlTextureSampleNode(INode node) : base(node) { }
    }
    #endregion

    #region Math Nodes
    [YamlTag("MatrixMultiply")]
    [YamlMapping(typeof(MatrixMultiplyNode))]
    public class YamlMatrixMultiplyNode : YamlNode
    {
        public YamlMatrixMultiplyNode() { }
        public YamlMatrixMultiplyNode(INode node) : base(node) { }
    }

    [YamlTag("Multiply")]
    [YamlMapping(typeof(MultiplyNode))]
    public class YamlMultiplyNode : YamlNode
    {
        public YamlMultiplyNode() { }
        public YamlMultiplyNode(INode node) : base(node) { }
    }

    [YamlTag("Subtraction")]
    [YamlMapping(typeof(SubtractionNode))]
    public class YamlSubtractionNode : YamlNode
    {
        public YamlSubtractionNode() { }
        public YamlSubtractionNode(INode node) : base(node) { }
    } 
    #endregion

    #region Function Nodes
    [YamlTag("PhongLighting")]
    [YamlMapping(typeof(PhongLightingNode))]
    public class YamlPhongLightingNode : YamlNode
    {
        public YamlPhongLightingNode() { }
        public YamlPhongLightingNode(INode node) : base(node) { }
    }

    [YamlTag("ClipSpaceTransform")]
    [YamlMapping(typeof(ClipSpaceTransformNode))]
    public class YamlClipSpaceTransformNode : YamlNode
    {
        public YamlClipSpaceTransformNode() { }
        public YamlClipSpaceTransformNode(INode node) : base(node) { }
    }
    #endregion

    #region Output Nodes
    [YamlTag("PhongVSOutput")]
    [YamlMapping(typeof(PhongVSOutputNode))]
    public class YamlPhongVSOutputNode : YamlNode
    {
        public YamlPhongVSOutputNode() { }
        public YamlPhongVSOutputNode(INode node) : base(node) { }
    }

    [YamlTag("VSTexturedOutput")]
    [YamlMapping(typeof(VSTexturedOutputNode))]
    public class YamlVSTexturedOutputNode : YamlNode
    {
        public YamlVSTexturedOutputNode() { }
        public YamlVSTexturedOutputNode(INode node) : base(node) { }
    }

    [YamlTag("PSOutput")]
    [YamlMapping(typeof(PSOutputNode))]
    public class YamlPSOutputNode : YamlNode
    {
        public YamlPSOutputNode() { }
        public YamlPSOutputNode(INode node) : base(node) { }
    }
    #endregion


}
