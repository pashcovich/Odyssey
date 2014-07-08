using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using System.Collections.Generic;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    public interface INode
    {
        IVariable Output { get; set; }
        bool IsVerbose { get; set; }
        bool OpensBlock { get; set; }
        bool ClosesBlock { get; set; }
        string Id { get; }
        string Reference { get; }
        string Operation(ref int indentation);
        string Access();
        IEnumerable<INode> DescendantNodes { get; }
        IEnumerable<IMethod> RequiredMethods { get; }
        void Validate(TechniqueKey key);

    }

}
