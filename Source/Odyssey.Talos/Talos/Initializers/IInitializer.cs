using Odyssey.Graphics.Effects;

namespace Odyssey.Talos.Initializers
{
    public interface IInitializer
    {
        EngineReference[] AcceptedReferences { get; }
        void SetupInitialization(ShaderInitializer initializer);
    }
}