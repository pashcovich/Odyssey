using Odyssey.Interaction;
using Odyssey.UserInterface.Controls;

namespace Odyssey.UserInterface
{
    public interface IUserInterfaceState
    {
        PointerManager PointerManager { get; }

        void SetOverlay(OverlayBase overlay);

        void Initialize();

        void Update();

        UIElement Entered { get; set; }
    }
}