namespace Odyssey.Daedalus.Shaders.Nodes.Math
{
    public class AdditionNode : MathNodeBase
    {
        protected override char GetOperator()
        {
            const char add = '+';
            return add;
        }
    }
}