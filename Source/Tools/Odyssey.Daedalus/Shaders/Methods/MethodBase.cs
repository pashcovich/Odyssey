using System.Net.Sockets;
using Odyssey.Graphics.Shaders;
using Odyssey.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using SharpDX.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public abstract partial class MethodBase : IMethod, IDataSerializable
    {
        private readonly Dictionary<string, MethodReference> methodReferences;
        private readonly Dictionary<TechniqueKey, MethodSignature> methodSignatures;

        private readonly bool isIntrinsic;
        public bool IsIntrinsic { get { return isIntrinsic; } }

        protected MethodBase(bool isIntrinsic = false)
        {
            methodSignatures = new Dictionary<TechniqueKey, MethodSignature>();
            methodReferences = new Dictionary<string, MethodReference>();
            this.isIntrinsic = isIntrinsic;
        }

        public abstract string Body { get; }

        public string Definition
        {
            get { return string.Format("{0}\n{1}", Signature, Body); }
        }

        public IEnumerable<MethodReference> MethodReferences { get { return methodReferences.Values; } }

        public string Name { get; set; }

        public virtual string Signature { get { return ActiveSignature.Signature(); } }

        protected MethodSignature ActiveSignature { get; set; }

        public virtual void ActivateSignature(TechniqueKey key)
        {
            try
            {
                var signatures = from kvp in methodSignatures
                    where key.Supports(kvp.Key)
                    let rank = key.Rank(kvp.Key)
                    orderby rank descending 
                    select new {Rank = rank, Signature = kvp.Value};
                    
                ActiveSignature = signatures.First().Signature;
            }
            catch (InvalidOperationException e)
            {
                LogEvent.Tool.Error("Shader does not support Technique [{0}]", key);
            }

            foreach (var kvp in methodReferences)
                kvp.Value.Method.ActivateSignature(key);
        }

        public void AddReference(MethodReference reference)
        {
            methodReferences.Add(reference.Method.Name, reference);
        }

        public string Call(params string[] args)
        {
            return ActiveSignature.Call(args);
        }

        public string Call(params IVariable[] args)
        {
            var argumentStrings = from variable in args
                                  select variable.FullName;

            return ActiveSignature.Call(argumentStrings.ToArray());
        }

        [Pure]
        public bool ContainsMethodReference(string name)
        {
            return methodReferences.ContainsKey(name);
        }

        public bool ContainsSignature(TechniqueKey key)
        {
            return methodSignatures.ContainsKey(key);
        }

        public MethodReference GetReference(string methodName)
        {
            Contract.Requires<ArgumentException>(ContainsMethodReference(methodName));
            return methodReferences[methodName];
        }

        public bool MatchesSignature(string[] types)
        {
            return methodSignatures.Any(kvp => kvp.Value.MatchesTypes(types));
        }

        public bool SupportsFeatures(TechniqueKey key)
        {
            return methodSignatures.Any(kvp => kvp.Key.Supports(key));
        }

        public void RegisterSignature(MethodSignature signature)
        {
            methodSignatures.Add(signature.Key, signature);
        }

        public void RegisterSignatures(IEnumerable<MethodSignature> signatures)
        {
            foreach (var signature in signatures)
                RegisterSignature(signature);
        }

        public void SetFlag(string name, bool value)
        {
            System.Type methodType = GetType();
            methodType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance).SetValue(this, value);
        }

        public void SetMethod(string methodName, MethodReference reference)
        {
            System.Type methodType = GetType();
            methodType.GetProperty(methodName, BindingFlags.Public | BindingFlags.Instance).SetValue(this, reference);
        }

        public void Serialize(BinarySerializer serializer)
        {
            // TODO Implement Method IDataSerializable
        }
    }
}