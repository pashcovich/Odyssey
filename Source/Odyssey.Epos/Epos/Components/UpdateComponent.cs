using SharpYaml.Serialization;

namespace Odyssey.Epos.Components
{
    public enum UpdateFrequency
    {
        Static,
        RealTime
    }

    public class UpdateComponent : Component
    {

        public UpdateFrequency UpdateFrequency { get; set; }
        public bool RequiresUpdate { get; set; }

        public UpdateComponent() : base(ComponentTypeManager.GetType<UpdateComponent>())
        {
            RequiresUpdate = true;
        }

    }
}
