

using Odyssey.Devices;
namespace Odyssey.Interaction
{
    interface IKeyboardController
    {
        void OnKeyDown(object sender, KeyEventArgs e);
        //void OnKeyPress(object sender, KeyEventArgs e);
        void OnKeyUp(object sender, KeyEventArgs e);
    }
}
