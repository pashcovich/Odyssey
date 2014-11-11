namespace Odyssey.Serialization
{
    /// <summary>
    /// Specify the size used for encoding length for array while using a <see cref="BinarySerializer"/>, just before an array is encoded.
    /// </summary>
    public enum ArrayLengthType
    {
        /// <summary>
        /// Use variable length 7Bit Encoding that will output from 1 byte to 5 byte depending on the range of length value.
        /// </summary>
        Dynamic = 0,

        /// <summary>
        /// Output a length as a byte. The length must not be greater than 255.
        /// </summary>
        Byte = 255,

        /// <summary>
        /// Output a length as an ushort. The length must not be greater than 65535.
        /// </summary>
        UShort = 65535,

        /// <summary>
        /// Output a length as an int. The length must not be greater than 2^31 - 1.
        /// </summary>
        Int = 0x7FFFFFF
    }
}