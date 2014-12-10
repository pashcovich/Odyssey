#region Using Directives

using Odyssey.Interaction;
using Odyssey.UserInterface.Controls;

#endregion

namespace Odyssey.UserInterface
{
    public interface IUserInterfaceState
    {
        PointerManager PointerManager { get; }

        void SetOverlay(IOverlay overlay);

        void Initialize();

        void Update();
    }
}