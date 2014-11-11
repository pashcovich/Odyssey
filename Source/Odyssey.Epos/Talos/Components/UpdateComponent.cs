using SharpYaml.Serialization;

namespace Odyssey.Epos.Components
{
    public enum UpdateFrequency
    {
        Static,
        RealTime
    }

    [YamlTag("Update")]
    public class UpdateComponent : Component
    {

        [YamlMember(1)]
        public UpdateFrequency UpdateFrequency { get; set; }
        [YamlIgnore] public bool RequiresUpdate { get; set; }

        public UpdateComponent() : base(ComponentTypeManager.GetType<UpdateComponent>())
        {
            RequiresUpdate = true;
        }

    }
}
