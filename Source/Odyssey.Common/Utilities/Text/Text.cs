using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace Odyssey.Utilities.Text
{
    public static class Text
    {
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
    }
}
