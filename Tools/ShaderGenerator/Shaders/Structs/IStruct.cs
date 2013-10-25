using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using System;
using System.Collections.Generic;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Structs
{
    public interface IStruct : IVariable
    {
        IEnumerable<Variable> Variables { get; }
        string Declaration { get; }
        CustomType CustomType { get; }
        bool Contains(Semantic semantic);
        bool Contains(Variable field);

        Variable this[string key] { get; }
    }
}
