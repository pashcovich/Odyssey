using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Structs
{
    public partial class Struct : Variable, IStruct, IContainer
    {
        private readonly VariableCollection variables;

        private string customType;

        public string CustomType
        {
            get { return customType; }
            protected internal set { customType = value; }
        }

        public Struct()
        {
            Type = Type.Struct;
            variables = new VariableCollection { Owner = this };
        }

        public int VariableCount { get { return variables.Count; } }

        public IEnumerable<IVariable> Variables { get { return variables.Values; } }

        public IVariable this[string key]
        {
            get
            {
                Contract.Requires<KeyNotFoundException>(Contains(key), "Node not found");
                return variables[key];
            }
        }

        public IVariable this[int index]
        {
            get
            {
                Contract.Requires<ArgumentOutOfRangeException>(VariableCount > index);
                return variables.ElementAt(index).Value;
            }
        }

        public override string Definition
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(String.Format("{0} {1}", Mapper.Map(Type), CustomType));
                sb.AppendLine("{");
                foreach (IVariable variable in variables.Select(kvp => kvp.Value))
                {
                    sb.Append(variable);
                }
                sb.AppendLine("};");
                return sb.ToString();
            }
        }

        public virtual string Declaration
        {
            get
            {
                return String.Format("\t{0} {1};\n", CustomType, Name);
            }
        }

        public virtual void Add(IVariable variable)
        {
            Contract.Requires<ArgumentException>(!Contains(variable));
            variable.Owner = this;

            if (variable.Semantic != string.Empty)
            {
                var semanticVars = variables.Where(kvp => kvp.Value.Semantic == variable.Semantic).ToArray();
                if (semanticVars.Length > 0)
                {
                    variable.Index = semanticVars.Length;
                    semanticVars[0].Value.Index = 0;
                }
            }
            variables.Add(variable.Name, variable);
        }

        [Pure]
        public bool Contains(IVariable variable)
        {
            return Contains(variable.Name);
        }

        [Pure]
        public bool Contains(string variableName)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(variableName));
            return variables.ContainsKey(variableName);
        }

        public override string ToString()
        {
            return Definition;
        }

        public override void Serialize(BinarySerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Serialize(ref customType);

            variables.Serialize(serializer);

            if (serializer.Mode != SerializerMode.Read) return;
            foreach (var kvp in variables)
                kvp.Value.Owner = this;
        }
    }
}