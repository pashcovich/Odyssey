using System;
namespace Odyssey.Daedalus.Shaders
{
    public interface IValueVariable
    {
        float[] Value { get; set; }
        string PrintArray();
    }
}
