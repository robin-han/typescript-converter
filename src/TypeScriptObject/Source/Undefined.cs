using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScriptObject
{
    public class Undefined
    {
        public static readonly Undefined Value = new Undefined();

        private Undefined()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator bool(Undefined obj)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator ==(Undefined str1, Undefined str2)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(Undefined str1, Undefined str2)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is Undefined)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
