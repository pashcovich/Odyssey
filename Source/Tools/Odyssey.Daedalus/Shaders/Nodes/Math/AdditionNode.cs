using Odyssey.Graphics.Shaders;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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