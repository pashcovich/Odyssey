using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Graphics.Shaders;
using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Odyssey.Daedalus.Shaders.Nodes.Operators
{
    public class ReferenceNode : NodeBase
    {
        private IVariable value;
        private IVariable output;

        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        [SupportedType(Type.Struct)]
        public IVariable Value
        {
            get { return value; }
            set
            {
                this.value = value;
            }
        }

        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        [SupportedType(Type.Struct)]
        public override IVariable Output
        {
            get
            {
                if (output == null)
                {
                    if (IsVerbose)
                    {
                        string name = Char.IsUpper(Value.Name[0]) ? string.Format("{0}{1}", Variable.GetPrefix(Value.Type), Value.Name) : Value.Name;
                        Output = Variable.InitVariable(name, Value.Type, customType: Value is Struct ? ((Struct)Value).CustomType : Shaders.CustomType.None);
                    }
                    else Output = Value;
                }
                return output;
            }
            set { output = value; }
        }

        public override void Validate(TechniqueKey key)
        {
            Contract.Requires<InvalidOperationException>(Value.Type == Output.Type, "Type mismatch between Value and Output.");
            base.Validate(key);
        }

        public override string Access()
        {
            return Value.FullName;
        }

        protected override void RegisterNodes()
        {
        }

        protected override void SerializeVariables(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);
            if (serializer.Mode == SerializerMode.Write)
                Variable.WriteVariable(serializer, value);
            else
                value = Variable.ReadVariable(serializer);
        }
    }
}