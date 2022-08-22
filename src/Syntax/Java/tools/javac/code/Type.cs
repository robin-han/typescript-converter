using System;
using System.Collections.Generic;
using System.Text;

namespace com.sun.tools.javac.code
{
    /** This class represents Java types. The class itself defines the behavior of
     *  the following types:
     *  <pre>
     *  base types (tags: BYTE, CHAR, SHORT, INT, LONG, FLOAT, DOUBLE, BOOLEAN),
     *  type `void' (tag: VOID),
     *  the bottom type (tag: BOT),
     *  the missing type (tag: NONE).
     *  </pre>
     *  <p>The behavior of the following types is defined in subclasses, which are
     *  all static inner classes of this class:
     *  <pre>
     *  class types (tag: CLASS, class: ClassType),
     *  array types (tag: ARRAY, class: ArrayType),
     *  method types (tag: METHOD, class: MethodType),
     *  package types (tag: PACKAGE, class: PackageType),
     *  type variables (tag: TYPEVAR, class: TypeVar),
     *  type arguments (tag: WILDCARD, class: WildcardType),
     *  generic method types (tag: FORALL, class: ForAll),
     *  the error type (tag: ERROR, class: ErrorType).
     *  </pre>
     *
     *  <p><b>This is NOT part of any supported API.
     *  If you write code that depends on this, you do so at your own risk.
     *  This code and its internal interfaces are subject to change or
     *  deletion without notice.</b>
     *
     *  @see TypeTag
     */
    public abstract class Type
    {
        // TODO:

        public TypeSymbol tsym;
    }
}
