using System.Linq;
using Odyssey.Daedalus.Shaders.Methods;
using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Odyssey.Daedalus.Shaders
{
    public class ShaderBuilder
    {
        const string separator = "//------------------------------------------------------------------------------\n";
        readonly List<string> stringList;
        readonly List<INode> parsedNodes;

        int tabLength;
        string tabSpaces;
        private int currentIndentation;
        readonly TechniqueKey key;

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
        public string Output => Flatten(stringList);

        public ShaderBuilder(TechniqueKey key)
        {
            this.key = key;
            stringList = new List<string>();
            parsedNodes = new List<INode>();
            IndentSpaces = 4;
            currentIndentation = 1;
        }

        public void AddSeparator(string label)
        {
            stringList.Add(separator);
            stringList.Add($"// {label}\n");
            stringList.Add(separator);
        }

        public void Add(string data = "\n")
        {
            stringList.Add(data);
            stringList.Add("\n");
        }

        public void BuildMethod(string signature, INode node, out IEnumerable<IMethod> requiredMethods)
        {
            parsedNodes.Clear();

            var slMethod = new List<string>();
            var methods = new List<IMethod>();
            slMethod.Add(signature);
            slMethod.Add("\n{\n"); 
            Build(node, slMethod,methods);
            slMethod.Add("}");

            EntryPoint = Flatten(slMethod);
            requiredMethods = methods;
        }

        string Flatten(IEnumerable<string> strings)
        {
            var sb = new StringBuilder();
                foreach (string line in strings)
                    sb.Append(line.Replace("\t", tabSpaces));
                return sb.ToString();
        }

        void Build(INode node, ICollection<string> slMethod, List<IMethod> methods)
        {
            if (!parsedNodes.Contains(node))
                node.Validate(key);

            var nodeMethods = node.RequiredMethods.ToArray();
            if (nodeMethods.Any())
            {
                var newMethods = nodeMethods.Except(methods);
                methods.AddRange(newMethods);
            }

            foreach (INode nextNode in node.DescendantNodes.Where(nextNode => !parsedNodes.Contains(nextNode)))
            {
                Build(nextNode, slMethod, methods);
                parsedNodes.Add(nextNode);
            }

            if (node.IsVerbose)
            {
                slMethod.Add(node.Operation(ref currentIndentation));
            }

        }

    }
}
