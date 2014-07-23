using Odyssey.Graphics.Shaders;

namespace Odyssey.Graphics.Organization.Commands
{
    public interface ITechniqueRenderCommand : IRenderCommand
    {
        Technique Technique { get; }
    }
}
