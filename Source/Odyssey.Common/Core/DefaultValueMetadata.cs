namespace Odyssey.Core
{
    /// <summary>
    /// Abstract class that could be overloaded in order to define how to get default value of an <see cref="PropertyKey"/>.
    /// </summary>
    public abstract class DefaultValueMetadata : PropertyKeyMetadata
    {
        public abstract object GetDefaultValue(ref PropertyContainer obj);

        /// <summary>
        /// Gets a value indicating whether this value is kept.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this value is kept; otherwise, <c>false</c>.
        /// </value>
        public virtual bool KeepValue
        {
            get { return false; }
        }

        public static StaticDefaultValueMetadata<T> Static<T>(T defaultValue, bool keepDefaultValue = false)
        {
            return new StaticDefaultValueMetadata<T>(defaultValue, keepDefaultValue);
        }
    }

    public abstract class DefaultValueMetadata<T> : DefaultValueMetadata
    {
        public abstract T GetDefaultValueT(ref PropertyContainer obj);

        public override object GetDefaultValue(ref PropertyContainer obj)
        {
            return GetDefaultValueT(ref obj);
        }
    }

    public class StaticDefaultValueMetadata<T> : DefaultValueMetadata<T>
    {
        private readonly T defaultValue;
        private readonly bool keepDefaultValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticDefaultValueMetadata"/> class.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="keepDefaultValue">if set to <c>true</c> [keep default value].</param>
        public StaticDefaultValueMetadata(T defaultValue, bool keepDefaultValue = false)
        {
            this.defaultValue = defaultValue;
            this.keepDefaultValue = keepDefaultValue;
        }

        /// <inheritdoc/>
        public override T GetDefaultValueT(ref PropertyContainer obj)
        {
            return defaultValue;
        }

        /// <inheritdoc/>
        public override bool KeepValue
        {
            get
            {
                return keepDefaultValue;
            }
        }
    }
}
