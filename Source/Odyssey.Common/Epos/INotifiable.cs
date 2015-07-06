using System;

namespace Odyssey.Epos
{
    public interface INotifiable
    {
        void Notify(object sender, EventArgs e);
    }
}
