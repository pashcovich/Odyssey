using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.UserInterface.Style
{
    public struct Depth : IComparable<Depth>, IEquatable<Depth>
    {
        public const int Foreground = 100;
        public const int Background = 1;
        #region Properties

        /// <summary>
        /// The window layer. A value of 1 means the background. The highest value represents the currently focused window.
        /// </summary>
        public int WindowLayer { get; set; }

        /// <summary>
        /// Inside a window there can be controls that have to overlap other ones (such as the expandable panel of the DropDownBox). Increase this value to render them correctly.
        /// </summary>
        public int ComponentLayer { get; set; }

        /// <summary>
        /// A value of 1 means that the control is in the background of the window layer. An higher value means that the control has to be drawn on top of the other ones.
        /// </summary>
        public float ZOrder { get; set; }

        #endregion

        public Depth(int windowLayer, int componentLayer, float zOrder) : this()
        {
            WindowLayer = windowLayer;
            ComponentLayer = componentLayer;
            ZOrder = zOrder;
        }

        #region IComparable<Depth> Members

        public int CompareTo(Depth other)
        {
            if (WindowLayer > other.WindowLayer)
                return -100;
            else if (WindowLayer < other.WindowLayer)
                return +100;
            else if (ComponentLayer > other.ComponentLayer)
                return -50;
            else if (ComponentLayer < other.ComponentLayer)
                return +50;
            else
                return (int)Math.Ceiling(other.ZOrder - ZOrder);
        }

        #endregion

        /// <summary>
        /// Creates Depth information assuming that the current object will be the child of the control whose depth is passed as a parameter.
        /// </summary>
        /// <param name="parentDepth">The parent depth information.</param>
        public static Depth AsChildOf(Depth parentDepth)
        {
            return new Depth
                       {
                           WindowLayer = parentDepth.WindowLayer,
                           ComponentLayer = parentDepth.ComponentLayer,
                           ZOrder = parentDepth.ZOrder - 1.0f
                       };
        }

        public static Depth Topmost
        {
            get
            {
                return new Depth(1,1,Foreground);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", WindowLayer, ComponentLayer, ZOrder);
        }

        #region Equality
        public static bool operator ==(Depth depth1, Depth depth2)
        {
            if (depth1.Equals(depth2))
                return true;
            else return false;
        }

        public static bool operator !=(Depth depth1, Depth depth2)
        {
            return !(depth1 == depth2);
        }


        public bool Equals(Depth other)
        {
            if (WindowLayer == other.WindowLayer &&
                ComponentLayer == other.ComponentLayer &&
                ZOrder == other.ZOrder)
                return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Depth)) return false;
            return Equals((Depth)obj);
        }

 public override int GetHashCode()
        {
            unchecked
            {
                int result = WindowLayer.GetHashCode();
                result = (result*397) ^ ComponentLayer.GetHashCode();
                result = (result*397) ^ ZOrder.GetHashCode();
                return result;
            }
        }
        #endregion

       
    }
}


