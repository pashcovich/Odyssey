using System.Runtime.InteropServices;
using SharpDX;

namespace Odyssey.Graphics.Effects
{
    [StructLayout(LayoutKind.Explicit)]
    public class MaterialBuffer
    {
        [FieldOffset(0)]
        public float kA;
        [FieldOffset(4)]
        public float kD;
        [FieldOffset(8)]
        public float kS;
        [FieldOffset(12)]
        public float sPower;
        [FieldOffset(16)]
        public Color4 Ambient;
        [FieldOffset(32)]
        public Color4 Diffuse;
        [FieldOffset(48)]
        public Color4 Specular;

        public MaterialBuffer(float kA, float kD, float kS, float sP, Color4 ambient, Color4 diffuse, Color4 specular)
        {
            this.kA = kA;
            this.kD= kD;
            this.kS= kS;
            sPower= sP;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
        }




        
    }
}
