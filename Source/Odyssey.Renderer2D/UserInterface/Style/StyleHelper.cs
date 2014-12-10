using System;
using System.Globalization;
using System.Text.RegularExpressions;
using SharpDX;

namespace Odyssey.UserInterface.Style
{
    /// <summary>
    /// Contains static methods needed for the serialization/deserialization process.
    /// </summary>
    internal static class StyleHelper
    {


        internal static Size2F DecodeSize(string s)
        {
            var regex = new Regex(@"Width:\s?(?<width>\d+)\s?Height:\s?(?<height>\d+)");
            var m = regex.Match(s);
            int width = Int16.Parse(m.Groups["width"].Value, CultureInfo.InvariantCulture);
            int height = Int16.Parse(m.Groups["height"].Value, CultureInfo.InvariantCulture);

            return new Size2F(width, height);
        }

        internal static Thickness DecodeThickness(string xmlPadding)
        {
            int value;

            if (Int32.TryParse(xmlPadding, out value))
                return new Thickness(value);

            var regex = new Regex(@"\s?(?<top>\d+)\s?(?<right>\d+)\s?(?<bottom>\d+)\s?(?<left>\d+)");
            var m = regex.Match(xmlPadding);
            int top = Int16.Parse(m.Groups["top"].Value, CultureInfo.InvariantCulture);
            int right = Int16.Parse(m.Groups["right"].Value, CultureInfo.InvariantCulture);
            int bottom = Int16.Parse(m.Groups["bottom"].Value, CultureInfo.InvariantCulture);
            int left = Int16.Parse(m.Groups["left"].Value, CultureInfo.InvariantCulture);

            return new Thickness
            {
                Bottom = bottom,
                Left = left,
                Right = right,
                Top = top
            };
        }

        internal static string EncodeSize(Size2F size)
        {
            return String.Format(CultureInfo.InvariantCulture, "Width:{0:D0} Height:{1:D0}", size.Width, size.Height);
        }

        internal static string EncodeThickness(Thickness padding)
        {
            return String.Format(CultureInfo.InvariantCulture,
                "{0} {1} {2} {3}", padding.Top, padding.Right, padding.Bottom, padding.Left);
        }


        internal static void Parse(string text, out int val1, out int val2)
        {
            var values = text.Split(',');
            val1 = Int16.Parse(values[0]);
            val2 = Int16.Parse(values[1]);
        }
    }
}