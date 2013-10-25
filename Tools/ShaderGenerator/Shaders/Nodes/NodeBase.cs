using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    public delegate string NodeOperation(IVariable variable);

    [DataContract(IsReference = true)]
    [KnownType("KnownTypes")]
    public abstract class NodeBase : INode
    {
        [DataMember]
        public bool IsVerbose { get; set; }

        [DataMember]
        public abstract IVariable Output { get; set; }

        public abstract IEnumerable<INode> DescendantNodes { get; }
        public virtual IEnumerable<IMethod> RequiredMethods { get { yield break; } }

        public string Reference
        {
            get
            {
                if (IsVerbose)
                    return Output.Name;
                else
                    return Access();
            }
        }

        public NodeBase()
        {
            IsVerbose = false;
        }

        public abstract string Operation();
        public abstract string Access();


        public virtual void Validate(TechniqueKey key)
        {
            ValidateBindings(key);
        }

        void ValidateProperty(PropertyInfo property, TechniqueKey key, object data)
        {
            bool test = false;
            bool dataNull = data == null;
            Type expectedType = Type.None;
            if (!dataNull)
                expectedType = data is INode ? ((INode)data).Output.Type : ((IVariable)data).Type;
            foreach (object attribute in property.GetCustomAttributes(true).OfType<SupportedTypeAttribute>())
            {
                if (!CheckFlags(key, property))
                {
                    test = true;
                    LogEvent.Tool.Warning("Property [{0}] is marked as not being required. Skipping validation.", property.Name);
                    continue;
                }
                SupportedTypeAttribute supportedTypeAttribute = (SupportedTypeAttribute)attribute;
                if (data == null)
                    throw new InvalidOperationException(string.Format("Property [{0}] cannot be null.", property.Name));
                else if (supportedTypeAttribute.SupportedType == expectedType)
                {
                    test = true;
                    break;
                }
            }
            if (!test)
                throw new InvalidOperationException(string.Format("Node [{0}] cannot be of type [{1}].", property.Name, expectedType));
        }

        void ValidateBindings(TechniqueKey key)
        {
            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(INode))
                .Concat(this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(IVariable)));

            foreach (var property in properties)
            {
                var data = property.GetValue(this);
                ValidateProperty(property, key, data);
            }
        }

        internal static bool CheckFlags(TechniqueKey key,PropertyInfo property)
        {
            var vsData = property.GetCustomAttributes(true).OfType<VertexShaderAttribute>();
            var psData = property.GetCustomAttributes(true).OfType<PixelShaderAttribute>();

            foreach (VertexShaderAttribute vsAttribute in vsData)
                if (!key.Supports(vsAttribute.Features))
                    return false;
            foreach (PixelShaderAttribute psAttribute in psData)
                if (!key.Supports(psAttribute.Features))
                    return false;

            return true;
        }
        
        internal static System.Type[] KnownTypes()
        {
            var derivedTypes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from lType in lAssembly.GetTypes()
                                where lType.IsSubclassOf(typeof(NodeBase))
                                select lType).ToArray();

            return derivedTypes;
        }

    }
}
