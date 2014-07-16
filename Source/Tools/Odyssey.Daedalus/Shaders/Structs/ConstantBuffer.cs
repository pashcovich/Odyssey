using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using EngineReference = Odyssey.Engine.EngineReference;

namespace Odyssey.Daedalus.Shaders.Structs
{
    [DataContract]
    public partial class ConstantBuffer : Struct
    {
        [DataMember]
        private Dictionary<string, EngineReference> references;

        [DataMember]
        public UpdateType UpdateType { get; set; }

        public ConstantBuffer()
        {
            Type = Shaders.Type.ConstantBuffer;
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
    }
}