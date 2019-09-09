namespace TypeScriptObject
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
        protected object __value__;
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
        /// 
        /// </summary>
        public static Number ToNumber(object obj)
        {
            String name = TypeOf(obj);
            if (name == "number")
            {
                return Number.parseFloat((Number)obj);
            }
            if (name == "string")
            {
                return Number.parseFloat((String)obj);
            }
            return NaN;
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
            switch (obj.GetType().Name)
            {
                case "Undefined":
                    return "undefined";

                case "String":
                    return "string";

                case "Number":
                case "Double":
                case "Int32":
                case "Int64":
                    return "number";

                case "Boolean":
                    return "boolean";

                default:
                    return "object";
            }
        }

        /// <summary>
        /// Get typescript object's actual value.
        /// </summary>
        public static object GetValue(object obj)
        {
            Object tobj = obj as Object;
            while (tobj != null)
            {
                obj = tobj.__value__;
                tobj = obj as Object;
            }
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsUndefined(Object obj)
        {
            return !IsNull(obj) && object.ReferenceEquals(obj.__value__, Undefined.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsNull(object obj)
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
