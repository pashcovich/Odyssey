using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Utilities
{
    public static class EnumExtensions
    {
        public static IEnumerable<Enum> GetFlags(this Enum input)
        {
            return Enum.GetValues(input.GetType()).Cast<Enum>().Where(input.HasFlag);
        }

        public static string WriteValues(this Enum input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Enum value in input.GetFlags())
                sb.Append(value + ", ");
            sb = sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
