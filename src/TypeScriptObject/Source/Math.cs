using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.CSharp
{
    public static class Math
    {
        private static readonly Random _random = new Random();
        private static readonly Number NaN = Number.NaN;

        #region Const
        public static readonly Number E = 2.718281828459045;
        public static readonly Number PI = 3.141592653589793;
        public static readonly Number LN2 = 0.6931471805599453;
        public static readonly Number LN10 = 2.302585092994046;
        public static readonly Number LOG2E = 1.4426950408889634;
        public static readonly Number LOG10E = 0.4342944819032518;
        public static readonly Number SQRT1_2 = 0.7071067811865476;
        public static readonly Number SQRT2 = 1.4142135623730951;
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public static Number abs(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Abs(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number acos(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Acos(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number acosh(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Cosh(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number asin(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Asin(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number asinh(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Sinh(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number atan(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Atan(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number atanh(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Tanh(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number atan2(Number y, Number x)
        {
            if (IsUndefined(x) || IsUndefined(y))
            {
                return NaN;
            }

            double v1 = IsNull(y) ? 0 : y;
            double v2 = IsNull(x) ? 0 : x;
            return System.Math.Atan2(v1, v2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number ceil(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Ceiling(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number cos(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Cos(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number cosh(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Cosh(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number exp(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Exp(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number floor(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Floor(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number log(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Log(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number log10(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Log10(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number max(params Number[] values)
        {
            Number max = Number.NEGATIVE_INFINITY;
            foreach (Number v in values)
            {
                max = v > max ? v : max;
            }
            return max;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number min(params Number[] values)
        {
            Number min = Number.POSITIVE_INFINITY;
            foreach (Number v in values)
            {
                min = v < min ? v : min;
            }
            return min;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number pow(Number x, Number y)
        {
            if (IsUndefined(x) || IsUndefined(y))
            {
                return NaN;
            }

            double v1 = IsNull(x) ? 0 : x;
            double v2 = IsNull(y) ? 0 : y;
            return System.Math.Pow(v1, v2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number random()
        {
            return new Number(_random.NextDouble());
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number round(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Round(v, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number sign(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Sign(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number sin(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Sin(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number sinh(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Sinh(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number sqrt(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Sqrt(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number tan(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Tan(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number tanh(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Tanh(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number trunc(Number num)
        {
            if (IsUndefined(num))
            {
                return NaN;
            }

            double v = IsNull(num) ? 0 : num;
            return System.Math.Truncate(v);
        }
        #endregion

        #region Private Methods
        private static bool IsNull(Number num)
        {
            return Object.IsNull(num);
        }

        private static bool IsUndefined(Number num)
        {
            return Object.IsUndefined(num);
        }
        #endregion
    }
}
