#region Using Directives

using System;

#endregion

namespace Odyssey.UserInterface.Data
{
    /// <summary>
    ///     Arguments for BindingValueChanged events.
    /// </summary>
    internal class BindingValueChangedEventArgs : EventArgs
    {
        private readonly object newValue;
        private readonly object oldValue;

        internal BindingValueChangedEventArgs(object oldValue, object newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        /// <summary>
        ///     The new value of the binding.
        /// </summary>
        public object NewValue
        {
            get { return newValue; }
        }

        /// <summary>
        ///     The old value of the binding.
        /// </summary>
        public object OldValue
        {
            get { return oldValue; }
        }
    }
}