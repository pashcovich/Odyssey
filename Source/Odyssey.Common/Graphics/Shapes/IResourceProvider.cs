namespace Odyssey.Graphics.Shapes
{
    public interface IResourceProvider {
        bool ContainsResource(string resourceName);

        Gradient GetResource(string resourceName);
    }
}