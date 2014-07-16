using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes.Operators
{
    public enum ConditionType
    {
        LessThan,
    }

    public enum StepType
    {
        Increment,
    }

    public class ForBlock : NodeBase
    {
        private string index;
        private ConditionType conditionType;
        private StepType step;

        [SupportedType(Type.Float)]
        public INode StartCondition { get; set; }
        [SupportedType(Type.Float)]
        public INode EndCondition { get; set; }

        public string Index
        {
            get { return index; }
            set { index = value; }
        }

        public ConditionType ConditionType
        {
            get { return conditionType; }
            set { conditionType = value; }
        }

        public StepType Step
        {
            get { return step; }
            set { step = value; }
        }

        [IgnoreValidation(true)]
        public override IVariable Output { get; set; }

        public ForBlock()
        {
            Index = "i";
            IsVerbose = true;
            Declare = false;
            ConditionType = ConditionType.LessThan;
            Step = StepType.Increment;
        }

        protected override string Assignment()
        {
            return Access();
        }

        public override string Access()
        {
            return string.Format("for (int {0} = {1}; {0} {2} {3}; {0}{4})", Index, StartCondition.Reference,
                ConvertCondition(ConditionType), EndCondition.Reference, ConvertStep(Step));
        }

        protected override void RegisterNodes()
        {
            AddNode("StartCondition",StartCondition);
            AddNode("EndCondition",EndCondition);
        }

        protected override void SerializeProperties(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);
            serializer.Serialize(ref index);
            serializer.SerializeEnum(ref conditionType);
            serializer.SerializeEnum(ref step);
        }

        protected override void SerializeVariables(BinarySerializer serializer)
        {
            
        }

        internal static string ConvertCondition(ConditionType type)
        {
            switch (type)
            {
                case ConditionType.LessThan:
                    return "<";
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        internal static string ConvertStep(StepType type)
        {
            switch (type)
            {
                case StepType.Increment:
                    return "++";
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}
