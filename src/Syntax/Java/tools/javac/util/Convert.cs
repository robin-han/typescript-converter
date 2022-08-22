using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (c) 1999, 2019, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

namespace com.sun.tools.javac.util
{
    /// <summary>
    /// Utility class for static conversion methods between numbers
    ///  and strings in various formats.
    /// 
    ///  <para>Note regarding UTF-8.
    ///  The JVMS defines its own version of the UTF-8 format so that it
    ///  contains no zero bytes (modified UTF-8). This is not actually the same
    ///  as Charset.forName("UTF-8").
    /// 
    /// </para>
    ///  <para>
    ///  See also:
    ///  <ul>
    ///  <li><a href="http://docs.oracle.com/javase/specs/jvms/se7/html/jvms-4.html#jvms-4.4.7">
    ///    JVMS 4.4.7 </a></li>
    ///  <li><a href="http://docs.oracle.com/javase/7/docs/api/java/io/DataInput.html#modified-utf-8">
    ///      java.io.DataInput: Modified UTF-8 </a></li>
    ///    <li><a href="https://en.wikipedia.org/wiki/UTF-8#Modified_UTF-8">
    ///      Modified UTF-8 (Wikipedia) </a></li>
    ///  </ul>
    /// 
    ///  The methods here support modified UTF-8.
    /// 
    /// </para>
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class Convert
    {

        /// <summary>
        /// Convert string to integer.
        /// </summary>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static int string2int(String s, int radix) throws NumberFormatException
        public static int string2int(string s, int radix)
        {
            //if (radix == 10)
            //{
            //    return Integer.parseInt(s, radix);
            //}
            //else
            //{
            //    char[] cs = s.ToCharArray();
            //    int limit = int.MaxValue / (radix / 2);
            //    int n = 0;
            //    foreach (char c in cs)
            //    {
            //        int d = Character.digit(c, radix);
            //        if (n < 0 || n > limit || n * radix > int.MaxValue - d)
            //        {
            //            throw new System.FormatException();
            //        }
            //        n = n * radix + d;
            //    }
            //    return n;
            //}
            return System.Convert.ToInt32(s, radix);
        }

        /// <summary>
        /// Convert string to long integer.
        /// </summary>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static long string2long(String s, int radix) throws NumberFormatException
        public static long string2long(string s, int radix)
        {
            //if (radix == 10)
            //{
            //    return Long.parseLong(s, radix);
            //}
            //else
            //{
            //    char[] cs = s.ToCharArray();
            //    long limit = long.MaxValue / (radix / 2);
            //    long n = 0;
            //    foreach (char c in cs)
            //    {
            //        int d = Character.digit(c, radix);
            //        if (n < 0 || n > limit || n * radix > long.MaxValue - d)
            //        {
            //            throw new System.FormatException();
            //        }
            //        n = n * radix + d;
            //    }
            //    return n;
            //}
            return System.Convert.ToInt64(s, radix);
        }

        /* Conversion routines between names, strings, and byte arrays in Utf8 format
         */

        /// <summary>
        /// Convert `len' bytes from utf8 to characters.
        ///  Parameters are as in System.arraycopy
        ///  Return first index in `dst' past the last copied char. </summary>
        ///  <param name="src">        The array holding the bytes to convert. </param>
        ///  <param name="sindex">     The start index from which bytes are converted. </param>
        ///  <param name="dst">        The array holding the converted characters.. </param>
        ///  <param name="dindex">     The start index from which converted characters
        ///                    are written. </param>
        ///  <param name="len">        The maximum number of bytes to convert. </param>
        public static int utf2chars(sbyte[] src, int sindex, char[] dst, int dindex, int len)
        {
            int i = sindex;
            int j = dindex;
            int limit = sindex + len;
            while (i < limit)
            {
                int b = src[i++] & 0xFF;
                if (b >= 0xE0)
                {
                    b = (b & 0x0F) << 12;
                    b = b | (src[i++] & 0x3F) << 6;
                    b = b | (src[i++] & 0x3F);
                }
                else if (b >= 0xC0)
                {
                    b = (b & 0x1F) << 6;
                    b = b | (src[i++] & 0x3F);
                }
                dst[j++] = (char)b;
            }
            return j;
        }

        /// <summary>
        /// Return bytes in Utf8 representation as an array of characters. </summary>
        ///  <param name="src">        The array holding the bytes. </param>
        ///  <param name="sindex">     The start index from which bytes are converted. </param>
        ///  <param name="len">        The maximum number of bytes to convert. </param>
        public static char[] utf2chars(sbyte[] src, int sindex, int len)
        {
            char[] dst = new char[len];
            int len1 = utf2chars(src, sindex, dst, 0, len);
            char[] result = new char[len1];
            Array.Copy(dst, 0, result, 0, len1);
            return result;
        }

        /// <summary>
        /// Return all bytes of a given array in Utf8 representation
        ///  as an array of characters. </summary>
        ///  <param name="src">        The array holding the bytes. </param>
        public static char[] utf2chars(sbyte[] src)
        {
            return utf2chars(src, 0, src.Length);
        }

        /// <summary>
        /// Return bytes in Utf8 representation as a string. </summary>
        ///  <param name="src">        The array holding the bytes. </param>
        ///  <param name="sindex">     The start index from which bytes are converted. </param>
        ///  <param name="len">        The maximum number of bytes to convert. </param>
        public static string utf2string(sbyte[] src, int sindex, int len)
        {
            char[] dst = new char[len];
            int len1 = utf2chars(src, sindex, dst, 0, len);
            return new string(dst, 0, len1);
        }

        /// <summary>
        /// Return all bytes of a given array in Utf8 representation
        ///  as a string. </summary>
        ///  <param name="src">        The array holding the bytes. </param>
        public static string utf2string(sbyte[] src)
        {
            return utf2string(src, 0, src.Length);
        }

        /// <summary>
        /// Copy characters in source array to bytes in target array,
        ///  converting them to Utf8 representation.
        ///  The target array must be large enough to hold the result.
        ///  returns first index in `dst' past the last copied byte. </summary>
        ///  <param name="src">        The array holding the characters to convert. </param>
        ///  <param name="sindex">     The start index from which characters are converted. </param>
        ///  <param name="dst">        The array holding the converted characters.. </param>
        ///  <param name="dindex">     The start index from which converted bytes
        ///                    are written. </param>
        ///  <param name="len">        The maximum number of characters to convert. </param>
        public static int chars2utf(char[] src, int sindex, sbyte[] dst, int dindex, int len)
        {
            int j = dindex;
            int limit = sindex + len;
            for (int i = sindex; i < limit; i++)
            {
                char ch = src[i];
                if ((char)1 <= ch && ch <= (char)0x7F)
                {
                    dst[j++] = (sbyte)ch;
                }
                else if (ch <= (char)0x7FF)
                {
                    dst[j++] = unchecked((sbyte)(0xC0 | (ch >> 6)));
                    dst[j++] = unchecked((sbyte)(0x80 | (ch & 0x3F)));
                }
                else
                {
                    dst[j++] = unchecked((sbyte)(0xE0 | (ch >> 12)));
                    dst[j++] = unchecked((sbyte)(0x80 | ((ch >> 6) & 0x3F)));
                    dst[j++] = unchecked((sbyte)(0x80 | (ch & 0x3F)));
                }
            }
            return j;
        }

        /// <summary>
        /// Return characters as an array of bytes in Utf8 representation. </summary>
        ///  <param name="src">        The array holding the characters. </param>
        ///  <param name="sindex">     The start index from which characters are converted. </param>
        ///  <param name="len">        The maximum number of characters to convert. </param>
        public static sbyte[] chars2utf(char[] src, int sindex, int len)
        {
            sbyte[] dst = new sbyte[len * 3];
            int len1 = chars2utf(src, sindex, dst, 0, len);
            sbyte[] result = new sbyte[len1];
            Array.Copy(dst, 0, result, 0, len1);
            return result;
        }

        /// <summary>
        /// Return all characters in given array as an array of bytes
        ///  in Utf8 representation. </summary>
        ///  <param name="src">        The array holding the characters. </param>
        public static sbyte[] chars2utf(char[] src)
        {
            return chars2utf(src, 0, src.Length);
        }

        /// <summary>
        /// Return string as an array of bytes in in Utf8 representation.
        /// </summary>
        public static sbyte[] string2utf(string s)
        {
            return chars2utf(s.ToCharArray());
        }

        /// <summary>
        /// Escapes each character in a string that has an escape sequence or
        /// is non-printable ASCII.  Leaves non-ASCII characters alone.
        /// </summary>
        public static string quote(string s)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                buf.Append(quote(s[i]));
            }
            return buf.ToString();
        }

        /// <summary>
        /// Escapes a character if it has an escape sequence or is
        /// non-printable ASCII.  Leaves non-ASCII characters alone.
        /// </summary>
        public static string quote(char ch)
        {
            switch (ch)
            {
                case '\b':
                    return "\\b";
                case '\f':
                    return "\\f";
                case '\n':
                    return "\\n";
                case '\r':
                    return "\\r";
                case '\t':
                    return "\\t";
                case '\'':
                    return "\\'";
                case '\"':
                    return "\\\"";
                case '\\':
                    return "\\\\";
                default:
                    return (isPrintableAscii(ch)) ? ch.ToString() : string.Format("\\u{0:x4}", (int)ch);
            }
        }

        /// <summary>
        /// Is a character printable ASCII?
        /// </summary>
        private static bool isPrintableAscii(char ch)
        {
            return ch >= ' ' && ch <= '~';
        }

        /// <summary>
        /// Escape all unicode characters in string.
        /// </summary>
        public static string escapeUnicode(string s)
        {
            int len = s.Length;
            int i = 0;
            while (i < len)
            {
                char ch = s[i];
                if (ch > (char)255)
                {
                    StringBuilder buf = new StringBuilder();
                    buf.Append(s.Substring(0, i));
                    while (i < len)
                    {
                        ch = s[i];
                        if (ch > (char)255)
                        {
                            buf.Append("\\u");
                            buf.Append(forDigit((ch >> 12) % 16, 16)); //Character.forDigit
                            buf.Append(forDigit((ch >> 8) % 16, 16));
                            buf.Append(forDigit((ch >> 4) % 16, 16));
                            buf.Append(forDigit((ch) % 16, 16));
                        }
                        else
                        {
                            buf.Append(ch);
                        }
                        i++;
                    }
                    s = buf.ToString();
                }
                else
                {
                    i++;
                }
            }
            return s;
        }

        /* Conversion routines for qualified name splitting
         */
        /// <summary>
        /// Return the last part of a qualified name. </summary>
        ///  <param name="name"> the qualified name </param>
        ///  <returns> the last part of the qualified name </returns>
        public static Name shortName(Name name)
        {
            int start = name.lastIndexOf((sbyte)'.') + 1;
            int end = name.getByteLength();
            if (start == 0 && end == name.length())
            {
                return name;
            }
            return name.subName(name.lastIndexOf((sbyte)'.') + 1, name.getByteLength());
        }

        /// <summary>
        /// Return the last part of a qualified name from its string representation </summary>
        ///  <param name="name"> the string representation of the qualified name </param>
        ///  <returns> the last part of the qualified name </returns>
        public static string shortName(string name)
        {
            return name.Substring(name.LastIndexOf('.') + 1);
        }

        /// <summary>
        /// Return the package name of a class name, excluding the trailing '.',
        ///  "" if not existent.
        /// </summary>
        public static Name packagePart(Name classname)
        {
            return classname.subName(0, classname.lastIndexOf((sbyte)'.'));
        }

        public static string packagePart(string classname)
        {
            int lastDot = classname.LastIndexOf('.');
            return (lastDot < 0 ? "" : classname.Substring(0, lastDot));
        }

        public static List<Name> enclosingCandidates(Name name)
        {
            List<Name> names = new List<Name>();
            int index;
            while ((index = name.lastIndexOf((sbyte)'$')) > 0)
            {
                name = name.subName(0, index);
                // names = names.prepend(name);
                names.Insert(0, name);
            }
            return names;
        }

        public static List<Name> classCandidates(Name name)
        {
            List<Name> names = new List<Name>();
            string nameStr = name.ToString();
            int index = -1;
            while ((index = nameStr.IndexOf('.', index + 1)) > 0)
            {
                string pack = nameStr.Substring(0, index + 1);
                string clz = nameStr.Substring(index + 1).Replace('.', '$');
                // names = names.prepend(name.table.names.fromString(pack + clz));
                names.Insert(0, name.table.names.fromString(pack + clz));
            }
            // return  names.reverse();
            names.Reverse();
            return names;
        }


        /// <summary>
        /// The minimum radix available for conversion to and from strings.
        /// The constant value of this field is the smallest value permitted
        /// for the radix argument in radix-conversion methods such as the
        /// {@code digit} method, the {@code forDigit} method, and the
        /// {@code toString} method of class {@code Integer}.
        /// </summary>
        /// <seealso cref=     Character#digit(char, int) </seealso>
        /// <seealso cref=     Character#forDigit(int, int) </seealso>
        /// <seealso cref=     Integer#toString(int, int) </seealso>
        /// <seealso cref=     Integer#valueOf(String) </seealso>
        private const int MIN_RADIX = 2;

        /// <summary>
        /// The maximum radix available for conversion to and from strings.
        /// The constant value of this field is the largest value permitted
        /// for the radix argument in radix-conversion methods such as the
        /// {@code digit} method, the {@code forDigit} method, and the
        /// {@code toString} method of class {@code Integer}.
        /// </summary>
        /// <seealso cref=     Character#digit(char, int) </seealso>
        /// <seealso cref=     Character#forDigit(int, int) </seealso>
        /// <seealso cref=     Integer#toString(int, int) </seealso>
        /// <seealso cref=     Integer#valueOf(String) </seealso>
        private const int MAX_RADIX = 36;

        /// <summary>
        /// Determines the character representation for a specific digit in
        /// the specified radix. If the value of {@code radix} is not a
        /// valid radix, or the value of {@code digit} is not a valid
        /// digit in the specified radix, the null character
        /// ({@code '\u005Cu0000'}) is returned.
        /// <para>
        /// The {@code radix} argument is valid if it is greater than or
        /// equal to {@code MIN_RADIX} and less than or equal to
        /// {@code MAX_RADIX}. The {@code digit} argument is valid if
        /// {@code 0 <= digit < radix}.
        /// </para>
        /// <para>
        /// If the digit is less than 10, then
        /// {@code '0' + digit} is returned. Otherwise, the value
        /// {@code 'a' + digit - 10} is returned.
        ///     
        /// </para>
        /// </summary>
        /// <param name="digit">   the number to convert to a character. </param>
        /// <param name="radix">   the radix. </param>
        /// <returns>  the {@code char} representation of the specified digit
        ///          in the specified radix. </returns>
        /// <seealso cref=     Character#MIN_RADIX </seealso>
        /// <seealso cref=     Character#MAX_RADIX </seealso>
        /// <seealso cref=     Character#digit(char, int) </seealso>
        private static char forDigit(int digit, int radix)
        {
            if ((digit >= radix) || (digit < 0))
            {
                return '\0';
            }
            if ((radix < MIN_RADIX) || (radix > MAX_RADIX))
            {
                return '\0';
            }
            if (digit < 10)
            {
                return (char)('0' + digit);
            }
            return (char)('a' - 10 + digit);
        }

    }

}