using Odyssey.Content;

namespace Odyssey.Graphics
{
    public interface IColorResource : IResource {
        ColorType Type { get; }
        float Opacity { get; set; }
    }
}