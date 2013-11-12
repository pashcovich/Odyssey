using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using Odyssey.Utils;
using ShaderGenerator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    [DataContract(IsReference = true)]
    [KnownType("KnownTypes")]
    public abstract partial class Variable : IEquatable<Variable>, IVariable, IYamlVariable
    {
        internal static Dictionary<string, int> NodeCounter = new Dictionary<string, int>();

        [DataMember]
        private Dictionary<string, string> markupData;
        [DataMember]
        public virtual Type Type { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public ShaderReference ShaderReference { get; internal set; }

        [DataMember]
        public Semantic Semantic { get; set; }
        [DataMember]
        public int? Index { get; set; }
        [DataMember]
        public IStruct Owner { get; internal set; }

        public IEnumerable<KeyValuePair<string, string>> Markup { get { return markupData; } }

        public string FullName
        {
            get
            {
                if (Owner == null || Owner.Type == Shaders.Type.ConstantBuffer)
                    return Name;
                else
                    return string.Format("{0}.{1}", Owner.Name, Name);
            }
        }

        public bool IsEmpty
        {
            get { return this == default(Variable); }
        }

        public virtual string Definition
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(Comments))
                    foreach (string commentLine in Comments.Split('\n'))
                        sb.AppendLine(string.Format("// {0}", commentLine));
                foreach (var kvp in Markup)
                    sb.AppendLine(string.Format("// {0} = <{1}>", kvp.Key, kvp.Value));

                sb.AppendFormat("{0} {1}", Mapper.Map(Type), Name);
                if (Semantic != Semantic.None)
                {
                    sb.AppendFormat(": {0}", Mapper.Map(Semantic));
                    if (Index != -1)
                        sb.Append(Index);
                }
                else if (Index.HasValue)
                    sb.Append(string.Format(" : {0}", GetRegister(this)));
                sb.Append(";\n");

                return sb.ToString();
            }
        }

        public Variable()
        {
            markupData = new Dictionary<string, string>();
            string type = this.GetType().Name;
            if (!NodeCounter.ContainsKey(type))
                NodeCounter.Add(type, 0);

            Name = string.Format("{0}{1}", type, NodeCounter[type]++);
        }

        #region Equality
        public bool Equals(Variable other)
        {
            return (Type == other.Type) && (Name == other.Name) && (Semantic == other.Semantic) && (Index == other.Index);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() + Name.GetHashCode() + Semantic.GetHashCode() + Index.GetHashCode();
        }

        // <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Variable)) return false;
            return Equals((Variable)obj);
        }

        public static bool operator ==(Variable left, Variable right)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(left, right))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)left == null) || ((object)right == null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Variable left, Variable right)
        {
            return !(left == right);
        }
        #endregion

        public override string ToString()
        {
            return Definition;
        }

        public bool HasMarkup
        {
            get { return markupData.Any(); }
        }

        public bool ContainsMarkup(string key)
        {
            return markupData.ContainsKey(key);
        }

        public void SetMarkup(string key, string value)
        {
            markupData[key] = value;
        }

        public void SetMarkup(string key, object value)
        {
            SetMarkup(key, value.ToString());
        }

        public void SetMarkup(IEnumerable<KeyValuePair<string, string>> values)
        {
            Contract.Requires<ArgumentNullException>(values != null);
            foreach (var kvp in values)
                SetMarkup(kvp.Key, kvp.Value);
        }

        public string GetMarkupValue(string key)
        {
            Contract.Requires<ArgumentException>(ContainsMarkup(key), "Variable does not contain requested markup.");
            return markupData[key];
        }

        public YamlVariable ToYaml()
        {
            var attribute = ReflectionHelper.GetAttribute<YamlMappingAttribute>(this.GetType());
            return (YamlVariable)Activator.CreateInstance(attribute.MatchingType, new[] { this });
        }

        internal static string GetRegister(IVariable variable)
        {
            string prefix = GetPrefix(variable.Type);
            return string.Format("{0}{1}", prefix, variable.Index);
        }

        internal static string GetPrefix(Type type)
        {
            string prefix = string.Empty;
            switch (type)
            {
                case Shaders.Type.ConstantBuffer:
                    prefix = "b";
                    break;

                case Shaders.Type.Texture2D:
                case Shaders.Type.Texture3D:
                case Shaders.Type.TextureCube:
                    prefix = "t";
                    break;

                case Shaders.Type.SamplerComparisonState:
                case Shaders.Type.Sampler:
                    prefix = "s";
                    break;

                case Type.Matrix:
                    prefix = "m";
                    break;

                case Type.Float:
                    prefix = "f";
                    break;

                case Type.Float2:
                case Type.Float3:
                case Type.Float4:
                    prefix = "v";
                    break;

                default:
                    prefix = "x";
                    break;
            }

            return prefix;
        }

        static System.Type[] KnownTypes()
        {
            return new[] {typeof(SharpDX.Direct3D11.Filter),
                typeof(SharpDX.Direct3D11.TextureAddressMode),
                typeof(SharpDX.Direct3D11.Comparison),
                typeof(Odyssey.Graphics.Materials.TextureReference),
                typeof(Odyssey.Graphics.Materials.EngineReference)
            };
        }

        internal static IVariable InitVariable(string name, Type type, CustomType customType = CustomType.None)
        {
            IVariable variable = null;
            switch (type)
            {
                case Type.Struct:
                    variable = new Struct
                    {
                        Name = name,
                        Type = type,
                        CustomType = customType
                    };
                    break;

                case Type.Float:
                case Type.Float2:
                case Type.Float3:
                case Type.Float4:
                    variable = new Vector
                    {
                        Name = name,
                        Type = type
                    };
                    break;

                case Type.Matrix:
                    variable = new Matrix
                    {
                        Name = name,
                        Type = type
                    };
                    break;
            }

            return variable;
        }

        IStruct IVariable.Owner
        {
            get { return Owner; }
            set { Owner = value; }
        }

    }
}
