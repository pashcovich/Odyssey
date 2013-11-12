using Odyssey.Content.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Structs
{
    [YamlMapping(typeof(YamlStruct))]
    [DataContract]
    public partial class Struct : Variable, IStruct
    {
        [DataMember]
        Dictionary<string, Variable> variables;

        [DataMember]
        public CustomType CustomType { get; internal protected set; }
        
        public Struct()
        {
            Type = Shaders.Type.Struct;
            variables = new Dictionary<string, Variable>();
        }

        public int VariableCount { get { return variables.Count; } }
        public IEnumerable<Variable> Variables { get { return variables.Values; } }

        public Variable this[string key]
        {
            get
            {
                Contract.Requires<KeyNotFoundException>(Contains(key));
                return variables[key];
            }
        }

        public override string Definition
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(string.Format("{0} {1}", Mapper.Map(Type), Mapper.Map(CustomType)));
                sb.AppendLine("{");
                foreach (var kvp in variables)
                {
                    Variable variable = kvp.Value;
                    sb.Append("\t");
                    sb.Append(variable.ToString());
                }
                sb.AppendLine("};");
                return sb.ToString();
            }
        }

        public virtual string Declaration
        {
            get
            {
                return string.Format("{0} {1};\n", Mapper.Map(CustomType), Name);
            }
        }

        public virtual void Add(Variable variable)
        {
            Contract.Requires<ArgumentException>(!Contains(variable));
            variable.Owner = this;
            
            if (variable.Semantic != Shaders.Semantic.None)
            {
                var semanticVars = variables.Where(kvp => kvp.Value.Semantic == variable.Semantic);
                variable.Index = semanticVars.Count();
            }
            variables.Add(variable.Name, variable);
        }

        public bool Contains(Variable variable)
        {
            return Contains(variable.Name);
        }

        public bool Contains(Semantic semantic)
        {
            Contract.Requires<ArgumentException>(semantic != Semantic.None);
            foreach (var kvp in variables)
            {
                Variable v = kvp.Value;
                if (v.Semantic == semantic)
                    return true;
            }
            return false;
        }

        public bool Contains(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(variableName));
            return variables.ContainsKey(variableName);
        }

        public override string ToString()
        {
            return Definition;
        }

    }
}