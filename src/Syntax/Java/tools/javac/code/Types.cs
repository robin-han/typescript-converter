using System;
using System.Collections.Generic;
using System.Text;

namespace com.sun.tools.javac.code
{
    /// <summary>
    /// Utility class containing various operations on types.
    /// 
    /// <para>Unless other names are more illustrative, the following naming
    /// conventions should be observed in this file:
    /// 
    /// <dl>
    /// <dt>t</dt>
    /// <dd>If the first argument to an operation is a type, it should be named t.</dd>
    /// <dt>s</dt>
    /// <dd>Similarly, if the second argument to an operation is a type, it should be named s.</dd>
    /// <dt>ts</dt>
    /// <dd>If an operations takes a list of types, the first should be named ts.</dd>
    /// <dt>ss</dt>
    /// <dd>A second list of types should be named ss.</dd>
    /// </dl>
    /// 
    /// </para>
    /// <para><b>This is NOT part of any supported API.
    /// If you write code that depends on this, you do so at your own risk.
    /// This code and its internal interfaces are subject to change or
    /// deletion without notice.</b>
    /// </para>
    /// </summary>
    public class Types
    {
        /// <summary>
        /// Find the type of the method descriptor associated to this class symbol -
        /// if the symbol 'origin' is not a functional interface, an exception is thrown.
        /// </summary>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public Type findDescriptorType(Type origin) throws FunctionDescriptorLookupError
        public virtual Type findDescriptorType(Type origin)
        {
            // return descCache.get(origin.tsym).getType(origin);
            throw new System.NotImplementedException();
        }

        public virtual Type createErrorType(Type originalType)
        {
            // return new ErrorType(originalType, syms.errSymbol);
            throw new System.NotImplementedException();
        }
    }
}
