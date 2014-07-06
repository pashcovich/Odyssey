using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Odyssey.Talos.Components
{
    public class SpriteComponent : Component
    {
        public float ScaleFactor { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public SpriteComponent() : base(ComponentTypeManager.GetType<SpriteComponent>())
        {
            ScaleFactor = 1;
        }
    }
}
