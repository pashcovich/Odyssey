using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public class IntrinsicFunction : MethodBase
    {
        private readonly int arguments;
        public int Arguments { get { return arguments; } }

        public IntrinsicFunction(string methodName, int arguments) : base(true)
        {
            Name = methodName;
            this.arguments = arguments;
        }
        
        public override string Body
        {
            get { return string.Format("Intrinsic Functions do not have a body"); }
        }
    }
}
