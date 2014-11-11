using System;
using SharpDX.Multimedia;

namespace Odyssey.Serialization
{
    /// <summary>
    /// Use this attribute to specify the id of a dynamic type with <see cref="BinarySerializer"/>.
    /// </summary>
    public class DynamicSerializerAttribute : Attribute
    {
        private readonly FourCC id;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSerializerAttribute" /> class.
        /// </summary>
        /// <param name="id">The id to register as a dynamic type.</param>
        public DynamicSerializerAttribute(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSerializerAttribute" /> class.
        /// </summary>
        /// <param name="id">The id to register as a dynamic type.</param>
        public DynamicSerializerAttribute(string id)
        {
            this.id = id;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public FourCC Id { get { return id; } }
    }
}