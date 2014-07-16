using System;
using Odyssey.Graphics.Effects;

namespace Odyssey.Daedalus.Shaders
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class PixelShaderAttribute : Attribute
    {
        public PixelShaderFlags Features { get; private set; }

        public PixelShaderAttribute(PixelShaderFlags psFeatures)
        {
            Features = psFeatures;
        }
    }
}
