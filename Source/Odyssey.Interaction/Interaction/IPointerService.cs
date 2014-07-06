using System;

namespace Odyssey.Interaction
{
    /// <summary>
    /// Provides access to platform-independent pointer events
    /// </summary>
    public interface IPointerService
    {
        /// <summary>
        /// Gets the current state of the pointer
        /// </summary>
        /// <returns>An instance of <see cref="PointerState"/> class</returns>
        PointerState GetState();

        /// <summary>
        /// Fills the provided object with the current pointer state information
        /// </summary>
        /// <remarks>All properties of provided object will be cleared.</remarks>
        /// <param name="state">The object that needs to be filled with pointer information</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="state"/> is null</exception>
        void GetState(PointerState state);
    }
}