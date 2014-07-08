using System;
namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public interface IValueVariable
    {
        float[] Value { get; set; }
        string PrintArray();
    }
}
