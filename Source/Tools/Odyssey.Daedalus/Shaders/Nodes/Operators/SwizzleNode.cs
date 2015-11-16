using Odyssey.Serialization;
using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Daedalus.Shaders.Nodes.Operators
{
    public class SwizzleNode : NodeBase
    {
        private IVariable output;

        private Swizzle[] swizzle;

        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        public INode Input { get; set; }

        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        public override IVariable Output
        {
            get
            {
                Type type = Vector.TypeFromArrayLength(Swizzle.Where(s => s != Shaders.Swizzle.Null).Count());
                if (output == null)
                {
                    if (IsVerbose)
                    {
                        string name = char.IsUpper(Input.Output.Name[0]) ? $"{Variable.GetPrefix(Input.Output.Type)}{Input.Output.Name}" : Input.Output.Name;
                        Output = Variable.InitVariable(name, type);
                    }
                    else Output = new Vector() { Type = type, Name = Input.Reference };
                }
                return output;
            }
            set { output = value; }
        }

        public Swizzle[] Swizzle
        {
            get { return swizzle; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                swizzle = value;
            }
        }

        public string PrintSwizzle()
        {
            string result = String.Empty;

            foreach (Swizzle s in Swizzle)
            {
                if (s == Shaders.Swizzle.Null)
                    continue;
                result += s.ToString().ToLower();
            }
            return result;
        }

        private static bool IsDefaultSwizzle(Swizzle[] swizzle)
        {
            Swizzle[] defaultSwizzle = { Shaders.Swizzle.X, Shaders.Swizzle.Y, Shaders.Swizzle.Z, Shaders.Swizzle.W };

            return !swizzle.Where((t, i) => t != defaultSwizzle[i]).Any();
        }

        public override string Access()
        {
            return $"{Input.Reference}.{PrintSwizzle()}";
        }

        protected override void RegisterNodes()
        {
            AddNode("Input", Input);
        }

        protected override void SerializeProperties(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);

            serializer.Serialize(ref swizzle, serializer.SerializeEnum);
        }
    }
}