using Odyssey.Geometry;
using Odyssey.UserInterface.Style;
using SharpDX;

namespace Odyssey.UserInterface.Controls
{
    public class Panel : ContainerControl
    {
        private const string ControlTag = "Panel";
        private static int count;

        #region Constructors

        public Panel()
            : base(ControlTag + ++count, ControlTag)
        {
        }

        protected Panel(string tag, string controlClass)
            : base(tag, controlClass)
        {}

        #endregion
    }
}