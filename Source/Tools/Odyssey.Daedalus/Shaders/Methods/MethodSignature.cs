using Odyssey.Graphics.Shaders;
using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public struct MethodSignature
    {
        private readonly Type returnType;

        public MethodSignature(IMethod method, TechniqueKey key, string[] signatureTypes, string[] parameters, Type returnType)
            : this()
        {
            Contract.Requires<ArgumentException>(signatureTypes.Length == parameters.Length);
            Method = method;
            Key = key;
            SignatureTypes = signatureTypes;
            Parameters = parameters;
            this.returnType = returnType;
        }

        public Type ReturnType { get { return returnType; } }

        public TechniqueKey Key { get; private set; }

        public IMethod Method { get; private set; }

        public string[] Parameters { get; private set; }

        public string[] SignatureTypes { get; private set; }

        public string Call(params string[] arguments)
        {
            Contract.Requires<ArgumentNullException>(arguments != null);
            Contract.Requires<ArgumentException>(arguments.Length == SignatureTypes.Length);
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0}(", Method.Name));
            foreach (string argument in arguments)
                sb.Append(string.Format("{0}, ", argument));
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            return sb.ToString(); ;
        }

        public bool MatchesTypes(string[] arguments)
        {
            Contract.Requires<ArgumentNullException>(arguments != null);
            Contract.Requires<ArgumentException>(SignatureTypes.Length == arguments.Length);

            for (int i = 0; i < arguments.Length; i++)
                if (SignatureTypes[i] != arguments[i])
                    return false;

            return true;
        }

        public string Signature()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0} {1}(", Mapper.Map(returnType), Method.Name));
            for (int i = 0; i < SignatureTypes.Length; i++)
            {
                string type = SignatureTypes[i];
                string parameter = Parameters[i];
                sb.Append(string.Format("{0} {1}, ", type, parameter));
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            return sb.ToString();
        }
    }
}