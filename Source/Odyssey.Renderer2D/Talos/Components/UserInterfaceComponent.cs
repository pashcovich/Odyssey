using Odyssey.Graphics;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using System;
using System.Diagnostics.Contracts;

namespace Odyssey.Talos.Components
{
    [RequiredComponent(typeof(UpdateComponent))]
    public class UserInterfaceComponent : ContentComponent
    {
        public IUserInterfaceState UserInterfaceState { get; private set; }

        public Overlay Overlay { get; set; }

        public override bool IsInited { get { return Overlay.IsInited; } }

        public UserInterfaceComponent()
            : base(ComponentTypeManager.GetType<UserInterfaceComponent>())
        {
        }

        public override void Initialize()
        {
            if (Overlay == null)
            throw new InvalidOperationException("'Overlay' cannot be null");
            UserInterfaceState = Services.GetService<IUserInterfaceState>();
            if (UserInterfaceState == null)
                throw new InvalidOperationException("UserInterfaceState");
            UserInterfaceState.Initialize();
            UserInterfaceState.SetOverlay(Overlay);
            Overlay.Initialize();
        }

        public override bool Validate()
        {
            // TODO add possibility of loading GUI from disk
            return true;
        }

    }
}