using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.DataVisualization.TypeScript
{
    public class Boolean : Object
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public Boolean(bool value)
        {
            this._value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        private Boolean(Undefined undefined)
        {
            this._value = undefined;
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
            return (bool)s._value;
        }
        #endregion
    }
}
