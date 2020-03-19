using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.CSharp
{
    public class Boolean : Object
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public Boolean(bool value)
        {
            this.__value__ = value;
        }

        /// <summary>
        /// 
        /// </summary>
        private Boolean(Undefined undefined)
        {
            this.__value__ = undefined;
        }
        #endregion

        #region Operator Implicit
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Boolean(bool value)
        {
            return new Boolean(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Boolean(bool? value)
        {
            if (value == null)
            {
                return null;
            }
            return new Boolean(value.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Boolean(Undefined undefined)
        {
            return new Boolean(undefined);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator bool(Boolean s)
        {
            if (IsNull(s) || IsUndefined(s))
            {
                return false;
            }
            return (bool)s.__value__;
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator bool?(Boolean s)
        {
            if (IsNull(s) || IsUndefined(s))
            {
                return null;
            }
            return (bool)s.__value__;
        }
        #endregion

        #region Public Method

        ///<summary>
        ///
        ///</summary>
        public override String toString()
        {
            CheckUndefined();

            if ((bool)this.__value__ == true)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean valueOf()
        {
            return this;
        }
        #endregion
    }
}
