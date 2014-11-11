namespace Odyssey.Serialization
{
    /// <summary>
    /// Implement this interface to serialize datas with <see cref="BinarySerializer"/>.
    /// </summary>
    public interface IDataSerializable
    {
        /// <summary>
        /// Reads or writes datas from/to the given binary serializer.
        /// </summary>
        /// <param name="serializer">The binary serializer.</param>
        void Serialize(BinarySerializer serializer);
    }
}
