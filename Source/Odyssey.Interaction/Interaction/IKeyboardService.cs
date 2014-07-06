using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Interaction
{
    /// <summary>
    /// Provides access to <see cref="KeyboardState"/> snapshot
    /// </summary>
    public interface IKeyboardService
    {
        /// <summary>
        /// Returns immediate state of keyboard at the moment of call
        /// </summary>
        /// <returns>An instance of <see cref="KeyboardState"/> with the information about pressed keys</returns>
        KeyboardState GetState();
    }
}
