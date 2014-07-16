using System;
using SharpDX.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes.Math
{
    public abstract class MathNodeBase : NodeBase
    {
        IVariable output;
        private bool parenthesize;

        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        public INode Input1 { get; set; }

        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        public INode Input2 { get; set; }

        public bool AssignToInput1 { get; set; }

        public bool Parenthesize
        {
            get { return parenthesize; }
            set { parenthesize = value; }
        }

        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        public override IVariable Output
        {
            get
            {
                if (output != null) return output;

                Type type = GetResultType(Input1.Output.Type, Input2.Output.Type);

                string name1 = Char.IsLower(Input1.Output.Name[0]) ? Input1.Output.Name.Substring(1, Input1.Output.Name.Length - 1) : Input1.Output.Name;
                string name2 = Char.IsLower(Input2.Output.Name[0]) ? Input2.Output.Name.Substring(1, Input2.Output.Name.Length - 1) : Input2.Output.Name;

                Output = Variable.InitVariable(string.Format("{0}{1}{2}", Variable.GetPrefix(type), name1, name2), type);
                return output;
            }
            set { output = value; }
        }

        protected MathNodeBase()
        {
            IsVerbose = false;
        }

        protected virtual Type GetResultType(Type factor1, Type factor2)
        {
            return factor1;
        }

        protected abstract char GetOperator();

        protected override string Assignment()
        {
            return AssignToInput1
                ? string.Format("{0} {1}= {2};", Input1.Output.FullName, GetOperator(), Input2.Reference)
                : string.Format("{0} = {1};", Output.FullName, Access());
        }

        public override string Access()
        {
            string access = string.Format("{0} {1} {2}", Input1.Reference, GetOperator(), Input2.Reference);
            return string.Format(Parenthesize ? "({0})" : "{0}", access);
        }

        protected override void RegisterNodes()
        {
            AddNode("Input1", Input1);
            AddNode("Input2", Input2);
        }

        protected override void SerializeProperties(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);
            serializer.Serialize(ref parenthesize);
        }

    }
}