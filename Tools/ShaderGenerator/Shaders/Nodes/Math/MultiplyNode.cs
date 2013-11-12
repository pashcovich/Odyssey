using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math
{
    [YamlMapping(typeof(YamlMultiplyNode))]
    public class MultiplyNode : NodeBase
    {
        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        public INode Input1 { get; set; }

        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        public INode Input2 { get; set; }

        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        public override IVariable Output { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                yield return Input1;
                foreach (var node in Input1.DescendantNodes)
                    yield return node;

                yield return Input2;
                foreach (var node in Input2.DescendantNodes)
                    yield return node;
            }
        }

        public MultiplyNode()
        {
            IsVerbose = true;
        }

        public override void Validate(TechniqueKey key)
        {
            Contract.Requires<InvalidOperationException>(Input2.Output.Type == Input1.Output.Type);
            base.Validate(key);
        }

        public override string Operation()
        {
            return string.Format("\t{0} {1} = {2};\n", Mapper.Map(Output.Type), Output.Name, Access());
        }

        public override string Access()
        {
            return string.Format("{0} * {1}", Input1.Reference, Input2.Reference);
        }


    }
}
