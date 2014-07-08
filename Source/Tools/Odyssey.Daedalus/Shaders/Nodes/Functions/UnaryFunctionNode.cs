using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions
{
    public class UnaryFunctionNode : NodeBase
    {
        private IVariable output;

        public IMethod Function { get; set; }

        [DataMember]
        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        [SupportedType(Type.Matrix)]
        public INode Input1 { get; set; }

        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
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
            Nodes.Add("Input1", Input1);
        }
    }
}