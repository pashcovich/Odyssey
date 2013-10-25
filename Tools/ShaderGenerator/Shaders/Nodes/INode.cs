using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    public interface INode
    {
        IVariable Output { get; }
        bool IsVerbose { get; }
        string Reference { get; }
        string Operation();
        string Access();
        IEnumerable<INode> DescendantNodes { get; }
        IEnumerable<IMethod> RequiredMethods { get; }
        void Validate(TechniqueKey key);
        
    }

}
