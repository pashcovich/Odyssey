using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public class ShaderBuilder
    {
        const string separator = "//------------------------------------------------------------------------------\n";
        List<string> stringList;
        List<INode> parsedNodes;

        int tabLength;
        string tabSpaces;
        TechniqueKey key;

        public int IndentSpaces
        {
            get { return tabLength; }
            set
            {
                if (tabLength == value)
                    return;

                tabLength = value;
                tabSpaces = new String(' ', tabLength);

            }
        }

        public string EntryPoint {get; private set;}
        public string Output { get { return Flatten(stringList); } }

        public ShaderBuilder(TechniqueKey key)
        {
            this.key = key;
            stringList = new List<string>();
            parsedNodes = new List<INode>();
            IndentSpaces = 4;
        }

        public void AddSeparator(string label)
        {
            Add();
            stringList.Add(separator);
            stringList.Add(string.Format("// {0}\n", label));
            stringList.Add(separator);
            Add();
        }

        public void Add(string data = "\n")
        {
            stringList.Add(data);
            stringList.Add("\n");
        }

        public void BuildMethod(string signature, INode node, out IEnumerable<IMethod> requiredMethods)
        {
            parsedNodes.Clear();

            List<string> slMethod = new List<string>();
            List<IMethod> methods = new List<IMethod>();
            slMethod.Add(signature);
            slMethod.Add("\n{\n"); 
            Build(node, slMethod,methods);
            slMethod.Add("}");
            
            EntryPoint = Flatten(slMethod);
            requiredMethods = methods;
        }

        string Flatten(List<string> strings)
        {
            StringBuilder sb = new StringBuilder();
                foreach (string line in strings)
                    sb.Append(line.Replace("\t", tabSpaces));
                return sb.ToString();
        }

        void Build(INode node, List<string> slMethod, List<IMethod> methods)
        {
            node.Validate(key);

            foreach (IMethod method in node.RequiredMethods)
                methods.Add(method);

            foreach (INode nextNode in node.DescendantNodes)
            {
                if (!parsedNodes.Contains(nextNode))
                {
                    Build(nextNode, slMethod, methods);
                    parsedNodes.Add(nextNode);
                }
            }
            if (node.IsVerbose)
                slMethod.Add(node.Operation());
            
        }

    }
}
