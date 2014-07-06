using Odyssey.Content;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;

namespace Odyssey.UserInterface.Style
{
    [ContentReader(typeof(TextDefinitionsReader))]
    public struct TextDescription : IEquatable<TextDescription>
    {
        public const string Error = "Error";

        public static TextDescription Default
        {
            get
            {
                return new TextDescription
                           {
                               Name = "Default",
                               Color = new Color4(1, 1, 1, 1),
                               FontFamily = "Arial",
                               FontStyle = FontStyle.Normal,
                               Size = 16,
                               TextAlignment = TextAlignment.Leading,
                               ParagraphAlignment = ParagraphAlignment.Center
                           };
            }
        }

        public Color4 Color { get; set; }

        public string FontFamily { get; set; }

        public FontStyle FontStyle { get; set; }

        public FontWeight FontWeight { get; set; }

        public Color4 HighlightedColor { get; set; }

        public bool IsOutlined { get; set; }

        public string Name { get; set; }

        public ParagraphAlignment ParagraphAlignment { get; set; }

        public Color4 SelectedColor { get; private set; }

        public int Size { get; set; }

        public TextAlignment TextAlignment { get; set; }

        public override string ToString()
        {
            string styleTag = string.Empty;

            //if (IsBold) styleTag += "B";
            //if (IsItalic) styleTag += "I";
            //if (IsStrikeout) styleTag += "S";
            //if (IsUnderlined) styleTag += "U";
            if (IsOutlined) styleTag += "O";

            if (styleTag == string.Empty)
                styleTag = "R";

            return string.Format("{0} {1} {2} {3} C:[{4}] H:[{5}] S:[{6}] Ha:{7} Va:{8}",
                Name, FontFamily, Size,
                styleTag,
                Color.ToRgba().ToString("X8"),
                HighlightedColor.ToRgba().ToString("X8"),
                SelectedColor.ToRgba().ToString("X8"),
                TextAlignment, ParagraphAlignment);
        }

        #region Equality

        #region IEquatable<ControlDescription>

        public static bool operator !=(TextDescription fDesc1, TextDescription fDesc2)
        {
            return !(fDesc1 == fDesc2);
        }

        public static bool operator ==(TextDescription fDesc1, TextDescription fDesc2)
        {
            return fDesc1.Name == fDesc2.Name;
        }

        #endregion IEquatable<ControlDescription>

        public bool Equals(TextDescription other)
        {
            return Name == other.Name && FontFamily == other.FontFamily && Size == other.Size &&

                // FontStyle == other.FontStyle && Color == other.Color && SelectedColor == other.SelectedColor &&
                   HighlightedColor == other.HighlightedColor && TextAlignment == other.TextAlignment &&
                   ParagraphAlignment == other.ParagraphAlignment;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(TextDescription)) return false;
            return Equals((TextDescription)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Name != null ? Name.GetHashCode() : 0);
                result = (result * 397) ^ (FontFamily != null ? FontFamily.GetHashCode() : 0);
                result = (result * 397) ^ Size;

                //result = (result * 397) ^ FontStyle.GetHashCode();
                result = (result * 397) ^ Color.GetHashCode();
                result = (result * 397) ^ SelectedColor.GetHashCode();
                result = (result * 397) ^ HighlightedColor.GetHashCode();
                result = (result * 397) ^ TextAlignment.GetHashCode();
                result = (result * 397) ^ ParagraphAlignment.GetHashCode();
                return result;
            }
        }

        #endregion Equality
    }
}