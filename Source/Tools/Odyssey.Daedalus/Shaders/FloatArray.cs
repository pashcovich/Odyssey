using System.Text;
using Odyssey.Daedalus.Shaders.Nodes;
using System;
using System.Linq;
using Odyssey.Serialization;

namespace Odyssey.Daedalus.Shaders
{
    public sealed partial class FloatArray
    {
        private float[] value;
        private Type arrayItemType;
        private int length;

        public FloatArray()
        {
            Type = Type.FloatArray;
        }

        [SupportedType(Type.FloatArray)]
        public override Type Type
        {
            get { return base.Type; }
            set
            {
                var attributes =
                    this.GetType().GetProperty("Type").GetCustomAttributes(true).OfType<SupportedTypeAttribute>();
                if (attributes.All(att => att.SupportedType != value))
                    throw new InvalidOperationException("Can only assign a FloatArray type to this variable");
                base.Type = value;


            }
        }

        public Type ArrayItemType
        {
            get { return arrayItemType; }
            set { arrayItemType = value; }
        }

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public float[] Value
        {
            get { return value; }
            set
            {
                if (Length != 0 && value.Length != Length * ComponentsFromType(Type))
                    throw new InvalidOperationException("Value must match array length");
                else if (Length == 0)
                {
                    Length = value.Length;
                }
                this.value = value; 
                
            }
        }

        public override void Serialize(BinarySerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Serialize(ref value, serializer.Serialize, SerializeFlags.Nullable);
            serializer.SerializeEnum(ref arrayItemType);
            serializer.Serialize(ref length);
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

                sb.AppendFormat("{0} {1}[{2}]", Mapper.Map(ArrayItemType), Name, Length);
                if (!string.IsNullOrEmpty(Semantic))
                {
                    sb.AppendFormat(": {0}", Semantic);
                    if (Index.HasValue)
                        sb.Append(Index);
                }
                if (Value != null)
                    sb.AppendFormat(" = {0}", ((IValueVariable) this).PrintArray());

                sb.Append(";\n");

                return sb.ToString();
            }
        }


        string IValueVariable.PrintArray()
        {
            string array = Value.Aggregate(string.Empty, (current, v) => current + string.Format("{0}f, ", v));
            array = array.Remove(array.Length - 2);
            array = string.Format("{{{0}}}", array);
            return array;
        }

    }
}
