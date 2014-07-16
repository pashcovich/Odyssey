using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using Odyssey.Daedalus.Serialization;
using Odyssey.Daedalus.Shaders.Methods;
using Odyssey.Graphics.Shaders;
using Odyssey.Utilities;
using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Daedalus.Shaders.Nodes
{
    [DataContract(IsReference = true)]
    [KnownType("KnownTypes")]
    public abstract partial class NodeBase : INode, IDataSerializable
    {
        public const string CubeMapFlag = "CubeMap";
        public const string DiffuseMapFlag = "DiffuseMap";
        public const string ShadowsFlag = "Shadows";
        public const string SpecularFlag = "Specular";

        internal static readonly Dictionary<string, int> NodeCounter = new Dictionary<string, int>();

        private bool declare;

        private string id;

        private bool isVerbose;

        private Dictionary<string, INode> nodes;
        private bool closesBlock;
        private bool opensBlock;

        protected NodeBase()
        {
            IsVerbose = false;
            Declare = true;
            string type = GetType().Name;
            if (!NodeCounter.ContainsKey(type))
                NodeCounter.Add(type, 1);

            id = string.Format("{0}{1}", type, NodeCounter[type]++);
            nodes = new Dictionary<string, INode>();
        }

        public bool ClosesBlock 
        {
            get { return closesBlock; }
            set { closesBlock = value; }
        }

        public bool Declare { get { return declare; } set { declare = value; } }

        public virtual IEnumerable<INode> DescendantNodes
        {
            get { return nodes.Values; }
        }

        public string Id
        {
            get { return id; }
        }

        public bool IsVerbose { get { return isVerbose; } set { isVerbose = value; } }

        public bool OpensBlock
        {
            get { return opensBlock; }
            set { opensBlock = value; }
        }

        public abstract IVariable Output { get; set; }

        [IgnoreValidation(true)]
        public INode PreCondition { get; set; }

        public string Reference
        {
            get
            {
                if (IsVerbose)
                    return Output.FullName;
                else
                {
                    ISwizzle swizzleVar = Output as ISwizzle;
                    return swizzleVar != null && swizzleVar.HasSwizzle ? string.Format("{0}.{1}", Access(), swizzleVar.PrintSwizzle()) : Access();
                }
            }
        }

        public virtual IEnumerable<IMethod> RequiredMethods { get { yield break; } }

        public abstract string Access();

        protected void AddNode(string name, INode node)
        {
            Contract.Requires<ArgumentNullException>(node!= null, "node");
            if (!nodes.ContainsKey(name))
                nodes.Add(name, node);
        }

        public virtual string Operation(ref int indentation)
        {
            StringBuilder sb = new StringBuilder();
            if (OpensBlock)
            {
                sb.Append("\t{\n");
                indentation++;
            }

            string assignment = declare
                ? string.Format("\t{0} {1}\n", Mapper.Map(Output.Type), Assignment())
                : string.Format("\t{0}\n", Assignment());
            if (indentation > 1)
            {
                string indent = "\t";
                for (int i = 1; i < indentation; i++)
                    indent += "\t";

                assignment = assignment.Replace("\t", indent);
            }
            sb.Append(assignment);
            if (ClosesBlock)
            {
                sb.Append("\t}\n");
                indentation--;
            }

            return sb.ToString();
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref id);

            serializer.BeginChunk("CHLD");
            bool hasPrecondition = PreCondition != null;
            serializer.Serialize(ref hasPrecondition);
            if (hasPrecondition)
            {
                if (serializer.Mode == SerializerMode.Write)
                    WriteNode(serializer, PreCondition);
                else
                    PreCondition = ReadNode(serializer);
            }
            SerializeDescendants(serializer);
            serializer.EndChunk();

            serializer.BeginChunk("FUNC");
            SerializeMethods(serializer);
            serializer.EndChunk();

            serializer.BeginChunk("VARS");
            SerializeVariables(serializer);
            serializer.EndChunk();

            serializer.BeginChunk("PROP");
            SerializeProperties(serializer);
            serializer.EndChunk();
        }

        public virtual void Validate(TechniqueKey key)
        {
            ValidateBindings(key);
            if (PreCondition != null)
                AddNode("PreCondition", PreCondition);
            RegisterNodes();
        }

        internal static bool CheckFlags(TechniqueKey key, PropertyInfo property)
        {
            var vsData = property.GetCustomAttributes(true).OfType<VertexShaderAttribute>();
            var psData = property.GetCustomAttributes(true).OfType<PixelShaderAttribute>();

            return vsData.All(vsAttribute => key.Supports(vsAttribute.Features)) && psData.All(psAttribute => key.Supports(psAttribute.Features));
        }

        internal static NodeBase ReadNode(BinarySerializer serializer)
        {
            ShaderGraphSerializer sg = (ShaderGraphSerializer)serializer;

            bool isNewNode = false;
            serializer.Serialize(ref isNewNode);
            if (isNewNode)
            {
                string outputType = null;
                serializer.Serialize(ref outputType);
                NodeBase node = (NodeBase)Activator.CreateInstance(System.Type.GetType(outputType));
                node.Serialize(serializer);
                sg.MarkNodeAsParsed(node);
                return node;
            }
            else
            {
                string nodeId = null;
                serializer.Serialize(ref nodeId);
                if (sg.IsNodeParsed(nodeId))
                    return sg.GetNode(nodeId);
                else throw new InvalidOperationException(string.Format("Node '{0}' not found", nodeId));
            }
        }

        internal static void WriteNode(BinarySerializer serializer, INode node)
        {
            ShaderGraphSerializer sg = (ShaderGraphSerializer)serializer;
            
            NodeBase n = (NodeBase)node;

            // Is this node new one or a reference to another one encountered before?
            bool isNewNode = !sg.IsNodeParsed(n.id);
            serializer.Serialize(ref isNewNode);
            if (!isNewNode)
                serializer.Serialize(ref n.id);
            else
            {
                string outputType = node.GetType().FullName;
                serializer.Serialize(ref outputType);
                n.Serialize(serializer);
                sg.MarkNodeAsParsed(n);
            }
        }

        protected virtual string Assignment()
        {
            return string.Format("{0} = {1};", Output.FullName, Access());
        }

        protected virtual void AssignNodes(string key, NodeBase node, PropertyInfo nodeProperty)
        {
            AddNode(key, node);
            nodeProperty.SetValue(this, node);
        }

        protected abstract void RegisterNodes();

        private void SerializeDescendants(BinarySerializer serializer)
        {
            var nodeProperties = ReflectionHelper.GetProperties<INode>(this).ToDictionary(p => p.Name, p => p);
            int nodeCount = nodes.Count;
            var nodeList = nodes.ToList();
            serializer.Serialize(ref nodeCount);

            for (int i = 0; i < nodeCount; i++)
            {
                if (serializer.Mode == SerializerMode.Write)
                {
                    var kvp = nodeList[i];
                    string nodeKey = kvp.Key;
                    serializer.Serialize(ref nodeKey);
                    WriteNode(serializer, kvp.Value);
                }
                else
                {
                    string nodeKey = string.Empty;
                    serializer.Serialize(ref nodeKey);
                    NodeBase node = ReadNode(serializer);
                    PropertyInfo nodeProperty;
                    nodeProperties.TryGetValue(nodeKey, out nodeProperty);
                    AssignNodes(nodeKey, node, nodeProperty);
                }
            }
        }

        protected virtual void SerializeProperties(BinarySerializer serializer)
        {
            serializer.Serialize(ref isVerbose);
            serializer.Serialize(ref declare);
            serializer.Serialize(ref opensBlock);
            serializer.Serialize(ref closesBlock);
        }

        protected virtual void SerializeMethods(BinarySerializer serializer)
        { }

        protected virtual void SerializeVariables(BinarySerializer serializer)
        {
            if (serializer.Mode == SerializerMode.Write)
                Variable.WriteVariable(serializer, Output);
            else
                Output = Variable.ReadVariable(serializer);
        }

        private static bool SupportsType(Type type, Type expectedType)
        {
            if (type == expectedType)
                return true;
            else
            {
                switch (type)
                {
                    case Type.Vector:
                        return expectedType == Type.Float || expectedType == Type.Float2 || expectedType == Type.Float3 ||
                               expectedType == Type.Float4 || expectedType == Type.FloatArray;

                    case Type.Matrix:
                        return expectedType == Type.Float3x3 || expectedType == Type.Float4x4;

                    default:
                        return false;
                }
            }
        }

        private static void ValidateProperty(PropertyInfo property, TechniqueKey key, object data)
        {
            bool test = false;
            bool dataNull = data == null;
            Type expectedType = Type.None;
            var node = data as INode;

            if (!dataNull)
                expectedType = node != null ? node.Output.Type : ((IVariable)data).Type;

            foreach (SupportedTypeAttribute attribute in property.GetCustomAttributes(true).OfType<SupportedTypeAttribute>())
            {
                if (!CheckFlags(key, property))
                {
                    test = true;
                    Log.Daedalus.Warning("Property [{0}] is marked as not being required. Skipping validation.", property.Name);
                    continue;
                }
                SupportedTypeAttribute supportedTypeAttribute = attribute;
                if (data == null)
                    throw new InvalidOperationException(string.Format("Property [{0}] cannot be null.", property.Name));
                else if (SupportsType(supportedTypeAttribute.SupportedType, expectedType))
                {
                    test = true;
                    break;
                }
            }
            if (!test)
                throw new InvalidOperationException(string.Format("Node [{0}] cannot be of type [{1}].", property.Name, expectedType));
        }

        private void ValidateBindings(TechniqueKey key)
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(INode))
                .Concat(GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(IVariable)));

            foreach (var property in properties)
            {
                var data = property.GetValue(this);
                var ignoreAttribute = property.GetCustomAttribute<IgnoreValidationAttribute>();
                if (ignoreAttribute != null && ignoreAttribute.Value)
                {
                    Log.Daedalus.Warning(string.Format("{0} is marked to ignore validation.", property.Name));
                    continue;
                }

                ValidateProperty(property, key, data);
            }
        }
    }
}