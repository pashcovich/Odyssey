using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Odyssey.Content;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using SharpDX.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Serialization
{
    public class ShaderGraphSerializer : BinarySerializer
    {
        /// <summary>
        /// Odyssey Shader Graph Binary format.
        /// </summary>
        internal const string OSGB = "OSGB";
        internal const int Version = 0x100;
        private Dictionary<string, Variable> parsedVariables;
        private Dictionary<string, NodeBase> parsedNodes;

        public ShaderGraphSerializer(Stream stream, SerializerMode mode) : base(stream, mode)
        {
            AllowIdentity = true;
        }

        void Write(ref List<TechniqueDescription> techniques)
        {
            BeginChunk(OSGB);
            // Writes the version
            Writer.Write(Version);
            Serialize(ref techniques);
            EndChunk();
        }

        void Read(ref List<TechniqueDescription> techniques )
        {
            BeginChunk(OSGB);
            int version = Reader.ReadInt32();
            if (version != Version)
            {
                throw new NotSupportedException(string.Format("OSGB version [0x{0:X}] is not supported. Expecting [0x{1:X}]", version, Version));
            }
            Serialize(ref techniques);
            EndChunk();
        }

        public void Save(IEnumerable<TechniqueDescription> techniqueCollection)
        {
            var techniques = new List<TechniqueDescription>(techniqueCollection);
            Write(ref techniques);
        }

        public IEnumerable<TechniqueDescription> Load(Stream stream)
        {
            var techniques = new List<TechniqueDescription>();
            parsedVariables = new Dictionary<string, Variable>();
            parsedNodes = new Dictionary<string, NodeBase>();
            Read(ref techniques);
            return techniques;
        }

        public void MarkVariableAsParsed(Variable variable)
        {
            parsedVariables.Add(variable.Name, variable);
        }

        public bool IsVariableParsed(string name)
        {
            return parsedVariables.ContainsKey(name);
        }

        public Variable GetVariable(string name)
        {
            return parsedVariables[name];
        }

        public void MarkNodeAsParsed(NodeBase node)
        {
            parsedNodes.Add(node.Id, node);
        }

        public bool IsNodeParsed(string id)
        {
            return parsedNodes.ContainsKey(id);
        }

        public NodeBase GetNode(string id)
        {
            return parsedNodes[id];
        }

    }
}
