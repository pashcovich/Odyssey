using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Shaders;
using Odyssey.Daedalus.Shaders.Structs;
using SharpDX.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes.Math
{
    public class ScalarNode : NodeBase
    {
        private float value;
        private IVariable output;

        [IgnoreValidation(true)]
        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }

        [SupportedType(Type.Float)]
        public override IVariable Output
        {
            get
            {
                if (output == null)
                {
                    Output = Variable.InitVariable("scalar", Type.Float);
                }
                return output;

            }
            set { output = value; }
        }

        public override string Access()
        {
            return Value == System.Math.Floor(Value) ? ((int) Value).ToString() : string.Format("{0}f",Value);
        }

        protected override void SerializeProperties(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);
            serializer.Serialize(ref value);
        }

        protected override void RegisterNodes()
        {
        }
    }
}
