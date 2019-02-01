using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GrapeCity.DataVisualization.TypeScript
{
    public class String : Object
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public String(string value)
        {
            this._value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        private String(Undefined value)
        {
            this._value = value;
        }
        #endregion

        #region Operator Implicit
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator String(string s)
        {
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

            return s._value as string;
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
        public static String operator +(String str1, String str2)
        {
            return GetText(str1) + GetText(str2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator ==(String str1, String str2)
        {
            return Equals(str1, str2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(String str1, String str2)
        {
            return !Equals(str1, str2);
        }
        #endregion

        #region Methods
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

            int index = GetText(this).LastIndexOf(GetText(str));
            fromIndex = fromIndex < 0 ? 0 : 0;
            if (index >= 0 && fromIndex >= index)
            {
                return index;
            }
            return -1;
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
            int start = (IsNull(startIndex) || IsUndefined(startIndex)) ? 0 : (int)startIndex;
            int end = (IsNull(endIndex) || IsUndefined(endIndex)) ? 0 : (int)endIndex;

            start = start < 0 ? text.Length + start : start;
            end = end < 0 ? text.Length + end : end;
            return text.Substring(start, end - start);
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<String> Split()
        {
            return this.split(null);
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
                ss = text.Split(new string[] { separator }, count, StringSplitOptions.RemoveEmptyEntries);
            }

            Array<String> ret = new Array<String>();
            count = System.Math.Min(count, ss.Length);
            for (int i = 0; i < count; i++)
            {
                ret.Add(ss[i]);
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

            int index = IsNull(start) || IsUndefined(start) ? 0 : (int)start;
            int count = 0;
            if (IsUndefined(length))
            {
                count = int.MaxValue;
            }
            else if (length > 0)
            {
                count = (int)length;
            }

            string text = GetText(this);
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

            int startIndex = IsNull(start) || IsUndefined(start) ? 0 : (int)start;
            int endIndex = 0;
            if (IsUndefined(end))
            {
                endIndex = int.MaxValue;
            }
            else if (end > 0)
            {
                endIndex = (int)end;
            }

            string text = GetText(this);
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
        public String toString()
        {
            this.CheckUndefined();

            return GetText(this);
        }

        #endregion

        #region Override Methods
        /// <summary>
        /// 
        /// </summary>
        public static bool Equals(String a, String b)
        {
            return Object.Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Private Methods
        private static string GetText(String str)
        {
            if (IsNull(str))
            {
                return "null";
            }

            if (IsUndefined(str))
            {
                return "undefined";
            }
            return str._value as string;
        }

        #endregion
    }
}
