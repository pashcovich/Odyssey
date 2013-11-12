using Odyssey.Content.Shaders;
using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    [YamlMapping(typeof(YamlPhongVSOutputNode))]
    [DataContract]
    public class PhongVSOutputNode : VSNormalTexturedOutputNode
    {
        [DataMember]
        [SupportedType(Type.Float4)]
        public INode WorldPosition { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                foreach (var node in base.DescendantNodes)
                    yield return node;

                yield return WorldPosition;
                foreach (var node in WorldPosition.DescendantNodes)
                    yield return node;

            }
        }

        public override string Operation()
        {
            Struct vsOutput = (Struct)Output;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\t{0} {1};", vsOutput.CustomType, vsOutput.Name));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Position].FullName, Position.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Normal].FullName, Normal.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.WorldPosition].FullName, WorldPosition.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Texture].FullName, Texture.Reference));
            sb.AppendLine(string.Format("\treturn {0};", vsOutput.Name));

            return sb.ToString();
        }

    }

    [DataContract]
    public class PhongShadowsVSOutputNode : PhongVSOutputNode
    {
        [DataMember]
        [SupportedType(Type.Float4)]
        public INode ShadowProjection { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                foreach (var node in base.DescendantNodes)
                    yield return node;

                yield return ShadowProjection;
                foreach (var node in ShadowProjection.DescendantNodes)
                    yield return node;
            }
        }

        public override string Operation()
        {
            Struct vsOutput = (Struct)Output;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\t{0} {1};", vsOutput.CustomType, vsOutput.Name));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Position].FullName, Position.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Normal].FullName, Normal.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.WorldPosition].FullName, WorldPosition.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.ShadowProjection].FullName, ShadowProjection.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Texture].FullName, Texture.Reference));
            sb.AppendLine(string.Format("\treturn {0};", vsOutput.Name));

            return sb.ToString();
        }

    }
}
