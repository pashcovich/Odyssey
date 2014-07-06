using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Data
{
    /// <summary>
    /// Arguments for BindingValueChanged events.
    /// </summary>
    internal class BindingValueChangedEventArgs : EventArgs
    {
        //-----------------------------------------------------
        //
        //  Constructors
        //
        //-----------------------------------------------------

        private readonly object newValue;

        private readonly object oldValue;

        internal BindingValueChangedEventArgs(object oldValue, object newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        //------------------------------------------------------
        //
        //  Public Properties
        //
        //-----------------------------------------------------

        /// <summary>
        /// The new value of the binding.
        /// </summary>
        public object NewValue
        {
            get { return newValue; }
        }

        /// <summary>
        /// The old value of the binding.
        /// </summary>
        public object OldValue
        {
            get { return oldValue; }
        }

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------
    }
}