namespace Odyssey.Serialization
{
    /// <summary>
    /// Serialization mode used by <see cref="BinarySerializer"/>.
    /// </summary>
    public enum SerializerMode
    {
        /// <summary>
        /// Reads the data from the stream.
        /// </summary>
        Read,

        /// <summary>
        /// Writes the data to the stream.
        /// </summary>
        Write
    }
}
