using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using System.Collections.Generic;
using EngineReference = Odyssey.Engine.EngineReference;

namespace Odyssey.Daedalus.Shaders
{
    public interface IVariable
    {
        string Name { get; }
        string FullName { get; }
        string Definition { get; }
        string Comments { get; }
        IEnumerable<KeyValuePair<string, string>> Markup { get; }
        Type Type { get; }
        string Semantic { get; }
        int? Index { get; set; }
        IStruct Owner { get; set; }
        EngineReference EngineReference { get; }

        string GetMarkupValue(string key);
        bool HasMarkup { get; }
        bool ContainsMarkup(string key);
        void SetMarkup(string key, string value);
    }
}
