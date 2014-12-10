#region Using Directives

using System.ComponentModel;
using SharpYaml.Serialization;

#endregion

namespace Odyssey.Content
{
    public class AssetIdentifier
    {
        public AssetIdentifier()
        {
        }

        public AssetIdentifier(string name, string path, string type, AssetOperation assetOperation = AssetOperation.None)
        {
            Name = name;
            Path = path;
            Type = type;
            Operation = assetOperation;
        }

        [YamlMember("name")]
        public string Name { get; set; }

        [YamlMember("path")]
        public string Path { get; set; }

        [YamlMember("type")]
        public string Type { get; set; }

        [YamlMember("operation")]
        [DefaultValue(null)]
        public AssetOperation Operation { get; set; }
    }
}