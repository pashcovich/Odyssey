using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Tools.ShaderGenerator.Serialization;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using SharpDX.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public sealed class VariableCollection : Dictionary<string,IVariable>, IDataSerializable
    {
        public IContainer Owner { get; internal set; }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.BeginChunk("VARS");
            int varCount = Count;
            serializer.Serialize(ref varCount);
            var vars = Values.Cast<Variable>().ToList();

            for (int i = 0; i < varCount; i++)
            {
                string varType = string.Empty;
                Variable variable;

                if (serializer.Mode == SerializerMode.Write)
                {
                    variable = vars[i];
                    varType = variable.GetType().FullName;
                    serializer.Serialize(ref varType);
                    variable.Serialize(serializer);
                }
                else
                {
                    serializer.Serialize(ref varType);
                    variable = ((Variable)Activator.CreateInstance(System.Type.GetType(varType)));
                    variable.Serialize(serializer);
                    Owner.Add(variable);
                }

            }
            serializer.EndChunk();
        }

    }
}
