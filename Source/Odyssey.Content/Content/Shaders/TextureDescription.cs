using Odyssey.Graphics.Materials;
using System.Runtime.Serialization;

namespace Odyssey.Graphics.Materials
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

        public TextureDescription(int index, string resourceKey, TextureReference texture, int samplerIndex, UpdateType frequency = UpdateType.Scene)
        {
            Index = index;
            Key = resourceKey;
            Texture = texture;
            SamplerIndex = samplerIndex;
            UpdateFrequency = frequency;
        }

    }
}
