using Odyssey.Engine;

namespace Odyssey.Epos.Initializers
{
    public interface IInitializer
    {
        EngineReference[] AcceptedReferences { get; }
        void SetupInitialization(ShaderInitializer initializer);
    }
}