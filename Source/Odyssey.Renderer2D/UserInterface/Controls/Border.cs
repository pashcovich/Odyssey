using Odyssey.Graphics.Drawing;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Controls
{
    public class Border : ContentControl
    {
        public Border() : base(VisualStyle.Empty)
        {
            string typeName = GetType().Name;

            CanRaiseEvents = false;

            Template = new DataTemplate
            {
                Key = string.Format("{0}.Template", typeName),
                DataType = GetType(),
                VisualTree = new Rectangle() 
            };
        }
    }
}