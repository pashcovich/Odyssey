using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using ShaderGenerator.Data;
using System.Collections.Generic;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public interface IVariable
    {
        string Name { get; }
        string FullName { get; }
        string Definition { get; }
        string Comments { get; }
        IEnumerable<KeyValuePair<string, string>> Markup { get; }
        Type Type { get; }
        Semantic Semantic { get; }
        int? Index { get; set; }
        IStruct Owner { get; set; }
        ShaderReference ShaderReference { get; }

        string GetMarkupValue(string key);
        bool HasMarkup { get; }
        bool ContainsMarkup(string key);
        void SetMarkup(string key, string value);
    }
}
