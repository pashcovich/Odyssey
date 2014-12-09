using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls
{
    public class Border : ContentControl
    {
        public Border() : base(typeof (Border).Name)
        {
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            return base.ArrangeOverride(availableSizeWithoutMargins);
        }
    }
}