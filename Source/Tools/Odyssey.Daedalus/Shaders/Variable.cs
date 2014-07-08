using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Tools.ShaderGenerator.Serialization;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;

using Odyssey.Utilities;
using SharpDX.Direct3D11;
using SharpDX.Serialization;
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
    public abstract partial class Variable : IEquatable<Variable>, IVariable, IDataSerializable
    {
        internal static Dictionary<string, int> VariableCounter = new Dictionary<string, int>();

        private readonly Dictionary<string, string> markupData;

        public virtual Type Type { get; set; }

        public string Name { get; set; }

        public string Comments { get; set; }

        public ShaderReference ShaderReference { get; internal set; }

        public string Semantic { get; set; }

        public int? Index { get; set; }

        public IStruct Owner { get; internal set; }

        public bool IsConstant { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Markup { get { return markupData; } }

        public virtual string FullName
        {
            get
            {
                if (Owner == null || Owner.Type == Type.ConstantBuffer)
                    return Name;
                else
                    return String.Format("{0}.{1}", Owner.Name, Name);
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

                if (!String.IsNullOrEmpty(Comments))
                    foreach (string commentLine in Comments.Split('\n'))
                        sb.AppendLine(String.Format("\t// {0}", commentLine));

                foreach (var kvp in Markup)
                    sb.AppendLine(String.Format("\t// {0} = <{1}>", kvp.Key, kvp.Value));

                sb.AppendFormat("\t{0} {1}", Mapper.Map(Type), Name);
                if (!String.IsNullOrEmpty(Semantic))
                {
                    sb.AppendFormat(": {0}", Semantic);
                    if (Index.HasValue)
                        sb.Append(Index);
                }

                sb.Append(";\n");

                return sb.ToString();
            }
        }

        protected Variable()
        {
            markupData = new Dictionary<string, string>();
            string type = GetType().Name;
            if (!VariableCounter.ContainsKey(type))
                VariableCounter.Add(type, 0);

            Name = String.Format("{0}{1}", type, VariableCounter[type]++);
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
            if (ReferenceEquals(left, right))
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

        #endregion Equality

        public override string ToString()
        {
            return Definition;
        }

        public bool HasMarkup
        {
            get { return markupData.Any(); }
        }

        [Pure]
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

        internal static string GetRegister(IVariable variable)
        {
            string prefix = GetPrefix(variable.Type);
            return String.Format("{0}{1}", prefix, variable.Index);
        }

        internal static string GetPrefix(Type type)
        {
            string prefix;
            switch (type)
            {
                case Type.ConstantBuffer:
                    prefix = "b";
                    break;

                case Type.Texture2D:
                case Type.Texture3D:
                case Type.TextureCube:
                    prefix = "t";
                    break;

                case Type.SamplerComparisonState:
                case Type.Sampler:
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

        private static System.Type[] KnownTypes()
        {
            return new[] {typeof(Filter),
                typeof(TextureAddressMode),
                typeof(Comparison),
                typeof(TextureReference),
                typeof(EngineReference)
            };
        }

        internal static IVariable InitVariable(string name, Type type, string semantic = null, string customType = Shaders.CustomType.None)
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
                        Type = type,
                        Semantic = semantic
                    };
                    break;

                case Type.Matrix:
                    variable = new Matrix
                    {
                        Name = name,
                        Type = type
                    };
                    break;

                case Type.ConstantBuffer:
                    return new ConstantBuffer { Name = name, Type = type };

                case Type.Texture2D:
                case Type.Texture3D:
                case Type.TextureCube:
                    return new Texture() { Name = name, Type = type };

                case Type.Sampler:
                case Type.SamplerComparisonState:
                    return new Sampler() { Name = name, Type = type };
            }

            return variable;
        }

        IStruct IVariable.Owner
        {
            get { return Owner; }
            set { Owner = value; }
        }

        public virtual void Serialize(BinarySerializer serializer)
        {
            string name = Name;
            serializer.Serialize(ref name);

            Type type = Type;
            serializer.SerializeEnum(ref type);

            string comments = Comments;
            serializer.Serialize(ref comments, SerializeFlags.Nullable);

            string semantic = Semantic;
            serializer.Serialize(ref semantic, SerializeFlags.Nullable);

            int index = Index ?? -1;
            serializer.Serialize(ref index);

            bool isConstant = IsConstant;
            serializer.Serialize(ref isConstant);

            var shaderReference = ShaderReference;
            serializer.Serialize(ref shaderReference, SerializeFlags.Nullable);

            var markupKvp = markupData.ToList();
            int count = markupKvp.Count;
            serializer.Serialize(ref count);

            for (int i = 0; i < count; i++)
            {
                string key = String.Empty;
                string value = String.Empty;

                if (serializer.Mode == SerializerMode.Write)
                {
                    var kvp = markupKvp[i];
                    key = kvp.Key;
                    value = kvp.Value;
                }

                serializer.Serialize(ref key);
                serializer.Serialize(ref value);

                if (serializer.Mode == SerializerMode.Read)
                    markupData.Add(key, value);
            }

            if (serializer.Mode == SerializerMode.Read)
            {
                Name = name;
                Type = type;
                Comments = comments;
                Semantic = semantic;
                if (index != -1)
                    Index = index;
                IsConstant = isConstant;
                ShaderReference = shaderReference;
                //var sg = (ShaderGraphSerializer)serializer;

                //if (owner == null) return;

                //if (sg.IsParsed(owner.Name))
                //    Owner = (Struct) sg.GetVariable(owner.Name);
                //else
                //{
                //    Owner = owner;
                //    sg.MarkAsParsed(owner);
                //}
            }
        }

        internal static void WriteVariable(BinarySerializer serializer, IVariable variable)
        {
            string outputType = variable.GetType().FullName;
            Variable var = (Variable)variable;
            serializer.Serialize(ref outputType);
            var.Serialize(serializer);
        }

        internal static Variable ReadVariable(BinarySerializer serializer)
        {
            string outputType = null;
            serializer.Serialize(ref outputType);
            Variable variable = (Variable)Activator.CreateInstance(System.Type.GetType(outputType));
            variable.Serialize(serializer);
            return variable;
        }

        public static int ComponentsFromType(Type type)
        {
            int components = 0;
            switch (type)
            {
                case Type.Float:
                    components = 1;
                    break;

                case Type.Float2:
                    components = 2;
                    break;

                case Type.Float3:
                    components = 3;
                    break;

                case Type.Float4:
                    components = 4;
                    break;
            }

            return components;
        }
    }
}