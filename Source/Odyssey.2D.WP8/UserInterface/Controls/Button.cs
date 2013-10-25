using Odyssey.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public class Button : ButtonBase
    {
        public override bool Contains(SharpDX.Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        public override void Initialize(IDirectXProvider directX)
        {
            base.Initialize(directX);
        }

        public override void Render(IDirectXTarget target)
        {
            base.Render(target);
        }
    }
}
