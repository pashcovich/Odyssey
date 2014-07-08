using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using SharpDX.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions
{
    public class FunctionNode : MultipleInputsNodeBase
    {
        private IVariable output;
        private Type returnType;
        private IMethod method;

        [IgnoreValidation(true)]
        public override IVariable Output
        {
            get
            {
                if (output == null)
                {
                    Output = new Vector
                    {
                        Name = "vResult",
                        Type = ReturnType
                    };
                }
                return output;
            }
            set
            {
                output = value;
            }
        }

        public override IEnumerable<IMethod> RequiredMethods
        {
            get { yield return method; }
        }

        public Type ReturnType
        {
            get { return returnType; }
            set { returnType = value; }
        }

        public IMethod Method
        {
            get { return method; }
            set { method = value; }
        }

        public override string Access()
        {
            string[] arguments = Inputs.Select(i => i.Reference).ToArray();
            return Method.Call(arguments);
        }

        public override void Validate(TechniqueKey key)
        {
            Contract.Requires<ArgumentNullException>(Method != null, "method"); 
            Contract.Requires<InvalidOperationException>(Output.Type == ReturnType, "Output variable does not match return type");
            base.Validate(key);
            Method.ActivateSignature(key);
        }

        protected override void SerializeProperties(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);
            serializer.SerializeEnum(ref returnType);
            MethodBase methodBase = (MethodBase)method ;
            methodBase.Serialize(serializer);

            if (serializer.Mode == SerializerMode.Read)
                method = methodBase;
        }
    }
}
