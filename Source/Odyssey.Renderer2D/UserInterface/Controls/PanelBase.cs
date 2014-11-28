using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Controls
{
    public abstract class PanelBase : Panel
    {
        #region Constructors

        protected PanelBase() : base(ControlStyle.Empty)
        {
        }

        protected PanelBase(string controlClass)
            : base(controlClass)
        {}

        #endregion
    }
}