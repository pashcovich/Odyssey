using System.Runtime.Serialization;
using SharpDX.Direct3D11;
using Odyssey.Serialization;

namespace Odyssey.Graphics.Shaders
{
    [DataContract]
    public struct SamplerStateDescription : IDataSerializable
    {
        public int Index;
        public string Name;
        public Filter Filter;
        public TextureAddressMode TextureAddressMode;
        public Comparison Comparison;

        public SamplerStateDescription(int index, string name, Filter filter, Comparison comparison, TextureAddressMode textureAddressMode)
        {
            Index = index;
            Name = name;
            Filter = filter;
            Comparison = comparison;
            TextureAddressMode = textureAddressMode;
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref Name);
            serializer.SerializeEnum(ref Filter);
            serializer.SerializeEnum(ref TextureAddressMode);
            serializer.SerializeEnum(ref Comparison);
        }
    }
}
