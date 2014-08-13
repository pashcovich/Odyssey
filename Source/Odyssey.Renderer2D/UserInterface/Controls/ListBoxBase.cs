using Odyssey.Utilities.Logging;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public abstract class ListBoxBase : ItemsControl, IContainer
    {
        private const string ControlTag = "ListBox";

        protected ListBoxBase()
            : base("Panel")
        {
        }

        protected override void Arrange()
        {
            if (Children.IsEmpty)
                return;

            UserInterface.Style.Layout.UpdateLayoutHorizontal(this, Children);
        }
    }
}