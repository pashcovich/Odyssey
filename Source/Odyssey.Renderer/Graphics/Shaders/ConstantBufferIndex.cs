using System;

namespace Odyssey.Graphics.Shaders
{
    public struct ConstantBufferIndex : IEquatable<ConstantBufferIndex>
    {
        public long Id { get; private set; }
        public string Technique { get; private set; }

        public ConstantBufferIndex(long id, string technique) : this()
        {
            Id = id;
            Technique = technique;
        }

        #region IEquatable
        public bool Equals(ConstantBufferIndex other)
        {
            return Id == other.Id && string.Equals(Technique, other.Technique);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ConstantBufferIndex && Equals((ConstantBufferIndex)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (Technique != null ? Technique.GetHashCode() : 0);
            }
        }

        public static bool operator ==(ConstantBufferIndex left, ConstantBufferIndex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ConstantBufferIndex left, ConstantBufferIndex right)
        {
            return !left.Equals(right);
        } 
        #endregion
    }
}
