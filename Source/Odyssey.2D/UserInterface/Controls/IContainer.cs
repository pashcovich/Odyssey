using System;
namespace Odyssey.UserInterface.Controls
{
    public interface IContainer
    {
        ControlCollection Controls { get; }
        ControlCollection PrivateControlCollection { get; }
        ControlCollection PublicControlCollection { get; }
        event EventHandler<ControlEventArgs> ControlAdded;
    }
}