using Odyssey.Graphics.Shaders;
using System;
using System.Diagnostics.Contracts;
using System.Text;
using Odyssey.Serialization;

namespace Odyssey.Daedalus.Shaders.Methods
{
    public struct MethodSignature : IDataSerializable
    {
        private Type returnType;
        private TechniqueKey key;
        private IMethod method;
        private string[] parameters;
        private string[] signatureTypes;

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

        public TechniqueKey Key
        {
            get { return key; }
            private set { key = value; }
        }

        public IMethod Method
        {
            get { return method; }
            private set { method = value; }
        }

        public string[] Parameters
        {
            get { return parameters; }
            private set { parameters = value; }
        }

        public string[] SignatureTypes
        {
            get { return signatureTypes; }
            private set { signatureTypes = value; }
        }

        public string Call(params string[] arguments)
        {
            Contract.Requires<ArgumentNullException>(arguments != null);
            Contract.Requires<ArgumentException>(arguments.Length == SignatureTypes.Length);
            var sb = new StringBuilder();
            sb.Append($"{Method.Name}(");
            foreach (var argument in arguments)
                sb.Append($"{argument}, ");
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
            var sb = new StringBuilder();
            sb.Append($"{Mapper.Map(returnType)} {Method.Name}(");
            for (int i = 0; i < SignatureTypes.Length; i++)
            {
                sb.Append($"{SignatureTypes[i]} {Parameters[i]}, ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            return sb.ToString();
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.SerializeEnum(ref returnType);
            serializer.Serialize(ref key);
            if (serializer.Mode == SerializerMode.Write)
                MethodBase.WriteMethod(serializer, method);
            else method = MethodBase.ReadMethod(serializer);
            serializer.Serialize(ref parameters, serializer.Serialize);
            serializer.Serialize(ref signatureTypes, serializer.Serialize);

        }
    }
}