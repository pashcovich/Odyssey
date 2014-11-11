using SharpDX;

namespace Odyssey.Epos.Components
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
