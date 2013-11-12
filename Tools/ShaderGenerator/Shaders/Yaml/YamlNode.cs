using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Utils;
using Odyssey.Utils.Text;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    public class YamlNode
    {
        const string outputKey = "output";
        static int counter;

        [YamlIgnore]
        public string id { get; set; }

        [YamlMember(0)]
        [DefaultValue(false)]
        public bool isVerbose { get; set; }

        [YamlMember(1)]
        [DefaultValue(null)]
        public Dictionary<string, YamlVariable> variables { get; set; }

        [YamlMember(2)]
        [DefaultValue(null)]
        public Dictionary<string, YamlNode> nodes { get; set; }

        [YamlMember(3)]
        [DefaultValue(null)]
        public YamlVariable output { get; set; }

        [YamlMember(4)]
        [DefaultValue(null)]
        public Dictionary<string, bool> flags { get; set; }

        public YamlNode()
        {
            id = this.GetType().Name + counter++;
        }

        public YamlNode(INode node)
        {
            id = node.GetType().Name;
            var nodeProperties = ReflectionHelper.GetProperties<INode>(node);
            int nodeCount = nodeProperties.Count();
            YamlSerializer serializer = YamlShader.CurrentSerializer;

            var flagProperties = ReflectionHelper.GetProperties<bool>(node);
            int flagCount = flagProperties.Count();
            if (flagCount > 1)
                flags = flagProperties.ToDictionary(b => Text.FirstCharacterToLowerCase(b.Name), b => (bool)b.GetValue(node));
            else if (flagCount == 1)
                isVerbose = node.IsVerbose;

            if (nodeCount > 0)
            {
                nodes = new Dictionary<string, YamlNode>();
                foreach (var p in nodeProperties)
                {
                    INode nodeChild = (INode)p.GetValue(node);
                    if (nodeChild == null)
                        continue;
                    string nodeId = nodeChild.Id;
                    string nodeName = Text.FirstCharacterToLowerCase(p.Name);

                    YamlNode linkedNode = serializer.ContainsNode(nodeId) ? serializer.GetYamlNode(nodeId) : ((IYamlNode)p.GetValue(node)).ToYaml();
                    nodes.Add(nodeName, linkedNode);
                    if (!serializer.ContainsNode(nodeId))
                        serializer.AddNode(nodeId, linkedNode);
                }
            }

            var variableProperties = ReflectionHelper.GetProperties<IVariable>(node);
            int variableCount = variableProperties.Count();
            if (variableCount > 1)
            {
                variables = variableProperties.Where(v => v.GetValue(node) != null).ToDictionary(v => Text.FirstCharacterToLowerCase(v.Name),
                    v => serializer.ContainsVariable(((IVariable)v.GetValue(node)).FullName)
                        ? serializer.GetYamlVariable(((IVariable)v.GetValue(node)).FullName)
                        : serializer.RegisterVariable((IVariable)v.GetValue(node)));


                variables.Remove(outputKey);
                if (node.Output != null)
                {
                    if (node.IsVerbose || variables.Count(kvp => string.Equals(kvp.Value.name,node.Output.Name)) == 0)
                    output = serializer.ContainsVariable(node.Output.FullName) ? serializer.GetYamlVariable(node.Output.FullName)
                        : serializer.RegisterVariable(node.Output);
                }
            }
            else if (variableCount == 1)
            {
                if (node.Output.Type == Type.Struct)
                    output = serializer.GetYamlVariable(node.Output.FullName);
                else
                    output = serializer.RegisterVariable(node.Output);
            }

            if (variables != null && variables.Count() == 0)
                variables = null;
        }

        public INode ToNode()
        {
            var serializer = YamlShader.CurrentSerializer;
            var mappingAttribute = ReflectionHelper.GetAttribute<YamlMappingAttribute>(this.GetType());
            INode node = (INode)Activator.CreateInstance(mappingAttribute.MatchingType);
            node.IsVerbose = isVerbose;
            if (output != null)
                node.Output = serializer.ContainsYamlVariable(output.fullName) ? (Variable)serializer.GetVariable(output.fullName) : (Variable)serializer.RegisterYamlVariable(output); ;
            foreach (var property in ReflectionHelper.GetProperties<INode>(node))
            {
                string propertyName = Text.FirstCharacterToLowerCase(property.Name);
                var ignoreValidationAttribute = property.GetCustomAttribute<IgnoreValidationAttribute>();
                if ((nodes == null || !nodes.ContainsKey(propertyName)) && ignoreValidationAttribute != null && ignoreValidationAttribute.Value)
                    continue;
                YamlNode yamlNode = null;
                bool test = nodes.TryGetValue(Text.FirstCharacterToLowerCase(property.Name), out yamlNode);
                if (!test)
                    continue;
                INode childNode = serializer.ContainsYamlNode(yamlNode.id) ? (INode)serializer.GetNode(yamlNode.id) : (INode)serializer.RegisterYamlNode(yamlNode);
                property.SetValue(node, childNode);
            }
            foreach (var property in ReflectionHelper.GetProperties<IVariable>(node).Where(p => p.Name != "Output"))
            {
                string propertyName = Text.FirstCharacterToLowerCase(property.Name);
                var ignoreValidationAttribute = property.GetCustomAttribute<IgnoreValidationAttribute>();
                if ( (variables == null || !variables.ContainsKey(propertyName)) && ignoreValidationAttribute != null && ignoreValidationAttribute.Value)
                    continue;
                YamlVariable yamlVar = variables[propertyName];
                Variable variable = serializer.ContainsYamlVariable(yamlVar.fullName) ? (Variable)serializer.GetVariable(yamlVar.fullName) : (Variable)serializer.RegisterYamlVariable(yamlVar);
                property.SetValue(node, variable);
            }
            foreach (var property in ReflectionHelper.GetProperties<bool>(node).Where(p => p.Name != "IsVerbose"))
            {
                bool value = flags[Text.FirstCharacterToLowerCase(property.Name)];
                property.SetValue(node, value);
            }

            return node;

        }



    }
}
