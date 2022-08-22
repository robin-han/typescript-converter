using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;
using System.Collections.Generic;

/*
 * Copyright (c) 1999, 2014, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

namespace com.sun.tools.javac.code
{
    using java.lang.common.api;
    //using Kind = com.sun.source.tree.Tree.Kind;

    //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    using static com.sun.tools.javac.code.TypeTag.NumericClasses;

    /// <summary>
    /// An interface for type tag values, which distinguish between different
    ///  sorts of types.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public sealed class TypeTag
    {
        /// <summary>
        /// The tag of the basic type `byte'.
        /// </summary>
        public static readonly TypeTag BYTE = new TypeTag("BYTE", InnerEnum.BYTE, BYTE_CLASS, BYTE_SUPERCLASSES, true);

        /// <summary>
        /// The tag of the basic type `char'.
        /// </summary>
        public static readonly TypeTag CHAR = new TypeTag("CHAR", InnerEnum.CHAR, CHAR_CLASS, CHAR_SUPERCLASSES, true);

        /// <summary>
        /// The tag of the basic type `short'.
        /// </summary>
        public static readonly TypeTag SHORT = new TypeTag("SHORT", InnerEnum.SHORT, SHORT_CLASS, SHORT_SUPERCLASSES, true);

        /// <summary>
        /// The tag of the basic type `long'.
        /// </summary>
        public static readonly TypeTag LONG = new TypeTag("LONG", InnerEnum.LONG, LONG_CLASS, LONG_SUPERCLASSES, true);

        /// <summary>
        /// The tag of the basic type `float'.
        /// </summary>
        public static readonly TypeTag FLOAT = new TypeTag("FLOAT", InnerEnum.FLOAT, FLOAT_CLASS, FLOAT_SUPERCLASSES, true);
        /// <summary>
        /// The tag of the basic type `int'.
        /// </summary>
        public static readonly TypeTag INT = new TypeTag("INT", InnerEnum.INT, INT_CLASS, INT_SUPERCLASSES, true);
        /// <summary>
        /// The tag of the basic type `double'.
        /// </summary>
        public static readonly TypeTag DOUBLE = new TypeTag("DOUBLE", InnerEnum.DOUBLE, DOUBLE_CLASS, DOUBLE_CLASS, true);
        /// <summary>
        /// The tag of the basic type `boolean'.
        /// </summary>
        public static readonly TypeTag BOOLEAN = new TypeTag("BOOLEAN", InnerEnum.BOOLEAN, 0, 0, true);

        /// <summary>
        /// The tag of the type `void'.
        /// </summary>
        public static readonly TypeTag VOID = new TypeTag("VOID", InnerEnum.VOID);

        /// <summary>
        /// The tag of all class and interface types.
        /// </summary>
        public static readonly TypeTag CLASS = new TypeTag("CLASS", InnerEnum.CLASS);

        /// <summary>
        /// The tag of all array types.
        /// </summary>
        public static readonly TypeTag ARRAY = new TypeTag("ARRAY", InnerEnum.ARRAY);

        /// <summary>
        /// The tag of all (monomorphic) method types.
        /// </summary>
        public static readonly TypeTag METHOD = new TypeTag("METHOD", InnerEnum.METHOD);

        /// <summary>
        /// The tag of all package "types".
        /// </summary>
        public static readonly TypeTag PACKAGE = new TypeTag("PACKAGE", InnerEnum.PACKAGE);

        /// <summary>
        /// The tag of all module "types".
        /// </summary>
        public static readonly TypeTag MODULE = new TypeTag("MODULE", InnerEnum.MODULE);

        /// <summary>
        /// The tag of all (source-level) type variables.
        /// </summary>
        public static readonly TypeTag TYPEVAR = new TypeTag("TYPEVAR", InnerEnum.TYPEVAR);

        /// <summary>
        /// The tag of all type arguments.
        /// </summary>
        public static readonly TypeTag WILDCARD = new TypeTag("WILDCARD", InnerEnum.WILDCARD);

        /// <summary>
        /// The tag of all polymorphic (method-) types.
        /// </summary>
        public static readonly TypeTag FORALL = new TypeTag("FORALL", InnerEnum.FORALL);

        /// <summary>
        /// The tag of deferred expression types in method context
        /// </summary>
        public static readonly TypeTag DEFERRED = new TypeTag("DEFERRED", InnerEnum.DEFERRED);

        /// <summary>
        /// The tag of the bottom type {@code <null>}.
        /// </summary>
        public static readonly TypeTag BOT = new TypeTag("BOT", InnerEnum.BOT);

        /// <summary>
        /// The tag of a missing type.
        /// </summary>
        public static readonly TypeTag NONE = new TypeTag("NONE", InnerEnum.NONE);

        /// <summary>
        /// The tag of the error type.
        /// </summary>
        public static readonly TypeTag ERROR = new TypeTag("ERROR", InnerEnum.ERROR);

        /// <summary>
        /// The tag of an unknown type
        /// </summary>
        public static readonly TypeTag UNKNOWN = new TypeTag("UNKNOWN", InnerEnum.UNKNOWN);

        /// <summary>
        /// The tag of all instantiatable type variables.
        /// </summary>
        public static readonly TypeTag UNDETVAR = new TypeTag("UNDETVAR", InnerEnum.UNDETVAR);

        /// <summary>
        /// Pseudo-types, these are special tags
        /// </summary>
        public static readonly TypeTag UNINITIALIZED_THIS = new TypeTag("UNINITIALIZED_THIS", InnerEnum.UNINITIALIZED_THIS);

        public static readonly TypeTag UNINITIALIZED_OBJECT = new TypeTag("UNINITIALIZED_OBJECT", InnerEnum.UNINITIALIZED_OBJECT);

        private static readonly IList<TypeTag> valueList = new List<TypeTag>();

        static TypeTag()
        {
            valueList.Add(BYTE);
            valueList.Add(CHAR);
            valueList.Add(SHORT);
            valueList.Add(LONG);
            valueList.Add(FLOAT);
            valueList.Add(INT);
            valueList.Add(DOUBLE);
            valueList.Add(BOOLEAN);
            valueList.Add(VOID);
            valueList.Add(CLASS);
            valueList.Add(ARRAY);
            valueList.Add(METHOD);
            valueList.Add(PACKAGE);
            valueList.Add(MODULE);
            valueList.Add(TYPEVAR);
            valueList.Add(WILDCARD);
            valueList.Add(FORALL);
            valueList.Add(DEFERRED);
            valueList.Add(BOT);
            valueList.Add(NONE);
            valueList.Add(ERROR);
            valueList.Add(UNKNOWN);
            valueList.Add(UNDETVAR);
            valueList.Add(UNINITIALIZED_THIS);
            valueList.Add(UNINITIALIZED_OBJECT);
        }

        public enum InnerEnum
        {
            BYTE,
            CHAR,
            SHORT,
            LONG,
            FLOAT,
            INT,
            DOUBLE,
            BOOLEAN,
            VOID,
            CLASS,
            ARRAY,
            METHOD,
            PACKAGE,
            MODULE,
            TYPEVAR,
            WILDCARD,
            FORALL,
            DEFERRED,
            BOT,
            NONE,
            ERROR,
            UNKNOWN,
            UNDETVAR,
            UNINITIALIZED_THIS,
            UNINITIALIZED_OBJECT
        }

        public readonly InnerEnum innerEnumValue;
        private readonly string nameValue;
        private readonly int ordinalValue;
        private static int nextOrdinal = 0;

        internal readonly int superClasses;
        internal readonly int numericClass;
        internal readonly bool isPrimitive;

        private TypeTag(string name, InnerEnum innerEnum) : this(name, innerEnum, 0, 0, false)
        {

            nameValue = name;
            ordinalValue = nextOrdinal++;
            innerEnumValue = innerEnum;
        }

        private TypeTag(string name, InnerEnum innerEnum, int numericClass, int superClasses, bool isPrimitive)
        {
            this.superClasses = superClasses;
            this.numericClass = numericClass;
            this.isPrimitive = isPrimitive;

            nameValue = name;
            ordinalValue = nextOrdinal++;
            innerEnumValue = innerEnum;
        }

        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: JAVA to C# Converter Cracked By X-Cracker does not convert types within enums:
        public static class NumericClasses
        {
            public static int BYTE_CLASS = 1;
            public static int CHAR_CLASS = 2;
            public static int SHORT_CLASS = 4;
            public static int INT_CLASS = 8;
            public static int LONG_CLASS = 16;
            public static int FLOAT_CLASS = 32;
            public static int DOUBLE_CLASS = 64;

            public static int BYTE_SUPERCLASSES = BYTE_CLASS | SHORT_CLASS | INT_CLASS | LONG_CLASS | FLOAT_CLASS | DOUBLE_CLASS;

            public static int CHAR_SUPERCLASSES = CHAR_CLASS | INT_CLASS | LONG_CLASS | FLOAT_CLASS | DOUBLE_CLASS;

            public static int SHORT_SUPERCLASSES = SHORT_CLASS | INT_CLASS | LONG_CLASS | FLOAT_CLASS | DOUBLE_CLASS;

            public static int INT_SUPERCLASSES = INT_CLASS | LONG_CLASS | FLOAT_CLASS | DOUBLE_CLASS;

            public static int LONG_SUPERCLASSES = LONG_CLASS | FLOAT_CLASS | DOUBLE_CLASS;

            public static int FLOAT_SUPERCLASSES = FLOAT_CLASS | DOUBLE_CLASS;
        }

        public bool isStrictSubRangeOf(TypeTag tag)
        {
            /*  Please don't change the implementation of this method to call method
             *  isSubRangeOf. Both methods are called from hotspot code, the current
             *  implementation is better performance-wise than the commented modification.
             */
            return (this.superClasses & tag.numericClass) != 0 && this != tag;
        }

        public bool isSubRangeOf(TypeTag tag)
        {
            return (this.superClasses & tag.numericClass) != 0;
        }

        /// <summary>
        /// Returns the number of type tags.
        /// </summary>
        public static int getTypeTagCount()
        {
            // last two tags are not included in the total as long as they are pseudo-types
            return (UNDETVAR.ordinal() + 1);
        }

        public Kind getKindLiteral()
        {
            switch (this.innerEnumValue)
            {
                case InnerEnum.INT:
                    return Kind.INT_LITERAL;
                case InnerEnum.LONG:
                    return Kind.LONG_LITERAL;
                case InnerEnum.FLOAT:
                    return Kind.FLOAT_LITERAL;
                case InnerEnum.DOUBLE:
                    return Kind.DOUBLE_LITERAL;
                case InnerEnum.BOOLEAN:
                    return Kind.BOOLEAN_LITERAL;
                case InnerEnum.CHAR:
                    return Kind.CHAR_LITERAL;
                case InnerEnum.CLASS:
                    return Kind.STRING_LITERAL;
                case InnerEnum.BOT:
                    return Kind.NULL_LITERAL;
                default:
                    throw new AssertionError("unknown literal kind " + this);
            }
        }

        public TypeKind getPrimitiveTypeKind()
        {
            switch (this.innerEnumValue)
            {
                case InnerEnum.BOOLEAN:
                    return TypeKind.BOOLEAN;
                case InnerEnum.BYTE:
                    return TypeKind.BYTE;
                case InnerEnum.SHORT:
                    return TypeKind.SHORT;
                case InnerEnum.INT:
                    return TypeKind.INT;
                case InnerEnum.LONG:
                    return TypeKind.LONG;
                case InnerEnum.CHAR:
                    return TypeKind.CHAR;
                case InnerEnum.FLOAT:
                    return TypeKind.FLOAT;
                case InnerEnum.DOUBLE:
                    return TypeKind.DOUBLE;
                case InnerEnum.VOID:
                    return TypeKind.VOID;
                default:
                    throw new AssertionError("unknown primitive type " + this);
            }
        }

        /// <summary>
        /// Returns true if the given value is within the allowed range for this type. </summary>
        public bool checkRange(int value)
        {
            switch (this.innerEnumValue)
            {
                case InnerEnum.BOOLEAN:
                    return 0 <= value && value <= 1;
                case InnerEnum.BYTE:
                    return sbyte.MinValue <= value && value <= sbyte.MaxValue;
                case InnerEnum.CHAR:
                    return char.MinValue <= value && value <= char.MaxValue;
                case InnerEnum.SHORT:
                    return short.MinValue <= value && value <= short.MaxValue;
                case InnerEnum.INT:
                    return true;
                default:
                    throw new AssertionError();
            }
        }

        public static IList<TypeTag> values()
        {
            return valueList;
        }

        public int ordinal()
        {
            return ordinalValue;
        }

        public override string ToString()
        {
            return nameValue;
        }

        public static TypeTag valueOf(string name)
        {
            foreach (TypeTag enumInstance in TypeTag.valueList)
            {
                if (enumInstance.nameValue == name)
                {
                    return enumInstance;
                }
            }
            throw new System.ArgumentException(name);
        }
    }

}