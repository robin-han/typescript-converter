namespace TypeScript.CSharp
{
    public class Object
    {
        #region Public Static Fields
        /// <summary>
        /// 
        /// </summary>
        public static readonly Undefined undefined = Undefined.Value;

        /// <summary>
        /// 
        /// </summary>
        public static readonly Number NaN = Number.NaN;

        /// <summary>
        /// 
        /// </summary>
        public static readonly Number Infinity = Number.POSITIVE_INFINITY;
        #endregion

        #region Fields
        /// <summary>
        /// Indicates the underlying value.
        /// </summary>
        private object _underlyingValue = null;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the underlying value.
        /// </summary>
        protected object __value__
        {
            get
            {
                return this._underlyingValue;
            }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException("value");
                }
                this._underlyingValue = value;
            }
        }
        #endregion

        #region Operator Implicit
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator bool(Object obj)
        {
            return !IsNull(obj) && !IsUndefined(obj);
        }
        #endregion

        #region Operator Overload
        /// <summary>
        /// 
        /// </summary>
        public static bool operator ==(Object obj1, Object obj2)
        {
            return Equals(obj1, obj2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(Object obj1, Object obj2)
        {
            return !Equals(obj1, obj2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator ==(object obj1, Object obj2)
        {
            if (obj1 is Undefined)
            {
                return IsUndefined(obj2) || IsNull(obj2);
            }

            return Equals(ToObject(obj1), obj2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(object obj1, Object obj2)
        {
            if (obj1 is Undefined)
            {
                return !(IsUndefined(obj2) || IsNull(obj2));
            }

            return !Equals(ToObject(obj1), obj2);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator ==(Object obj1, object obj2)
        {
            if (obj2 is Undefined)
            {
                return IsUndefined(obj1) || IsNull(obj1);
            }

            return Equals(obj1, ToObject(obj2));
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(Object obj1, object obj2)
        {
            if (obj2 is Undefined)
            {
                return !(IsUndefined(obj1) || IsNull(obj1));
            }

            return !Equals(obj1, ToObject(obj2));
        }
        #endregion

        #region Override Methods
        public override bool Equals(object obj)
        {
            if (!(obj is Object))
            {
                return false;
            }
            if (object.ReferenceEquals(this, NaN) || object.ReferenceEquals(obj, NaN))
            {
                return false;
            }

            if (base.Equals(obj))
            {
                return true;
            }
            else
            {
                return AreEquals(this, (Object)obj);
            }
        }

        public static bool Equals(Object a, Object b)
        {
            if (object.ReferenceEquals(a, NaN) || object.ReferenceEquals(b, NaN))
            {
                return false;
            }

            if (object.Equals(a, b))
            {
                return true;
            }
            else
            {
                return AreEquals(a, b);
            }
        }

        private static bool AreEquals(Object a, Object b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

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

            if (!Equals(a.GetType(), b.GetType()))
            {
                return false;
            }

            return AreInnerValueEquals(a.__value__, b.__value__);
        }

        private static bool AreInnerValueEquals(object valueA, object valueB)
        {
            bool isObjectA = valueA is Object;
            bool isObjectB = valueB is Object;

            if (isObjectA && isObjectB)
            {
                return Equals(valueA as Object, valueB as Object);
            }
            else if (isObjectA || isObjectB)
            {
                return false;
            }
            else if (valueA != null && valueB != null)
            {
                if (valueA is double && valueB is double)
                {
                    return (double)valueA == (double)valueB;
                }
                else
                {
                    return object.Equals(valueA, valueB);
                }
            }
            else //Object.__value__ cannot not null.
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            if (!IsNull(this.__value__))
            {
                return this.__value__.GetHashCode();
            }
            return base.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            this.CheckUndefined();

            if (!IsNull(this.__value__))
            {
                return this.__value__.ToString();
            }
            return base.ToString();
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        public virtual Array<T> AsArray<T>()
        {
            return this as Array<T>;
        }

        /// <summary>
        /// 
        /// </summary>        
        public virtual String toString()
        {
            return "[object Object]";
        }

        // /// <summary>
        // /// 
        // /// </summary>        
        // public Object valueOf()
        // {
        //     return this;
        // }

        /// <summary>
        /// 
        /// </summary>
        public static bool isNaN(Number num)
        {
            return Number.isNaN(num);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool isFinite(Number num)
        {
            return Number.isFinite(num);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number parseInt(String value, Number fromBase = null)
        {
            return Number.parseInt(value, fromBase);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number parseFloat(String value)
        {
            return Number.parseFloat(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number parseFloat(Number value)
        {
            return Number.parseFloat(value);
        }

        /// <summary>
        /// Represents javascript Number(...) function.
        /// </summary>
        public static Number ToNumber(object obj)
        {
            obj = ToObject(obj);
            if (obj is Number)
            {
                return Number.parseFloat((Number)obj);
            }
            if (obj is String)
            {
                return Number.parseFloat((String)obj);
            }
            return NaN;
        }


        public static String ToString(object obj)
        {
            if (obj == null)
            {
                return "null";
            }
            if (IsUndefined(obj))
            {
                return "undefined";
            }
            if (obj is Object)
            {
                return ((Object)obj).toString();
            }
            return System.Convert.ToString(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        private static Object ToObject(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is Object tsObj)
            {
                return tsObj;
            }

            if (ObjectUtil.IsPrimitiveNumber(obj))
            {
                return (Number)ObjectUtil.ToDouble(obj);
            }
            if (ObjectUtil.IsPrimitiveString(obj))
            {
                return (String)ObjectUtil.ToString(obj);
            }
            if (ObjectUtil.IsPrimitiveDate(obj))
            {
                return (Date)ObjectUtil.ToDateTime(obj);
            }
            if (ObjectUtil.IsPrimitiveBoolean(obj))
            {
                return (Boolean)ObjectUtil.ToBoolean(obj);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string TypeOf(object obj)
        {
            obj = GetValue(obj);

            if (obj == null)
            {
                return "object";
            }
            if (IsUndefined(obj))
            {
                return "undefined";
            }

            if (ObjectUtil.IsString(obj))
            {
                return "string";
            }
            if (ObjectUtil.IsNumber(obj))
            {
                return "number";
            }
            if (ObjectUtil.IsBoolean(obj))
            {
                return "boolean";
            }

            return "object";
        }

        /// <summary>
        /// Get typescript object's underlying value.
        /// </summary>
        public static object GetValue(object obj)
        {
            while ((obj is Object tsObj) && tsObj.__value__ != null)
            {
                obj = tsObj.__value__;
            }
            return obj;
        }

        #region Internal and Private Methods
        /// <summary>
        /// 
        /// </summary>
        internal static bool IsUndefined(object obj)
        {
            return GetValue(obj) == (object)undefined;
        }

        /// <summary>
        /// 
        /// </summary>
        internal static bool IsNull(object obj)
        {
            return obj == null;
        }
        #endregion

        // [System.Diagnostics.Conditional("DEBUG")]
        protected void CheckUndefined()
        {
            if (IsUndefined(this))
            {
                throw new System.NullReferenceException("undefined object");
            }
        }
        #endregion
    }
}
