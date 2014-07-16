using Odyssey.Daedalus.Shaders.Methods;
using Odyssey.Graphics.Shaders;
using System.Collections.Generic;

namespace Odyssey.Daedalus.Shaders.Nodes
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
