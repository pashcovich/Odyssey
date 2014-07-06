using Odyssey.Graphics;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using SharpYaml.Serialization;
using System;
using System.Diagnostics.Contracts;

namespace Odyssey.Talos.Components
{
    [YamlTag("UserInterface")]
    [RequiredComponent(typeof(UpdateComponent))]
    public class UserInterfaceComponent : Component, IInitializable
    {
        [YamlIgnore]
        public IUserInterfaceState UserInterfaceState { get; private set; }

        [YamlIgnore]
        public OverlayBase Overlay { get; set; }

        public bool IsInited { get { return Overlay.IsInited; } }

        public UserInterfaceComponent()
            : base(ComponentTypeManager.GetType<UserInterfaceComponent>())
        {
        }

        public void Initialize()
        {
            Contract.Requires<InvalidOperationException>(Overlay != null);
            UserInterfaceState = Services.GetService<IUserInterfaceState>();
            if (UserInterfaceState == null)
                throw new InvalidOperationException("UserInterfaceState");
            UserInterfaceState.Initialize();
            UserInterfaceState.SetOverlay(Overlay);
            Overlay.Initialize();
        }

        public void Unload()
        {
            Overlay.Unload();
        }
    }
}