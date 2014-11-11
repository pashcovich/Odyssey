using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Daedalus.Serialization;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Serialization;

namespace Odyssey.Daedalus.Shaders
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
                if (serializer.Mode == SerializerMode.Write)
                    Variable.WriteVariable(serializer, vars[i]);
                else
                {
                    Variable variable = Variable.ReadVariable(serializer);
                    Owner.Add(variable);
                }
            }
            serializer.EndChunk();
        }

    }
}
