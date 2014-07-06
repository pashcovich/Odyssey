using System.Runtime.Serialization;
using Odyssey.Graphics.Effects;

namespace Odyssey.Graphics.Shaders
{
    [DataContract]
    public class TextureDescription
    {
        [DataMember]
        public int Index { get; private set; }
        [DataMember]
        public int SamplerIndex { get; private set; }
        [DataMember]
        public string Key { get; private set; }
        [DataMember]
        public TextureReference Texture { get; private set; }
        [DataMember]
        public UpdateType UpdateFrequency { get; private set; }
        [DataMember]
        public ShaderType ShaderType { get; private set; }

        public TextureDescription(int index, string resourceKey, TextureReference texture, int samplerIndex, UpdateType frequency = UpdateType.SceneStatic, ShaderType shaderType = ShaderType.Pixel)
        {
            Index = index;
            Key = resourceKey;
            Texture = texture;
            SamplerIndex = samplerIndex;
            UpdateFrequency = frequency;
            ShaderType = shaderType;
        }

    }
}
