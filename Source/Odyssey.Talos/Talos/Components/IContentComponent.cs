using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    public interface IContentComponent
    {
        [YamlMember(1)]
        string AssetName { get; set; }
    }
}