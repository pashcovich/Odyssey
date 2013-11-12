using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using SharpYaml;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    public class YamlSerializer
    {
        static Dictionary<string, YamlVariable> parsedVariables;
        static Dictionary<string, IVariable> parsedYamlVariables;
        static Dictionary<string, YamlNode> parsedNodes;
        static Dictionary<string, INode> parsedYamlNodes;
        Serializer serializer;

        public YamlSerializer()
        {
            SerializerSettings settings = new SerializerSettings
            {
                 EmitShortTypeName = true,
                 EmitDefaultValues = false
            };

            settings.RegisterAssembly(Assembly.GetExecutingAssembly());
            settings.RegisterTagMapping("EngineReference", typeof(EngineReference));
            settings.RegisterTagMapping("TextureReference", typeof(TextureReference));
            serializer = new Serializer(settings);
            parsedNodes = new Dictionary<string, YamlNode>();
            parsedYamlNodes = new Dictionary<string, INode>();
            parsedVariables = new Dictionary<string, YamlVariable>();
            parsedYamlVariables = new Dictionary<string,IVariable>();
        }

        internal static void Clear()
        {
            parsedNodes.Clear();
            parsedYamlNodes.Clear();
            parsedVariables.Clear();
            parsedYamlVariables.Clear();
        }

        internal void SetVariables(Dictionary<string,YamlVariable> variables)
        {
            foreach (var kvp in variables)
                parsedVariables.Add(kvp.Key, kvp.Value);
        }

        internal YamlVariable RegisterVariable(IVariable variable)
        {
            var yVar = ((IYamlVariable)variable).ToYaml();
            parsedVariables.Add(variable.FullName, yVar);

            return yVar;
        }
        
        internal IVariable RegisterYamlVariable(YamlVariable yamlVariable, IStruct structVariable = null)
        {
            var variable = yamlVariable.ToVariable();
            if (structVariable != null)
                variable.Owner = structVariable;
            parsedYamlVariables.Add(variable.FullName, variable);
            return variable;
        }

        internal INode RegisterYamlNode(YamlNode yamlNode)
        {
            INode node = yamlNode.ToNode();
            parsedYamlNodes.Add(yamlNode.id, node);
            return node;
        }

        internal YamlVariable GetYamlVariable(string name)
        {
            return parsedVariables[name];
        }

        internal INode GetNode(string name)
        {
            return parsedYamlNodes[name];
        }

        internal IVariable GetVariable(string name)
        {
            return parsedYamlVariables[name];
        }

        internal bool ContainsVariable(string name)
        {
            return parsedVariables.ContainsKey(name);
        }

        internal bool ContainsYamlVariable(string name)
        {
            return parsedYamlVariables.ContainsKey(name);
        }

        internal bool ContainsYamlNode(string name)
        {
            return parsedYamlNodes.ContainsKey(name);
        }

        internal bool ContainsNode(string nodeId)
        {
            return parsedNodes.ContainsKey(nodeId);
        }

        internal YamlNode GetYamlNode(string nodeId)
        {
            return parsedNodes[nodeId];
        }

        internal void AddNode(string nodeId, YamlNode node)
        {
            parsedNodes.Add(nodeId, node);
        }

        public void SerializeGraph(Dictionary<Shader,string[]> shaderGraph, string path)
        {
            YamlShader.CurrentSerializer = this;

            var yamlShaderGraph = (from kvp in shaderGraph
                                  select new
                                  {
                                      Shader = new YamlShader(kvp.Key),
                                      Techniques = kvp.Value
                                  }).ToDictionary(k => k.Shader, k=> k.Techniques);

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path))
                    serializer.Serialize(sw, yamlShaderGraph, yamlShaderGraph.GetType());


        }

        public Dictionary<Shader, string[]> DeserializeGraph(string path)
        {
            YamlShader.CurrentSerializer = this;
            Dictionary<YamlShader, string[]> yamlShaderGraph = null;

            using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
                yamlShaderGraph = (Dictionary<YamlShader, string[]>)serializer.Deserialize(sr, typeof(Dictionary<YamlShader, string[]>));

            var shaderGraph = (from kvp in yamlShaderGraph
                               select new
                               {
                                   Shader = kvp.Key.ToShader(),
                                   Techniques = kvp.Value
                               }).ToDictionary(k => k.Shader, k => k.Techniques);

            return shaderGraph;
        }


    }
}
