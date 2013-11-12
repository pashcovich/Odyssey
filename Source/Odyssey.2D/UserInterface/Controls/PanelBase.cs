using Odyssey.UserInterface.Style;
using SharpDX;

namespace Odyssey.UserInterface.Controls
{
    public abstract class PanelBase : ContainerControl
    {
        private const string ControlTag = "Panel";
        private static int count;

        #region Constructors

        public PanelBase()
            : base(ControlTag)
        {
        }

        protected PanelBase(string tag, string controlClass)
            : base(controlClass)
        {}

        #endregion
    }
}