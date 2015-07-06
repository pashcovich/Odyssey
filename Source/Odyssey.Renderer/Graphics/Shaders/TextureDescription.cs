using Odyssey.Graphics.Effects;
using Odyssey.Serialization;

namespace Odyssey.Graphics.Shaders
{
    public struct TextureDescription : IDataSerializable
    {
        public int Index;
        public int SamplerIndex;
        public string Key;
        public string Texture;
        public CBUpdateType CbUpdateFrequency;
        public ShaderType ShaderType;

        public TextureDescription(int index, string resourceKey, string texture, int samplerIndex, CBUpdateType frequency = CBUpdateType.SceneStatic, ShaderType shaderType = ShaderType.Pixel)
        {
            Index = index;
            Key = resourceKey;
            Texture = texture;
            SamplerIndex = samplerIndex;
            CbUpdateFrequency = frequency;
            ShaderType = shaderType;
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref Index);
            serializer.Serialize(ref SamplerIndex);
            serializer.Serialize(ref Key);
            serializer.Serialize(ref Texture);
            serializer.SerializeEnum(ref CbUpdateFrequency);
            serializer.SerializeEnum(ref ShaderType);
        }
    }
}
