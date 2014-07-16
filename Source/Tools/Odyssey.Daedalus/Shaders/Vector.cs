using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Math;
using SharpDX.Serialization;
using System;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Daedalus.Shaders
{
    public enum Swizzle
    {
        Null = 0,
        X = 1,
        Y = 2,
        Z = 3,
        W = 4
    }

    public sealed partial class Vector : Variable, IValueVariable
    {
        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        [SupportedType(Type.Vector)]
        public override Type Type
        {
            get
            {
                if (base.Type == Type.None)
                {
                    Type = Value != null ? TypeFromArrayLength(Value.Length) : Type.Vector;
                }

                return base.Type;
            }
            set
            {
                var attributes = this.GetType().GetProperty("Type").GetCustomAttributes(true).OfType<SupportedTypeAttribute>();
                if (attributes.All(att => att.SupportedType != value))
                    throw new InvalidOperationException("Cannot assign a non-Vector type to this variable");
                base.Type = value;
            }
        }

        public float[] Value { get; set; }

        public override void Serialize(BinarySerializer serializer)
        {
            base.Serialize(serializer);
            float[] value = Value;
            serializer.Serialize(ref value, serializer.Serialize);
            if (serializer.Mode == SerializerMode.Read)
                Value = value;
        }

        public override string Definition
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(Comments))
                    foreach (string commentLine in Comments.Split('\n'))
                        sb.AppendLine(string.Format("\t// {0}", commentLine));

                foreach (var kvp in Markup)
                    sb.AppendLine(string.Format("\t// {0} = <{1}>", kvp.Key, kvp.Value));

                sb.Append("\t");

                if (IsConstant)
                    sb.Append("static const ");

                sb.AppendFormat("{0} {1}", Mapper.Map(Type), Name);
                if (!string.IsNullOrEmpty(Semantic))
                {
                    sb.AppendFormat(": {0}", Semantic);
                    if (Index.HasValue)
                        sb.Append(Index);
                }
                if (Value != null)
                    sb.AppendFormat(" = {0}", ((IValueVariable)this).PrintArray());

                sb.Append(";\n");

                return sb.ToString();
            }
        }

        string IValueVariable.PrintArray()
        {
            string array = Value.Aggregate(string.Empty, (current, v) => current + string.Format("{0}f, ", v));
            array = array.Remove(array.Length - 2);
            array = string.Format("{0}({1})", Mapper.Map(Type), array);
            return array;
        }

        internal static Type TypeFromArrayLength(int length)
        {
            switch (length)
            {
                case 1:
                    return Type.Float;

                case 2:
                    return Type.Float2;

                case 3:
                    return Type.Float3;

                case 4:
                    return Type.Float4;

                default:
                    throw new InvalidOperationException("length");
            }
        }
    }
}