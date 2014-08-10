using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using SharpDX;

namespace Odyssey.Utilities.Text
{
    public static class Text
    {
        static readonly Regex resourceRegex = new Regex(@"(?<=\{)\s*(?<name>\w*[^}]*)\s*(?=\})");

        public static string GetCapitalLetters(string value)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(value));

            StringBuilder sb = new StringBuilder();

            foreach (char c in value)
            {
                if (Char.IsUpper(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }

        public static string FirstCharacterToLowerCase(string s)
        {
            return Char.ToLowerInvariant(s[0]) + s.Substring(1);
        }

        public static string FirstCharacterToUpperCase(string s)
        {
            return Char.ToUpperInvariant(s[0]) + s.Substring(1);
        }

        internal static int AbgrToRgba(int abgr)
        {
            byte[] bytes = BitConverter.GetBytes(abgr);
            return BitConverter.ToInt32(new byte[] { bytes[3], bytes[2], bytes[1], bytes[0] }, 0);
        }

        internal static Color4 DecodeColor4Abgr(string color, bool stripHashSymbol = true)
        {
            if (stripHashSymbol)
                color = color.Substring(1);

            return string.IsNullOrEmpty(color)
                ? new Color4(0, 0, 0, 0)
                : new Color4(AbgrToRgba(Int32.Parse(color, NumberStyles.HexNumber)));
        }

        internal static Vector2 DecodeFloatVector2(string s)
        {
            string[] matches = s.Split(',');
            float x = Single.Parse(matches[0], CultureInfo.InvariantCulture);
            float y = Single.Parse(matches[1], CultureInfo.InvariantCulture);
            return new Vector2(x, y);
        }

        internal static Vector2 DecodeVector2(string s)
        {
            Regex regex = new Regex(@"(?<x>\d+),(?<y>\d+)");
            Match m = regex.Match(s);
            int x = Int16.Parse(m.Groups["x"].Value, CultureInfo.InvariantCulture);
            int y = Int16.Parse(m.Groups["y"].Value, CultureInfo.InvariantCulture);

            return new Vector2(x, y);
        }

        internal static Vector3 DecodeVector3(string s)
        {
            Regex regex = new Regex(@"X:\s?(?<x>\d+)\s?Y:\s?(?<y>\d+)\s?Z:\s?(?<z>\d+)\s?");
            Match m = regex.Match(s);
            int x = Int16.Parse(m.Groups["x"].Value, CultureInfo.InvariantCulture);
            int y = Int16.Parse(m.Groups["y"].Value, CultureInfo.InvariantCulture);
            int z = Int16.Parse(m.Groups["z"].Value, CultureInfo.InvariantCulture);

            return new Vector3(x, y, z);
        }

        internal static string EncodeVector2(Vector2 v)
        {
            return String.Format(CultureInfo.InvariantCulture, "X:{0:F0} Y:{1:F0}", v.X, v.Y);
        }

        internal static string EncodeVector3(Vector3 v)
        {
            return String.Format(CultureInfo.InvariantCulture, "X:{0:F0} Y:{1:F0} Z:{2:F0}", v.X, v.Y, v.Z);
        }

        internal static string ParseResource(string s)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(s), "String cannot be null");
           
            var match = resourceRegex.Match(s);
            return match.Success ? match.Groups["name"].Value : null;
        }

    }
}
