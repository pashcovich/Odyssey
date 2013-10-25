using Odyssey.Graphics.Rendering.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering.Management
{
    public struct ShadowMappingDescription : IEquatable<ShadowMappingDescription>
    {
        public static int DefaultShadowMapSize = 1024;

        public int ShadowMapWidth { get; private set; }
        public int ShadowMapHeight { get; private set; }
        public ShadowAlgorithm Algorithm { get; private set; }

        public ShadowMappingDescription(ShadowAlgorithm algorithm, int size)
            : this(algorithm, size, size)
        {
        }

        public ShadowMappingDescription(ShadowAlgorithm algorithm, int mapWidth, int mapHeight)
            : this()
        {
            ShadowMapWidth = mapWidth;
            ShadowMapHeight = mapHeight;
            Algorithm = algorithm;
        }

        public static ShadowMappingDescription NoShadows
        {
            get
            {
                return new ShadowMappingDescription(ShadowAlgorithm.None, 0, 0);
            }
        }


        public bool Equals(ShadowMappingDescription other)
        {
            return (this.Algorithm == other.Algorithm &&
                this.ShadowMapWidth == other.ShadowMapWidth && this.ShadowMapHeight == other.ShadowMapHeight);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(ShadowMappingDescription)) return false;
            return Equals((ShadowMappingDescription)obj);
        }

        public override int GetHashCode()
        {
            return Algorithm.GetHashCode() + ShadowMapWidth.GetHashCode() + ShadowMapHeight.GetHashCode();
        }

        public static bool operator ==(ShadowMappingDescription left, ShadowMappingDescription right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ShadowMappingDescription left, ShadowMappingDescription right)
        {
            return !left.Equals(right);
        }
    }
}
