using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators
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
        [SupportedType(Type.Float)]
        public INode StartCondition { get; set; }
        [SupportedType(Type.Float)]
        public INode EndCondition { get; set; }

        public string Index { get; set; }
        public ConditionType ConditionType { get; set; }
        public StepType Step { get; set; }

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
            Nodes.Add("StartCondition",StartCondition);
            Nodes.Add("EndCondition",EndCondition);
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
