
// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
// 
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
// 
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Odyssey.Daedalus.Serialization;
using Odyssey.Graphics.Shaders;
using Odyssey.Utilities.Logging;
using Odyssey.Serialization;

namespace Odyssey.Daedalus.Shaders.Methods
{
    public abstract partial class MethodBase : IMethod, IDataSerializable
    {

        private bool isIntrinsic;
        private Dictionary<string, MethodReference> methodReferences;
        private Dictionary<TechniqueKey, MethodSignature> methodSignatures;
        private string name;

        protected MethodBase(bool isIntrinsic = false)
            : this()
        {
            this.isIntrinsic = isIntrinsic;
        }

        private MethodBase()
        {
            methodSignatures = new Dictionary<TechniqueKey, MethodSignature>();
            methodReferences = new Dictionary<string, MethodReference>();
        }
        public abstract string Body { get; }

        public string Definition
        {
            get { return string.Format("{0}\n{1}", Signature, Body); }
        }

        public bool IsIntrinsic
        {
            get { return isIntrinsic; }
        }

        public IEnumerable<MethodReference> MethodReferences
        {
            get { return methodReferences.Values; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public virtual string Signature
        {
            get { return ActiveSignature.Signature(); }
        }

        protected MethodSignature ActiveSignature { get; set; }

        public virtual void ActivateSignature(TechniqueKey key)
        {
            try
            {
                var signatures = from kvp in methodSignatures
                                 where key.Supports(kvp.Key)
                                 let rank = key.Rank(kvp.Key)
                                 orderby rank descending
                                 select new { Rank = rank, Signature = kvp.Value };

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

        public void RegisterSignature(MethodSignature signature)
        {
            methodSignatures.Add(signature.Key, signature);
        }

        public void RegisterSignatures(IEnumerable<MethodSignature> signatures)
        {
            foreach (var signature in signatures)
                RegisterSignature(signature);
        }

        public virtual void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref name);
            serializer.Serialize(ref isIntrinsic);
            serializer.Serialize(ref methodReferences, serializer.Serialize);
            serializer.Serialize(ref methodSignatures, (ref TechniqueKey key) => serializer.Serialize(ref key),
                (ref MethodSignature ms) => serializer.Serialize(ref ms));
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

        public bool SupportsFeatures(TechniqueKey key)
        {
            return methodSignatures.Any(kvp => kvp.Key.Supports(key));
        }

        internal static MethodBase ReadMethod(BinarySerializer serializer)
        {
            ShaderGraphSerializer sg = (ShaderGraphSerializer) serializer;
            string outputType = null;
            serializer.Serialize(ref outputType);
            var method = (MethodBase) Activator.CreateInstance(System.Type.GetType(outputType));
            method.Serialize(serializer);

            if (sg.IsMethodParsed(method.Name))
                return sg.GetMethod(method.Name);
            else
            {
                sg.MarkMethodAsParsed(method);
                return method;
            }
        }

        internal static void WriteMethod(BinarySerializer serializer, IMethod method)
        {
            string methodType = method.GetType().FullName;
            var m = (MethodBase) method;
            serializer.Serialize(ref methodType);
            m.Serialize(serializer);
        }

    }
}