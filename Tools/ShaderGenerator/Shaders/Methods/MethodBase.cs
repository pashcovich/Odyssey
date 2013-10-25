using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public abstract partial class MethodBase : IMethod
    {
        public abstract string Body { get; }
        public Type ReturnType { get; protected set; }
        public string Name { get; set; }
        public string Definition
        {
            get
            {
                return string.Format("{0}\n{1}", Signature, Body);
            }
        }
        public abstract string Signature { get; }

        public string Call(params string[] args)
        {
            string call = string.Format("{0}(", Name);
            foreach (string argument in args)
                call += string.Format("{0}, ",argument);
            call = call.Remove(call.Length - 2, 2);
            call += ")";
            return call;
        }
    }
}
