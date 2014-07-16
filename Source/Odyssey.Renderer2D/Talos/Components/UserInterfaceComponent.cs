﻿using Odyssey.Graphics;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using System;
using System.Diagnostics.Contracts;

namespace Odyssey.Talos.Components
{
    [RequiredComponent(typeof(UpdateComponent))]
    public class UserInterfaceComponent : Component, IInitializable
    {
        public IUserInterfaceState UserInterfaceState { get; private set; }

        public Overlay Overlay { get; set; }

        public bool IsInited { get { return Overlay.IsInited; } }

        public UserInterfaceComponent()
            : base(ComponentTypeManager.GetType<UserInterfaceComponent>())
        {
        }

        public void Initialize()
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

        public void Unload()
        {
            Overlay.Unload();
        }
    }
}