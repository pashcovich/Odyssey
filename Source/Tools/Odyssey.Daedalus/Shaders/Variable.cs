
// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
// 
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
// 
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Odyssey.Daedalus.Serialization;
using Odyssey.Daedalus.Shaders.Structs;
using SharpDX.Serialization;
using EngineReference = Odyssey.Engine.EngineReference;

namespace Odyssey.Daedalus.Shaders
{
    [DataContract(IsReference = true)]
    public abstract partial class Variable : IEquatable<Variable>, IVariable, IDataSerializable
    {

        internal static readonly Dictionary<string, int> VariableCounter = new Dictionary<string, int>();
        private string comments;
        private EngineReference engineReference;
        private string id;
        private bool isConstant;
        private Dictionary<string, string> markupData;
        private string name;
        private string semantic;
        private Type type;

        protected Variable()
        {
            markupData = new Dictionary<string, string>();
            string type = GetType().Name;
            if (!VariableCounter.ContainsKey(type))
                VariableCounter.Add(type, 1);
            
            id = String.Format("{0}{1}", type, VariableCounter[type]++);
            name = id;
        }

        public string Comments
        {
            get { return comments; }
            set { comments = value; }
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

        public EngineReference EngineReference
        {
            get { return engineReference; }
            internal set { engineReference = value; }
        }

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

        public bool HasMarkup
        {
            get { return markupData.Any(); }
        }

        public string Id { get { return id; } }
        public int? Index { get; set; }

        public bool IsConstant
        {
            get { return isConstant; }
            set { isConstant = value; }
        }

        IStruct IVariable.Owner
        {
            get { return Owner; }
            set { Owner = value; }
        }

        public IEnumerable<KeyValuePair<string, string>> Markup
        {
            get { return markupData; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public IStruct Owner { get; internal set; }

        public string Semantic
        {
            get { return semantic; }
            set { semantic = value; }
        }

        public virtual Type Type
        {
            get { return type; }
            set { type = value; }
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

        public static bool operator !=(Variable left, Variable right)
        {
            return !(left == right);
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

        [Pure]
        public bool ContainsMarkup(string key)
        {
            return markupData.ContainsKey(key);
        }

        public bool Equals(Variable other)
        {
            return (Type == other.Type) && (Name == other.Name) && (Semantic == other.Semantic) && (Index == other.Index);
        }

        // <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Variable)) return false;
            return Equals((Variable)obj);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() + Name.GetHashCode() + Semantic.GetHashCode() + Index.GetHashCode();
        }

        public string GetMarkupValue(string key)
        {
            Contract.Requires<ArgumentException>(ContainsMarkup(key), "Variable does not contain requested markup.");
            return markupData[key];
        }

        public virtual void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref id);
            serializer.Serialize(ref name);
            serializer.SerializeEnum(ref type);
            serializer.Serialize(ref comments, SerializeFlags.Nullable);
            serializer.Serialize(ref semantic, SerializeFlags.Nullable);
            int index = Index ?? -1;
            serializer.Serialize(ref index);
            serializer.Serialize(ref isConstant);
            serializer.Serialize(ref engineReference, SerializeFlags.Nullable);
            serializer.Serialize(ref markupData, serializer.Serialize, serializer.Serialize);
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

        public override string ToString()
        {
            return Definition;
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

        internal static string GetRegister(IVariable variable)
        {
            string prefix = GetPrefix(variable.Type);
            return String.Format("{0}{1}", prefix, variable.Index);
        }

        internal static IVariable InitVariable(string name, Type type, string semantic = null, string customType = CustomType.None)
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
                    return new ConstantBuffer {Name = name, Type = type};

                case Type.Texture2D:
                case Type.Texture3D:
                case Type.TextureCube:
                    return new Texture() {Name = name, Type = type};

                case Type.Sampler:
                case Type.SamplerComparisonState:
                    return new Sampler() {Name = name, Type = type};
            }

            return variable;
        }

        internal static Variable ReadVariable(BinarySerializer serializer)
        {
            ShaderGraphSerializer sg = (ShaderGraphSerializer)serializer;

            bool isNewVar =false;
            serializer.Serialize(ref isNewVar);
            if (isNewVar)
            {
                string outputType = null;
                serializer.Serialize(ref outputType);
                var variable = (Variable)Activator.CreateInstance(System.Type.GetType(outputType));
                variable.Serialize(serializer);
                sg.MarkVariableAsParsed(variable);
                return variable;
            }
            else
            {
                string varId = null;
                serializer.Serialize(ref varId);
                if (sg.IsVariableParsed(varId))
                    return sg.GetVariable(varId);
                else throw new InvalidOperationException(string.Format("Variable '{0}' not found", varId));
            }
        }

        internal static void WriteVariable(BinarySerializer serializer, IVariable variable)
        {
            ShaderGraphSerializer sg = (ShaderGraphSerializer)serializer;

            Variable v = (Variable)variable;

            // Is this node new one or a reference to another one encountered before?
            bool isNewVar = !sg.IsVariableParsed(v.Id);
            serializer.Serialize(ref isNewVar);
            if (!isNewVar)
            {
                string varName = v.Id;
                serializer.Serialize(ref varName);
            }
            else
            {
                string outputType = v.GetType().FullName;
                serializer.Serialize(ref outputType);
                v.Serialize(serializer);
                sg.MarkVariableAsParsed(v);
            }

        }

    }
}