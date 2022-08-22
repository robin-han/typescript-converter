using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace java.lang.common.extensions
{
    static class StringExtension
    {
        public static string replaceAll(this string text, Regex regex, string replacement)
        {
            return regex.Replace(text, replacement);
        }

        public static string replace(this string text, Regex regex, string replacement)
        {
            return regex.Replace(text, replacement, 1);
        }
    }
}
