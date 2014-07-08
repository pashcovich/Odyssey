using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Serialization;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;

using Odyssey.Utilities;
using Odyssey.Utilities.Collections;
using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    [DataContract(IsReference = true)]
    [KnownType("KnownTypes")]
    public abstract partial class NodeBase : INode, IDataSerializable
    {
        internal static Dictionary<string, int> NodeCounter = new Dictionary<string, int>();
        private string id;
        private bool isVerbose;
        private bool declare;
        private Dictionary<string, INode> nodes;

        protected Dictionary<string, INode> Nodes { get { return nodes; } set { nodes = value; } }

        public const string DiffuseMapFlag = "DiffuseMap";
        public const string CubeMapFlag = "CubeMap";
        public const string SpecularFlag = "Specular";
        public const string ShadowsFlag = "Shadows";

        public bool IsVerbose { get { return isVerbose; } set { isVerbose = value; } }

        public bool Declare { get { return declare; } set { declare = value; } }

        public bool OpensBlock { get; set; }

        public bool ClosesBlock { get; set; }

        [IgnoreValidation(true)]
        public INode PreCondition { get; set; }

        public abstract IVariable Output { get; set; }

        public string Id
        {
            get { return id; }
        }

        public virtual IEnumerable<INode> DescendantNodes
        {
            get
            {
                foreach (var node in nodes.Values)
                {
                    yield return node;
                    //foreach ( var nodeChild in node.DescendantNodes)
                    //{
                    //    yield return nodeChild;
                    //}
                }
            }
        }

        public virtual IEnumerable<IMethod> RequiredMethods { get { yield break; } }

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

        protected NodeBase()
        {
            IsVerbose = false;
            Declare = true;
            string type = GetType().Name;
            if (!NodeCounter.ContainsKey(type))
                NodeCounter.Add(type, 0);

            id = string.Format("{0}{1}", type, NodeCounter[type]++);
            nodes = new Dictionary<string, INode>();
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

        protected virtual string Assignment()
        {
            return string.Format("{0} = {1};", Output.FullName, Access());
        }

        public abstract string Access();

        public virtual void Validate(TechniqueKey key)
        {
            ValidateBindings(key);
            if (PreCondition != null)
                Nodes.Add("PreCondition", PreCondition);
            RegisterNodes();
        }

        protected abstract void RegisterNodes();

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

        internal static bool CheckFlags(TechniqueKey key, PropertyInfo property)
        {
            var vsData = property.GetCustomAttributes(true).OfType<VertexShaderAttribute>();
            var psData = property.GetCustomAttributes(true).OfType<PixelShaderAttribute>();

            return vsData.All(vsAttribute => key.Supports(vsAttribute.Features)) && psData.All(psAttribute => key.Supports(psAttribute.Features));
        }

        internal static System.Type[] KnownTypes()
        {
            var derivedTypes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from lType in lAssembly.GetTypes()
                                where lType.IsSubclassOf(typeof(NodeBase))
                                select lType).ToArray();

            return derivedTypes;
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
            serializer.EndChunk();

            serializer.BeginChunk("VARS");

            serializer.EndChunk();

            serializer.BeginChunk("PROP");
            SerializeProperties(serializer);
            serializer.EndChunk();
        }

        protected virtual void SerializeProperties(BinarySerializer serializer)
        {
            serializer.Serialize(ref isVerbose);
            serializer.Serialize(ref declare);

            if (serializer.Mode == SerializerMode.Write)
                Variable.WriteVariable(serializer, Output);
            else
                Output = Variable.ReadVariable(serializer);
        }

        protected void SerializeDescendants(BinarySerializer serializer)
        {
            var nodeProperties = ReflectionHelper.GetProperties<INode>(this).ToDictionary(p => p.Name, p => p);
            int nodeCount = nodes.Count;
            var nodeList = nodes.ToList();
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

        protected virtual void AssignNodes(string key, NodeBase node, PropertyInfo nodeProperty)
        {
            nodes.Add(key, node);
            nodeProperty.SetValue(this, node);
        }

        internal static void WriteNode(BinarySerializer serializer, INode node)
        {
            string outputType = node.GetType().FullName;
            NodeBase n = (NodeBase)node;
            serializer.Serialize(ref outputType);
            n.Serialize(serializer);
        }

        internal static NodeBase ReadNode(BinarySerializer serializer)
        {
            ShaderGraphSerializer sg = (ShaderGraphSerializer)serializer;
            string outputType = null;
            serializer.Serialize(ref outputType);
            NodeBase node = (NodeBase)Activator.CreateInstance(System.Type.GetType(outputType));
            node.Serialize(serializer);

            if (sg.IsNodeParsed(node.id))
                return sg.GetNode(node.id);
            else
            {
                sg.MarkNodeAsParsed(node);
                return node;
            }
        }
    }
}