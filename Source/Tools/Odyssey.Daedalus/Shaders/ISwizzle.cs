namespace Odyssey.Daedalus.Shaders
{
    public interface ISwizzle
    {
        bool HasSwizzle { get; }
        Swizzle[] Swizzle { get; }
        string PrintSwizzle();
    }
}
