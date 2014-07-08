using Odyssey.Graphics.Shaders;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math
{
    public class SubtractionNode : MathNodeBase
    {
        protected override char GetOperator()
        {
            const char sub = '-';
            return sub;
        }
    }
}