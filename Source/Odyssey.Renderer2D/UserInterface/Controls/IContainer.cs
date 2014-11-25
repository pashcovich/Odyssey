using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls
{
    public interface IContainer : IControl
    {
        ControlCollection Controls { get; }
        void Layout(Vector3 availableSize);
    }
}