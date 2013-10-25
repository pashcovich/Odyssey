using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering.Management;
using SharpDX.Direct3D11;
using System.Collections.Generic;
using System.Threading;

namespace Odyssey.Graphics.Rendering
{
    public interface ICommand
    {
        CommandAttributes CommandAttributes { get; }
        CommandType CommandType { get; }

        string Name { get; }
        void Initialize(SceneStateEventArgs args);
        void Execute(IDirectXTarget target);
        void Dispose();

        bool Supports(CommandAttributes commandAttributes);
    }

    public interface IUpdateCommand : ICommand
    {
        int Priority { get; }
        bool IsRunning { get; }
        void Stop();
    }

    public interface IRenderCommand : ICommand
    {
        void UpdateItems();

        IMaterial Material { get; }

        ResourceType ExpectsResource { get; }
        ResourceType ReturnsResource { get; }
        void ReceiveInput(IRenderCommand previousStage);
        IEnumerable<IRenderableMesh> Items { get; }
    }
}