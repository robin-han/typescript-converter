using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.DataVisualization.TypeScript
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
            this._value = d;
        }

        /// <summary>
        /// 
        /// </summary>
        private Number(Undefined value)
        {
            this._value = value;
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
            if (int.TryParse(value, out int result))
            {
                if (fromBase != null && fromBase != 10)
                {
                    result = Convert.ToInt32(result.ToString(), (int)fromBase);
                }
                return new Number(result);
            }
            return NaN;
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
        #endregion

        #region Implicit Convert
        /// <summary>
        /// 
        /// </summary>
        public static explicit operator int(Number num)
        {
            return Convert.ToInt32(num._value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator long(Number num)
        {
            return Convert.ToInt64(num._value);
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
        public static implicit operator double(Number num)
        {
            return Convert.ToDouble(num._value);
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
            num._value = v + 1;
            return num;
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
            num._value = v - 1;
            return num;
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
            num._value = +v;
            return num;
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
            num._value = -v;
            return num;
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
        public static bool operator ==(Number num1, Number num2)
        {
            return Equals(num1, num2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(Number num1, Number num2)
        {
            return !Equals(num1, num2);
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
        #endregion

        #region Override Methods
        public static bool Equals(Number a, Number b)
        {
            bool isANull = IsNull(a) || IsUndefined(a);
            bool isBNull = IsNull(b) || IsUndefined(b);

            if (isANull && isBNull)
            {
                return true;
            }
            if (isANull || isBNull)
            {
                return false;
            }

            double v1 = ToDouble(a);
            double v2 = ToDouble(b);

            return v1 == v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Number))
            {
                return false;
            }


            return Equals(this, obj as Number);
        }

        /// <summary>
        /// 
        /// </summary>
        public override int GetHashCode()
        {
            this.CheckUndefined();

            if (!IsNull(this._value))
            {
                return this._value.GetHashCode();
            }
            return base.GetHashCode();
        }

        #endregion

        #region Public Methods
        public String toString(Number fromBase = null)
        {
            this.CheckUndefined();

            if (!IsNull(this._value))
            {
                throw new NullReferenceException();
            }

            if (fromBase != null && fromBase != 10)
            {
                return Convert.ToInt32(this._value.ToString(), (int)fromBase).ToString();
            }
            return this._value.ToString();
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
            return num._value == NaN._value;
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

            return Convert.ToDouble(num._value);
        }
        #endregion
    }
}
