using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace TypeScript.Syntax
{
    internal class Util
    {
        //#1: IEnumerable
        public static bool HasEnumerableFlag(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string[] tags = GetCommentTags(text);
                if (tags != null)
                {
                    return tags.Contains("#1");
                }
            }
            return false;
        }

        public static string[] GetCommentTags(string text)
        {
            Match match = Regex.Match(text, "/\\*(.*)\\*/");
            if (match != null)
            {
                string value = match.Value.Trim('/', '*').Replace(" ", "");
                return value.Split(',');
            }
            return null;
        }
    }
}
