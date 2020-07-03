using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace TypeScript.CSharp
{
    public class String : Object, IComparable, IComparable<String>
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public String(string value)
        {
            this.__value__ = value ?? throw new ArgumentNullException("value");
        }

        /// <summary>
        /// 
        /// </summary>
        private String(Undefined value)
        {
            this.__value__ = value;
        }
        #endregion

        #region Operator Implicit
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator String(string s)
        {
            if (s == null)
            {
                return null;
            }
            return new String(s);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator string(String s)
        {
            if (IsNull(s) || IsUndefined(s))
            {
                return null;
            }

            return s.__value__ as string;
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator String(Undefined undefined)
        {
            return new String(undefined);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator bool(String s)
        {
            if (IsNull(s) || IsUndefined(s))
            {
                return false;
            }
            return !string.IsNullOrEmpty(GetText(s));
        }
        #endregion

        #region Operator Overload
        /// <summary>
        /// 
        /// </summary>
        public static Number operator +(String str)
        {
            return parseFloat(str);
        }

        /// <summary>
        /// 
        /// </summary>
        public static String operator +(String str1, String str2)
        {
            return (string)ToString(str1) + (string)ToString(str2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator >(String str1, String str2)
        {
            return Compare(str1, str2) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator <(String str1, String str2)
        {
            return Compare(str1, str2) < 0;
        }
        #endregion

        #region Properties
        public String this[Number index]
        {
            get
            {
                this.CheckUndefined();
                return GetText(this)[(int)index].ToString();
            }
        }

        public Number length
        {
            get
            {
                this.CheckUndefined();

                return GetText(this).Length;
            }
        }

        public Number Length
        {
            get
            {
                return this.length;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Number charCodeAt(Number index)
        {
            this.CheckUndefined();

            string text = GetText(this);
            return Char.ConvertToUtf32(text, (int)index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public String charAt(Number index)
        {
            this.CheckUndefined();

            string text = GetText(this);
            int pos = (int)index;
            if (pos < 0 || pos >= text.Length)
            {
                return "";
            }
            return text[pos].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public String concat(params String[] strs)
        {
            StringBuilder sb = new StringBuilder(GetText(this));
            foreach (var str in strs)
            {
                sb.Append(GetText(str));
            }
            return new String(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public Number indexOf(String str)
        {
            return this.indexOf(str, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public Number indexOf(String str, Number fromIndex)
        {
            this.CheckUndefined();

            if (IsNull(str) || IsUndefined(str))
            {
                return -1;
            }

            int index = GetText(this).IndexOf(GetText(str));
            if (index >= 0 && index >= fromIndex)
            {
                return index;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number lastIndexOf(String str)
        {
            return this.lastIndexOf(str, int.MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public Number lastIndexOf(String str, Number fromIndex)
        {
            this.CheckUndefined();

            if (IsNull(str) || IsUndefined(str))
            {
                return -1;
            }

            int index = ((string)this).LastIndexOf(str);
            fromIndex = fromIndex < 0 ? 0 : fromIndex;
            if (index >= 0 && fromIndex >= index)
            {
                return index;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public RegExpArray match(string pattern)
        {
            this.CheckUndefined();

            string input = GetText(this);
            if (!RegExp.IsMatch(input, pattern))
            {
                return null;
            }

            RegExpArray ret = new RegExpArray();
            Match match = RegExp.Match(input, pattern);
            foreach (Group g in match.Groups)
            {
                ret.push(g.Value);
            }
            ret.input = input;
            ret.index = match.Index;

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        public RegExpArray match(RegExp regex)
        {
            this.CheckUndefined();

            string input = GetText(this);
            if (!regex.IsMatch(input))
            {
                return null;
            }

            RegExpArray ret = new RegExpArray();
            if (regex.global)
            {
                foreach (Match match in regex.Matches(input))
                {
                    ret.Add(match.Value);
                }
            }
            else
            {
                Match match = regex.Match(input);
                foreach (Group g in match.Groups)
                {
                    ret.Add(g.Value);
                }
                ret.input = input;
                ret.index = match.Index;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public String replace(String substr, String newSubStr)
        {
            this.CheckUndefined();

            string text = GetText(this);
            if (IsNull(substr) || IsUndefined(substr))
            {
                return text;
            }

            return text.Replace(GetText(substr), GetText(newSubStr));
        }

        /// <summary>
        /// 
        /// </summary>
        public String replace(RegExp regex, String replacement)
        {
            this.CheckUndefined();

            string text = GetText(this);
            if (regex.global)
            {
                return regex.Replace(text, replacement);
            }
            else
            {
                return regex.Replace(text, replacement, 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String replace(RegExp regex, Func<String, String, String> func)
        {
            this.CheckUndefined();

            string text = GetText(this);
            if (!regex.IsMatch(text))
            {
                return this;
            }

            if (regex.global)
            {
                string result = "";
                int index = 0;
                foreach (Match match in regex.Matches(text))
                {
                    GroupCollection groups = match.Groups;
                    string replacement = func(groups[0].Value, groups[1].Value);
                    result += (text.Substring(index, match.Index - index) + match.Result(replacement));
                    index = match.Index + match.Length;
                }
                result += text.Substring(index);
                return result;
            }
            else
            {
                Match match = regex.Match(text);
                GroupCollection groups = match.Groups;
                string replacement = func(groups[0].Value, groups[1].Value);
                return text.Substring(0, match.Index) + match.Result(replacement) + text.Substring(match.Index + match.Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String replace(RegExp regex, Func<String, String, String, String> func)
        {
            this.CheckUndefined();

            string text = GetText(this);
            if (!regex.IsMatch(text))
            {
                return this;
            }

            if (regex.global)
            {
                string result = "";
                int index = 0;
                foreach (Match match in regex.Matches(text))
                {
                    GroupCollection groups = match.Groups;
                    string replacement = func(groups[0].Value, groups[1].Value, groups[2].Value);
                    result += (text.Substring(index, match.Index - index) + match.Result(replacement));
                    index = match.Index + match.Length;
                }
                result += text.Substring(index);
                return result;
            }
            else
            {
                Match match = regex.Match(text);
                GroupCollection groups = match.Groups;
                string replacement = func(groups[0].Value, groups[1].Value, groups[2].Value);
                return text.Substring(0, match.Index) + match.Result(replacement) + text.Substring(match.Index + match.Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String replace(RegExp regex, Func<String, String, String, String, String> func)
        {
            this.CheckUndefined();

            string text = GetText(this);
            if (!regex.IsMatch(text))
            {
                return this;
            }

            if (regex.global)
            {
                string result = "";
                int index = 0;
                foreach (Match match in regex.Matches(text))
                {
                    GroupCollection groups = match.Groups;
                    string replacement = func(groups[0].Value, groups[1].Value, groups[2].Value, groups[3].Value);
                    result += (text.Substring(index, match.Index - index) + match.Result(replacement));
                    index = match.Index + match.Length;
                }
                result += text.Substring(index);
                return result;
            }
            else
            {
                Match match = regex.Match(text);
                GroupCollection groups = match.Groups;
                string replacement = func(groups[0].Value, groups[1].Value, groups[2].Value, groups[3].Value);
                return text.Substring(0, match.Index) + match.Result(replacement) + text.Substring(match.Index + match.Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Number search(String pattern)
        {
            return this.search(new RegExp(pattern));
        }

        /// <summary>
        /// 
        /// </summary>
        public Number search(RegExp regex)
        {
            this.CheckUndefined();

            string text = GetText(this);
            Match match = regex.Match(text);
            if (match.Success)
            {
                return match.Index;
            }
            return -1;
        }


        /// <summary>
        /// 
        /// </summary>
        public String slice(Number startIndex)
        {
            return this.slice(startIndex, int.MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public String slice(Number startIndex, Number endIndex)
        {
            this.CheckUndefined();

            string text = GetText(this);
            int length = text.Length;

            int start = (IsNull(startIndex) || IsUndefined(startIndex)) ? 0 : (int)startIndex;
            int end = (IsNull(endIndex) || IsUndefined(endIndex)) ? 0 : (int)endIndex;

            start = start < 0 ? text.Length + start : start;
            end = end < 0 ? text.Length + end : end;
            start = System.Math.Max(0, start);
            end = System.Math.Min(length, end);

            if (end <= start)
            {
                return "";
            }
            return text.Substring(start, end - start);
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<String> split(String separator)
        {
            return this.split(separator, int.MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<String> split(String separator, Number limit)
        {
            this.CheckUndefined();

            int count = int.MaxValue;
            if (IsNull(limit))
            {
                count = 0;
            }
            else if (limit > 0)
            {
                count = (int)limit;
            }

            string text = GetText(this);
            string[] ss;
            if (IsNull(separator) || IsUndefined(separator))
            {
                ss = new string[] { text };
            }
            else
            {
                ss = text.Split(new string[] { separator }, count, StringSplitOptions.None);
            }

            Array<String> ret = new Array<String>();
            count = System.Math.Min(count, ss.Length);
            for (int i = 0; i < count; i++)
            {
                ret.Add(ss[i]);
            }
            return ret;
        }

        public Array<String> split(RegExp regex)
        {
            this.CheckUndefined();

            Array<String> ret = new Array<String>();

            string text = GetText(this);
            string[] parts = regex.Split(text);
            foreach (string part in parts)
            {
                ret.Add(part);
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public String substr(Number start)
        {
            return this.substr(start, int.MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public String substr(Number start, Number length)
        {
            this.CheckUndefined();

            string text = GetText(this);
            int textLength = text.Length;

            int index = IsNull(start) || IsUndefined(start) ? 0 : (int)start;
            if (index < 0)
            {
                index = System.Math.Max(0, index + textLength);
            }

            int count = 0;
            if (IsNull(length) || IsUndefined(length))
            {
                count = int.MaxValue;
            }
            else
            {
                count = (int)length;
            }

            if (index >= textLength || count <= 0)
            {
                return "";
            }
            count = System.Math.Min(count, textLength - index);
            return text.Substring(index, count);
        }

        /// <summary>
        /// 
        /// </summary>
        public String substring(Number start)
        {
            return this.substring(start, int.MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public String substring(Number start, Number end)
        {
            this.CheckUndefined();

            string text = GetText(this);
            int textLength = text.Length;

            int startIndex = (IsNull(start) || IsUndefined(start) || isNaN(start) || start < 0) ? 0 : (int)start;
            int endIndex = (IsNull(end) || IsUndefined(end) || isNaN(end) || end < 0) ? 0 : (int)end;
            startIndex = System.Math.Min(startIndex, textLength);
            endIndex = System.Math.Min(endIndex, textLength);
            if (startIndex > endIndex)
            {
                int tmp = startIndex;
                startIndex = endIndex;
                endIndex = startIndex;
            }

            if (startIndex >= textLength)
            {
                return "";
            }
            return text.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        public String toLowerCase()
        {
            this.CheckUndefined();

            return GetText(this).ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        public String toUpperCase()
        {
            this.CheckUndefined();

            return GetText(this).ToUpper();
        }

        /// <summary>
        /// 
        /// </summary>
        public String toLocaleLowerCase()
        {
            return this.toLowerCase();
        }

        /// <summary>
        /// 
        /// </summary>
        public String toLocaleUpperCase()
        {
            return this.toUpperCase();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Number localeCompare(String str)
        {
            this.CheckUndefined();

            return Compare(this, str);
        }

        /// <summary>
        /// 
        /// </summary>
        public String trim()
        {
            this.CheckUndefined();

            return GetText(this).Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        public String trimStart()
        {
            this.CheckUndefined();

            return GetText(this).TrimStart();
        }

        /// <summary>
        /// 
        /// </summary>
        public String trimEnd()
        {
            this.CheckUndefined();

            return GetText(this).TrimEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        public override String toString()
        {
            this.CheckUndefined();

            return GetText(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String valueOf()
        {
            return this;
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// 
        /// </summary>
        public static String fromCharCode(Number charCode)
        {
            int codeValue = (int)charCode;
            // U+D800 ~ U+DFFF
            if (55296 <= codeValue && codeValue <= 57343)
            {
                return ((Char)codeValue).ToString();
            }
            return Char.ConvertFromUtf32(codeValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsNullOrEmpty(String str)
        {
            if (IsNull(str) || IsUndefined(str))
            {
                return true;
            }
            return string.IsNullOrEmpty(str.__value__ as string);
        }

        /// <summary>
        /// Compares two string object.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int Compare(String str1, String str2)
        {
            return string.Compare(ToString(str1), ToString(str2), System.Globalization.CultureInfo.CurrentCulture, System.Globalization.CompareOptions.StringSort);
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            this.CheckUndefined();

            return GetText(this).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public String ToString(IFormatProvider provider)
        {
            this.CheckUndefined();

            return GetText(this).ToString(provider);
        }
        #endregion

        #region Interface Implements
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                obj = "null";
            }
            if (IsUndefined(obj))
            {
                obj = "undefined";
            }

            if (obj is string)
            {
                obj = (String)(string)obj;
            }
            if (!(obj is String))
            {
                throw new ArgumentException("obj must be string");
            }
            return Compare(this, (String)obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="another"></param>
        /// <returns></returns>
        public int CompareTo(String another)
        {
            return Compare(this, another);
        }

        #endregion

        #region Private Methods
        private static string GetText(String str)
        {
            return (string)str.__value__;
        }
        #endregion
    }
}
