namespace GrapeCity.DataVisualization.TypeScript
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
        #endregion

        #region Fields
        /// <summary>
        /// 
        /// </summary>
        protected object _value;
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
        #endregion

        #region Override Methods
        public static bool Equals(Object a, Object b)
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

            if (!Equals(a.GetType(), b.GetType()))
            {
                return false;
            }
            return AreValueEquals(a._value, b._value);
        }

        public override bool Equals(object obj)
        {
            bool isANull = IsNull(this) || IsUndefined(this);
            bool isBNull = IsNull(obj) || IsUndefined(obj);
            if (isANull && isBNull)
            {
                return true;
            }
            if (isANull || isBNull)
            {
                return false;
            }

            if (!(obj is Object))
            {
                return false;
            }

            return AreValueEquals(this._value, (obj as Object)._value);
        }

        protected static bool AreValueEquals(object valueA, object valueB)
        {
            if (valueA is Object && valueB is Object)
            {
                return Equals(valueA as Object, valueB as Object);
            }
            else if (valueA is Object || valueB is Object)
            {
                return false;
            }
            else
            {
                return object.Equals(valueA, valueB);
            }
        }

        public override int GetHashCode()
        {
            if (!IsNull(this._value))
            {
                return this._value.GetHashCode();
            }
            return base.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            this.CheckUndefined();

            if (!IsNull(this._value))
            {
                return this._value.ToString();
            }
            return base.ToString();
        }

        #endregion

        #region Public Methods
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
        #endregion

        #region Internal Methods
        /// <summary>
        /// 
        /// </summary>
        internal static bool IsUndefined(object obj)
        {
            if (obj is Undefined)
            {
                return true;
            }

            if (obj is Object)
            {
                return object.Equals((obj as Object)._value, undefined);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        internal static bool IsNull(object obj)
        {
            return obj == null;
        }
        #endregion

        #region Private Methods
        protected void CheckUndefined()
        {
            if (IsUndefined(this))
            {
                throw new System.NullReferenceException();
            }
        }
        #endregion
    }
}
