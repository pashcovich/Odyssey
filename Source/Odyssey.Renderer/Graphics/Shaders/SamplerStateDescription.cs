using System.Runtime.Serialization;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics.Shaders
{
    [DataContract]
    public class SamplerStateDescription
    {
        [DataMember]
        public int Index { get; internal set; }
        [DataMember]
        public Filter Filter { get; internal set; }
        [DataMember]
        public TextureAddressMode TextureAddressMode { get; internal set; }
        [DataMember]
        public Comparison Comparison { get; internal set;}
    }
}
