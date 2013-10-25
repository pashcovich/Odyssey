using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using Odyssey.Content.Shaders;
using ShaderGenerator.Data;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public interface IVariable
    {
        string Name { get; }
        string FullName { get; }
        string Definition { get; }
        string Comments { get; }
        IEnumerable<KeyValuePair<string, object>> Markup { get; }
        Type Type { get; }
        int Index { get; set; }
        IStruct Owner { get; }
        ShaderReference ShaderReference {get;}

        T GetMarkupValue<T>(string key);
        bool ContainsMarkup(string key);
        void SetMarkup<T>(string key, T value);
    }
}
