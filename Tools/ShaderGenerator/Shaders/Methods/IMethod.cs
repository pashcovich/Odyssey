using System;
namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public interface IMethod 
    {
        string Name { get; }
        string Body { get; }
        string Signature { get; }
        string Definition { get; }
        Type ReturnType { get; }
        string Call(params string[] args);
    }
}
