using System;
using System.Runtime.Serialization;
using Odyssey.Graphics.Effects;
using SharpDX.Serialization;

namespace Odyssey.Graphics
{
    [DataContract]
    public class ShaderReference : IDataSerializable
    {
        private int index;
        private ReferenceType type;
        private object value;

        [DataMember]
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        [DataMember]
        public ReferenceType Type
        {
            get { return type; }
        }

        [DataMember]
        public object Value
        {
            get { return value; }
        }

        protected internal ShaderReference(ReferenceType type, object value, int index = 0)
        {
            this.index = index;
            this.type = type;
            this.value = value;
        }

        public ShaderReference()
        {}

        public ShaderReference(EngineReference reference)
            : this(ReferenceType.Entity, reference)
        {}

        public ShaderReference(TextureReference reference)
            : this(ReferenceType.Texture, reference)
        { }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref index);
            serializer.SerializeEnum(ref type);

            string enumType = string.Empty; 
            string enumValue = string.Empty;
            if (serializer.Mode == SerializerMode.Write)
            {
                enumType = value.GetType().FullName;
                enumValue=value.ToString();
            }
            serializer.Serialize(ref enumType);
            serializer.Serialize(ref enumValue);
            if (serializer.Mode == SerializerMode.Read)
                value = Enum.Parse(System.Type.GetType(string.Format("{0},{1}",enumType,"Odyssey.Common")), enumValue);
        }
    }
}
