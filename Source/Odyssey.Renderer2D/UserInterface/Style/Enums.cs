using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Style
{
    
    public enum BorderStyle
    {
        None,
        Flat,
        Raised,
        Sunken,
    }

    [Flags]
    public enum Borders
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
        All = Top | Bottom | Left | Right,
    }

   

   
}
