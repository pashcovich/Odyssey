using Odyssey.Engine;

namespace Odyssey.Talos.Initializers
{
    public interface IInitializer
    {
        EngineReference[] AcceptedReferences { get; }
        void SetupInitialization(ShaderInitializer initializer);
    }
}