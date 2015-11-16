using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using Odyssey.Daedalus.Shaders.Nodes.Math;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Daedalus.Shaders.Nodes.Operators
{
    public class DeclarationNode : MultipleInputsNodeBase
    {
        [SupportedType(Type.Vector)]
        public override IVariable Output { get; set; }

        public override string Access()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}(", Mapper.Map(Output.Type));
            foreach (var node in Inputs)
            {
                sb.AppendFormat("{0}, ", node.Reference);
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            return sb.ToString();
        }

        public override void Validate(TechniqueKey key)
        {
            Contract.Requires<InvalidOperationException>(Output != null, "Node requires an explicit Output variable");
            base.Validate(key);
        }

        public static DeclarationNode InitNode(string name, Type type, params float[] values)
        {
            Contract.Requires<ArgumentNullException>(values != null);
            int count = Variable.ComponentsFromType(type);
            if (count != values.Length)
                throw new InvalidOperationException("Mismatch between type and array length");
            var nodes = new List<INode>();
            for (int i = 0; i < count; i++)
            {
                nodes.Add(new ScalarNode(){Value = values[i]});
            }

            return new DeclarationNode()
            {
                Inputs = nodes,
                IsVerbose = true,
                Output = new Vector() {Name = name, Type = type}
            };
        }


    }
}
