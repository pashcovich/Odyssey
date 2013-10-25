﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Utils.Text
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
    }
}
