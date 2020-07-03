using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.CSharp
{
    /// <summary>
    /// Defines class.
    /// </summary>
    public class ObjectUtil
    {
        /// <summary>
        /// Checks whether the object is dotnet primitive number.
        /// </summary>
        internal static bool IsPrimitiveNumber(object obj)
        {
            if (obj == null) { return false; }

            Type type = obj.GetType();
            return type == typeof(double)
               || type == typeof(float)
               || type == typeof(decimal)
               || type == typeof(long)
               || type == typeof(int)
               || type == typeof(short)
               || type == typeof(sbyte)
               || type == typeof(ulong)
               || type == typeof(uint)
               || type == typeof(ushort)
               || type == typeof(byte);
        }

        /// <summary>
        /// Checks whether the object is dotnet primitive string.
        /// </summary>
        internal static bool IsPrimitiveString(object obj)
        {
            if (obj == null) { return false; }

            return obj.GetType() == typeof(string);
        }

        /// <summary>
        /// Checks whether the object is dotnet primitive boolean.
        /// </summary>
        internal static bool IsPrimitiveBoolean(object obj)
        {
            if (obj == null) { return false; }

            return obj.GetType() == typeof(bool);
        }

        /// <summary>
        /// Checks whether the object is dotnet primitive datetime.
        /// </summary>
        internal static bool IsPrimitiveDate(object obj)
        {
            if (obj == null) { return false; }

            return obj.GetType() == typeof(DateTime);
        }

        /// <summary>
        /// Checks whether the object is number object.
        /// </summary>
        public static bool IsNumber(object obj)
        {
            if (obj is Number)
            {
                return true;
            }

            object value = Object.GetValue(obj);
            return IsPrimitiveNumber(value);
        }

        /// <summary>
        /// Check whether the object is string object.
        /// </summary>
        public static bool IsString(object obj)
        {
            if (obj is String)
            {
                return true;
            }

            object value = Object.GetValue(obj);
            return IsPrimitiveString(value);
        }

        /// <summary>
        /// Checks whether the object is boolean object.
        /// </summary>
        public static bool IsBoolean(object obj)
        {
            if (obj is Boolean)
            {
                return true;
            }

            object value = Object.GetValue(obj);
            return IsPrimitiveBoolean(value);
        }

        /// <summary>
        /// Checks whether the object is date object.
        /// </summary>
        public static bool IsDate(object obj)
        {
            if (obj is Date)
            {
                return true;
            }

            object value = Object.GetValue(obj);
            return IsPrimitiveDate(value);
        }

        /// <summary>
        /// Converts object to double.
        /// </summary>
        public static double ToDouble(object obj)
        {
            object value = Object.GetValue(obj);

            if (value == null)
            {
                return 0;
            }
            if (IsPrimitiveNumber(value))
            {
                return Convert.ToDouble(value);
            }
            if (IsPrimitiveString(value))
            {
                if (double.TryParse(Convert.ToString(value), out double d))
                {
                    return d;
                }
                return double.NaN;
            }
            if (IsPrimitiveBoolean(value))
            {
                return Convert.ToBoolean(value) ? 1 : 0;
            }
            if (IsPrimitiveDate(value))
            {
                return new Date(Convert.ToDateTime(value)).valueOf();
            }
            return double.NaN;
        }

        /// <summary>
        /// Convert object to Int32.
        /// </summary>
        public static int ToInt32(object obj)
        {
            return Convert.ToInt32(ToDouble(obj));
        }

        /// <summary>
        /// Converts object to string.
        /// </summary>
        public static string ToString(object obj)
        {
            return Object.ToString(obj);
        }

        /// <summary>
        /// Convert object to boolean.
        /// </summary>
        public static bool ToBoolean(object obj)
        {
            object value = Object.GetValue(obj);

            if (value == null || value is Undefined)
            {
                return false;
            }

            if (IsPrimitiveBoolean(value))
            {
                return Convert.ToBoolean(value);
            }
            if (IsPrimitiveNumber(value))
            {
                double d = Convert.ToDouble(value);
                return (d != 0 && !double.IsNaN(d));
            }
            if (IsPrimitiveString(value))
            {
                string s = Convert.ToString(value);
                return !string.IsNullOrEmpty(s);
            }
            return true;
        }

        /// <summary>
        /// Convert object to Date type.
        /// </summary>
        public static Date ToDate(object obj)
        {
            if (obj is Date)
            {
                return (Date)obj;
            }

            object value = Object.GetValue(obj);
            if (IsPrimitiveDate(value))
            {
                return Convert.ToDateTime(value);
            }
            return null;
        }

        /// <summary>
        /// Convert object to datetime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj)
        {
            object value = Object.GetValue(obj);
            return Convert.ToDateTime(value);
        }

        /// <summary>
        /// Compare two object act as javascript compare behavior.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed number value that indicates the result.
        ///     -2: x is not less, equal or greater y.
        ///     -1: x less than y. 
        ///      0:  x equals y. 
        ///      1:  x greater than y.
        /// </returns>
        public static int Compare(object x, object y)
        {
            x = Object.GetValue(x);
            y = Object.GetValue(y);

            bool xIsString = IsPrimitiveString(x);
            bool yIsString = IsPrimitiveString(y);
            if ((xIsString && (Object.IsNull(y) || Object.IsUndefined(y) || yIsString))
             || (yIsString && (Object.IsNull(x) || Object.IsUndefined(x) || xIsString)))
            {
                return String.Compare(Object.ToString(x), Object.ToString(y));
            }

            if (IsPrimitiveBoolean(x)) { x = Convert.ToBoolean(x) ? 1 : 0; }
            if (IsPrimitiveBoolean(y)) { y = Convert.ToBoolean(y) ? 1 : 0; }
            if (IsPrimitiveDate(x)) { x = new Date(Convert.ToDateTime(x)).valueOf(); }
            if (IsPrimitiveDate(y)) { y = new Date(Convert.ToDateTime(y)).valueOf(); }

            Type xType = x == null ? null : x.GetType();
            Type yType = y == null ? null : y.GetType();
            if (x is IComparable && (xType == yType || xType.IsAssignableFrom(yType)))
            {
                return ((IComparable)x).CompareTo(y);
            }
            if (y is IComparable && (yType == xType || yType.IsAssignableFrom(xType)))
            {
                return -((IComparable)y).CompareTo(x);
            }

            bool xIsNumber = IsNumber(x);
            bool yIsNumber = IsNumber(y);
            if (xIsNumber || yIsNumber)
            {
                x = (x == null ? 0 : x);
                y = (y == null ? 0 : y);

                if (xIsNumber && yIsNumber)
                {
                    double x1 = ToDouble(x);
                    double y1 = ToDouble(y);
                    return (x1 == y1 ? 0 : (x1 > y1 ? 1 : -1));
                }
                if (xIsNumber && IsString(y) && double.TryParse(ToString(y), out double y2))
                {
                    double x2 = ToDouble(x);
                    return (x2 == y2 ? 0 : (x2 > y2 ? 1 : -1));
                }
                if (yIsNumber && IsString(x) && double.TryParse(ToString(x), out double x3))
                {
                    double y3 = ToDouble(y);
                    return (x3 == y3 ? 0 : (x3 > y3 ? 1 : -1));
                }
                return -2;
            }

            return (Equals(x, y) ? 0 : -2);
        }
    }
}
