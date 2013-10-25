using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public struct Binding
    {
        public IVariable Variable;
        public INode Node;

        public Binding(IVariable variable, INode node)
        {
            Variable = variable;
            Node = node;
        }


        
    }
}
