using System;

namespace Odyssey.Serialization
{
    /// <summary>
    /// Flags used when serializing a value with a <see cref="BinarySerializer"/>.
    /// </summary>
    [Flags]
    public enum SerializeFlags
    {
        /// <summary>
        /// Normal serialize (not dynamic, not nullable).
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Serialize a value as a dynamic value (the output stream will contain a magic-code for each encoded object). 
        /// </summary>
        Dynamic = 1,

        /// <summary>
        /// Serialize a value that can be null.
        /// </summary>
        Nullable = 2,
    }
}
