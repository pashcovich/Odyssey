using SharpYaml.Serialization;

namespace Odyssey.Epos.Components
{
    public interface IContentComponent
    {
        [YamlMember(1)]
        string AssetName { get; set; }
    }
}