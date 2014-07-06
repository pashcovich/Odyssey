using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Odyssey.Interaction
{
    /// <summary>
    /// State of a button or key.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct ButtonState : IEquatable<ButtonState>
    {
        private ButtonStateFlags flags;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonState"/> struct.
        /// </summary>
        /// <param name="flags">The state.</param>
        public ButtonState(ButtonStateFlags flags)
        {
            this.flags = flags;
        }

        /// <summary>
        /// Gets the state of this button as an enum.
        /// </summary>
        /// <value>The state.</value>
        public ButtonStateFlags Flags
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the button is being pressed.
        /// </summary>
        /// <value><c>true</c> if the button is being pressed; otherwise, <c>false</c>.</value>
        public bool Down
        {
            get
            {
                return (flags & ButtonStateFlags.Down) != 0;
            }
            set
            {
                if (value)
                {
                    flags |= ButtonStateFlags.Down;
                }
                else
                {
                    flags &= ~ButtonStateFlags.Down;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the button was pressed since last frame.
        /// </summary>
        /// <value><c>true</c> if the button was pressed since last frame; otherwise, <c>false</c>.</value>
        public bool Pressed
        {
            get
            {
                return (flags & ButtonStateFlags.Pressed) != 0;
            }
            set
            {
                if (value)
                {
                    flags |= ButtonStateFlags.Pressed;
                }
                else
                {
                    flags &= ~ButtonStateFlags.Pressed;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the button was released since last frame.
        /// </summary>
        /// <value><c>true</c> if the button was released since last frame; otherwise, <c>false</c>.</value>
        public bool Released
        {
            get
            {
                return (flags & ButtonStateFlags.Released) != 0;
            }
            set
            {
                if (value)
                {
                    flags |= ButtonStateFlags.Released;
                }
                else
                {
                    flags &= ~ButtonStateFlags.Released;
                }
            }
        }

        /// <summary>
        /// Resets the Pressed and Released events.
        /// </summary>
        internal void ResetEvents()
        {
            Pressed = false;
            Released = false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(ButtonState other)
        {
            return flags.Equals(other.flags);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ButtonState && Equals((ButtonState)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return flags.GetHashCode();
            }
        }

        /// <summary>
        /// Implements the ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ButtonState left, ButtonState right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ButtonState left, ButtonState right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("{0}", flags);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ButtonState"/> to <see cref="ButtonStateFlags"/>.
        /// </summary>
        /// <param name="buttonState">State of the button.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ButtonStateFlags(ButtonState buttonState)
        {
            return buttonState.Flags;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ButtonStateFlags"/> to <see cref="ButtonState"/>.
        /// </summary>
        /// <param name="buttonFlags">State of the button.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ButtonState(ButtonStateFlags buttonFlags)
        {
            return new ButtonState(buttonFlags);
        }
    }
}
