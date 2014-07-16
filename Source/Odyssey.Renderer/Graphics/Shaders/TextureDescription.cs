using System.Runtime.Serialization;
using Odyssey.Graphics.Effects;
using SharpDX.Serialization;

namespace Odyssey.Graphics.Shaders
{
    public struct TextureDescription : IDataSerializable
    {
        public int Index;
        public int SamplerIndex;
        public string Key;
        public string Texture;
        public UpdateType UpdateFrequency;
        public ShaderType ShaderType;

        public TextureDescription(int index, string resourceKey, string texture, int samplerIndex, UpdateType frequency = UpdateType.SceneStatic, ShaderType shaderType = ShaderType.Pixel)
        {
            Index = index;
            Key = resourceKey;
            Texture = texture;
            SamplerIndex = samplerIndex;
            UpdateFrequency = frequency;
            ShaderType = shaderType;
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref Index);
            serializer.Serialize(ref SamplerIndex);
            serializer.Serialize(ref Key);
            serializer.Serialize(ref Texture);
            serializer.SerializeEnum(ref UpdateFrequency);
            serializer.SerializeEnum(ref ShaderType);
        }
    }
}
