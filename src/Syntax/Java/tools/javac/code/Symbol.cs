
using System;
using System.Collections.Generic;
using System.Text;

namespace com.sun.tools.javac.code
{
    using com.sun.tools.javac.util;

    /** Root class for Java symbols. It contains subclasses
     *  for specific sorts of symbols, such as variables, methods and operators,
     *  types, packages. Each subclass is represented as a static inner class
     *  inside Symbol.
     *
     *  <p><b>This is NOT part of any supported API.
     *  If you write code that depends on this, you do so at your own risk.
     *  This code and its internal interfaces are subject to change or
     *  deletion without notice.</b>
     */
    public abstract class Symbol
    {
        // TODO:

        /// <summary>
        /// The name of this symbol in Utf8 representation.
        /// </summary>
        public Name name;
    }

    public class VarSymbol : Symbol
    {
        // TODO:
    }

    public abstract class TypeSymbol : Symbol
    {
        // TODO:
    }

    public class MethodSymbol : Symbol
    {
        // TODO:
    }

    public class OperatorSymbol : MethodSymbol
    {
        // TODO:
    }

    public class ClassSymbol : TypeSymbol
    {
        // TODO:
    }

    public class PackageSymbol : TypeSymbol
    {
        // TODO:
    }

}

