namespace Odyssey.Epos.Components
{
    public class RelationComponent : Component
    {
        public Entity Target { get; set; }

        public RelationComponent()
            : base(ComponentTypeManager.GetType<RelationComponent>())
        {
        }
    }
}
