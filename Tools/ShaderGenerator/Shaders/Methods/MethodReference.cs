using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public struct MethodReference
    {
        public IMethod Method { get; private set; }
        public IEnumerable<string> Arguments { get; private set; }

        public MethodReference(IMethod method, IEnumerable<string> arguments) : this()
        {
            Method = method;
            Arguments = arguments;
        }

        public string Call()
        {
            return Method.Call(Arguments.ToArray());
        }
    }
}
