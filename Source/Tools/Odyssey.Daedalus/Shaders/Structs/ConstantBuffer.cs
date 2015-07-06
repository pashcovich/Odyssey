using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using EngineReference = Odyssey.Engine.EngineReference;

namespace Odyssey.Daedalus.Shaders.Structs
{
    public partial class ConstantBuffer : Struct
    {
        private Dictionary<string, EngineReference> references;
        private CBUpdateType cbUpdateType;

        public CBUpdateType CbUpdateType
        {
            get { return cbUpdateType; }
            set { cbUpdateType = value; }
        }

        public ConstantBuffer()
        {
            Type = Type.ConstantBuffer;
            Index = 0;
            references = new Dictionary<string, EngineReference>();
        }

        public IEnumerable<EngineReference> References { get { return references.Values; } }

        public void SetReference(EngineReference reference)
        {
            string refId = reference.ToString();
            if (!references.ContainsKey(refId))
                references.Add(refId, reference);
            reference.Index = references.Count - 1;
        }

        public override void Add(IVariable variable)
        {
            base.Add(variable);
            if (variable.EngineReference != null)
                SetReference(variable.EngineReference);
        }

        public override string Definition
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(string.Format("{0} {1} : register({2})", Mapper.Map(Type), Name, GetRegister(this)));
                sb.AppendLine("{");
                foreach (var variable in Variables)
                {
                    sb.AppendFormat("{0}", variable.Type == Type.Struct ? ((IStruct)variable).Declaration : variable.Definition);
                }
                sb.AppendLine("};");
                return sb.ToString();
            }
        }

        public override void Serialize(BinarySerializer serializer)
        {
            base.Serialize(serializer);
            serializer.SerializeEnum(ref cbUpdateType);
            serializer.Serialize(ref references, serializer.Serialize, (ref EngineReference er) => serializer.Serialize(ref er));
        }
    }
}