namespace Odyssey.Epos.Components
{
    public class GlowComponent : Component
    {
        public float GlowStrength { get; set; }

        public GlowComponent()
            : base(ComponentTypeManager.GetType<GlowComponent>())
        {
        }
    }
}
