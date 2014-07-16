using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using SharpDX;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics.Organization.Commands
{
    [DebuggerDisplay("{Type}: {commands.Count} commands")]
    public class RenderSceneToTextureCommand : RenderToTextureCommandBase
    {
        private readonly CommandManager commands;

        public RenderSceneToTextureCommand(IServiceRegistry services, IEnumerable<Command> commands, Texture2DDescription description = default(Texture2DDescription))
            : base(services, description)
        {
            Contract.Requires<ArgumentNullException>(commands != null, "commands");
            this.commands = new CommandManager(commands);
        }

        public override void Render()
        {
            commands.Run();
        }

        public override void Unload()
        {
            base.Unload();
            foreach (var command in commands)
                command.Unload();
        }
    }
}
