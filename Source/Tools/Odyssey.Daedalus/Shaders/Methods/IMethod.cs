using Odyssey.Graphics.Shaders;
using System.Collections.Generic;

namespace Odyssey.Daedalus.Shaders.Methods
{
    public interface IMethod
    {
        string Body { get; }

        string Definition { get; }

        IEnumerable<MethodReference> MethodReferences { get; }

        string Name { get; }

        string Signature { get; }

        void ActivateSignature(TechniqueKey key);

        void AddReference(MethodReference reference);

        void ClearReferences();

        void SetFlag(string flagName, bool value);
        
        string Call(params string[] args);

        string Call(params IVariable[] args);

        bool IsIntrinsic { get; }
    }
}