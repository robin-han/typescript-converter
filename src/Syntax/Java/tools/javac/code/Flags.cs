using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

/*
 * Copyright (c) 1999, 2020, Oracle and/or its affiliates. All rights reserved.
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
    using com.sun.tools.javac.util;

    //using Assert = com.sun.tools.javac.util.Assert;
    //using StringUtils = com.sun.tools.javac.util.StringUtils;

    /// <summary>
    /// Access flags and other modifiers for Java classes and members.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class Flags
    {

        private Flags()
        {
        } // uninstantiable

        public static string ToString(long flags)
        {
            StringBuilder buf = new StringBuilder();
            string sep = "";
            foreach (Flag flag in asFlagSet(flags))
            {
                buf.Append(sep);
                buf.Append(flag);
                sep = " ";
            }
            return buf.ToString();
        }

        public static List<Flag> asFlagSet(long flags)
        {
            // EnumSet<Flag> flagSet = EnumSet.noneOf(typeof(Flag));
            List<Flag> flagSet = new List<Flag>();
            foreach (Flag flag in Flag.values())
            {
                if ((flags & flag.value) != 0)
                {
                    // flagSet.add(flag);
                    flagSet.Add(flag);
                    flags &= ~flag.value;
                }
            }
            Assert.check(flags == 0);
            return flagSet;
        }

        /* Standard Java flags.
         */
        public const int PUBLIC = 1;
        public static readonly int PRIVATE = 1 << 1;
        public static readonly int PROTECTED = 1 << 2;
        public static readonly int STATIC = 1 << 3;
        public static readonly int FINAL = 1 << 4;
        public static readonly int SYNCHRONIZED = 1 << 5;
        public static readonly int VOLATILE = 1 << 6;
        public static readonly int TRANSIENT = 1 << 7;
        public static readonly int NATIVE = 1 << 8;
        public static readonly int INTERFACE = 1 << 9;
        public static readonly int ABSTRACT = 1 << 10;
        public static readonly int STRICTFP = 1 << 11;

        /* Flag that marks a symbol synthetic, added in classfile v49.0. */
        public static readonly int SYNTHETIC = 1 << 12;

        /// <summary>
        /// Flag that marks attribute interfaces, added in classfile v49.0. </summary>
        public static readonly int ANNOTATION = 1 << 13;

        /// <summary>
        /// An enumeration type or an enumeration constant, added in
        ///  classfile v49.0. 
        /// </summary>
        public static readonly int ENUM = 1 << 14;

        /// <summary>
        /// Added in SE8, represents constructs implicitly declared in source. </summary>
        public static readonly int MANDATED = 1 << 15;

        public const int StandardFlags = 0x0fff;

        // Because the following access flags are overloaded with other
        // bit positions, we translate them when reading and writing class
        // files into unique bits positions: ACC_SYNTHETIC <-> SYNTHETIC,
        // for example.
        public const int ACC_SUPER = 0x0020;
        public const int ACC_BRIDGE = 0x0040;
        public const int ACC_VARARGS = 0x0080;
        public const int ACC_MODULE = 0x8000;

        /// <summary>
        ///***************************************
        /// Internal compiler flags (no bits in the lower 16).
        /// ****************************************
        /// </summary>

        /// <summary>
        /// Flag is set if symbol is deprecated.  See also DEPRECATED_REMOVAL.
        /// </summary>
        public static readonly int DEPRECATED = 1 << 17;

        /// <summary>
        /// Flag is set for a variable symbol if the variable's definition
        ///  has an initializer part.
        /// </summary>
        public static readonly int HASINIT = 1 << 18;

        /// <summary>
        /// Flag is set for compiler-generated anonymous method symbols
        ///  that `own' an initializer block.
        /// </summary>
        public static readonly int BLOCK = 1 << 20;

        /// <summary>
        /// Flag bit 21 is available. (used earlier to tag compiler-generated abstract methods that implement
        ///  an interface method (Miranda methods)).
        /// </summary>

        /// <summary>
        /// Flag is set for nested classes that do not access instance members
        ///  or `this' of an outer class and therefore don't need to be passed
        ///  a this$n reference.  This value is currently set only for anonymous
        ///  classes in superclass constructor calls.
        ///  todo: use this value for optimizing away this$n parameters in
        ///  other cases.
        /// </summary>
        public static readonly int NOOUTERTHIS = 1 << 22;

        /// <summary>
        /// Flag is set for package symbols if a package has a member or
        ///  directory and therefore exists.
        /// </summary>
        public static readonly int EXISTS = 1 << 23;

        /// <summary>
        /// Flag is set for compiler-generated compound classes
        ///  representing multiple variable bounds
        /// </summary>
        public static readonly int COMPOUND = 1 << 24;

        /// <summary>
        /// Flag is set for class symbols if a class file was found for this class.
        /// </summary>
        public static readonly int CLASS_SEEN = 1 << 25;

        /// <summary>
        /// Flag is set for class symbols if a source file was found for this
        ///  class.
        /// </summary>
        public static readonly int SOURCE_SEEN = 1 << 26;

        /* State flags (are reset during compilation).
         */

        /// <summary>
        /// Flag for class symbols is set and later re-set as a lock in
        ///  Enter to detect cycles in the superclass/superinterface
        ///  relations.  Similarly for constructor call cycle detection in
        ///  Attr.
        /// </summary>
        public static readonly int LOCKED = 1 << 27;

        /// <summary>
        /// Flag for class symbols is set and later re-set to indicate that a class
        ///  has been entered but has not yet been attributed.
        /// </summary>
        public static readonly int UNATTRIBUTED = 1 << 28;

        /// <summary>
        /// Flag for synthesized default constructors of anonymous classes.
        /// </summary>
        public static readonly int ANONCONSTR = 1 << 29;

        /// <summary>
        /// Flag for class symbols to indicate it has been checked and found
        ///  acyclic.
        /// </summary>
        public static readonly int ACYCLIC = 1 << 30;

        /// <summary>
        /// Flag that marks bridge methods.
        /// </summary>
        public static readonly long BRIDGE = 1L << 31;

        /// <summary>
        /// Flag that marks formal parameters.
        /// </summary>
        public static readonly long PARAMETER = 1L << 33;

        /// <summary>
        /// Flag that marks varargs methods.
        /// </summary>
        public static readonly long VARARGS = 1L << 34;

        /// <summary>
        /// Flag for annotation type symbols to indicate it has been
        ///  checked and found acyclic.
        /// </summary>
        public static readonly long ACYCLIC_ANN = 1L << 35;

        /// <summary>
        /// Flag that marks a generated default constructor.
        /// </summary>
        public static readonly long GENERATEDCONSTR = 1L << 36;

        /// <summary>
        /// Flag that marks a hypothetical method that need not really be
        ///  generated in the binary, but is present in the symbol table to
        ///  simplify checking for erasure clashes - also used for 292 poly sig methods.
        /// </summary>
        public static readonly long HYPOTHETICAL = 1L << 37;

        /// <summary>
        /// Flag that marks an internal proprietary class.
        /// </summary>
        public static readonly long PROPRIETARY = 1L << 38;

        /// <summary>
        /// Flag that marks a multi-catch parameter.
        /// </summary>
        public static readonly long UNION = 1L << 39;

        /// <summary>
        /// Flags an erroneous TypeSymbol as viable for recovery.
        /// TypeSymbols only.
        /// </summary>
        public static readonly long RECOVERABLE = 1L << 40;

        /// <summary>
        /// Flag that marks an 'effectively final' local variable.
        /// </summary>
        public static readonly long EFFECTIVELY_FINAL = 1L << 41;

        /// <summary>
        /// Flag that marks non-override equivalent methods with the same signature,
        /// or a conflicting match binding (BindingSymbol).
        /// </summary>
        public static readonly long CLASH = 1L << 42;

        /// <summary>
        /// Flag that marks either a default method or an interface containing default methods.
        /// </summary>
        public static readonly long DEFAULT = 1L << 43;

        /// <summary>
        /// Flag that marks class as auxiliary, ie a non-public class following
        /// the public class in a source file, that could block implicit compilation.
        /// </summary>
        public static readonly long AUXILIARY = 1L << 44;

        /// <summary>
        /// Flag that marks that a symbol is not available in the current profile
        /// </summary>
        public static readonly long NOT_IN_PROFILE = 1L << 45;

        /// <summary>
        /// Flag that indicates that an override error has been detected by Check.
        /// </summary>
        public static readonly long BAD_OVERRIDE = 1L << 45;

        /// <summary>
        /// Flag that indicates a signature polymorphic method (292).
        /// </summary>
        public static readonly long SIGNATURE_POLYMORPHIC = 1L << 46;

        /// <summary>
        /// Flag that indicates that an inference variable is used in a 'throws' clause.
        /// </summary>
        public static readonly long THROWS = 1L << 47;

        /// <summary>
        /// Flag that marks potentially ambiguous overloads
        /// </summary>
        public static readonly long POTENTIALLY_AMBIGUOUS = 1L << 48;

        /// <summary>
        /// Flag that marks a synthetic method body for a lambda expression
        /// </summary>
        public static readonly long LAMBDA_METHOD = 1L << 49;

        /// <summary>
        /// Flag to control recursion in TransTypes
        /// </summary>
        public static readonly long TYPE_TRANSLATED = 1L << 50;

        /// <summary>
        /// Flag to indicate class symbol is for module-info
        /// </summary>
        public static readonly long MODULE = 1L << 51;

        /// <summary>
        /// Flag to indicate the given ModuleSymbol is an automatic module.
        /// </summary>
        public static readonly long AUTOMATIC_MODULE = 1L << 52; //ModuleSymbols only

        /// <summary>
        /// Flag to indicate the given PackageSymbol contains any non-.java and non-.class resources.
        /// </summary>
        public static readonly long HAS_RESOURCE = 1L << 52; //PackageSymbols only

        /// <summary>
        /// Flag to indicate the given ParamSymbol has a user-friendly name filled.
        /// </summary>
        public static readonly long NAME_FILLED = 1L << 52; //ParamSymbols only

        /// <summary>
        /// Flag to indicate the given ModuleSymbol is a system module.
        /// </summary>
        public static readonly long SYSTEM_MODULE = 1L << 53;

        /// <summary>
        /// Flag to indicate the given symbol has a @Deprecated annotation.
        /// </summary>
        public static readonly long DEPRECATED_ANNOTATION = 1L << 54;

        /// <summary>
        /// Flag to indicate the given symbol has been deprecated and marked for removal.
        /// </summary>
        public static readonly long DEPRECATED_REMOVAL = 1L << 55;

        /// <summary>
        /// Flag to indicate the API element in question is for a preview API.
        /// </summary>
        public static readonly long PREVIEW_API = 1L << 56; //any Symbol kind

        /// <summary>
        /// Flag for synthesized default constructors of anonymous classes that have an enclosing expression.
        /// </summary>
        public static readonly long ANONCONSTR_BASED = 1L << 57;

        /// <summary>
        /// Flag that marks finalize block as body-only, should not be copied into catch clauses.
        /// Used to implement try-with-resources.
        /// </summary>
        public static readonly long BODY_ONLY_FINALIZE = 1L << 17; //blocks only

        /// <summary>
        /// Flag to indicate the API element in question is for a preview API.
        /// </summary>
        public static readonly long PREVIEW_ESSENTIAL_API = 1L << 58; //any Symbol kind

        /// <summary>
        /// Flag to indicate the given variable is a match binding variable.
        /// </summary>
        public static readonly long MATCH_BINDING = 1L << 59;

        /// <summary>
        /// A flag to indicate a match binding variable whose scope extends after the current statement.
        /// </summary>
        public static readonly long MATCH_BINDING_TO_OUTER = 1L << 60;

        /// <summary>
        /// Flag to indicate that a class is a record. The flag is also used to mark fields that are
        /// part of the state vector of a record and to mark the canonical constructor
        /// </summary>
        public static readonly long RECORD = 1L << 61; // ClassSymbols, MethodSymbols and VarSymbols

        /// <summary>
        /// Flag to mark a record constructor as a compact one
        /// </summary>
        public static readonly long COMPACT_RECORD_CONSTRUCTOR = 1L << 51; // MethodSymbols only

        /// <summary>
        /// Flag to mark a record field that was not initialized in the compact constructor
        /// </summary>
        public static readonly long UNINITIALIZED_FIELD = 1L << 51; // VarSymbols only

        /// <summary>
        /// Flag is set for compiler-generated record members, it could be applied to
        ///  accessors and fields
        /// </summary>
        public static readonly int GENERATED_MEMBER = 1 << 24; // MethodSymbols and VarSymbols

        /// <summary>
        /// Flag to indicate sealed class/interface declaration.
        /// </summary>
        public static readonly long SEALED = 1L << 62; // ClassSymbols

        /// <summary>
        /// Flag to indicate that the class/interface was declared with the non-sealed modifier.
        /// </summary>
        public static readonly long NON_SEALED = 1L << 63; // ClassSymbols

        /// <summary>
        /// Modifier masks.
        /// </summary>
        public static readonly int
            AccessFlags = PUBLIC | PROTECTED | PRIVATE,
            LocalClassFlags = FINAL | ABSTRACT | STRICTFP | ENUM | SYNTHETIC,
            StaticLocalFlags = LocalClassFlags | STATIC | INTERFACE,
            MemberClassFlags = LocalClassFlags | INTERFACE | AccessFlags,
            MemberStaticClassFlags = MemberClassFlags | STATIC,
            ClassFlags = LocalClassFlags | INTERFACE | PUBLIC | ANNOTATION,
            InterfaceVarFlags = FINAL | STATIC | PUBLIC, VarFlags = AccessFlags | FINAL | STATIC | VOLATILE | TRANSIENT | ENUM,
            ConstructorFlags = AccessFlags,
            InterfaceMethodFlags = ABSTRACT | PUBLIC,
            MethodFlags = AccessFlags | ABSTRACT | STATIC | NATIVE | SYNCHRONIZED | FINAL | STRICTFP,
            RecordMethodFlags = AccessFlags | ABSTRACT | STATIC | SYNCHRONIZED | FINAL | STRICTFP;

        public static readonly long
            ExtendedStandardFlags = (long)StandardFlags | DEFAULT | SEALED | NON_SEALED,
            ExtendedMemberClassFlags = (long)MemberClassFlags | SEALED | NON_SEALED,
            ExtendedMemberStaticClassFlags = (long)MemberStaticClassFlags | SEALED | NON_SEALED,
            ExtendedClassFlags = (long)ClassFlags | SEALED | NON_SEALED,
            ModifierFlags = ((long)StandardFlags & ~INTERFACE) | DEFAULT | SEALED | NON_SEALED,
            InterfaceMethodMask = (long)(ABSTRACT | PRIVATE | STATIC | PUBLIC | STRICTFP) | DEFAULT,
            AnnotationTypeElementMask = ABSTRACT | PUBLIC,
            LocalVarFlags = (long)FINAL | PARAMETER,
            ReceiverParamFlags = PARAMETER;

        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("preview") public static java.util.Set<javax.lang.model.element.Modifier> asModifierSet(long flags)
        public static ISet<Modifier> asModifierSet(long flags)
        {
            ISet<Modifier> modifiers = modifierSets[flags];
            if (modifiers == null)
            {
                modifiers = new HashSet<Modifier>();
                if (0 != (flags & PUBLIC))
                {
                    modifiers.Add(Modifier.PUBLIC);
                }
                if (0 != (flags & PROTECTED))
                {
                    modifiers.Add(Modifier.PROTECTED);
                }
                if (0 != (flags & PRIVATE))
                {
                    modifiers.Add(Modifier.PRIVATE);
                }
                if (0 != (flags & ABSTRACT))
                {
                    modifiers.Add(Modifier.ABSTRACT);
                }
                if (0 != (flags & STATIC))
                {
                    modifiers.Add(Modifier.STATIC);
                }
                if (0 != (flags & SEALED))
                {
                    modifiers.Add(Modifier.SEALED);
                }
                if (0 != (flags & NON_SEALED))
                {
                    modifiers.Add(Modifier.NON_SEALED);
                }
                if (0 != (flags & FINAL))
                {
                    modifiers.Add(Modifier.FINAL);
                }
                if (0 != (flags & TRANSIENT))
                {
                    modifiers.Add(Modifier.TRANSIENT);
                }
                if (0 != (flags & VOLATILE))
                {
                    modifiers.Add(Modifier.VOLATILE);
                }
                if (0 != (flags & SYNCHRONIZED))
                {
                    modifiers.Add(Modifier.SYNCHRONIZED);
                }
                if (0 != (flags & NATIVE))
                {
                    modifiers.Add(Modifier.NATIVE);
                }
                if (0 != (flags & STRICTFP))
                {
                    modifiers.Add(Modifier.STRICTFP);
                }
                if (0 != (flags & DEFAULT))
                {
                    modifiers.Add(Modifier.DEFAULT);
                }
                // modifiers = Collections.unmodifiableSet(modifiers);
                modifierSets[flags] = modifiers;
            }
            return modifiers;
        }

        // Cache of modifier sets.
        private static readonly IDictionary<long, ISet<Modifier>> modifierSets = new ConcurrentDictionary<long, ISet<Modifier>>();

        //public static bool isStatic(Symbol symbol)
        //{
        //    return (symbol.flags() & STATIC) != 0;
        //}

        //public static bool isEnum(Symbol symbol)
        //{
        //    return (symbol.flags() & ENUM) != 0;
        //}

        //public static bool isConstant(Symbol.VarSymbol symbol)
        //{
        //    return symbol.getConstValue() != null;
        //}




        public sealed class Flag
        {
            public static readonly Flag PUBLIC = new Flag("PUBLIC", InnerEnum.PUBLIC, Flags.PUBLIC);
            public static readonly Flag PRIVATE = new Flag("PRIVATE", InnerEnum.PRIVATE, Flags.PRIVATE);
            public static readonly Flag PROTECTED = new Flag("PROTECTED", InnerEnum.PROTECTED, Flags.PROTECTED);
            public static readonly Flag STATIC = new Flag("STATIC", InnerEnum.STATIC, Flags.STATIC);
            public static readonly Flag FINAL = new Flag("FINAL", InnerEnum.FINAL, Flags.FINAL);
            public static readonly Flag SYNCHRONIZED = new Flag("SYNCHRONIZED", InnerEnum.SYNCHRONIZED, Flags.SYNCHRONIZED);
            public static readonly Flag VOLATILE = new Flag("VOLATILE", InnerEnum.VOLATILE, Flags.VOLATILE);
            public static readonly Flag TRANSIENT = new Flag("TRANSIENT", InnerEnum.TRANSIENT, Flags.TRANSIENT);
            public static readonly Flag NATIVE = new Flag("NATIVE", InnerEnum.NATIVE, Flags.NATIVE);
            public static readonly Flag INTERFACE = new Flag("INTERFACE", InnerEnum.INTERFACE, Flags.INTERFACE);
            public static readonly Flag ABSTRACT = new Flag("ABSTRACT", InnerEnum.ABSTRACT, Flags.ABSTRACT);
            public static readonly Flag DEFAULT = new Flag("DEFAULT", InnerEnum.DEFAULT, Flags.DEFAULT);
            public static readonly Flag STRICTFP = new Flag("STRICTFP", InnerEnum.STRICTFP, Flags.STRICTFP);
            public static readonly Flag BRIDGE = new Flag("BRIDGE", InnerEnum.BRIDGE, Flags.BRIDGE);
            public static readonly Flag SYNTHETIC = new Flag("SYNTHETIC", InnerEnum.SYNTHETIC, Flags.SYNTHETIC);
            public static readonly Flag ANNOTATION = new Flag("ANNOTATION", InnerEnum.ANNOTATION, Flags.ANNOTATION);
            public static readonly Flag DEPRECATED = new Flag("DEPRECATED", InnerEnum.DEPRECATED, Flags.DEPRECATED);
            public static readonly Flag HASINIT = new Flag("HASINIT", InnerEnum.HASINIT, Flags.HASINIT);
            public static readonly Flag BLOCK = new Flag("BLOCK", InnerEnum.BLOCK, Flags.BLOCK);
            public static readonly Flag ENUM = new Flag("ENUM", InnerEnum.ENUM, Flags.ENUM);
            public static readonly Flag MANDATED = new Flag("MANDATED", InnerEnum.MANDATED, Flags.MANDATED);
            public static readonly Flag NOOUTERTHIS = new Flag("NOOUTERTHIS", InnerEnum.NOOUTERTHIS, Flags.NOOUTERTHIS);
            public static readonly Flag EXISTS = new Flag("EXISTS", InnerEnum.EXISTS, Flags.EXISTS);
            public static readonly Flag COMPOUND = new Flag("COMPOUND", InnerEnum.COMPOUND, Flags.COMPOUND);
            public static readonly Flag CLASS_SEEN = new Flag("CLASS_SEEN", InnerEnum.CLASS_SEEN, Flags.CLASS_SEEN);
            public static readonly Flag SOURCE_SEEN = new Flag("SOURCE_SEEN", InnerEnum.SOURCE_SEEN, Flags.SOURCE_SEEN);
            public static readonly Flag LOCKED = new Flag("LOCKED", InnerEnum.LOCKED, Flags.LOCKED);
            public static readonly Flag UNATTRIBUTED = new Flag("UNATTRIBUTED", InnerEnum.UNATTRIBUTED, Flags.UNATTRIBUTED);
            public static readonly Flag ANONCONSTR = new Flag("ANONCONSTR", InnerEnum.ANONCONSTR, Flags.ANONCONSTR);
            public static readonly Flag ACYCLIC = new Flag("ACYCLIC", InnerEnum.ACYCLIC, Flags.ACYCLIC);
            public static readonly Flag PARAMETER = new Flag("PARAMETER", InnerEnum.PARAMETER, Flags.PARAMETER);
            public static readonly Flag VARARGS = new Flag("VARARGS", InnerEnum.VARARGS, Flags.VARARGS);
            public static readonly Flag ACYCLIC_ANN = new Flag("ACYCLIC_ANN", InnerEnum.ACYCLIC_ANN, Flags.ACYCLIC_ANN);
            public static readonly Flag GENERATEDCONSTR = new Flag("GENERATEDCONSTR", InnerEnum.GENERATEDCONSTR, Flags.GENERATEDCONSTR);
            public static readonly Flag HYPOTHETICAL = new Flag("HYPOTHETICAL", InnerEnum.HYPOTHETICAL, Flags.HYPOTHETICAL);
            public static readonly Flag PROPRIETARY = new Flag("PROPRIETARY", InnerEnum.PROPRIETARY, Flags.PROPRIETARY);
            public static readonly Flag UNION = new Flag("UNION", InnerEnum.UNION, Flags.UNION);
            public static readonly Flag EFFECTIVELY_FINAL = new Flag("EFFECTIVELY_FINAL", InnerEnum.EFFECTIVELY_FINAL, Flags.EFFECTIVELY_FINAL);
            public static readonly Flag CLASH = new Flag("CLASH", InnerEnum.CLASH, Flags.CLASH);
            public static readonly Flag AUXILIARY = new Flag("AUXILIARY", InnerEnum.AUXILIARY, Flags.AUXILIARY);
            public static readonly Flag NOT_IN_PROFILE = new Flag("NOT_IN_PROFILE", InnerEnum.NOT_IN_PROFILE, Flags.NOT_IN_PROFILE);
            public static readonly Flag BAD_OVERRIDE = new Flag("BAD_OVERRIDE", InnerEnum.BAD_OVERRIDE, Flags.BAD_OVERRIDE);
            public static readonly Flag SIGNATURE_POLYMORPHIC = new Flag("SIGNATURE_POLYMORPHIC", InnerEnum.SIGNATURE_POLYMORPHIC, Flags.SIGNATURE_POLYMORPHIC);
            public static readonly Flag THROWS = new Flag("THROWS", InnerEnum.THROWS, Flags.THROWS);
            public static readonly Flag LAMBDA_METHOD = new Flag("LAMBDA_METHOD", InnerEnum.LAMBDA_METHOD, Flags.LAMBDA_METHOD);
            public static readonly Flag TYPE_TRANSLATED = new Flag("TYPE_TRANSLATED", InnerEnum.TYPE_TRANSLATED, Flags.TYPE_TRANSLATED);
            public static readonly Flag MODULE = new Flag("MODULE", InnerEnum.MODULE, Flags.MODULE);
            public static readonly Flag AUTOMATIC_MODULE = new Flag("AUTOMATIC_MODULE", InnerEnum.AUTOMATIC_MODULE, Flags.AUTOMATIC_MODULE);
            public static readonly Flag SYSTEM_MODULE = new Flag("SYSTEM_MODULE", InnerEnum.SYSTEM_MODULE, Flags.SYSTEM_MODULE);
            public static readonly Flag DEPRECATED_ANNOTATION = new Flag("DEPRECATED_ANNOTATION", InnerEnum.DEPRECATED_ANNOTATION, Flags.DEPRECATED_ANNOTATION);
            public static readonly Flag DEPRECATED_REMOVAL = new Flag("DEPRECATED_REMOVAL", InnerEnum.DEPRECATED_REMOVAL, Flags.DEPRECATED_REMOVAL);
            public static readonly Flag HAS_RESOURCE = new Flag("HAS_RESOURCE", InnerEnum.HAS_RESOURCE, Flags.HAS_RESOURCE);
            public static readonly Flag POTENTIALLY_AMBIGUOUS = new Flag("POTENTIALLY_AMBIGUOUS", InnerEnum.POTENTIALLY_AMBIGUOUS, Flags.POTENTIALLY_AMBIGUOUS);
            public static readonly Flag ANONCONSTR_BASED = new Flag("ANONCONSTR_BASED", InnerEnum.ANONCONSTR_BASED, Flags.ANONCONSTR_BASED);
            public static readonly Flag NAME_FILLED = new Flag("NAME_FILLED", InnerEnum.NAME_FILLED, Flags.NAME_FILLED);
            public static readonly Flag PREVIEW_API = new Flag("PREVIEW_API", InnerEnum.PREVIEW_API, Flags.PREVIEW_API);
            public static readonly Flag PREVIEW_ESSENTIAL_API = new Flag("PREVIEW_ESSENTIAL_API", InnerEnum.PREVIEW_ESSENTIAL_API, Flags.PREVIEW_ESSENTIAL_API);
            public static readonly Flag MATCH_BINDING = new Flag("MATCH_BINDING", InnerEnum.MATCH_BINDING, Flags.MATCH_BINDING);
            public static readonly Flag MATCH_BINDING_TO_OUTER = new Flag("MATCH_BINDING_TO_OUTER", InnerEnum.MATCH_BINDING_TO_OUTER, Flags.MATCH_BINDING_TO_OUTER);
            public static readonly Flag RECORD = new Flag("RECORD", InnerEnum.RECORD, Flags.RECORD);
            public static readonly Flag RECOVERABLE = new Flag("RECOVERABLE", InnerEnum.RECOVERABLE, Flags.RECOVERABLE);
            public static readonly Flag SEALED = new Flag("SEALED", InnerEnum.SEALED, Flags.SEALED);
            public static readonly Flag NON_SEALED = new Flag("NON_SEALED", InnerEnum.NON_SEALED, Flags.NON_SEALED);

            private static readonly IList<Flag> valueList = new List<Flag>();

            static Flag()
            {
                valueList.Add(PUBLIC);
                valueList.Add(PRIVATE);
                valueList.Add(PROTECTED);
                valueList.Add(STATIC);
                valueList.Add(FINAL);
                valueList.Add(SYNCHRONIZED);
                valueList.Add(VOLATILE);
                valueList.Add(TRANSIENT);
                valueList.Add(NATIVE);
                valueList.Add(INTERFACE);
                valueList.Add(ABSTRACT);
                valueList.Add(DEFAULT);
                valueList.Add(STRICTFP);
                valueList.Add(BRIDGE);
                valueList.Add(SYNTHETIC);
                valueList.Add(ANNOTATION);
                valueList.Add(DEPRECATED);
                valueList.Add(HASINIT);
                valueList.Add(BLOCK);
                valueList.Add(ENUM);
                valueList.Add(MANDATED);
                valueList.Add(NOOUTERTHIS);
                valueList.Add(EXISTS);
                valueList.Add(COMPOUND);
                valueList.Add(CLASS_SEEN);
                valueList.Add(SOURCE_SEEN);
                valueList.Add(LOCKED);
                valueList.Add(UNATTRIBUTED);
                valueList.Add(ANONCONSTR);
                valueList.Add(ACYCLIC);
                valueList.Add(PARAMETER);
                valueList.Add(VARARGS);
                valueList.Add(ACYCLIC_ANN);
                valueList.Add(GENERATEDCONSTR);
                valueList.Add(HYPOTHETICAL);
                valueList.Add(PROPRIETARY);
                valueList.Add(UNION);
                valueList.Add(EFFECTIVELY_FINAL);
                valueList.Add(CLASH);
                valueList.Add(AUXILIARY);
                valueList.Add(NOT_IN_PROFILE);
                valueList.Add(BAD_OVERRIDE);
                valueList.Add(SIGNATURE_POLYMORPHIC);
                valueList.Add(THROWS);
                valueList.Add(LAMBDA_METHOD);
                valueList.Add(TYPE_TRANSLATED);
                valueList.Add(MODULE);
                valueList.Add(AUTOMATIC_MODULE);
                valueList.Add(SYSTEM_MODULE);
                valueList.Add(DEPRECATED_ANNOTATION);
                valueList.Add(DEPRECATED_REMOVAL);
                valueList.Add(HAS_RESOURCE);
                valueList.Add(POTENTIALLY_AMBIGUOUS);
                valueList.Add(ANONCONSTR_BASED);
                valueList.Add(NAME_FILLED);
                valueList.Add(PREVIEW_API);
                valueList.Add(PREVIEW_ESSENTIAL_API);
                valueList.Add(MATCH_BINDING);
                valueList.Add(MATCH_BINDING_TO_OUTER);
                valueList.Add(RECORD);
                valueList.Add(RECOVERABLE);
                valueList.Add(SEALED);
                valueList.Add(NON_SEALED);
            }

            public enum InnerEnum
            {
                PUBLIC,
                PRIVATE,
                PROTECTED,
                STATIC,
                FINAL,
                SYNCHRONIZED,
                VOLATILE,
                TRANSIENT,
                NATIVE,
                INTERFACE,
                ABSTRACT,
                DEFAULT,
                STRICTFP,
                BRIDGE,
                SYNTHETIC,
                ANNOTATION,
                DEPRECATED,
                HASINIT,
                BLOCK,
                ENUM,
                MANDATED,
                NOOUTERTHIS,
                EXISTS,
                COMPOUND,
                CLASS_SEEN,
                SOURCE_SEEN,
                LOCKED,
                UNATTRIBUTED,
                ANONCONSTR,
                ACYCLIC,
                PARAMETER,
                VARARGS,
                ACYCLIC_ANN,
                GENERATEDCONSTR,
                HYPOTHETICAL,
                PROPRIETARY,
                UNION,
                EFFECTIVELY_FINAL,
                CLASH,
                AUXILIARY,
                NOT_IN_PROFILE,
                BAD_OVERRIDE,
                SIGNATURE_POLYMORPHIC,
                THROWS,
                LAMBDA_METHOD,
                TYPE_TRANSLATED,
                MODULE,
                AUTOMATIC_MODULE,
                SYSTEM_MODULE,
                DEPRECATED_ANNOTATION,
                DEPRECATED_REMOVAL,
                HAS_RESOURCE,
                POTENTIALLY_AMBIGUOUS,
                ANONCONSTR_BASED,
                NAME_FILLED,
                PREVIEW_API,
                PREVIEW_ESSENTIAL_API,
                MATCH_BINDING,
                MATCH_BINDING_TO_OUTER,
                RECORD,
                RECOVERABLE,
                SEALED,
                NON_SEALED
            }

            public readonly InnerEnum innerEnumValue;
            private readonly string nameValue;
            private readonly int ordinalValue;
            private static int nextOrdinal = 0;

            internal Flag(string name, InnerEnum innerEnum, long flag)
            {
                this.value = flag;
                this.lowercaseName = name.ToLower();

                nameValue = name;
                ordinalValue = nextOrdinal++;
                innerEnumValue = innerEnum;
            }

            public override string ToString()
            {
                return lowercaseName;
            }

            internal readonly long value;
            internal readonly string lowercaseName;

            public static IList<Flag> values()
            {
                return valueList;
            }

            public int ordinal()
            {
                return ordinalValue;
            }

            public static Flag valueOf(string name)
            {
                foreach (Flag enumInstance in Flag.valueList)
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

}