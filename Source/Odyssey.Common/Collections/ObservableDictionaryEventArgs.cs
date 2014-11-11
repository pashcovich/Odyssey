using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Utilities.Collections
{
    /// <summary>
    /// Event arguments for the <see cref="ObservableDictionary{TKey,TValue}.ItemAdded"/> and <see cref="ObservableDictionary{TKey,TValue}.ItemRemoved"/> events.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    public class ObservableDictionaryEventArgs<TKey, TValue> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionaryEventArgs{TKey,TValue}"/> class from the provided <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="pair">The <see cref="KeyValuePair{TKey,TValue}"/> that contains the event arguments.</param>
        public ObservableDictionaryEventArgs(KeyValuePair<TKey, TValue> pair)
            : this(pair.Key, pair.Value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionaryEventArgs{TKey,TValue}"/> class from the provided key and value.
        /// </summary>
        /// <param name="key">The event's key argument.</param>
        /// <param name="value">The event's value argument.</param>
        public ObservableDictionaryEventArgs(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets the event's key argument.
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        /// Gets the event's value argument.
        /// </summary>
        public TValue Value { get; private set; }
    }
}
