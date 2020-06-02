using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.CSharp
{
    public class Number : Object
    {
        #region Public Static Fields
        /// <summary>
        /// 
        /// </summary>
        public static readonly Number MIN_VALUE = new Number(double.MinValue);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Number MAX_VALUE = new Number(double.MaxValue);

        /// <summary>
        /// 
        /// </summary>
        public new static readonly Number NaN = new Number(double.NaN);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Number NEGATIVE_INFINITY = new Number(double.NegativeInfinity);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Number POSITIVE_INFINITY = new Number(double.PositiveInfinity);

        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public Number(double d)
        {
            this.__value__ = d;
        }

        /// <summary>
        /// 
        /// </summary>
        private Number(Undefined value)
        {
            this.__value__ = value;
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// 
        /// </summary>
        public new static bool isNaN(Number num)
        {
            return IsNaN(num);
        }

        /// <summary>
        /// 
        /// </summary>
        public new static bool isFinite(Number num)
        {
            return num != POSITIVE_INFINITY && num != NEGATIVE_INFINITY;
        }

        /// <summary>
        /// 
        /// </summary>
        public new static Number parseInt(String value, Number fromBase = null)
        {
            try
            {
                if (fromBase != null && fromBase != 10)
                {
                    return Convert.ToInt32(value, (int)fromBase);
                }

                RegExpArray matchResult = value.match("^([\\s-+0-9]+).*");
                if (matchResult == null)
                {
                    return NaN;
                }
                else
                {
                    return int.Parse(matchResult[1]);
                }
            }
            catch
            {
                return NaN;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new static Number parseFloat(String value)
        {
            if (double.TryParse(value, out double result))
            {
                return new Number(result);
            }
            return NaN;
        }

        /// <summary>
        /// 
        /// </summary>
        public new static Number parseFloat(Number value)
        {
            return value;
        }
        #endregion

        #region Implicit Convert
        /// <summary>
        /// 
        /// </summary>
        public static explicit operator int(Number num)
        {
            return Convert.ToInt32(num.__value__);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator int?(Number num)
        {
            if (num == null)
            {
                return null;
            }
            return Convert.ToInt32(num.__value__);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator long(Number num)
        {
            return Convert.ToInt64(num.__value__);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator long?(Number num)
        {
            if (num == null)
            {
                return null;
            }
            return Convert.ToInt64(num.__value__);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator float(Number num)
        {
            return Convert.ToSingle(num.__value__);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator float?(Number num)
        {
            if (num == null)
            {
                return null;
            }
            return Convert.ToSingle(num.__value__);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Number(double value)
        {
            return new Number(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Number(double? value)
        {
            if (value == null)
            {
                return null;
            }
            return new Number(value.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator double(Number num)
        {
            return Convert.ToDouble(num.__value__);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator double?(Number num)
        {
            if (num == null)
            {
                return null;
            }
            return Convert.ToDouble(num.__value__);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Number(Undefined value)
        {
            return new Number(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator bool(Number num)
        {
            if (IsInvalid(num))
            {
                return false;
            }

            double v = ToDouble(num);

            return v != 0;
        }
        #endregion

        #region Operator Overload
        /// <summary>
        /// 
        /// </summary>
        public static Number operator +(Number num1, Number num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return NaN;
            }

            double v1 = ToDouble(num1);
            double v2 = ToDouble(num2);

            return v1 + v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator -(Number num1, Number num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return NaN;
            }

            double v1 = ToDouble(num1);
            double v2 = ToDouble(num2);

            return v1 - v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator *(Number num1, Number num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return NaN;
            }

            double v1 = ToDouble(num1);
            double v2 = ToDouble(num2);

            return v1 * v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator /(Number num1, Number num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return NaN;
            }

            double v1 = ToDouble(num1);
            double v2 = ToDouble(num2);

            return v1 / v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator ++(Number num)
        {
            if (IsInvalid(num))
            {
                return NaN;
            }

            double v = ToDouble(num);
            return v + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator --(Number num)
        {
            if (IsInvalid(num))
            {
                return NaN;
            }

            double v = ToDouble(num);
            return v - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator +(Number num)
        {
            if (IsInvalid(num))
            {
                return NaN;
            }

            double v = ToDouble(num);
            return +v;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator -(Number num)
        {
            if (IsInvalid(num))
            {
                return NaN;
            }

            double v = ToDouble(num);
            return -v;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator %(Number num1, Number num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return NaN;
            }

            double v1 = ToDouble(num1);
            double v2 = ToDouble(num2);

            return v1 % v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator >(Number num1, Number num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return false;
            }

            double v1 = ToDouble(num1);
            double v2 = ToDouble(num2);

            return v1 > v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator <(Number num1, Number num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return false;
            }

            double v1 = ToDouble(num1);
            double v2 = ToDouble(num2);

            return v1 < v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator >=(Number num1, Number num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return false;
            }

            double v1 = ToDouble(num1);
            double v2 = ToDouble(num2);

            return v1 >= v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator <=(Number num1, Number num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return false;
            }

            double v1 = ToDouble(num1);
            double v2 = ToDouble(num2);

            return v1 <= v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator <<(Number num1, int num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return NaN;
            }

            return ((int)num1 << num2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator >>(Number num1, int num2)
        {
            if (IsInvalid(num1) || IsInvalid(num2))
            {
                return NaN;
            }

            return ((int)num1 >> num2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number operator |(Number num1, Number num2)
        {
            bool isValidNum1 = !IsInvalid(num1);
            bool isValidNum2 = !IsInvalid(num2);

            if (isValidNum1 && isValidNum2)
            {
                return ((int)num1 | (int)num2);
            }
            else if (isValidNum1)
            {
                return IsNull(num1) ? 0 : num1;
            }
            else if (isValidNum2)
            {
                return IsNull(num2) ? 0 : num2;
            }
            else
            {
                return 0;
            }
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

            return ToDouble(this).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string ToString(IFormatProvider provider)
        {
            this.CheckUndefined();

            return ToDouble(this).ToString(provider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            this.CheckUndefined();

            return ToDouble(this).ToString(format);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider provider)
        {
            this.CheckUndefined();

            return ToDouble(this).ToString(format, provider);
        }

        /// <summary>
        /// 
        /// </summary>
        public override String toString()
        {
            this.CheckUndefined();

            if (IsNaN(this))
            {
                return "NaN";
            }

            return this.__value__.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        public String toFixed(int digits = 0)
        {
            return this.toFixed((Number)digits);
        }

        /// <summary>
        /// 
        /// </summary>
        public String toFixed(Number digits)
        {
            this.CheckUndefined();

            double v = ToDouble(this);
            return System.Math.Round(v, (int)digits, MidpointRounding.AwayFromZero).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromBase"></param>
        /// <returns></returns>
        public String toString(Number toBase = null)
        {
            this.CheckUndefined();

            if (IsNaN(this))
            {
                return "NaN";
            }

            if (toBase != null && toBase != 10)
            {
                var value = Convert.ToInt64(this.__value__);
                return Convert.ToString(value, (int)toBase);
            }
            return this.__value__.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Number valueOf()
        {
            return this;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        private static bool IsInvalid(Number num)
        {
            return IsNaN(num) || IsUndefined(num);
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool IsNaN(Number num)
        {
            if (IsNull(num))
            {
                return false;
            }

            object value = num.__value__;
            if (object.ReferenceEquals(value, Undefined.Value))
            {
                return true;
            }
            else if (double.IsNaN(Convert.ToDouble(value)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static double ToDouble(Number num)
        {
            if (IsNull(num))
            {
                return 0;
            }

            return Convert.ToDouble(num.__value__);
        }
        #endregion
    }
}
