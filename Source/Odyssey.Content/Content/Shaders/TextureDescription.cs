using Odyssey.Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content.Shaders
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
        public UpdateFrequency UpdateFrequency { get; private set; }

        public TextureDescription(int index, string resourceKey, TextureReference texture, int samplerIndex, UpdateFrequency frequency = UpdateFrequency.Static)
        {
            Index = index;
            Key = resourceKey;
            Texture = texture;
            SamplerIndex = samplerIndex;
            UpdateFrequency = frequency;
        }

    }
}
