using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Constants
{
    public class ConstantNode : NodeBase
    {
        private IVariable value;
        private IVariable output;

        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        [SupportedType(Type.Matrix)]
        [SupportedType(Type.Struct)]
        public IVariable Value
        {
            get { return value; }
            set
            {
                this.value = value;
            }
        }

        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        [SupportedType(Type.Matrix)]
        [SupportedType(Type.Struct)]
        public override IVariable Output
        {
            get
            {
                if (output == null)
                {
                    if (IsVerbose)
                    {
                        string name = Char.IsUpper(Value.Name[0]) ? string.Format("{0}{1}", Variable.GetPrefix(Value.Type), Value.Name) : Value.Name;
                        Output = Variable.InitVariable(name, Value.Type, Value is Struct ? ((Struct)Value).CustomType : CustomType.None);
                    }
                    else Output = Value;
                }
                return output;

            }
            set { output = value; }
        }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                yield break;
            }
        }

        public override void Validate(TechniqueKey key)
        {
            Contract.Requires<InvalidOperationException>(Value.Type == Output.Type, "Type mismatch between Value and Output.");
            base.Validate(key);
        }

        public override string Operation()
        {
            return string.Format("\t{0} {1} = {2};\n", Mapper.Map(Output.Type), Output.Name, Access());
        }

        public override string Access()
        {
            return Value.FullName;
        }
    }
}