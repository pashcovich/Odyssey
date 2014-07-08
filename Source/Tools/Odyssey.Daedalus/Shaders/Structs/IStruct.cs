using System.Collections.Generic;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Structs
{
    public interface IStruct : IVariable
    {
        IEnumerable<IVariable> Variables { get; }
        string Declaration { get; }
        string CustomType { get; }
        bool Contains(string semantic);
        bool Contains(IVariable field);
        void Add(IVariable variable);

        IVariable this[string key] { get; }
    }
}
