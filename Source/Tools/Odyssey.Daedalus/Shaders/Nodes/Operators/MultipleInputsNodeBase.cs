using System.Collections.Generic;
using System.Reflection;

namespace Odyssey.Daedalus.Shaders.Nodes.Operators
{
    public abstract class MultipleInputsNodeBase : NodeBase
    {
        public List<INode> Inputs { get; set; }

        protected MultipleInputsNodeBase()
        {
            Inputs = new List<INode>();
        }

        protected override void RegisterNodes()
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                var node = Inputs[i];
                AddNode(string.Format("Input{0}",i), node);
            }
        }

        protected override void AssignNodes(string key, NodeBase node, PropertyInfo nodeProperty)
        {
            Inputs.Add(node);
        }

    }
}