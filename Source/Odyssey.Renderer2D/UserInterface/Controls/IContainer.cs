using System;
namespace Odyssey.UserInterface.Controls
{
    public interface IContainer : IControl
    {
        ControlCollection Controls { get; }
        void Arrange();
    }
}