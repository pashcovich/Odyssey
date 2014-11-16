using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SharpDX.Mathematics;

namespace Odyssey.Text
{
    public static class TextHelper
    {
        static readonly Regex resourceRegex = new Regex(@"(?<=\{)\s*(?<name>\w*[^}]*)\s*(?=\})");

        public static string GetCapitalLetters(string value)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(value));

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

        internal static int ArgbToRgba(int abgr)
        {
            byte[] bytes = BitConverter.GetBytes(abgr);
            return BitConverter.ToInt32(new[] { bytes[2], bytes[1], bytes[0], bytes[3]}, 0);
        }

        internal static Color4 DecodeColor4Abgr(string color, bool stripHashSymbol = true)
        {
            if (stripHashSymbol)
                color = color.Substring(1);

            return String.IsNullOrEmpty(color)
                ? new Color4(0, 0, 0, 0)
                : new Color4(ArgbToRgba(Int32.Parse(color, NumberStyles.HexNumber)));
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

        internal static Vector3 DecodeVector3(string value)
        {
            const string pattern = @"\s*(?<values>[0-9.,\-\s]*)";
            Regex regex = new Regex(pattern);
            var matches = regex.Match(value);
            string[] values = matches.Groups["values"].Value.Split(',');
            if (values.Length != 3)
                throw new InvalidOperationException(string.Format("Vector3 values are not in the correct format: {0}", value));
            float[] vValues = (from s in values select float.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            return new Vector3(vValues);
        }

        internal static Vector3 DecodeVector3LongForm(string s)
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


        internal static TEnum ParseEnum<TEnum>(string enumValue)
            where TEnum : struct
        {
            return (TEnum) Enum.Parse(typeof (TEnum), enumValue);
        }

        internal static string ParseResource(string s)
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(s), "String cannot be null");
           
            var match = resourceRegex.Match(s);
            return match.Success ? match.Groups["name"].Value : null;
        }

        public static string MakePlural<T>()
        {
            return String.Format("{0}s", typeof(T).Name);
        }

        public static bool IsExpressionArray(string expression, out string arrayName, out int index)
        {
            const string rArrayPattern = @"(?<array>\w*)\[(?<index>\d+)\]";
            Regex rArray = new Regex(rArrayPattern);

            Match match = rArray.Match(expression);
            if (match.Success)
            {
                arrayName = match.Groups["array"].Value;
                index = Int32.Parse(match.Groups["index"].Value);
                return true;
            }
            else
            {
                arrayName = String.Empty;
                index = -1;
                return false;
            }
        }

        public static bool IsExpressionArray(string expression, out string arrayName, out string index)
        {
            const string rArrayPattern = @"(?<array>\w*)\[(?<index>[a-zA-Z0-9\[\]]+)\]";
            Regex rArray = new Regex(rArrayPattern);

            Match match = rArray.Match(expression);
            if (match.Success)
            {
                arrayName = match.Groups["array"].Value;
                index = match.Groups["index"].Value;
                return true;
            }
            else
            {
                arrayName = string.Empty;
                index = string.Empty;
                return false;
            }
        }
    }
}
