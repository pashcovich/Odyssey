using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Style
{
    /// <summary>
    /// The different types of strip possible of a grid.
    /// </summary>
    public enum StripType
    {
        /// <summary>
        /// A strip having fixed size expressed in number of virtual pixels.
        /// </summary>
        Fixed,

        /// <summary>
        /// A strip that occupies exactly the size required by its content. 
        /// </summary>
        Auto,

        /// <summary>
        /// A strip that occupies the maximum available size, dispatched among the other stared-size columns.
        /// </summary>
        Star,
    }
}
