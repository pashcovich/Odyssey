using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Graphics.Shaders;
using Odyssey.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes
{
    public class CustomOutputNode : NodeBase
    {
        private IStruct output;
        private Dictionary<string, NodeIdentifier> fields;

        [DataMember]
        [SupportedType(Type.Struct)]
        public override IVariable Output
        {
            get
            {
                output = new Struct() { Name = "output", CustomType = CustomType.VSOut};
                foreach (var f in fields)
                {
                    output.Add(f.Value.Variable);
                }
                return output;
            }
            set { output = (Struct)value; }
        }

        public override string Operation(ref int indentation)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\t{0} {1};", output.CustomType, output.Name));
            foreach (NodeIdentifier nodeId in fields.Values)
            {
                sb.AppendLine(string.Format("\t{0} = {1};", output[nodeId.Variable.Name].FullName, nodeId.Node.Reference));
            }
            sb.AppendLine(string.Format("\treturn {0};", output.Name));
            return sb.ToString();
        }

        public override string Access()
        {
            return string.Empty;
        }

        public CustomOutputNode()
        {
            fields = new Dictionary<string, NodeIdentifier>();
            IsVerbose = true;
        }

        public void RegisterField(Variable variable, INode node, Type supportedType)
        {
            NodeIdentifier nodeId = new NodeIdentifier() {Variable = variable, SupportedType = supportedType, Node = node};
            fields.Add(nodeId.Variable.Name, nodeId);
        }

        public void RegisterField(string name, string semantic, INode node, Type type)
        {
            var variable = Variable.InitVariable(name, type, semantic);
            NodeIdentifier nodeId = new NodeIdentifier{Variable = variable, SupportedType = type, Node = node};
            fields.Add(nodeId.Variable.Name, nodeId);
        }

        public override void Validate(TechniqueKey key)
        {
            base.Validate(key);
            foreach (var nodeId in fields.Values)
            {
                var node = nodeId.Node;
                Type outputType = node.Output.Type;
                if (node == null)
                    throw new InvalidOperationException(string.Format("Node [{0}] cannot be null", nodeId.Variable.Name));
                if (nodeId.SupportedType != outputType)
                    throw new InvalidOperationException(string.Format("Node [{0}] cannot be of type [{1}]", nodeId.Variable.Name, outputType));
                if (fields.Count != ((Struct)Output).VariableCount)
                    throw new InvalidOperationException("Mismatch between Output struct and registered fields");
            }
        }

        protected override void RegisterNodes()
        {
            var fieldList = fields.ToList();
            foreach (var field in fieldList)
            {
                AddNode(field.Key, field.Value.Node);
            }
        }

        protected override void AssignNodes(string key, NodeBase node, PropertyInfo nodeProperty)
        {
        }

        protected override void SerializeProperties(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);
            var fieldList = fields.Values.ToList();
            serializer.Serialize(ref fieldList);
            if (serializer.Mode == SerializerMode.Read)
                fields = fieldList.ToDictionary(f => f.Variable.Name, f=>f);
        }

    }
}
