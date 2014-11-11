using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Odyssey.Serialization
{
    /// <summary>
    /// Equality comparer using the identify of the object.
    /// </summary>
    /// <typeparam name="T">Type of the parameter</typeparam>
    /// <remarks>
    /// From http://stackoverflow.com/questions/8946790/how-to-use-an-objects-identity-as-key-for-dictionaryk-v.
    /// </remarks>
    public class IdentityEqualityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        public int GetHashCode(T value)
        {
            return RuntimeHelpers.GetHashCode(value);
        }

        public bool Equals(T left, T right)
        {
            return left == right; // Reference identity comparison
        }
    }
}