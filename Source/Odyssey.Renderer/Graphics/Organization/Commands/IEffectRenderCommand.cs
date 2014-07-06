using Odyssey.Graphics.Shaders;

namespace Odyssey.Graphics.Organization.Commands
{
    public interface IEffectRenderCommand : IRenderCommand
    {
        Effect Effect { get; }
    }
}
