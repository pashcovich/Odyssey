namespace Odyssey.Talos.Components
{
    public class ControllerComponent : Component
    {
        public IEntityController Controller { get; set; }

        public ControllerComponent() : base(ComponentTypeManager.GetType<ControllerComponent>())
        {
        }
    }
}
