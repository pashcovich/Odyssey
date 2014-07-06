using SharpDX;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [YamlTag("Parent")]
    public class ParentComponent:Component
    {
        public IEntity Entity { get; set; }

        public ParentComponent() : base(ComponentTypeManager.GetType<ParentComponent>())
        {
        }
    }
}
