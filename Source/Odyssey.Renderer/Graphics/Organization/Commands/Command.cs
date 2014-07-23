using System;
using System.Diagnostics.Contracts;
using Odyssey.Engine;
using SharpDX;

namespace Odyssey.Graphics.Organization.Commands
{
    public abstract class Command : Component, ICommand
    {
        private readonly CommandType type;
        private readonly IOdysseyDeviceService deviceService;
        private readonly IServiceRegistry services;

        protected IServiceRegistry Services { get { return services; } }
        protected IOdysseyDeviceService DeviceService { get { return deviceService; } }

        public bool IsInited { get; protected set; }

        public CommandType Type { get { return type; } }

        public abstract void Initialize();
        public abstract void Execute();

        protected Command(IServiceRegistry services, CommandType type)
        {
            Contract.Requires<ArgumentNullException>(services != null, "services");
            this.services = services;
            deviceService = services.GetService<IOdysseyDeviceService>();
            this.type = type;
        }

    }
}