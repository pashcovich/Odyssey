using Odyssey.Graphics.Rendering.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ShaderFeatureAttribute : Attribute
    {
        ShaderFeature feature;

        public ShaderFeature Feature { get { return feature; } }

        public ShaderFeatureAttribute(ShaderFeature feature)
        {
            this.feature = feature;
        }
    }
}
