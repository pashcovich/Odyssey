using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;

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
