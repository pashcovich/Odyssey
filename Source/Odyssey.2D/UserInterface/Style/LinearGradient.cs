using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Style
{
    public class LinearGradient : IGradient
    {
        public string Name { get; set; }

        public GradientType Type { get; set; }

        public GradientStop[] GradientStops { get; set; }

        public LinearGradient()
        {
        }

        public static LinearGradient CreateUniform(Color4 color)
        {
            return new LinearGradient { Name = "DefaultUniform", GradientStops = new[] { new GradientStop(color, 0), new GradientStop(color, 1.0f) }, };
        }
      
    }
}
