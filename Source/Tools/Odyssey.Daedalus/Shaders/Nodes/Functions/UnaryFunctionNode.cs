using Odyssey.Daedalus.Shaders.Methods;
using Odyssey.Graphics.Shaders;
using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes.Functions
{
    public class UnaryFunctionNode : NodeBase
    {
        private IVariable output;

        public IMethod Function { get; set; }

        [DataMember]
        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        public INode Input1 { get; set; }

        [SupportedType(Type.Vector)]
        public override IVariable Output
        {
            get
            {
                if (output == null)
                {
                    string id = string.Empty;
                    Output = new Vector
                    {
                        Name = "vResult",
                        Type = Input1.Output.Type
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
            get
            {
                if (!Function.IsIntrinsic)
                    yield return Function;
            }
        }

        public override void Validate(TechniqueKey key)
        {
            Contract.Requires<InvalidOperationException>(Function != null, "No function specified.");
            base.Validate(key);
            Function.ActivateSignature(key);
        }

        public override string Access()
        {
            return Function.Call(Input1.Reference);
        }

        protected override void RegisterNodes()
        {
            AddNode("Input1", Input1);
        }

        protected override void SerializeMethods(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);
            if (serializer.Mode == SerializerMode.Write)
                MethodBase.WriteMethod(serializer, Function);
            else Function = MethodBase.ReadMethod(serializer);
        }
    }
}