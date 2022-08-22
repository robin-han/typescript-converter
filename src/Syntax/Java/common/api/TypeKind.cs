using System;
using System.Collections.Generic;
using System.Text;

namespace java.lang.common.api
{
    public enum TypeKind
    {
        /**
         * The primitive type {@code boolean}.
         */
        BOOLEAN,

        /**
         * The primitive type {@code byte}.
         */
        BYTE,

        /**
         * The primitive type {@code short}.
         */
        SHORT,

        /**
         * The primitive type {@code int}.
         */
        INT,

        /**
         * The primitive type {@code long}.
         */
        LONG,

        /**
         * The primitive type {@code char}.
         */
        CHAR,

        /**
         * The primitive type {@code float}.
         */
        FLOAT,

        /**
         * The primitive type {@code double}.
         */
        DOUBLE,

        /**
         * The pseudo-type corresponding to the keyword {@code void}.
         * @see NoType
         */
        VOID,

        /**
         * A pseudo-type used where no actual type is appropriate.
         * @see NoType
         */
        NONE,

        /**
         * The null type.
         */
        NULL,

        /**
         * An array type.
         */
        ARRAY,

        /**
         * A class or interface type.
         */
        DECLARED,

        /**
         * A class or interface type that could not be resolved.
         */
        ERROR,

        /**
         * A type variable.
         */
        TYPEVAR,

        /**
         * A wildcard type argument.
         */
        WILDCARD,

        /**
         * A pseudo-type corresponding to a package element.
         * @see NoType
         */
        PACKAGE,

        /**
         * A method, constructor, or initializer.
         */
        EXECUTABLE,

        /**
         * An implementation-reserved type.
         * This is not the type you are looking for.
         */
        OTHER,

        /**
          * A union type.
          *
          * @since 1.7
          */
        UNION,

        /**
          * An intersection type.
          *
          * @since 1.8
          */
        INTERSECTION

        /**
         * Returns {@code true} if this kind corresponds to a primitive
         * type and {@code false} otherwise.
         * @return {@code true} if this kind corresponds to a primitive type
         */

    }

    class TypeKinds
    {
        public bool isPrimitive(TypeKind kind)
        {
            switch (kind)
            {
                case TypeKind.BOOLEAN:
                case TypeKind.BYTE:
                case TypeKind.SHORT:
                case TypeKind.INT:
                case TypeKind.LONG:
                case TypeKind.CHAR:
                case TypeKind.FLOAT:
                case TypeKind.DOUBLE:
                    return true;

                default:
                    return false;
            }
        }
    }

}
