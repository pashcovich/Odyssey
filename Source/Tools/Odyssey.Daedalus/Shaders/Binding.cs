using Odyssey.Daedalus.Shaders.Nodes;

namespace Odyssey.Daedalus.Shaders
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
