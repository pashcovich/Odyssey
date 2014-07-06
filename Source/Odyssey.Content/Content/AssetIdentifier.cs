using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SharpYaml.Serialization;

namespace Odyssey.Content
{
    
    public class AssetIdentifier
    {
        [YamlMember("name")]
        public string Name { get; set; }
        [YamlMember("path")]
        public string Path { get; set; }

        [YamlMember("type")]
        public AssetType Type { get; set; }

        public AssetIdentifier()
        { }

        public AssetIdentifier(string name, string path, AssetType type)
        {
            Name = name;
            Path = path;
            Type = type;
        }
    }
}
