using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Daedalus.Shaders.Methods;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Graphics.Shaders;
using Odyssey.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes.Functions
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
        }

        protected override void SerializeMethods(BinarySerializer serializer)
        {
            base.SerializeMethods(serializer);
            
            if (serializer.Mode == SerializerMode.Write)
                MethodBase.WriteMethod(serializer, method);
            else
                method = MethodBase.ReadMethod(serializer);
        }
    }
}
