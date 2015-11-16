namespace Odyssey.Daedalus.Shaders.Nodes.Math
{
    public class DivisionNode : MathNodeBase
    {
        protected override char GetOperator()
        {
            const char div = '/';
            return div;
        }
    }
}
