using System;
using System.Collections.Generic;

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

namespace com.sun.tools.javac.tree
{
    using java.lang.common.api;
    using java.lang.common.extensions;

    using com.sun.source.tree;
    using com.sun.tools.javac.util;
    using static com.sun.tools.javac.tree.JCTree;
    using com.sun.tools.javac.code;
    //using RequiresDirective = com.sun.tools.javac.code.Directive.RequiresDirective;
    //using com.sun.tools.javac.code.Scope;
    //using com.sun.tools.javac.code.Symbol;
    using System.IO;
    //using Api = com.sun.tools.javac.util.DefinedBy.Api;
    //using DiagnosticPosition = com.sun.tools.javac.util.JCDiagnostic.DiagnosticPosition;

    using static com.sun.tools.javac.tree.JCTree.Tag;
    using Name = com.sun.tools.javac.util.Name;

    // using ModuleKind = com.sun.source.tree.ModuleTree.ModuleKind;
    //using ExportsDirective = com.sun.tools.javac.code.Directive.ExportsDirective;
    //using OpensDirective = com.sun.tools.javac.code.Directive.OpensDirective;
    //using ModuleType = com.sun.tools.javac.code.Type.ModuleType;
    // using static com.sun.tools.javac.tree.JCTree;

    /// <summary>
    /// Root class for abstract syntax tree nodes. It provides definitions
    /// for specific tree nodes as subclasses nested inside.
    /// 
    /// <para>Each subclass is highly standardized.  It generally contains
    /// only tree fields for the syntactic subcomponents of the node.  Some
    /// classes that represent identifier uses or definitions also define a
    /// Symbol field that denotes the represented identifier.  Classes for
    /// non-local jumps also carry the jump target as a field.  The root
    /// class Tree itself defines fields for the tree's type and position.
    /// No other fields are kept in a tree node; instead parameters are
    /// passed to methods accessing the node.
    /// 
    /// </para>
    /// <para>Except for the methods defined by com.sun.source, the only
    /// method defined in subclasses is `visit' which applies a given
    /// visitor to the tree. The actual tree processing is done by visitor
    /// classes in other packages. The abstract class Visitor, as well as
    /// an Factory interface for trees, are defined as inner classes in
    /// Tree.
    /// 
    /// </para>
    /// <para>To avoid ambiguities with the Tree API in com.sun.source all sub
    /// classes should, by convention, start with JC (javac).
    /// 
    /// </para>
    /// <para><b>This is NOT part of any supported API.
    /// If you write code that depends on this, you do so at your own risk.
    /// This code and its internal interfaces are subject to change or
    /// deletion without notice.</b>
    /// 
    /// </para>
    /// </summary>
    /// <seealso cref= TreeMaker </seealso>
    /// <seealso cref= TreeInfo </seealso>
    /// <seealso cref= TreeTranslator </seealso>
    /// <seealso cref= Pretty </seealso>
    public abstract class JCTree : Tree //, ICloneable, DiagnosticPosition
    {
        /* Tree tag values, identifying kinds of trees */
        public sealed class Tag
        {
            /// <summary>
            /// For methods that return an invalid tag if a given condition is not met
            /// </summary>
            public static readonly Tag NO_TAG = new Tag("NO_TAG", InnerEnum.NO_TAG);

            /// <summary>
            /// Toplevel nodes, of type TopLevel, representing entire source files.
            /// </summary>
            public static readonly Tag TOPLEVEL = new Tag("TOPLEVEL", InnerEnum.TOPLEVEL);

            /// <summary>
            /// Package level definitions.
            /// </summary>
            public static readonly Tag PACKAGEDEF = new Tag("PACKAGEDEF", InnerEnum.PACKAGEDEF);

            /// <summary>
            /// Import clauses, of type Import.
            /// </summary>
            public static readonly Tag IMPORT = new Tag("IMPORT", InnerEnum.IMPORT);

            /// <summary>
            /// Class definitions, of type ClassDef.
            /// </summary>
            public static readonly Tag CLASSDEF = new Tag("CLASSDEF", InnerEnum.CLASSDEF);

            /// <summary>
            /// Method definitions, of type MethodDef.
            /// </summary>
            public static readonly Tag METHODDEF = new Tag("METHODDEF", InnerEnum.METHODDEF);

            /// <summary>
            /// Variable definitions, of type VarDef.
            /// </summary>
            public static readonly Tag VARDEF = new Tag("VARDEF", InnerEnum.VARDEF);

            /// <summary>
            /// The no-op statement ";", of type Skip
            /// </summary>
            public static readonly Tag SKIP = new Tag("SKIP", InnerEnum.SKIP);

            /// <summary>
            /// Blocks, of type Block.
            /// </summary>
            public static readonly Tag BLOCK = new Tag("BLOCK", InnerEnum.BLOCK);

            /// <summary>
            /// Do-while loops, of type DoLoop.
            /// </summary>
            public static readonly Tag DOLOOP = new Tag("DOLOOP", InnerEnum.DOLOOP);

            /// <summary>
            /// While-loops, of type WhileLoop.
            /// </summary>
            public static readonly Tag WHILELOOP = new Tag("WHILELOOP", InnerEnum.WHILELOOP);

            /// <summary>
            /// For-loops, of type ForLoop.
            /// </summary>
            public static readonly Tag FORLOOP = new Tag("FORLOOP", InnerEnum.FORLOOP);

            /// <summary>
            /// Foreach-loops, of type ForeachLoop.
            /// </summary>
            public static readonly Tag FOREACHLOOP = new Tag("FOREACHLOOP", InnerEnum.FOREACHLOOP);

            /// <summary>
            /// Labelled statements, of type Labelled.
            /// </summary>
            public static readonly Tag LABELLED = new Tag("LABELLED", InnerEnum.LABELLED);

            /// <summary>
            /// Switch statements, of type Switch.
            /// </summary>
            public static readonly Tag SWITCH = new Tag("SWITCH", InnerEnum.SWITCH);

            /// <summary>
            /// Case parts in switch statements/expressions, of type Case.
            /// </summary>
            public static readonly Tag CASE = new Tag("CASE", InnerEnum.CASE);

            /// <summary>
            /// Switch expression statements, of type Switch.
            /// </summary>
            public static readonly Tag SWITCH_EXPRESSION = new Tag("SWITCH_EXPRESSION", InnerEnum.SWITCH_EXPRESSION);

            /// <summary>
            /// Synchronized statements, of type Synchronized.
            /// </summary>
            public static readonly Tag SYNCHRONIZED = new Tag("SYNCHRONIZED", InnerEnum.SYNCHRONIZED);

            /// <summary>
            /// Try statements, of type Try.
            /// </summary>
            public static readonly Tag TRY = new Tag("TRY", InnerEnum.TRY);

            /// <summary>
            /// Catch clauses in try statements, of type Catch.
            /// </summary>
            public static readonly Tag CATCH = new Tag("CATCH", InnerEnum.CATCH);

            /// <summary>
            /// Conditional expressions, of type Conditional.
            /// </summary>
            public static readonly Tag CONDEXPR = new Tag("CONDEXPR", InnerEnum.CONDEXPR);

            /// <summary>
            /// Conditional statements, of type If.
            /// </summary>
            public static readonly Tag IF = new Tag("IF", InnerEnum.IF);

            /// <summary>
            /// Expression statements, of type Exec.
            /// </summary>
            public static readonly Tag EXEC = new Tag("EXEC", InnerEnum.EXEC);

            /// <summary>
            /// Break statements, of type Break.
            /// </summary>
            public static readonly Tag BREAK = new Tag("BREAK", InnerEnum.BREAK);

            /// <summary>
            /// Yield statements, of type Yield.
            /// </summary>
            public static readonly Tag YIELD = new Tag("YIELD", InnerEnum.YIELD);

            /// <summary>
            /// Continue statements, of type Continue.
            /// </summary>
            public static readonly Tag CONTINUE = new Tag("CONTINUE", InnerEnum.CONTINUE);

            /// <summary>
            /// Return statements, of type Return.
            /// </summary>
            public static readonly Tag RETURN = new Tag("RETURN", InnerEnum.RETURN);

            /// <summary>
            /// Throw statements, of type Throw.
            /// </summary>
            public static readonly Tag THROW = new Tag("THROW", InnerEnum.THROW);

            /// <summary>
            /// Assert statements, of type Assert.
            /// </summary>
            public static readonly Tag ASSERT = new Tag("ASSERT", InnerEnum.ASSERT);

            /// <summary>
            /// Method invocation expressions, of type Apply.
            /// </summary>
            public static readonly Tag APPLY = new Tag("APPLY", InnerEnum.APPLY);

            /// <summary>
            /// Class instance creation expressions, of type NewClass.
            /// </summary>
            public static readonly Tag NEWCLASS = new Tag("NEWCLASS", InnerEnum.NEWCLASS);

            /// <summary>
            /// Array creation expressions, of type NewArray.
            /// </summary>
            public static readonly Tag NEWARRAY = new Tag("NEWARRAY", InnerEnum.NEWARRAY);

            /// <summary>
            /// Lambda expression, of type Lambda.
            /// </summary>
            public static readonly Tag LAMBDA = new Tag("LAMBDA", InnerEnum.LAMBDA);

            /// <summary>
            /// Parenthesized subexpressions, of type Parens.
            /// </summary>
            public static readonly Tag PARENS = new Tag("PARENS", InnerEnum.PARENS);

            /// <summary>
            /// Assignment expressions, of type Assign.
            /// </summary>
            public static readonly Tag ASSIGN = new Tag("ASSIGN", InnerEnum.ASSIGN);

            /// <summary>
            /// Type cast expressions, of type TypeCast.
            /// </summary>
            public static readonly Tag TYPECAST = new Tag("TYPECAST", InnerEnum.TYPECAST);

            /// <summary>
            /// Type test expressions, of type TypeTest.
            /// </summary>
            public static readonly Tag TYPETEST = new Tag("TYPETEST", InnerEnum.TYPETEST);

            /// <summary>
            /// Patterns.
            /// </summary>
            public static readonly Tag BINDINGPATTERN = new Tag("BINDINGPATTERN", InnerEnum.BINDINGPATTERN);

            /// <summary>
            /// Indexed array expressions, of type Indexed.
            /// </summary>
            public static readonly Tag INDEXED = new Tag("INDEXED", InnerEnum.INDEXED);

            /// <summary>
            /// Selections, of type Select.
            /// </summary>
            public static readonly Tag SELECT = new Tag("SELECT", InnerEnum.SELECT);

            /// <summary>
            /// Member references, of type Reference.
            /// </summary>
            public static readonly Tag REFERENCE = new Tag("REFERENCE", InnerEnum.REFERENCE);

            /// <summary>
            /// Simple identifiers, of type Ident.
            /// </summary>
            public static readonly Tag IDENT = new Tag("IDENT", InnerEnum.IDENT);

            /// <summary>
            /// Literals, of type Literal.
            /// </summary>
            public static readonly Tag LITERAL = new Tag("LITERAL", InnerEnum.LITERAL);

            /// <summary>
            /// Basic type identifiers, of type TypeIdent.
            /// </summary>
            public static readonly Tag TYPEIDENT = new Tag("TYPEIDENT", InnerEnum.TYPEIDENT);

            /// <summary>
            /// Array types, of type TypeArray.
            /// </summary>
            public static readonly Tag TYPEARRAY = new Tag("TYPEARRAY", InnerEnum.TYPEARRAY);

            /// <summary>
            /// Parameterized types, of type TypeApply.
            /// </summary>
            public static readonly Tag TYPEAPPLY = new Tag("TYPEAPPLY", InnerEnum.TYPEAPPLY);

            /// <summary>
            /// Union types, of type TypeUnion.
            /// </summary>
            public static readonly Tag TYPEUNION = new Tag("TYPEUNION", InnerEnum.TYPEUNION);

            /// <summary>
            /// Intersection types, of type TypeIntersection.
            /// </summary>
            public static readonly Tag TYPEINTERSECTION = new Tag("TYPEINTERSECTION", InnerEnum.TYPEINTERSECTION);

            /// <summary>
            /// Formal type parameters, of type TypeParameter.
            /// </summary>
            public static readonly Tag TYPEPARAMETER = new Tag("TYPEPARAMETER", InnerEnum.TYPEPARAMETER);

            /// <summary>
            /// Type argument.
            /// </summary>
            public static readonly Tag WILDCARD = new Tag("WILDCARD", InnerEnum.WILDCARD);

            /// <summary>
            /// Bound kind: extends, super, exact, or unbound
            /// </summary>
            public static readonly Tag TYPEBOUNDKIND = new Tag("TYPEBOUNDKIND", InnerEnum.TYPEBOUNDKIND);

            /// <summary>
            /// metadata: Annotation.
            /// </summary>
            public static readonly Tag ANNOTATION = new Tag("ANNOTATION", InnerEnum.ANNOTATION);

            /// <summary>
            /// metadata: Type annotation.
            /// </summary>
            public static readonly Tag TYPE_ANNOTATION = new Tag("TYPE_ANNOTATION", InnerEnum.TYPE_ANNOTATION);

            /// <summary>
            /// metadata: Modifiers
            /// </summary>
            public static readonly Tag MODIFIERS = new Tag("MODIFIERS", InnerEnum.MODIFIERS);

            /// <summary>
            /// An annotated type tree.
            /// </summary>
            public static readonly Tag ANNOTATED_TYPE = new Tag("ANNOTATED_TYPE", InnerEnum.ANNOTATED_TYPE);

            /// <summary>
            /// Error trees, of type Erroneous.
            /// </summary>
            public static readonly Tag ERRONEOUS = new Tag("ERRONEOUS", InnerEnum.ERRONEOUS);

            /// <summary>
            /// Unary operators, of type Unary.
            /// </summary>
            public static readonly Tag POS = new Tag("POS", InnerEnum.POS); // +
            public static readonly Tag NEG = new Tag("NEG", InnerEnum.NEG); // -
            public static readonly Tag NOT = new Tag("NOT", InnerEnum.NOT); // !
            public static readonly Tag COMPL = new Tag("COMPL", InnerEnum.COMPL); // ~
            public static readonly Tag PREINC = new Tag("PREINC", InnerEnum.PREINC); // ++ _
            public static readonly Tag PREDEC = new Tag("PREDEC", InnerEnum.PREDEC); // -- _
            public static readonly Tag POSTINC = new Tag("POSTINC", InnerEnum.POSTINC); // _ ++
            public static readonly Tag POSTDEC = new Tag("POSTDEC", InnerEnum.POSTDEC); // _ --

            /// <summary>
            /// unary operator for null reference checks, only used internally.
            /// </summary>
            public static readonly Tag NULLCHK = new Tag("NULLCHK", InnerEnum.NULLCHK);

            /// <summary>
            /// Binary operators, of type Binary.
            /// </summary>
            public static readonly Tag OR = new Tag("OR", InnerEnum.OR); // ||
            public static readonly Tag AND = new Tag("AND", InnerEnum.AND); // &&
            public static readonly Tag BITOR = new Tag("BITOR", InnerEnum.BITOR); // |
            public static readonly Tag BITXOR = new Tag("BITXOR", InnerEnum.BITXOR); // ^
            public static readonly Tag BITAND = new Tag("BITAND", InnerEnum.BITAND); // &
            public static readonly Tag EQ = new Tag("EQ", InnerEnum.EQ); // ==
            public static readonly Tag NE = new Tag("NE", InnerEnum.NE); // !=
            public static readonly Tag LT = new Tag("LT", InnerEnum.LT); // <
            public static readonly Tag GT = new Tag("GT", InnerEnum.GT); // >
            public static readonly Tag LE = new Tag("LE", InnerEnum.LE); // <=
            public static readonly Tag GE = new Tag("GE", InnerEnum.GE); // >=
            public static readonly Tag SL = new Tag("SL", InnerEnum.SL); // <<
            public static readonly Tag SR = new Tag("SR", InnerEnum.SR); // >>
            public static readonly Tag USR = new Tag("USR", InnerEnum.USR); // >>>
            public static readonly Tag PLUS = new Tag("PLUS", InnerEnum.PLUS); // +
            public static readonly Tag MINUS = new Tag("MINUS", InnerEnum.MINUS); // -
            public static readonly Tag MUL = new Tag("MUL", InnerEnum.MUL); // *
            public static readonly Tag DIV = new Tag("DIV", InnerEnum.DIV); // /
            public static readonly Tag MOD = new Tag("MOD", InnerEnum.MOD); // %

            /// <summary>
            /// Assignment operators, of type Assignop.
            /// </summary>
            public static readonly Tag BITOR_ASG = new Tag("BITOR_ASG", InnerEnum.BITOR_ASG, BITOR); // |=
            public static readonly Tag BITXOR_ASG = new Tag("BITXOR_ASG", InnerEnum.BITXOR_ASG, BITXOR); // ^=
            public static readonly Tag BITAND_ASG = new Tag("BITAND_ASG", InnerEnum.BITAND_ASG, BITAND); // &=

            public static readonly Tag SL_ASG = new Tag("SL_ASG", InnerEnum.SL_ASG, SL); // <<=
            public static readonly Tag SR_ASG = new Tag("SR_ASG", InnerEnum.SR_ASG, SR); // >>=
            public static readonly Tag USR_ASG = new Tag("USR_ASG", InnerEnum.USR_ASG, USR); // >>>=
            public static readonly Tag PLUS_ASG = new Tag("PLUS_ASG", InnerEnum.PLUS_ASG, PLUS); // +=
            public static readonly Tag MINUS_ASG = new Tag("MINUS_ASG", InnerEnum.MINUS_ASG, MINUS); // -=
            public static readonly Tag MUL_ASG = new Tag("MUL_ASG", InnerEnum.MUL_ASG, MUL); // *=
            public static readonly Tag DIV_ASG = new Tag("DIV_ASG", InnerEnum.DIV_ASG, DIV); // /=
            public static readonly Tag MOD_ASG = new Tag("MOD_ASG", InnerEnum.MOD_ASG, MOD); // %=

            public static readonly Tag MODULEDEF = new Tag("MODULEDEF", InnerEnum.MODULEDEF);
            public static readonly Tag EXPORTS = new Tag("EXPORTS", InnerEnum.EXPORTS);
            public static readonly Tag OPENS = new Tag("OPENS", InnerEnum.OPENS);
            public static readonly Tag PROVIDES = new Tag("PROVIDES", InnerEnum.PROVIDES);
            public static readonly Tag REQUIRES = new Tag("REQUIRES", InnerEnum.REQUIRES);
            public static readonly Tag USES = new Tag("USES", InnerEnum.USES);

            /// <summary>
            /// A synthetic let expression, of type LetExpr.
            /// </summary>
            public static readonly Tag LETEXPR = new Tag("LETEXPR", InnerEnum.LETEXPR); // ala scheme

            private static readonly IList<Tag> valueList = new List<Tag>();

            static Tag()
            {
                valueList.Add(NO_TAG);
                valueList.Add(TOPLEVEL);
                valueList.Add(PACKAGEDEF);
                valueList.Add(IMPORT);
                valueList.Add(CLASSDEF);
                valueList.Add(METHODDEF);
                valueList.Add(VARDEF);
                valueList.Add(SKIP);
                valueList.Add(BLOCK);
                valueList.Add(DOLOOP);
                valueList.Add(WHILELOOP);
                valueList.Add(FORLOOP);
                valueList.Add(FOREACHLOOP);
                valueList.Add(LABELLED);
                valueList.Add(SWITCH);
                valueList.Add(CASE);
                valueList.Add(SWITCH_EXPRESSION);
                valueList.Add(SYNCHRONIZED);
                valueList.Add(TRY);
                valueList.Add(CATCH);
                valueList.Add(CONDEXPR);
                valueList.Add(IF);
                valueList.Add(EXEC);
                valueList.Add(BREAK);
                valueList.Add(YIELD);
                valueList.Add(CONTINUE);
                valueList.Add(RETURN);
                valueList.Add(THROW);
                valueList.Add(ASSERT);
                valueList.Add(APPLY);
                valueList.Add(NEWCLASS);
                valueList.Add(NEWARRAY);
                valueList.Add(LAMBDA);
                valueList.Add(PARENS);
                valueList.Add(ASSIGN);
                valueList.Add(TYPECAST);
                valueList.Add(TYPETEST);
                valueList.Add(BINDINGPATTERN);
                valueList.Add(INDEXED);
                valueList.Add(SELECT);
                valueList.Add(REFERENCE);
                valueList.Add(IDENT);
                valueList.Add(LITERAL);
                valueList.Add(TYPEIDENT);
                valueList.Add(TYPEARRAY);
                valueList.Add(TYPEAPPLY);
                valueList.Add(TYPEUNION);
                valueList.Add(TYPEINTERSECTION);
                valueList.Add(TYPEPARAMETER);
                valueList.Add(WILDCARD);
                valueList.Add(TYPEBOUNDKIND);
                valueList.Add(ANNOTATION);
                valueList.Add(TYPE_ANNOTATION);
                valueList.Add(MODIFIERS);
                valueList.Add(ANNOTATED_TYPE);
                valueList.Add(ERRONEOUS);
                valueList.Add(POS);
                valueList.Add(NEG);
                valueList.Add(NOT);
                valueList.Add(COMPL);
                valueList.Add(PREINC);
                valueList.Add(PREDEC);
                valueList.Add(POSTINC);
                valueList.Add(POSTDEC);
                valueList.Add(NULLCHK);
                valueList.Add(OR);
                valueList.Add(AND);
                valueList.Add(BITOR);
                valueList.Add(BITXOR);
                valueList.Add(BITAND);
                valueList.Add(EQ);
                valueList.Add(NE);
                valueList.Add(LT);
                valueList.Add(GT);
                valueList.Add(LE);
                valueList.Add(GE);
                valueList.Add(SL);
                valueList.Add(SR);
                valueList.Add(USR);
                valueList.Add(PLUS);
                valueList.Add(MINUS);
                valueList.Add(MUL);
                valueList.Add(DIV);
                valueList.Add(MOD);
                valueList.Add(BITOR_ASG);
                valueList.Add(BITXOR_ASG);
                valueList.Add(BITAND_ASG);
                valueList.Add(SL_ASG);
                valueList.Add(SR_ASG);
                valueList.Add(USR_ASG);
                valueList.Add(PLUS_ASG);
                valueList.Add(MINUS_ASG);
                valueList.Add(MUL_ASG);
                valueList.Add(DIV_ASG);
                valueList.Add(MOD_ASG);
                valueList.Add(MODULEDEF);
                valueList.Add(EXPORTS);
                valueList.Add(OPENS);
                valueList.Add(PROVIDES);
                valueList.Add(REQUIRES);
                valueList.Add(USES);
                valueList.Add(LETEXPR);
            }

            public enum InnerEnum
            {
                NO_TAG,
                TOPLEVEL,
                PACKAGEDEF,
                IMPORT,
                CLASSDEF,
                METHODDEF,
                VARDEF,
                SKIP,
                BLOCK,
                DOLOOP,
                WHILELOOP,
                FORLOOP,
                FOREACHLOOP,
                LABELLED,
                SWITCH,
                CASE,
                SWITCH_EXPRESSION,
                SYNCHRONIZED,
                TRY,
                CATCH,
                CONDEXPR,
                IF,
                EXEC,
                BREAK,
                YIELD,
                CONTINUE,
                RETURN,
                THROW,
                ASSERT,
                APPLY,
                NEWCLASS,
                NEWARRAY,
                LAMBDA,
                PARENS,
                ASSIGN,
                TYPECAST,
                TYPETEST,
                BINDINGPATTERN,
                INDEXED,
                SELECT,
                REFERENCE,
                IDENT,
                LITERAL,
                TYPEIDENT,
                TYPEARRAY,
                TYPEAPPLY,
                TYPEUNION,
                TYPEINTERSECTION,
                TYPEPARAMETER,
                WILDCARD,
                TYPEBOUNDKIND,
                ANNOTATION,
                TYPE_ANNOTATION,
                MODIFIERS,
                ANNOTATED_TYPE,
                ERRONEOUS,
                POS,
                NEG,
                NOT,
                COMPL,
                PREINC,
                PREDEC,
                POSTINC,
                POSTDEC,
                NULLCHK,
                OR,
                AND,
                BITOR,
                BITXOR,
                BITAND,
                EQ,
                NE,
                LT,
                GT,
                LE,
                GE,
                SL,
                SR,
                USR,
                PLUS,
                MINUS,
                MUL,
                DIV,
                MOD,
                BITOR_ASG,
                BITXOR_ASG,
                BITAND_ASG,
                SL_ASG,
                SR_ASG,
                USR_ASG,
                PLUS_ASG,
                MINUS_ASG,
                MUL_ASG,
                DIV_ASG,
                MOD_ASG,
                MODULEDEF,
                EXPORTS,
                OPENS,
                PROVIDES,
                REQUIRES,
                USES,
                LETEXPR
            }

            public readonly InnerEnum innerEnumValue;
            private readonly string nameValue;
            private readonly int ordinalValue;
            private static int nextOrdinal = 0;

            internal readonly Tag noAssignTag;

            internal static readonly int numberOfOperators = MOD.ordinal() - POS.ordinal() + 1;

            internal Tag(string name, InnerEnum innerEnum, Tag noAssignTag)
            {
                this.noAssignTag = noAssignTag;

                nameValue = name;
                ordinalValue = nextOrdinal++;
                innerEnumValue = innerEnum;
            }

            internal Tag(string name, InnerEnum innerEnum) : this(name, innerEnum, null)
            {

                nameValue = name;
                ordinalValue = nextOrdinal++;
                innerEnumValue = innerEnum;
            }

            public static int getNumberOfOperators()
            {
                return numberOfOperators;
            }

            public Tag noAssignOp()
            {
                if (noAssignTag != null)
                {
                    return noAssignTag;
                }
                throw new AssertionError("noAssignOp() method is not available for non assignment tags");
            }

            public bool isPostUnaryOp()
            {
                return (this == POSTINC || this == POSTDEC);
            }

            public bool isIncOrDecUnaryOp()
            {
                return (this == PREINC || this == PREDEC || this == POSTINC || this == POSTDEC);
            }

            public bool isAssignop()
            {
                return noAssignTag != null;
            }

            public int operatorIndex()
            {
                return (this.ordinal() - POS.ordinal());
            }

            public static IList<Tag> values()
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

            public static Tag valueOf(string name)
            {
                foreach (Tag enumInstance in Tag.valueList)
                {
                    if (enumInstance.nameValue == name)
                    {
                        return enumInstance;
                    }
                }
                throw new System.ArgumentException(name);
            }
        }

        /* The (encoded) position in the source file. @see util.Position.
         */
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER NOTE: Fields cannot have the same name as methods:
        public int pos;

        /* The type of this node.
         */
        public Type type;

        // TODO: comments
        public string docComments = null;

        /* The tag of this node -- one of the constants declared above.
         */
        public abstract Tag getTag();

        /// <summary>
        /// Returns the kind of this tree.
        /// </summary>
        /// <returns></returns>
        public abstract Kind getKind();

        /* Returns true if the tag of this node is equals to tag.
         */
        public virtual bool hasTag(Tag tag)
        {
            return tag == getTag();
        }

        /// <summary>
        /// Convert a tree to a pretty-printed string. </summary>
        public override string ToString()
        {
            StringWriter s = new StringWriter();
            try
            {
                (new Pretty(s, false)).printExpr(this);
            }
            catch (IOException e)
            {
                // should never happen, because StringWriter is defined
                // never to throw any IOExceptions
                throw new AssertionError(e);
            }
            return s.ToString();
        }

        /// <summary>
        /// Set position field and return this tree.
        /// </summary>
        public virtual JCTree setPos(int pos)
        {
            this.pos = pos;
            return this;
        }

        /// <summary>
        /// Set type field and return this tree.
        /// </summary>
        public virtual JCTree setType(Type type)
        {
            this.type = type;
            return this;
        }

        /// <summary>
        /// Visit this tree with a given visitor.
        /// </summary>
        public abstract void accept(Visitor v);

        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public abstract <R,D> R accept(TreeVisitor<R,D> v, D d);
        public abstract R accept<R, D>(TreeVisitor<R, D> v, D d);

        ///// <summary>
        ///// Return a shallow copy of this tree.
        ///// </summary>
        //public override object clone()
        //{
        //    try
        //    {
        //        return base.clone();
        //    }
        //    catch (CloneNotSupportedException e)
        //    {
        //        throw new Exception(e);
        //    }
        //}

        ///// <summary>
        ///// Get a default position for this tree node.
        ///// </summary>
        //public virtual DiagnosticPosition pos()
        //{
        //    return this;
        //}

        // for default DiagnosticPosition
        public virtual JCTree getTree()
        {
            return this;
        }

        // for default DiagnosticPosition
        public virtual int getStartPosition()
        {
            return TreeInfo.getStartPos(this);
        }

        // for default DiagnosticPosition
        public virtual int getPreferredPosition()
        {
            return pos;
        }

        // for default DiagnosticPosition
        public virtual int getEndPosition(EndPosTable endPosTable)
        {
            return TreeInfo.getEndPos(this, endPosTable);
        }

        /// <summary>
        /// Everything in one source file is kept in a <seealso cref="JCCompilationUnit"/> structure.
        /// </summary>
        public class JCCompilationUnit : JCTree, CompilationUnitTree
        {
            /// <summary>
            /// All definitions in this file (ClassDef, Import, and Skip) </summary>
            public List<JCTree> defs;
            ///// <summary>
            ///// The source file name. </summary>
            //public JavaFileObject sourcefile;
            ///// <summary>
            ///// The module to which this compilation unit belongs. </summary>
            //public ModuleSymbol modle;
            ///// <summary>
            ///// The location in which this compilation unit was found. </summary>
            //public Location locn;
            ///// <summary>
            ///// The package to which this compilation unit belongs. </summary>
            //public PackageSymbol packge;
            ///// <summary>
            ///// A scope containing top level classes. </summary>
            //public WriteableScope toplevelScope;
            ///// <summary>
            ///// A scope for all named imports. </summary>
            //public NamedImportScope namedImportScope;
            ///// <summary>
            ///// A scope for all import-on-demands. </summary>
            //public StarImportScope starImportScope;
            ///// <summary>
            ///// Line starting positions, defined only if option -g is set. </summary>
            public Position.LineMap lineMap = null;
            ///// <summary>
            ///// A table that stores all documentation comments indexed by the tree
            ///// nodes they refer to. defined only if option -s is set. 
            ///// </summary>
            //public DocCommentTable docComments = null;
            /* An object encapsulating ending positions of source ranges indexed by
             * the tree nodes they belong to. Defined only if option -Xjcov is set. */
            public EndPosTable endPositions = null;
            protected internal JCCompilationUnit(List<JCTree> defs)
            {
                this.defs = defs;
            }
            public override void accept(Visitor v)
            {
                v.visitTopLevel(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.COMPILATION_UNIT;
            }

            public virtual JCModuleDecl getModuleDecl()
            {
                foreach (JCTree tree in defs)
                {
                    if (tree.hasTag(MODULEDEF))
                    {
                        return (JCModuleDecl)tree;
                    }
                }

                return null;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCPackageDecl getPackage()
            public virtual PackageTree getPackage()
            {
                // PackageDecl must be the first entry if it exists
                if (!defs.isEmpty() && defs[0].hasTag(PACKAGEDEF))
                {
                    return (JCPackageDecl)defs[0];
                }
                return null;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCAnnotation> getPackageAnnotations()
            public virtual IList<AnnotationTree> getPackageAnnotations()
            {
                JCPackageDecl pd = (JCPackageDecl)getPackage();
                return pd != null ? pd.getAnnotations() : new List<AnnotationTree>();
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public ExpressionTree getPackageName()
            public virtual ExpressionTree getPackageName()
            {
                JCPackageDecl pd = (JCPackageDecl)getPackage();
                return pd != null ? pd.getPackageName() : null;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCImport> getImports()
            public virtual IList<ImportTree> getImports()
            {
                List<ImportTree> imports = new List<ImportTree>();
                foreach (JCTree tree in defs)
                {
                    if (tree.hasTag(IMPORT))
                    {
                        imports.Add((JCImport)tree);
                    }
                    else if (!tree.hasTag(PACKAGEDEF) && !tree.hasTag(SKIP))
                    {
                        break;
                    }
                }
                return imports; //.toList();
            }
            ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            ////ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public javax.tools.JavaFileObject getSourceFile()
            //public virtual JavaFileObject getSourceFile()
            //{
            //    return sourcefile;
            //}
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Position.LineMap getLineMap()
            public virtual LineMap getLineMap()
            {
                return lineMap;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCTree> getTypeDecls()
            public virtual IList<Tree> getTypeDecls()
            {
                List<Tree> typeDefs = new List<Tree>();
                //for (typeDefs = defs; !typeDefs.isEmpty(); typeDefs = typeDefs.tail)
                //{
                //    if (!typeDefs.head.hasTag(PACKAGEDEF) && !typeDefs.head.hasTag(IMPORT))
                //    {
                //        break;
                //    }
                //}
                foreach (var typeDef in defs)
                {
                    if (!typeDef.hasTag(Tag.PACKAGEDEF) && !typeDef.hasTag(Tag.IMPORT))
                    {
                        break;
                    }
                    typeDefs.Add(typeDef);
                }
                return typeDefs;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitCompilationUnit(this, d);
            }

            public override Tag getTag()
            {
                return TOPLEVEL;
            }
        }

        /// <summary>
        /// Package definition.
        /// </summary>
        public class JCPackageDecl : JCTree, PackageTree
        {
            public List<JCAnnotation> annotations;
            /// <summary>
            /// The tree representing the package clause. </summary>
            public JCExpression pid;
            public PackageSymbol packge;
            public JCPackageDecl(List<JCAnnotation> annotations, JCExpression pid)
            {
                this.annotations = annotations;
                this.pid = pid;
            }
            public override void accept(Visitor v)
            {
                v.visitPackageDef(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.PACKAGE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCAnnotation> getAnnotations()
            public virtual IList<AnnotationTree> getAnnotations()
            {
                return new List<AnnotationTree>(annotations);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getPackageName()
            public virtual ExpressionTree getPackageName()
            {
                return pid;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitPackage(this, d);
            }
            public override Tag getTag()
            {
                return PACKAGEDEF;
            }
        }

        /// <summary>
        /// An import clause.
        /// </summary>
        public class JCImport : JCTree, ImportTree
        {
            public bool staticImport;
            /// <summary>
            /// The imported class(es). </summary>
            public JCTree qualid;
            public com.sun.tools.javac.code.Scope importScope;
            protected internal JCImport(JCTree qualid, bool importStatic)
            {
                this.qualid = qualid;
                this.staticImport = importStatic;
            }
            public override void accept(Visitor v)
            {
                v.visitImport(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public boolean isStatic()
            public virtual bool isStatic()
            {
                return staticImport;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getQualifiedIdentifier()
            public virtual Tree getQualifiedIdentifier()
            {
                return qualid;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.IMPORT;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitImport(this, d);
            }

            public override Tag getTag()
            {
                return IMPORT;
            }
        }

        public abstract class JCStatement : JCTree, StatementTree
        {
            public override JCTree setType(Type type)
            {
                base.setType(type);
                return this;
            }
            public override JCTree setPos(int pos)
            {
                base.setPos(pos);
                return this;
            }
        }

        public abstract class JCExpression : JCTree, ExpressionTree
        {
            public override JCTree setType(Type type)
            {
                base.setType(type);
                return this;
            }
            public override JCTree setPos(int pos)
            {
                base.setPos(pos);
                return this;
            }

            public virtual bool isPoly()
            {
                return false;
            }
            public virtual bool isStandalone()
            {
                return true;
            }
        }

        /// <summary>
        /// Common supertype for all poly expression trees (lambda, method references,
        /// conditionals, method and constructor calls)
        /// </summary>
        public abstract class JCPolyExpression : JCExpression
        {

            /// <summary>
            /// A poly expression can only be truly 'poly' in certain contexts
            /// </summary>
            public enum PolyKind
            {
                /// <summary>
                /// poly expression to be treated as a standalone expression </summary>
                STANDALONE,
                /// <summary>
                /// true poly expression </summary>
                POLY
            }

            /// <summary>
            /// is this poly expression a 'true' poly expression? </summary>
            public PolyKind polyKind;

            public override bool isPoly()
            {
                return polyKind == PolyKind.POLY;
            }
            public override bool isStandalone()
            {
                return polyKind == PolyKind.STANDALONE;
            }
        }

        /// <summary>
        /// Common supertype for all functional expression trees (lambda and method references)
        /// </summary>
        public abstract class JCFunctionalExpression : JCPolyExpression
        {

            public JCFunctionalExpression()
            {
                //a functional expression is always a 'true' poly
                polyKind = PolyKind.POLY;
            }

            /// <summary>
            /// list of target types inferred for this functional expression. </summary>
            public Type target;

            public virtual Type getDescriptorType(Types types)
            {
                return target != null ? types.findDescriptorType(target) : types.createErrorType(null);
            }
        }

        /// <summary>
        /// A class definition.
        /// </summary>
        public class JCClassDecl : JCStatement, ClassTree
        {
            /// <summary>
            /// the modifiers </summary>
            public JCModifiers mods;
            /// <summary>
            /// the name of the class </summary>
            public Name name;
            /// <summary>
            /// formal class parameters </summary>
            public List<JCTypeParameter> typarams;
            /// <summary>
            /// the classes this class extends </summary>
            public JCExpression extending;
            /// <summary>
            /// the interfaces implemented by this class </summary>
            public List<JCExpression> implementing;
            /// <summary>
            /// the subclasses allowed to extend this class, if sealed </summary>
            public List<JCExpression> permitting;
            /// <summary>
            /// all variables and methods defined in this class </summary>
            public List<JCTree> defs;
            /// <summary>
            /// the symbol </summary>
            public ClassSymbol sym;
            protected internal JCClassDecl(JCModifiers mods, Name name, List<JCTypeParameter> typarams, JCExpression extending, List<JCExpression> implementing, List<JCExpression> permitting, List<JCTree> defs, ClassSymbol sym)
            {
                this.mods = mods;
                this.name = name;
                this.typarams = typarams;
                this.extending = extending;
                this.implementing = implementing;
                this.permitting = permitting;
                this.defs = defs;
                this.sym = sym;
            }
            public override void accept(Visitor v)
            {
                v.visitClassDef(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @SuppressWarnings("preview") @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                if ((mods.flags & Flags.ANNOTATION) != 0)
                {
                    return Kind.ANNOTATION_TYPE;
                }
                else if ((mods.flags & Flags.INTERFACE) != 0)
                {
                    return Kind.INTERFACE;
                }
                else if ((mods.flags & Flags.ENUM) != 0)
                {
                    return Kind.ENUM;
                }
                else if ((mods.flags & Flags.RECORD) != 0)
                {
                    return Kind.RECORD;
                }
                else
                {
                    return Kind.CLASS;
                }
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCModifiers getModifiers()
            public virtual ModifiersTree getModifiers()
            {
                return mods;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Name getSimpleName()
            public virtual java.lang.common.api.Name getSimpleName()
            {
                return name;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCTypeParameter> getTypeParameters()
            public virtual IList<TypeParameterTree> getTypeParameters()
            {
                return new List<TypeParameterTree>(typarams);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExtendsClause()
            public virtual Tree getExtendsClause()
            {
                return extending;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getImplementsClause()
            public virtual IList<Tree> getImplementsClause()
            {
                return new List<Tree>(implementing);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @SuppressWarnings("removal") @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getPermitsClause()
            public virtual IList<Tree> getPermitsClause()
            {
                return new List<Tree>(permitting);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCTree> getMembers()
            public virtual IList<Tree> getMembers()
            {
                return new List<Tree>(defs);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitClass(this, d);
            }

            public override Tag getTag()
            {
                return CLASSDEF;
            }
        }

        /// <summary>
        /// A method definition.
        /// </summary>
        public class JCMethodDecl : JCTree, MethodTree
        {
            /// <summary>
            /// method modifiers </summary>
            public JCModifiers mods;
            /// <summary>
            /// method name </summary>
            public Name name;
            /// <summary>
            /// type of method return value </summary>
            public JCExpression restype;
            /// <summary>
            /// type parameters </summary>
            public List<JCTypeParameter> typarams;
            /// <summary>
            /// receiver parameter </summary>
            public JCVariableDecl recvparam;
            /// <summary>
            /// value parameters </summary>
            public List<JCVariableDecl> @params;
            /// <summary>
            /// exceptions thrown by this method </summary>
            public List<JCExpression> thrown;
            /// <summary>
            /// statements in the method </summary>
            public JCBlock body;
            /// <summary>
            /// default value, for annotation types </summary>
            public JCExpression defaultValue;
            /// <summary>
            /// method symbol </summary>
            public MethodSymbol sym;
            /// <summary>
            /// does this method completes normally </summary>
            public bool completesNormally;

            protected internal JCMethodDecl(JCModifiers mods, Name name, JCExpression restype, List<JCTypeParameter> typarams, JCVariableDecl recvparam, List<JCVariableDecl> @params, List<JCExpression> thrown, JCBlock body, JCExpression defaultValue, MethodSymbol sym)
            {
                this.mods = mods;
                this.name = name;
                this.restype = restype;
                this.typarams = typarams;
                this.@params = @params;
                this.recvparam = recvparam;
                // TODO: do something special if the given type is null?
                // receiver != null ? receiver : List.<JCTypeAnnotation>nil());
                this.thrown = thrown;
                this.body = body;
                this.defaultValue = defaultValue;
                this.sym = sym;
            }
            public override void accept(Visitor v)
            {
                v.visitMethodDef(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.METHOD;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCModifiers getModifiers()
            public virtual ModifiersTree getModifiers()
            {
                return mods;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Name getName()
            public virtual java.lang.common.api.Name getName()
            {
                return name;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getReturnType()
            public virtual Tree getReturnType()
            {
                return restype;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCTypeParameter> getTypeParameters()
            public virtual IList<TypeParameterTree> getTypeParameters()
            {
                return new List<TypeParameterTree>(typarams);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCVariableDecl> getParameters()
            public virtual IList<VariableTree> getParameters()
            {
                return new List<VariableTree>(@params);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCVariableDecl getReceiverParameter()
            public virtual VariableTree getReceiverParameter()
            {
                return recvparam;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getThrows()
            public virtual IList<ExpressionTree> getThrows()
            {
                return new List<ExpressionTree>(thrown);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCBlock getBody()
            public virtual BlockTree getBody()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getDefaultValue()
            public virtual Tree getDefaultValue()
            { // for annotation types
                return defaultValue;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitMethod(this, d);
            }

            public override Tag getTag()
            {
                return METHODDEF;
            }
        }

        /// <summary>
        /// A variable definition.
        /// </summary>
        public class JCVariableDecl : JCStatement, VariableTree
        {
            /// <summary>
            /// variable modifiers </summary>
            public JCModifiers mods;
            /// <summary>
            /// variable name </summary>
            public Name name;
            /// <summary>
            /// variable name expression </summary>
            public JCExpression nameexpr;
            /// <summary>
            /// type of the variable </summary>
            public JCExpression vartype;
            /// <summary>
            /// variable's initial value </summary>
            public JCExpression init;
            /// <summary>
            /// symbol </summary>
            public VarSymbol sym;
            /// <summary>
            /// explicit start pos </summary>
            public int startPos = Position.NOPOS;

            protected internal JCVariableDecl(JCModifiers mods, Name name, JCExpression vartype, JCExpression init, VarSymbol sym)
            {
                this.mods = mods;
                this.name = name;
                this.vartype = vartype;
                this.init = init;
                this.sym = sym;
            }

            protected internal JCVariableDecl(JCModifiers mods, JCExpression nameexpr, JCExpression vartype) : this(mods, null, vartype, null, null)
            {
                this.nameexpr = nameexpr;
                if (nameexpr.hasTag(Tag.IDENT))
                {
                    this.name = ((JCIdent)nameexpr).name;
                }
                else
                {
                    // Only other option is qualified name x.y.this;
                    this.name = ((JCFieldAccess)nameexpr).name;
                }
            }

            public virtual bool isImplicitlyTyped()
            {
                return vartype == null;
            }

            public override void accept(Visitor v)
            {
                v.visitVarDef(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.VARIABLE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCModifiers getModifiers()
            public virtual ModifiersTree getModifiers()
            {
                return mods;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Name getName()
            public virtual java.lang.common.api.Name getName()
            {
                return name;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getNameExpression()
            public virtual ExpressionTree getNameExpression()
            {
                return nameexpr;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getType()
            public virtual Tree getType()
            {
                return vartype;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getInitializer()
            public virtual ExpressionTree getInitializer()
            {
                return init;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitVariable(this, d);
            }

            public override Tag getTag()
            {
                return VARDEF;
            }
        }

        /// <summary>
        /// A no-op statement ";".
        /// </summary>
        public class JCSkip : JCStatement, EmptyStatementTree
        {
            protected internal JCSkip()
            {
            }
            public override void accept(Visitor v)
            {
                v.visitSkip(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.EMPTY_STATEMENT;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitEmptyStatement(this, d);
            }

            public override Tag getTag()
            {
                return SKIP;
            }
        }

        /// <summary>
        /// A statement block.
        /// </summary>
        public class JCBlock : JCStatement, BlockTree
        {
            /// <summary>
            /// flags </summary>
            public long flags;
            /// <summary>
            /// statements </summary>
            public List<JCStatement> stats;
            /// <summary>
            /// Position of closing brace, optional. </summary>
            public int endpos = Position.NOPOS;
            protected internal JCBlock(long flags, List<JCStatement> stats)
            {
                this.stats = stats;
                this.flags = flags;
            }
            public override void accept(Visitor v)
            {
                v.visitBlock(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.BLOCK;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCStatement> getStatements()
            public virtual IList<StatementTree> getStatements()
            {
                return new List<StatementTree>(stats);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public boolean isStatic()
            public virtual bool isStatic()
            {
                return (flags & Flags.STATIC) != 0;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitBlock(this, d);
            }

            public override Tag getTag()
            {
                return BLOCK;
            }
        }

        /// <summary>
        /// A do loop
        /// </summary>
        public class JCDoWhileLoop : JCStatement, DoWhileLoopTree
        {
            public JCStatement body;
            public JCExpression cond;
            protected internal JCDoWhileLoop(JCStatement body, JCExpression cond)
            {
                this.body = body;
                this.cond = cond;
            }
            public override void accept(Visitor v)
            {
                v.visitDoLoop(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.DO_WHILE_LOOP;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getCondition()
            public virtual ExpressionTree getCondition()
            {
                return cond;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCStatement getStatement()
            public virtual StatementTree getStatement()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitDoWhileLoop(this, d);
            }

            public override Tag getTag()
            {
                return DOLOOP;
            }
        }

        /// <summary>
        /// A while loop
        /// </summary>
        public class JCWhileLoop : JCStatement, WhileLoopTree
        {
            public JCExpression cond;
            public JCStatement body;
            protected internal JCWhileLoop(JCExpression cond, JCStatement body)
            {
                this.cond = cond;
                this.body = body;
            }
            public override void accept(Visitor v)
            {
                v.visitWhileLoop(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.WHILE_LOOP;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getCondition()
            public virtual ExpressionTree getCondition()
            {
                return cond;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCStatement getStatement()
            public virtual StatementTree getStatement()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitWhileLoop(this, d);
            }

            public override Tag getTag()
            {
                return WHILELOOP;
            }
        }

        /// <summary>
        /// A for loop.
        /// </summary>
        public class JCForLoop : JCStatement, ForLoopTree
        {
            public List<JCStatement> init;
            public JCExpression cond;
            public List<JCExpressionStatement> step;
            public JCStatement body;
            protected internal JCForLoop(List<JCStatement> init, JCExpression cond, List<JCExpressionStatement> update, JCStatement body)
            {
                this.init = init;
                this.cond = cond;
                this.step = update;
                this.body = body;
            }
            public override void accept(Visitor v)
            {
                v.visitForLoop(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.FOR_LOOP;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getCondition()
            public virtual ExpressionTree getCondition()
            {
                return cond;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCStatement getStatement()
            public virtual StatementTree getStatement()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCStatement> getInitializer()
            public virtual IList<StatementTree> getInitializer()
            {
                return new List<StatementTree>(init);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpressionStatement> getUpdate()
            public virtual IList<ExpressionStatementTree> getUpdate()
            {
                return new List<ExpressionStatementTree>(step);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitForLoop(this, d);
            }

            public override Tag getTag()
            {
                return FORLOOP;
            }
        }

        /// <summary>
        /// The enhanced for loop.
        /// </summary>
        public class JCEnhancedForLoop : JCStatement, EnhancedForLoopTree
        {
            public JCVariableDecl @var;
            public JCExpression expr;
            public JCStatement body;
            protected internal JCEnhancedForLoop(JCVariableDecl @var, JCExpression expr, JCStatement body)
            {
                this.@var = @var;
                this.expr = expr;
                this.body = body;
            }
            public override void accept(Visitor v)
            {
                v.visitForeachLoop(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.ENHANCED_FOR_LOOP;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCVariableDecl getVariable()
            public virtual VariableTree getVariable()
            {
                return @var;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return expr;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCStatement getStatement()
            public virtual StatementTree getStatement()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitEnhancedForLoop(this, d);
            }
            public override Tag getTag()
            {
                return FOREACHLOOP;
            }
        }

        /// <summary>
        /// A labelled expression or statement.
        /// </summary>
        public class JCLabeledStatement : JCStatement, LabeledStatementTree
        {
            public Name label;
            public JCStatement body;
            protected internal JCLabeledStatement(Name label, JCStatement body)
            {
                this.label = label;
                this.body = body;
            }
            public override void accept(Visitor v)
            {
                v.visitLabelled(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.LABELED_STATEMENT;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Name getLabel()
            public virtual java.lang.common.api.Name getLabel()
            {
                return label;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCStatement getStatement()
            public virtual StatementTree getStatement()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitLabeledStatement(this, d);
            }
            public override Tag getTag()
            {
                return LABELLED;
            }
        }

        /// <summary>
        /// A "switch ( ) { }" construction.
        /// </summary>
        public class JCSwitch : JCStatement, SwitchTree
        {
            public JCExpression selector;
            public List<JCCase> cases;
            protected internal JCSwitch(JCExpression selector, List<JCCase> cases)
            {
                this.selector = selector;
                this.cases = cases;
            }
            public override void accept(Visitor v)
            {
                v.visitSwitch(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.SWITCH;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return selector;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCCase> getCases()
            public virtual IList<CaseTree> getCases()
            {
                return new List<CaseTree>(cases);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitSwitch(this, d);
            }
            public override Tag getTag()
            {
                return SWITCH;
            }
        }

        /// <summary>
        /// A "case  :" of a switch.
        /// </summary>
        public class JCCase : JCStatement, CaseTree
        {
            //as CaseKind is deprecated for removal (as it is part of a preview feature),
            //using indirection through these fields to avoid unnecessary @SuppressWarnings:
            public static readonly CaseKind STATEMENT = CaseKind.STATEMENT;
            public static readonly CaseKind RULE = CaseKind.RULE;
            public readonly CaseKind caseKind;
            public List<JCExpression> pats;
            public List<JCStatement> stats;
            public JCTree body;
            public bool completesNormally;
            protected internal JCCase(CaseKind caseKind, List<JCExpression> pats, List<JCStatement> stats, JCTree body)
            {
                Assert.checkNonNull(pats);
                Assert.check(pats.isEmpty() || pats[0] != null);
                this.caseKind = caseKind;
                this.pats = pats;
                this.stats = stats;
                this.body = body;
            }
            public override void accept(Visitor v)
            {
                v.visitCase(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.CASE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @Deprecated @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            [Obsolete]
            public virtual ExpressionTree getExpression()
            {
                return pats[0];
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getExpressions()
            public virtual IList<ExpressionTree> getExpressions()
            {
                return new List<ExpressionTree>(pats);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCStatement> getStatements()
            public virtual IList<StatementTree> getStatements()
            {
                return caseKind == CaseKind.STATEMENT ? new List<StatementTree>(stats) : null;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getBody()
            public virtual Tree getBody()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public CaseKind getCaseKind()
            public virtual CaseKind getCaseKind()
            {
                return caseKind;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitCase(this, d);
            }
            public override Tag getTag()
            {
                return CASE;
            }
        }

        /// <summary>
        /// A "switch ( ) { }" construction.
        /// </summary>
        public class JCSwitchExpression : JCPolyExpression, SwitchExpressionTree
        {
            public JCExpression selector;
            public List<JCCase> cases;
            /// <summary>
            /// Position of closing brace, optional. </summary>
            public int endpos = Position.NOPOS;
            protected internal JCSwitchExpression(JCExpression selector, List<JCCase> cases)
            {
                this.selector = selector;
                this.cases = cases;
            }
            public override void accept(Visitor v)
            {
                v.visitSwitchExpression(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.SWITCH_EXPRESSION;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return selector;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCCase> getCases()
            public virtual IList<CaseTree> getCases()
            {
                return new List<CaseTree>(cases);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitSwitchExpression(this, d);
            }
            public override Tag getTag()
            {
                return SWITCH_EXPRESSION;
            }
        }

        /// <summary>
        /// A synchronized block.
        /// </summary>
        public class JCSynchronized : JCStatement, SynchronizedTree
        {
            public JCExpression @lock;
            public JCBlock body;
            protected internal JCSynchronized(JCExpression @lock, JCBlock body)
            {
                this.@lock = @lock;
                this.body = body;
            }
            public override void accept(Visitor v)
            {
                v.visitSynchronized(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.SYNCHRONIZED;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return @lock;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCBlock getBlock()
            public virtual BlockTree getBlock()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitSynchronized(this, d);
            }
            public override Tag getTag()
            {
                return SYNCHRONIZED;
            }
        }

        /// <summary>
        /// A "try { } catch ( ) { } finally { }" block.
        /// </summary>
        public class JCTry : JCStatement, TryTree
        {
            public JCBlock body;
            public List<JCCatch> catchers;
            public JCBlock finalizer;
            public List<JCTree> resources;
            public bool finallyCanCompleteNormally;
            protected internal JCTry(List<JCTree> resources, JCBlock body, List<JCCatch> catchers, JCBlock finalizer)
            {
                this.body = body;
                this.catchers = catchers;
                this.finalizer = finalizer;
                this.resources = resources;
            }
            public override void accept(Visitor v)
            {
                v.visitTry(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.TRY;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCBlock getBlock()
            public virtual BlockTree getBlock()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCCatch> getCatches()
            public virtual IList<CatchTree> getCatches()
            {
                return new List<CatchTree>(catchers);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCBlock getFinallyBlock()
            public virtual BlockTree getFinallyBlock()
            {
                return finalizer;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitTry(this, d);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCTree> getResources()
            public virtual IList<Tree> getResources()
            {
                return new List<Tree>(resources);
            }
            public override Tag getTag()
            {
                return TRY;
            }
        }

        /// <summary>
        /// A catch block.
        /// </summary>
        public class JCCatch : JCTree, CatchTree
        {
            public JCVariableDecl param;
            public JCBlock body;
            protected internal JCCatch(JCVariableDecl param, JCBlock body)
            {
                this.param = param;
                this.body = body;
            }
            public override void accept(Visitor v)
            {
                v.visitCatch(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.CATCH;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCVariableDecl getParameter()
            public virtual VariableTree getParameter()
            {
                return param;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCBlock getBlock()
            public virtual BlockTree getBlock()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitCatch(this, d);
            }
            public override Tag getTag()
            {
                return CATCH;
            }
        }

        /// <summary>
        /// A ( ) ? ( ) : ( ) conditional expression
        /// </summary>
        public class JCConditional : JCPolyExpression, ConditionalExpressionTree
        {
            public JCExpression cond;
            public JCExpression truepart;
            public JCExpression falsepart;
            protected internal JCConditional(JCExpression cond, JCExpression truepart, JCExpression falsepart)
            {
                this.cond = cond;
                this.truepart = truepart;
                this.falsepart = falsepart;
            }
            public override void accept(Visitor v)
            {
                v.visitConditional(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.CONDITIONAL_EXPRESSION;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getCondition()
            public virtual ExpressionTree getCondition()
            {
                return cond;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getTrueExpression()
            public virtual ExpressionTree getTrueExpression()
            {
                return truepart;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getFalseExpression()
            public virtual ExpressionTree getFalseExpression()
            {
                return falsepart;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitConditionalExpression(this, d);
            }
            public override Tag getTag()
            {
                return CONDEXPR;
            }
        }

        /// <summary>
        /// An "if ( ) { } else { }" block
        /// </summary>
        public class JCIf : JCStatement, IfTree
        {
            public JCExpression cond;
            public JCStatement thenpart;
            public JCStatement elsepart;
            protected internal JCIf(JCExpression cond, JCStatement thenpart, JCStatement elsepart)
            {
                this.cond = cond;
                this.thenpart = thenpart;
                this.elsepart = elsepart;
            }
            public override void accept(Visitor v)
            {
                v.visitIf(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.IF;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getCondition()
            public virtual ExpressionTree getCondition()
            {
                return cond;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCStatement getThenStatement()
            public virtual StatementTree getThenStatement()
            {
                return thenpart;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCStatement getElseStatement()
            public virtual StatementTree getElseStatement()
            {
                return elsepart;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitIf(this, d);
            }
            public override Tag getTag()
            {
                return IF;
            }
        }

        /// <summary>
        /// an expression statement
        /// </summary>
        public class JCExpressionStatement : JCStatement, ExpressionStatementTree
        {
            /// <summary>
            /// expression structure </summary>
            public JCExpression expr;
            protected internal JCExpressionStatement(JCExpression expr)
            {
                this.expr = expr;
            }
            public override void accept(Visitor v)
            {
                v.visitExec(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.EXPRESSION_STATEMENT;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return expr;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitExpressionStatement(this, d);
            }
            public override Tag getTag()
            {
                return EXEC;
            }

            /// <summary>
            /// Convert a expression-statement tree to a pretty-printed string. </summary>
            public override string ToString()
            {
                StringWriter s = new StringWriter();
                try
                {
                    (new Pretty(s, false)).printStat(this);
                }
                catch (IOException e)
                {
                    // should never happen, because StringWriter is defined
                    // never to throw any IOExceptions
                    throw new AssertionError(e);
                }
                return s.ToString();
            }
        }

        /// <summary>
        /// A break from a loop or switch.
        /// </summary>
        public class JCBreak : JCStatement, BreakTree
        {
            public Name label;
            public JCTree target;
            protected internal JCBreak(Name label, JCTree target)
            {
                this.label = label;
                this.target = target;
            }
            public override void accept(Visitor v)
            {
                v.visitBreak(this);
            }
            public virtual bool isValueBreak()
            {
                return target != null && target.hasTag(SWITCH_EXPRESSION);
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.BREAK;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Name getLabel()
            public virtual java.lang.common.api.Name getLabel()
            {
                return label;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitBreak(this, d);
            }
            public override Tag getTag()
            {
                return BREAK;
            }
        }

        /// <summary>
        /// A break-with from a switch expression.
        /// </summary>
        public class JCYield : JCStatement, YieldTree
        {
            public JCExpression value;
            public JCTree target;
            protected internal JCYield(JCExpression value, JCTree target)
            {
                this.value = value;
                this.target = target;
            }
            public override void accept(Visitor v)
            {
                v.visitYield(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.YIELD;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getValue()
            public virtual ExpressionTree getValue()
            {
                return value;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitYield(this, d);
            }
            public override Tag getTag()
            {
                return YIELD;
            }
        }

        /// <summary>
        /// A continue of a loop.
        /// </summary>
        public class JCContinue : JCStatement, ContinueTree
        {
            public Name label;
            public JCTree target;
            protected internal JCContinue(Name label, JCTree target)
            {
                this.label = label;
                this.target = target;
            }
            public override void accept(Visitor v)
            {
                v.visitContinue(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.CONTINUE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Name getLabel()
            public virtual java.lang.common.api.Name getLabel()
            {
                return label;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitContinue(this, d);
            }
            public override Tag getTag()
            {
                return CONTINUE;
            }
        }

        /// <summary>
        /// A return statement.
        /// </summary>
        public class JCReturn : JCStatement, ReturnTree
        {
            public JCExpression expr;
            protected internal JCReturn(JCExpression expr)
            {
                this.expr = expr;
            }
            public override void accept(Visitor v)
            {
                v.visitReturn(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.RETURN;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return expr;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitReturn(this, d);
            }
            public override Tag getTag()
            {
                return RETURN;
            }
        }

        /// <summary>
        /// A throw statement.
        /// </summary>
        public class JCThrow : JCStatement, ThrowTree
        {
            public JCExpression expr;
            protected internal JCThrow(JCExpression expr)
            {
                this.expr = expr;
            }
            public override void accept(Visitor v)
            {
                v.visitThrow(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.THROW;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return expr;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitThrow(this, d);
            }
            public override Tag getTag()
            {
                return THROW;
            }
        }

        /// <summary>
        /// An assert statement.
        /// </summary>
        public class JCAssert : JCStatement, AssertTree
        {
            public JCExpression cond;
            public JCExpression detail;
            protected internal JCAssert(JCExpression cond, JCExpression detail)
            {
                this.cond = cond;
                this.detail = detail;
            }
            public override void accept(Visitor v)
            {
                v.visitAssert(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.ASSERT;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getCondition()
            public virtual ExpressionTree getCondition()
            {
                return cond;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getDetail()
            public virtual ExpressionTree getDetail()
            {
                return detail;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitAssert(this, d);
            }
            public override Tag getTag()
            {
                return ASSERT;
            }
        }

        /// <summary>
        /// A method invocation
        /// </summary>
        public class JCMethodInvocation : JCPolyExpression, MethodInvocationTree
        {
            public List<JCExpression> typeargs;
            public JCExpression meth;
            public List<JCExpression> args;
            public Type varargsElement;
            protected internal JCMethodInvocation(List<JCExpression> typeargs, JCExpression meth, List<JCExpression> args)
            {
                this.typeargs = (typeargs == null) ? new List<JCExpression>() : typeargs; // List.nil()
                this.meth = meth;
                this.args = args;
            }
            public override void accept(Visitor v)
            {
                v.visitApply(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.METHOD_INVOCATION;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getTypeArguments()
            public virtual IList<Tree> getTypeArguments()
            {
                return new List<Tree>(typeargs);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getMethodSelect()
            public virtual ExpressionTree getMethodSelect()
            {
                return meth;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getArguments()
            public virtual IList<ExpressionTree> getArguments()
            {
                return new List<ExpressionTree>(args);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitMethodInvocation(this, d);
            }
            public override JCTree setType(Type type)
            {
                base.setType(type);
                return this;
            }
            public override Tag getTag()
            {
                return (APPLY);
            }
        }

        /// <summary>
        /// A new(...) operation.
        /// </summary>
        public class JCNewClass : JCPolyExpression, NewClassTree
        {
            public JCExpression encl;
            public List<JCExpression> typeargs;
            public JCExpression clazz;
            public List<JCExpression> args;
            public JCClassDecl def;
            public Symbol constructor;
            public Type varargsElement;
            public Type constructorType;
            protected internal JCNewClass(JCExpression encl, List<JCExpression> typeargs, JCExpression clazz, List<JCExpression> args, JCClassDecl def)
            {
                this.encl = encl;
                this.typeargs = (typeargs == null) ? new List<JCExpression>() : typeargs; //List.nil()
                this.clazz = clazz;
                this.args = args;
                this.def = def;
            }
            public override void accept(Visitor v)
            {
                v.visitNewClass(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.NEW_CLASS;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getEnclosingExpression()
            public virtual ExpressionTree getEnclosingExpression()
            { // expr.new C< ... > ( ... )
                return encl;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getTypeArguments()
            public virtual IList<Tree> getTypeArguments()
            {
                return new List<Tree>(typeargs);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getIdentifier()
            public virtual ExpressionTree getIdentifier()
            {
                return clazz;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getArguments()
            public virtual IList<ExpressionTree> getArguments()
            {
                return new List<ExpressionTree>(args);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCClassDecl getClassBody()
            public virtual ClassTree getClassBody()
            {
                return def;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitNewClass(this, d);
            }
            public override Tag getTag()
            {
                return NEWCLASS;
            }

            public virtual bool classDeclRemoved()
            {
                return false;
            }
        }

        /// <summary>
        /// A new[...] operation.
        /// </summary>
        public class JCNewArray : JCExpression, NewArrayTree
        {
            public JCExpression elemtype;
            public List<JCExpression> dims;
            // type annotations on inner-most component
            public List<JCAnnotation> annotations;
            // type annotations on dimensions
            public IList<IList<JCAnnotation>> dimAnnotations;
            public List<JCExpression> elems;
            protected internal JCNewArray(JCExpression elemtype, List<JCExpression> dims, List<JCExpression> elems)
            {
                this.elemtype = elemtype;
                this.dims = dims;
                this.annotations = new List<JCAnnotation>(); //List.nil();
                this.dimAnnotations = new List<IList<JCAnnotation>>();//List.nil();
                this.elems = elems;
            }
            public override void accept(Visitor v)
            {
                v.visitNewArray(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.NEW_ARRAY;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getType()
            public virtual Tree getType()
            {
                return elemtype;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getDimensions()
            public virtual IList<ExpressionTree> getDimensions()
            {
                return new List<ExpressionTree>(dims);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getInitializers()
            public virtual IList<ExpressionTree> getInitializers()
            {
                return new List<ExpressionTree>(elems);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitNewArray(this, d);
            }
            public override Tag getTag()
            {
                return NEWARRAY;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCAnnotation> getAnnotations()
            public virtual IList<AnnotationTree> getAnnotations()
            {
                return new List<AnnotationTree>(annotations);
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<com.sun.tools.javac.util.List<JCAnnotation>> getDimAnnotations()
            public virtual IList<IList<AnnotationTree>> getDimAnnotations()
            {
                List<IList<AnnotationTree>> ret = new List<IList<AnnotationTree>>();
                foreach (var dimAnno in dimAnnotations)
                {
                    ret.Add(new List<AnnotationTree>(dimAnno));
                }
                return ret;
            }
        }

        /// <summary>
        /// A lambda expression.
        /// </summary>
        public class JCLambda : JCFunctionalExpression, LambdaExpressionTree
        {

            public enum ParameterKind
            {
                IMPLICIT,
                EXPLICIT
            }

            public List<JCVariableDecl> @params;
            public JCTree body;
            public bool canCompleteNormally = true;
            public ParameterKind paramKind;

            public JCLambda(List<JCVariableDecl> @params, JCTree body)
            {
                this.@params = @params;
                this.body = body;
                if (@params.isEmpty() || @params[0].vartype != null)
                {
                    paramKind = ParameterKind.EXPLICIT;
                }
                else
                {
                    paramKind = ParameterKind.IMPLICIT;
                }
            }
            public override Tag getTag()
            {
                return LAMBDA;
            }
            public override void accept(Visitor v)
            {
                v.visitLambda(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R, D> R accept(TreeVisitor<R, D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitLambdaExpression(this, d);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.LAMBDA_EXPRESSION;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getBody()
            public virtual Tree getBody()
            {
                return body;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public java.util.List<? extends VariableTree> getParameters()
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
            public virtual IList<VariableTree> getParameters()
            {
                return new List<VariableTree>(@params);
            }
            public override JCTree setType(Type type)
            {
                base.setType(type);
                return this;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public BodyKind getBodyKind()
            public virtual BodyKind getBodyKind()
            {
                return body.hasTag(BLOCK) ? BodyKind.STATEMENT : BodyKind.EXPRESSION;
            }
        }

        /// <summary>
        /// A parenthesized subexpression ( ... )
        /// </summary>
        public class JCParens : JCExpression, ParenthesizedTree
        {
            public JCExpression expr;
            protected internal JCParens(JCExpression expr)
            {
                this.expr = expr;
            }
            public override void accept(Visitor v)
            {
                v.visitParens(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.PARENTHESIZED;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return expr;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitParenthesized(this, d);
            }
            public override Tag getTag()
            {
                return PARENS;
            }
        }

        /// <summary>
        /// A assignment with "=".
        /// </summary>
        public class JCAssign : JCExpression, AssignmentTree
        {
            public JCExpression lhs;
            public JCExpression rhs;
            protected internal JCAssign(JCExpression lhs, JCExpression rhs)
            {
                this.lhs = lhs;
                this.rhs = rhs;
            }
            public override void accept(Visitor v)
            {
                v.visitAssign(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.ASSIGNMENT;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getVariable()
            public virtual ExpressionTree getVariable()
            {
                return lhs;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return rhs;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitAssignment(this, d);
            }
            public override Tag getTag()
            {
                return ASSIGN;
            }
        }

        public abstract class JCOperatorExpression : JCExpression
        {
            public enum OperandPos
            {
                LEFT,
                RIGHT
            }

            protected internal Tag opcode;
            public OperatorSymbol @operator;

            public virtual OperatorSymbol getOperator()
            {
                return @operator;
            }

            public override Tag getTag()
            {
                return opcode;
            }

            public abstract JCExpression getOperand(OperandPos pos);
        }

        /// <summary>
        /// An assignment with "+=", "|=" ...
        /// </summary>
        public class JCAssignOp : JCOperatorExpression, CompoundAssignmentTree
        {
            public JCExpression lhs;
            public JCExpression rhs;
            protected internal JCAssignOp(Tag opcode, JCTree lhs, JCTree rhs, OperatorSymbol @operator)
            {
                this.opcode = opcode;
                this.lhs = (JCExpression)lhs;
                this.rhs = (JCExpression)rhs;
                this.@operator = @operator;
            }
            public override void accept(Visitor v)
            {
                v.visitAssignop(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return TreeInfo.tagToKind(getTag());
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getVariable()
            public virtual ExpressionTree getVariable()
            {
                return lhs;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return rhs;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitCompoundAssignment(this, d);
            }
            public override JCExpression getOperand(OperandPos pos)
            {
                return pos == OperandPos.LEFT ? lhs : rhs;
            }
        }

        /// <summary>
        /// A unary operation.
        /// </summary>
        public class JCUnary : JCOperatorExpression, UnaryTree
        {
            public JCExpression arg;
            protected internal JCUnary(Tag opcode, JCExpression arg)
            {
                this.opcode = opcode;
                this.arg = arg;
            }
            public override void accept(Visitor v)
            {
                v.visitUnary(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return TreeInfo.tagToKind(getTag());
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return arg;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitUnary(this, d);
            }
            public virtual void setTag(Tag tag)
            {
                opcode = tag;
            }
            public override JCExpression getOperand(OperandPos pos)
            {
                return arg;
            }
        }

        /// <summary>
        /// A binary operation.
        /// </summary>
        public class JCBinary : JCOperatorExpression, BinaryTree
        {
            public JCExpression lhs;
            public JCExpression rhs;
            protected internal JCBinary(Tag opcode, JCExpression lhs, JCExpression rhs, OperatorSymbol @operator)
            {
                this.opcode = opcode;
                this.lhs = lhs;
                this.rhs = rhs;
                this.@operator = @operator;
            }
            public override void accept(Visitor v)
            {
                v.visitBinary(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return TreeInfo.tagToKind(getTag());
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getLeftOperand()
            public virtual ExpressionTree getLeftOperand()
            {
                return lhs;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getRightOperand()
            public virtual ExpressionTree getRightOperand()
            {
                return rhs;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitBinary(this, d);
            }
            public override JCExpression getOperand(OperandPos pos)
            {
                return pos == OperandPos.LEFT ? lhs : rhs;
            }
        }

        /// <summary>
        /// A type cast.
        /// </summary>
        public class JCTypeCast : JCExpression, TypeCastTree
        {
            public JCTree clazz;
            public JCExpression expr;
            protected internal JCTypeCast(JCTree clazz, JCExpression expr)
            {
                this.clazz = clazz;
                this.expr = expr;
            }
            public override void accept(Visitor v)
            {
                v.visitTypeCast(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.TYPE_CAST;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getType()
            public virtual Tree getType()
            {
                return clazz;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return expr;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitTypeCast(this, d);
            }
            public override Tag getTag()
            {
                return TYPECAST;
            }
        }

        /// <summary>
        /// A type test.
        /// </summary>
        public class JCInstanceOf : JCExpression, InstanceOfTree
        {
            public JCExpression expr;
            public JCTree pattern;
            protected internal JCInstanceOf(JCExpression expr, JCTree pattern)
            {
                this.expr = expr;
                this.pattern = pattern;
            }
            public override void accept(Visitor v)
            {
                v.visitTypeTest(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.INSTANCE_OF;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getType()
            public virtual Tree getType()
            {
                return pattern is JCPattern ? pattern.hasTag(BINDINGPATTERN) ? ((JCBindingPattern)pattern).@var.vartype : null : pattern;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCPattern getPattern()
            public virtual PatternTree getPattern()
            {
                return pattern is JCPattern ? (JCPattern)pattern : null;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return expr;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitInstanceOf(this, d);
            }
            public override Tag getTag()
            {
                return TYPETEST;
            }
        }

        /// <summary>
        /// Pattern matching forms.
        /// </summary>
        public abstract class JCPattern : JCTree, PatternTree
        {
        }

        public class JCBindingPattern : JCPattern, BindingPatternTree
        {
            public JCVariableDecl @var;

            protected internal JCBindingPattern(JCVariableDecl @var)
            {
                this.@var = @var;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public VariableTree getVariable()
            public virtual VariableTree getVariable()
            {
                return @var;
            }

            public override void accept(Visitor v)
            {
                v.visitBindingPattern(this);
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.BINDING_PATTERN;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R, D> R accept(TreeVisitor<R, D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitBindingPattern(this, d);
            }

            public override Tag getTag()
            {
                return BINDINGPATTERN;
            }
        }

        /// <summary>
        /// An array selection
        /// </summary>
        public class JCArrayAccess : JCExpression, ArrayAccessTree
        {
            public JCExpression indexed;
            public JCExpression index;
            protected internal JCArrayAccess(JCExpression indexed, JCExpression index)
            {
                this.indexed = indexed;
                this.index = index;
            }
            public override void accept(Visitor v)
            {
                v.visitIndexed(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.ARRAY_ACCESS;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return indexed;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getIndex()
            public virtual ExpressionTree getIndex()
            {
                return index;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitArrayAccess(this, d);
            }
            public override Tag getTag()
            {
                return INDEXED;
            }
        }

        /// <summary>
        /// Selects through packages and classes
        /// </summary>
        public class JCFieldAccess : JCExpression, MemberSelectTree
        {
            /// <summary>
            /// selected Tree hierarchy </summary>
            public JCExpression selected;
            /// <summary>
            /// name of field to select thru </summary>
            public Name name;
            /// <summary>
            /// symbol of the selected class </summary>
            public Symbol sym;
            protected internal JCFieldAccess(JCExpression selected, Name name, Symbol sym)
            {
                this.selected = selected;
                this.name = name;
                this.sym = sym;
            }
            public override void accept(Visitor v)
            {
                v.visitSelect(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.MEMBER_SELECT;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getExpression()
            public virtual ExpressionTree getExpression()
            {
                return selected;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitMemberSelect(this, d);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Name getIdentifier()
            public virtual java.lang.common.api.Name getIdentifier()
            {
                return name;
            }
            public override Tag getTag()
            {
                return SELECT;
            }
        }

        /// <summary>
        /// Selects a member expression.
        /// </summary>
        public class JCMemberReference : JCFunctionalExpression, MemberReferenceTree
        {
            public ReferenceMode mode;
            public ReferenceKind kind;
            public Name name;
            public JCExpression expr;
            public List<JCExpression> typeargs;
            public Symbol sym;
            public Type varargsElement;
            public PolyKind refPolyKind;
            public bool ownerAccessible;
            internal OverloadKind overloadKind;
            public Type referentType;

            public enum OverloadKind
            {
                OVERLOADED,
                UNOVERLOADED,
                ERROR
            }

            /// <summary>
            /// Javac-dependent classification for member references, based
            /// on relevant properties w.r.t. code-generation
            /// </summary>
            public sealed class ReferenceKind
            {
                /// <summary>
                /// super # instMethod </summary>
                public static readonly ReferenceKind SUPER = new ReferenceKind("SUPER", InnerEnum.SUPER, ReferenceMode.INVOKE, false);
                /// <summary>
                /// Type # instMethod </summary>
                public static readonly ReferenceKind UNBOUND = new ReferenceKind("UNBOUND", InnerEnum.UNBOUND, ReferenceMode.INVOKE, true);
                /// <summary>
                /// Type # staticMethod </summary>
                public static readonly ReferenceKind STATIC = new ReferenceKind("STATIC", InnerEnum.STATIC, ReferenceMode.INVOKE, false);
                /// <summary>
                /// Expr # instMethod </summary>
                public static readonly ReferenceKind BOUND = new ReferenceKind("BOUND", InnerEnum.BOUND, ReferenceMode.INVOKE, false);
                /// <summary>
                /// Inner # new </summary>
                public static readonly ReferenceKind IMPLICIT_INNER = new ReferenceKind("IMPLICIT_INNER", InnerEnum.IMPLICIT_INNER, ReferenceMode.NEW, false);
                /// <summary>
                /// Toplevel # new </summary>
                public static readonly ReferenceKind TOPLEVEL = new ReferenceKind("TOPLEVEL", InnerEnum.TOPLEVEL, ReferenceMode.NEW, false);
                /// <summary>
                /// ArrayType # new </summary>
                public static readonly ReferenceKind ARRAY_CTOR = new ReferenceKind("ARRAY_CTOR", InnerEnum.ARRAY_CTOR, ReferenceMode.NEW, false);

                private static readonly IList<ReferenceKind> valueList = new List<ReferenceKind>();

                static ReferenceKind()
                {
                    valueList.Add(SUPER);
                    valueList.Add(UNBOUND);
                    valueList.Add(STATIC);
                    valueList.Add(BOUND);
                    valueList.Add(IMPLICIT_INNER);
                    valueList.Add(TOPLEVEL);
                    valueList.Add(ARRAY_CTOR);
                }

                public enum InnerEnum
                {
                    SUPER,
                    UNBOUND,
                    STATIC,
                    BOUND,
                    IMPLICIT_INNER,
                    TOPLEVEL,
                    ARRAY_CTOR
                }

                public readonly InnerEnum innerEnumValue;
                private readonly string nameValue;
                private readonly int ordinalValue;
                private static int nextOrdinal = 0;

                internal readonly ReferenceMode mode;
                internal readonly bool unbound;

                internal ReferenceKind(string name, InnerEnum innerEnum, ReferenceMode mode, bool unbound)
                {
                    this.mode = mode;
                    this.unbound = unbound;

                    nameValue = name;
                    ordinalValue = nextOrdinal++;
                    innerEnumValue = innerEnum;
                }

                public bool isUnbound()
                {
                    return unbound;
                }

                public static IList<ReferenceKind> values()
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

                public static ReferenceKind valueOf(string name)
                {
                    foreach (ReferenceKind enumInstance in ReferenceKind.valueList)
                    {
                        if (enumInstance.nameValue == name)
                        {
                            return enumInstance;
                        }
                    }
                    throw new System.ArgumentException(name);
                }
            }

            public JCMemberReference(ReferenceMode mode, Name name, JCExpression expr, List<JCExpression> typeargs)
            {
                this.mode = mode;
                this.name = name;
                this.expr = expr;
                this.typeargs = typeargs;
            }
            public override void accept(Visitor v)
            {
                v.visitReference(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.MEMBER_REFERENCE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public ReferenceMode getMode()
            public virtual ReferenceMode getMode()
            {
                return mode;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getQualifierExpression()
            public virtual ExpressionTree getQualifierExpression()
            {
                return expr;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Name getName()
            public virtual java.lang.common.api.Name getName()
            {
                return name;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getTypeArguments()
            public virtual IList<ExpressionTree> getTypeArguments()
            {
                return new List<ExpressionTree>(typeargs);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitMemberReference(this, d);
            }
            public override Tag getTag()
            {
                return REFERENCE;
            }
            public virtual bool hasKind(ReferenceKind kind)
            {
                return this.kind == kind;
            }

            /// <returns> the overloadKind </returns>
            public virtual OverloadKind getOverloadKind()
            {
                return overloadKind;
            }

            /// <param name="overloadKind"> the overloadKind to set </param>
            public virtual void setOverloadKind(OverloadKind overloadKind)
            {
                this.overloadKind = overloadKind;
            }
        }

        /// <summary>
        /// An identifier
        /// </summary>
        public class JCIdent : JCExpression, IdentifierTree
        {
            /// <summary>
            /// the name </summary>
            public Name name;
            /// <summary>
            /// the symbol </summary>
            public Symbol sym;
            protected internal JCIdent(Name name, Symbol sym)
            {
                this.name = name;
                this.sym = sym;
            }
            public override void accept(Visitor v)
            {
                v.visitIdent(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.IDENTIFIER;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Name getName()
            public virtual java.lang.common.api.Name getName()
            {
                return name;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitIdentifier(this, d);
            }
            public override Tag getTag()
            {
                return IDENT;
            }
        }

        /// <summary>
        /// A constant value given literally.
        /// </summary>
        public class JCLiteral : JCExpression, LiteralTree
        {
            public TypeTag typetag;
            /// <summary>
            /// value representation </summary>
            public object value;
            protected internal JCLiteral(TypeTag typetag, object value)
            {
                this.typetag = typetag;
                this.value = value;
            }
            public override void accept(Visitor v)
            {
                v.visitLiteral(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return typetag.getKindLiteral();
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Object getValue()
            public virtual object getValue()
            {
                switch (typetag.innerEnumValue)
                {
                    case TypeTag.InnerEnum.BOOLEAN:
                        int bi = (int)value;
                        return (bi != 0);
                    case TypeTag.InnerEnum.CHAR:
                        int ci = (int)value;
                        char c = (char)ci;
                        if (c != (char)ci)
                        {
                            throw new AssertionError("bad value for char literal");
                        }
                        return c;
                    default:
                        return value;
                }
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitLiteral(this, d);
            }
            public override JCTree setType(Type type)
            {
                base.setType(type);
                return this;
            }
            public override Tag getTag()
            {
                return LITERAL;
            }
        }

        /// <summary>
        /// Identifies a basic type. </summary>
        /// <seealso cref= TypeTag </seealso>
        public class JCPrimitiveTypeTree : JCExpression, PrimitiveTypeTree
        {
            /// <summary>
            /// the basic type id </summary>
            public TypeTag typetag;
            protected internal JCPrimitiveTypeTree(TypeTag typetag)
            {
                this.typetag = typetag;
            }
            public override void accept(Visitor v)
            {
                v.visitTypeIdent(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.PRIMITIVE_TYPE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public javax.lang.model.type.TypeKind getPrimitiveTypeKind()
            public virtual TypeKind getPrimitiveTypeKind()
            {
                return typetag.getPrimitiveTypeKind();
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitPrimitiveType(this, d);
            }
            public override Tag getTag()
            {
                return TYPEIDENT;
            }
        }

        /// <summary>
        /// An array type, A[]
        /// </summary>
        public class JCArrayTypeTree : JCExpression, ArrayTypeTree
        {
            public JCExpression elemtype;
            protected internal JCArrayTypeTree(JCExpression elemtype)
            {
                this.elemtype = elemtype;
            }
            public override void accept(Visitor v)
            {
                v.visitTypeArray(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.ARRAY_TYPE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getType()
            public virtual Tree getType()
            {
                return elemtype;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitArrayType(this, d);
            }
            public override Tag getTag()
            {
                return TYPEARRAY;
            }
        }

        /// <summary>
        /// A parameterized type, {@literal T<...>}
        /// </summary>
        public class JCTypeApply : JCExpression, ParameterizedTypeTree
        {
            public JCExpression clazz;
            public List<JCExpression> arguments;
            protected internal JCTypeApply(JCExpression clazz, List<JCExpression> arguments)
            {
                this.clazz = clazz;
                this.arguments = arguments;
            }
            public override void accept(Visitor v)
            {
                v.visitTypeApply(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.PARAMETERIZED_TYPE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getType()
            public virtual Tree getType()
            {
                return clazz;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getTypeArguments()
            public virtual IList<Tree> getTypeArguments()
            {
                return new List<Tree>(arguments);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitParameterizedType(this, d);
            }
            public override Tag getTag()
            {
                return TYPEAPPLY;
            }
        }

        /// <summary>
        /// A union type, T1 | T2 | ... Tn (used in multicatch statements)
        /// </summary>
        public class JCTypeUnion : JCExpression, UnionTypeTree
        {

            public List<JCExpression> alternatives;

            protected internal JCTypeUnion(List<JCExpression> components)
            {
                this.alternatives = components;
            }
            public override void accept(Visitor v)
            {
                v.visitTypeUnion(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.UNION_TYPE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getTypeAlternatives()
            public virtual IList<Tree> getTypeAlternatives()
            {
                return new List<Tree>(alternatives);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitUnionType(this, d);
            }
            public override Tag getTag()
            {
                return TYPEUNION;
            }
        }

        /// <summary>
        /// An intersection type, {@code T1 & T2 & ... Tn} (used in cast expressions)
        /// </summary>
        public class JCTypeIntersection : JCExpression, IntersectionTypeTree
        {

            public List<JCExpression> bounds;

            protected internal JCTypeIntersection(List<JCExpression> bounds)
            {
                this.bounds = bounds;
            }
            public override void accept(Visitor v)
            {
                v.visitTypeIntersection(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.INTERSECTION_TYPE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getBounds()
            public virtual IList<Tree> getBounds()
            {
                return new List<Tree>(bounds);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitIntersectionType(this, d);
            }
            public override Tag getTag()
            {
                return TYPEINTERSECTION;
            }
        }

        /// <summary>
        /// A formal class parameter.
        /// </summary>
        public class JCTypeParameter : JCTree, TypeParameterTree
        {
            /// <summary>
            /// name </summary>
            public Name name;
            /// <summary>
            /// bounds </summary>
            public List<JCExpression> bounds;
            /// <summary>
            /// type annotations on type parameter </summary>
            public List<JCAnnotation> annotations;
            protected internal JCTypeParameter(Name name, List<JCExpression> bounds, List<JCAnnotation> annotations)
            {
                this.name = name;
                this.bounds = bounds;
                this.annotations = annotations;
            }
            public override void accept(Visitor v)
            {
                v.visitTypeParameter(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.TYPE_PARAMETER;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Name getName()
            public virtual java.lang.common.api.Name getName()
            {
                return name;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getBounds()
            public virtual IList<Tree> getBounds()
            {
                return new List<Tree>(bounds);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCAnnotation> getAnnotations()
            public virtual IList<AnnotationTree> getAnnotations()
            {
                return new List<AnnotationTree>(annotations);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitTypeParameter(this, d);
            }
            public override Tag getTag()
            {
                return TYPEPARAMETER;
            }
        }

        public class JCWildcard : JCExpression, WildcardTree
        {
            public TypeBoundKind kind;
            public JCTree inner;
            protected internal JCWildcard(TypeBoundKind kind, JCTree inner)
            {
                this.kind = Assert.checkNonNull(kind);
                this.inner = inner;
            }
            public override void accept(Visitor v)
            {
                v.visitWildcard(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                switch (kind.kind.innerEnumValue)
                {
                    case BoundKind.InnerEnum.UNBOUND:
                        return Kind.UNBOUNDED_WILDCARD;
                    case BoundKind.InnerEnum.EXTENDS:
                        return Kind.EXTENDS_WILDCARD;
                    case BoundKind.InnerEnum.SUPER:
                        return Kind.SUPER_WILDCARD;
                    default:
                        throw new AssertionError("Unknown wildcard bound " + kind);
                }
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getBound()
            public virtual Tree getBound()
            {
                return inner;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitWildcard(this, d);
            }
            public override Tag getTag()
            {
                return Tag.WILDCARD;
            }
        }

        public class TypeBoundKind : JCTree
        {
            public BoundKind kind;
            protected internal TypeBoundKind(BoundKind kind)
            {
                this.kind = kind;
            }
            public override void accept(Visitor v)
            {
                v.visitTypeBoundKind(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                throw new AssertionError("TypeBoundKind is not part of a public API");
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                throw new AssertionError("TypeBoundKind is not part of a public API");
            }
            public override Tag getTag()
            {
                return TYPEBOUNDKIND;
            }
        }

        public class JCAnnotation : JCExpression, AnnotationTree
        {
            // Either Tag.ANNOTATION or Tag.TYPE_ANNOTATION
            internal Tag tag;

            public JCTree annotationType;
            public List<JCExpression> args;
            // public Attribute.Compound attribute;

            protected internal JCAnnotation(Tag tag, JCTree annotationType, List<JCExpression> args)
            {
                this.tag = tag;
                this.annotationType = annotationType;
                this.args = args;
            }

            public override void accept(Visitor v)
            {
                v.visitAnnotation(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return TreeInfo.tagToKind(getTag());
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCTree getAnnotationType()
            public virtual Tree getAnnotationType()
            {
                return annotationType;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getArguments()
            public virtual IList<ExpressionTree> getArguments()
            {
                return new List<ExpressionTree>(args);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitAnnotation(this, d);
            }
            public override Tag getTag()
            {
                return tag;
            }
        }

        public class JCModifiers : JCTree, ModifiersTree
        {
            public long flags;
            public List<JCAnnotation> annotations;
            protected internal JCModifiers(long flags, List<JCAnnotation> annotations)
            {
                this.flags = flags;
                this.annotations = annotations;
            }
            public override void accept(Visitor v)
            {
                v.visitModifiers(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.MODIFIERS;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Set<javax.lang.model.element.Modifier> getFlags()
            public virtual ISet<Modifier> getFlags()
            {
                return Flags.asModifierSet(flags);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCAnnotation> getAnnotations()
            public virtual IList<AnnotationTree> getAnnotations()
            {
                return new List<AnnotationTree>(annotations);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitModifiers(this, d);
            }
            public override Tag getTag()
            {
                return MODIFIERS;
            }
        }

        public class JCAnnotatedType : JCExpression, AnnotatedTypeTree
        {
            // type annotations
            public List<JCAnnotation> annotations;
            public JCExpression underlyingType;

            protected internal JCAnnotatedType(List<JCAnnotation> annotations, JCExpression underlyingType)
            {
                Assert.check(annotations != null && annotations.nonEmpty());
                this.annotations = annotations;
                this.underlyingType = underlyingType;
            }
            public override void accept(Visitor v)
            {
                v.visitAnnotatedType(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.ANNOTATED_TYPE;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCAnnotation> getAnnotations()
            public virtual IList<AnnotationTree> getAnnotations()
            {
                return new List<AnnotationTree>(annotations);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getUnderlyingType()
            public virtual ExpressionTree getUnderlyingType()
            {
                return underlyingType;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitAnnotatedType(this, d);
            }
            public override Tag getTag()
            {
                return ANNOTATED_TYPE;
            }
        }

        public abstract class JCDirective : JCTree, DirectiveTree
        {
        }

        public class JCModuleDecl : JCTree, ModuleTree
        {
            public JCModifiers mods;
            // public ModuleType type;
            internal readonly ModuleKind kind;
            public JCExpression qualId;
            public List<JCDirective> directives;
            // public ModuleSymbol sym;

            protected internal JCModuleDecl(JCModifiers mods, ModuleKind kind, JCExpression qualId, List<JCDirective> directives)
            {
                this.mods = mods;
                this.kind = kind;
                this.qualId = qualId;
                this.directives = directives;
            }

            public override void accept(Visitor v)
            {
                v.visitModuleDef(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.MODULE;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<? extends AnnotationTree> getAnnotations()
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
            public virtual IList<AnnotationTree> getAnnotations()
            {
                return new List<AnnotationTree>(mods.annotations);
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.source.tree.ModuleTree.ModuleKind getModuleType()
            public virtual ModuleKind getModuleType()
            {
                return kind;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getName()
            public virtual ExpressionTree getName()
            {
                return qualId;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCDirective> getDirectives()
            public virtual IList<DirectiveTree> getDirectives()
            {
                return new List<DirectiveTree>(directives);
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R, D> R accept(TreeVisitor<R, D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitModule(this, d);
            }

            public override Tag getTag()
            {
                return MODULEDEF;
            }
        }

        public class JCExports : JCDirective, ExportsTree
        {
            public JCExpression qualid;
            public List<JCExpression> moduleNames;
            // public ExportsDirective directive;

            protected internal JCExports(JCExpression qualId, List<JCExpression> moduleNames)
            {
                this.qualid = qualId;
                this.moduleNames = moduleNames;
            }

            public override void accept(Visitor v)
            {
                v.visitExports(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.EXPORTS;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getPackageName()
            public virtual ExpressionTree getPackageName()
            {
                return qualid;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getModuleNames()
            public virtual IList<ExpressionTree> getModuleNames()
            {
                return new List<ExpressionTree>(moduleNames);
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R, D> R accept(TreeVisitor<R, D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitExports(this, d);
            }

            public override Tag getTag()
            {
                return Tag.EXPORTS;
            }
        }

        public class JCOpens : JCDirective, OpensTree
        {
            public JCExpression qualid;
            public List<JCExpression> moduleNames;
            // public OpensDirective directive;

            protected internal JCOpens(JCExpression qualId, List<JCExpression> moduleNames)
            {
                this.qualid = qualId;
                this.moduleNames = moduleNames;
            }

            public override void accept(Visitor v)
            {
                v.visitOpens(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.OPENS;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getPackageName()
            public virtual ExpressionTree getPackageName()
            {
                return qualid;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getModuleNames()
            public virtual IList<ExpressionTree> getModuleNames()
            {
                return new List<ExpressionTree>(moduleNames);
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R, D> R accept(TreeVisitor<R, D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitOpens(this, d);
            }

            public override Tag getTag()
            {
                return Tag.OPENS;
            }
        }

        public class JCProvides : JCDirective, ProvidesTree
        {
            public JCExpression serviceName;
            public List<JCExpression> implNames;

            protected internal JCProvides(JCExpression serviceName, List<JCExpression> implNames)
            {
                this.serviceName = serviceName;
                this.implNames = implNames;
            }

            public override void accept(Visitor v)
            {
                v.visitProvides(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.PROVIDES;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R, D> R accept(TreeVisitor<R, D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitProvides(this, d);
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getServiceName()
            public virtual ExpressionTree getServiceName()
            {
                return serviceName;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<JCExpression> getImplementationNames()
            public virtual IList<ExpressionTree> getImplementationNames()
            {
                return new List<ExpressionTree>(implNames);
            }

            public override Tag getTag()
            {
                return PROVIDES;
            }
        }

        public class JCRequires : JCDirective, RequiresTree
        {
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER NOTE: Fields cannot have the same name as methods:
            public bool isTransitive_Renamed;
            public bool isStaticPhase;
            public JCExpression moduleName;
            // public RequiresDirective directive;

            protected internal JCRequires(bool isTransitive, bool isStaticPhase, JCExpression moduleName)
            {
                this.isTransitive_Renamed = isTransitive;
                this.isStaticPhase = isStaticPhase;
                this.moduleName = moduleName;
            }

            public override void accept(Visitor v)
            {
                v.visitRequires(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.REQUIRES;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R, D> R accept(TreeVisitor<R, D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitRequires(this, d);
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public boolean isTransitive()
            public virtual bool isTransitive()
            {
                return isTransitive_Renamed;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public boolean isStatic()
            public virtual bool isStatic()
            {
                return isStaticPhase;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getModuleName()
            public virtual ExpressionTree getModuleName()
            {
                return moduleName;
            }

            public override Tag getTag()
            {
                return REQUIRES;
            }
        }

        public class JCUses : JCDirective, UsesTree
        {
            public JCExpression qualid;

            protected internal JCUses(JCExpression qualId)
            {
                this.qualid = qualId;
            }

            public override void accept(Visitor v)
            {
                v.visitUses(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.USES;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public JCExpression getServiceName()
            public virtual ExpressionTree getServiceName()
            {
                return qualid;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R, D> R accept(TreeVisitor<R, D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitUses(this, d);
            }

            public override Tag getTag()
            {
                return USES;
            }
        }

        public class JCErroneous : JCExpression, ErroneousTree
        {
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
            //ORIGINAL LINE: public com.sun.tools.javac.util.List<? extends JCTree> errs;
            public List<JCTree> errs;
            protected internal JCErroneous(List<JCTree> errs)
            {
                this.errs = errs;
            }
            public override void accept(Visitor v)
            {
                v.visitErroneous(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                return Kind.ERRONEOUS;
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public com.sun.tools.javac.util.List<? extends JCTree> getErrorTrees()
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
            public virtual IList<Tree> getErrorTrees()
            {
                return new List<Tree>(errs);
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                return v.visitErroneous(this, d);
            }
            public override Tag getTag()
            {
                return ERRONEOUS;
            }
        }

        /// <summary>
        /// (let int x = 3; in x+2) </summary>
        public class LetExpr : JCExpression
        {
            public List<JCStatement> defs;
            public JCExpression expr;
            /// <summary>
            ///true if a expr should be run through Gen.genCond: </summary>
            public bool needsCond;
            protected internal LetExpr(List<JCStatement> defs, JCExpression expr)
            {
                this.defs = defs;
                this.expr = expr;
            }
            public override void accept(Visitor v)
            {
                v.visitLetExpr(this);
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public Kind getKind()
            public override Kind getKind()
            {
                throw new AssertionError("LetExpr is not part of a public API");
            }
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @Override @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public <R,D> R accept(TreeVisitor<R,D> v, D d)
            public override R accept<R, D>(TreeVisitor<R, D> v, D d)
            {
                throw new AssertionError("LetExpr is not part of a public API");
            }
            public override Tag getTag()
            {
                return LETEXPR;
            }
        }

        /// <summary>
        /// An interface for tree factories
        /// </summary>
        public interface Factory
        {
            JCCompilationUnit TopLevel(List<JCTree> defs);
            JCPackageDecl PackageDecl(List<JCAnnotation> annotations, JCExpression pid);
            JCImport Import(JCTree qualid, bool staticImport);
            JCClassDecl ClassDef(JCModifiers mods, Name name, List<JCTypeParameter> typarams, JCExpression extending, List<JCExpression> implementing, List<JCTree> defs);
            JCMethodDecl MethodDef(JCModifiers mods, Name name, JCExpression restype, List<JCTypeParameter> typarams, JCVariableDecl recvparam, List<JCVariableDecl> @params, List<JCExpression> thrown, JCBlock body, JCExpression defaultValue);
            JCVariableDecl VarDef(JCModifiers mods, Name name, JCExpression vartype, JCExpression init);
            JCSkip Skip();
            JCBlock Block(long flags, List<JCStatement> stats);
            JCDoWhileLoop DoLoop(JCStatement body, JCExpression cond);
            JCWhileLoop WhileLoop(JCExpression cond, JCStatement body);
            JCForLoop ForLoop(List<JCStatement> init, JCExpression cond, List<JCExpressionStatement> step, JCStatement body);
            JCEnhancedForLoop ForeachLoop(JCVariableDecl @var, JCExpression expr, JCStatement body);
            JCLabeledStatement Labelled(Name label, JCStatement body);
            JCSwitch Switch(JCExpression selector, List<JCCase> cases);
            JCSwitchExpression SwitchExpression(JCExpression selector, List<JCCase> cases);
            JCCase Case(CaseKind caseKind, List<JCExpression> pat, List<JCStatement> stats, JCTree body);
            JCSynchronized Synchronized(JCExpression @lock, JCBlock body);
            JCTry Try(JCBlock body, List<JCCatch> catchers, JCBlock finalizer);
            JCTry Try(List<JCTree> resources, JCBlock body, List<JCCatch> catchers, JCBlock finalizer);
            JCCatch Catch(JCVariableDecl param, JCBlock body);
            JCConditional Conditional(JCExpression cond, JCExpression thenpart, JCExpression elsepart);
            JCIf If(JCExpression cond, JCStatement thenpart, JCStatement elsepart);
            JCExpressionStatement Exec(JCExpression expr);
            JCBreak Break(Name label);
            JCYield Yield(JCExpression value);
            JCContinue Continue(Name label);
            JCReturn Return(JCExpression expr);
            JCThrow Throw(JCExpression expr);
            JCAssert Assert(JCExpression cond, JCExpression detail);
            JCMethodInvocation Apply(List<JCExpression> typeargs, JCExpression fn, List<JCExpression> args);
            JCNewClass NewClass(JCExpression encl, List<JCExpression> typeargs, JCExpression clazz, List<JCExpression> args, JCClassDecl def);
            JCNewArray NewArray(JCExpression elemtype, List<JCExpression> dims, List<JCExpression> elems);
            JCParens Parens(JCExpression expr);
            JCAssign Assign(JCExpression lhs, JCExpression rhs);
            JCAssignOp Assignop(Tag opcode, JCTree lhs, JCTree rhs);
            JCUnary Unary(Tag opcode, JCExpression arg);
            JCBinary Binary(Tag opcode, JCExpression lhs, JCExpression rhs);
            JCTypeCast TypeCast(JCTree expr, JCExpression type);
            JCInstanceOf TypeTest(JCExpression expr, JCTree clazz);
            JCBindingPattern BindingPattern(JCVariableDecl @var);
            JCArrayAccess Indexed(JCExpression indexed, JCExpression index);
            JCFieldAccess Select(JCExpression selected, Name selector);
            JCIdent Ident(Name idname);
            JCLiteral Literal(TypeTag tag, object value);
            JCPrimitiveTypeTree TypeIdent(TypeTag typetag);
            JCArrayTypeTree TypeArray(JCExpression elemtype);
            JCTypeApply TypeApply(JCExpression clazz, List<JCExpression> arguments);
            JCTypeParameter TypeParameter(Name name, List<JCExpression> bounds);
            JCWildcard Wildcard(TypeBoundKind kind, JCTree type);
            TypeBoundKind TypeBoundKind(BoundKind kind);
            JCAnnotation Annotation(JCTree annotationType, List<JCExpression> args);
            JCModifiers Modifiers(long flags, List<JCAnnotation> annotations);
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
            //ORIGINAL LINE: JCErroneous Erroneous(com.sun.tools.javac.util.List<? extends JCTree> errs);
            JCErroneous Erroneous<T1>(List<T1> errs) where T1 : JCTree;
            JCModuleDecl ModuleDef(JCModifiers mods, ModuleKind kind, JCExpression qualId, List<JCDirective> directives);
            JCExports Exports(JCExpression qualId, List<JCExpression> moduleNames);
            JCOpens Opens(JCExpression qualId, List<JCExpression> moduleNames);
            JCProvides Provides(JCExpression serviceName, List<JCExpression> implNames);
            JCRequires Requires(bool isTransitive, bool isStaticPhase, JCExpression qualId);
            JCUses Uses(JCExpression qualId);
            LetExpr LetExpr(List<JCStatement> defs, JCExpression expr);
        }

        /// <summary>
        /// A generic visitor class for trees.
        /// </summary>
        public abstract class Visitor
        {
            public virtual void visitTopLevel(JCCompilationUnit that)
            {
                visitTree(that);
            }
            public virtual void visitPackageDef(JCPackageDecl that)
            {
                visitTree(that);
            }
            public virtual void visitImport(JCImport that)
            {
                visitTree(that);
            }
            public virtual void visitClassDef(JCClassDecl that)
            {
                visitTree(that);
            }
            public virtual void visitMethodDef(JCMethodDecl that)
            {
                visitTree(that);
            }
            public virtual void visitVarDef(JCVariableDecl that)
            {
                visitTree(that);
            }
            public virtual void visitSkip(JCSkip that)
            {
                visitTree(that);
            }
            public virtual void visitBlock(JCBlock that)
            {
                visitTree(that);
            }
            public virtual void visitDoLoop(JCDoWhileLoop that)
            {
                visitTree(that);
            }
            public virtual void visitWhileLoop(JCWhileLoop that)
            {
                visitTree(that);
            }
            public virtual void visitForLoop(JCForLoop that)
            {
                visitTree(that);
            }
            public virtual void visitForeachLoop(JCEnhancedForLoop that)
            {
                visitTree(that);
            }
            public virtual void visitLabelled(JCLabeledStatement that)
            {
                visitTree(that);
            }
            public virtual void visitSwitch(JCSwitch that)
            {
                visitTree(that);
            }
            public virtual void visitCase(JCCase that)
            {
                visitTree(that);
            }
            public virtual void visitSwitchExpression(JCSwitchExpression that)
            {
                visitTree(that);
            }
            public virtual void visitSynchronized(JCSynchronized that)
            {
                visitTree(that);
            }
            public virtual void visitTry(JCTry that)
            {
                visitTree(that);
            }
            public virtual void visitCatch(JCCatch that)
            {
                visitTree(that);
            }
            public virtual void visitConditional(JCConditional that)
            {
                visitTree(that);
            }
            public virtual void visitIf(JCIf that)
            {
                visitTree(that);
            }
            public virtual void visitExec(JCExpressionStatement that)
            {
                visitTree(that);
            }
            public virtual void visitBreak(JCBreak that)
            {
                visitTree(that);
            }
            public virtual void visitYield(JCYield that)
            {
                visitTree(that);
            }
            public virtual void visitContinue(JCContinue that)
            {
                visitTree(that);
            }
            public virtual void visitReturn(JCReturn that)
            {
                visitTree(that);
            }
            public virtual void visitThrow(JCThrow that)
            {
                visitTree(that);
            }
            public virtual void visitAssert(JCAssert that)
            {
                visitTree(that);
            }
            public virtual void visitApply(JCMethodInvocation that)
            {
                visitTree(that);
            }
            public virtual void visitNewClass(JCNewClass that)
            {
                visitTree(that);
            }
            public virtual void visitNewArray(JCNewArray that)
            {
                visitTree(that);
            }
            public virtual void visitLambda(JCLambda that)
            {
                visitTree(that);
            }
            public virtual void visitParens(JCParens that)
            {
                visitTree(that);
            }
            public virtual void visitAssign(JCAssign that)
            {
                visitTree(that);
            }
            public virtual void visitAssignop(JCAssignOp that)
            {
                visitTree(that);
            }
            public virtual void visitUnary(JCUnary that)
            {
                visitTree(that);
            }
            public virtual void visitBinary(JCBinary that)
            {
                visitTree(that);
            }
            public virtual void visitTypeCast(JCTypeCast that)
            {
                visitTree(that);
            }
            public virtual void visitTypeTest(JCInstanceOf that)
            {
                visitTree(that);
            }
            public virtual void visitBindingPattern(JCBindingPattern that)
            {
                visitTree(that);
            }
            public virtual void visitIndexed(JCArrayAccess that)
            {
                visitTree(that);
            }
            public virtual void visitSelect(JCFieldAccess that)
            {
                visitTree(that);
            }
            public virtual void visitReference(JCMemberReference that)
            {
                visitTree(that);
            }
            public virtual void visitIdent(JCIdent that)
            {
                visitTree(that);
            }
            public virtual void visitLiteral(JCLiteral that)
            {
                visitTree(that);
            }
            public virtual void visitTypeIdent(JCPrimitiveTypeTree that)
            {
                visitTree(that);
            }
            public virtual void visitTypeArray(JCArrayTypeTree that)
            {
                visitTree(that);
            }
            public virtual void visitTypeApply(JCTypeApply that)
            {
                visitTree(that);
            }
            public virtual void visitTypeUnion(JCTypeUnion that)
            {
                visitTree(that);
            }
            public virtual void visitTypeIntersection(JCTypeIntersection that)
            {
                visitTree(that);
            }
            public virtual void visitTypeParameter(JCTypeParameter that)
            {
                visitTree(that);
            }
            public virtual void visitWildcard(JCWildcard that)
            {
                visitTree(that);
            }
            public virtual void visitTypeBoundKind(TypeBoundKind that)
            {
                visitTree(that);
            }
            public virtual void visitAnnotation(JCAnnotation that)
            {
                visitTree(that);
            }
            public virtual void visitModifiers(JCModifiers that)
            {
                visitTree(that);
            }
            public virtual void visitAnnotatedType(JCAnnotatedType that)
            {
                visitTree(that);
            }
            public virtual void visitErroneous(JCErroneous that)
            {
                visitTree(that);
            }
            public virtual void visitModuleDef(JCModuleDecl that)
            {
                visitTree(that);
            }
            public virtual void visitExports(JCExports that)
            {
                visitTree(that);
            }
            public virtual void visitOpens(JCOpens that)
            {
                visitTree(that);
            }
            public virtual void visitProvides(JCProvides that)
            {
                visitTree(that);
            }
            public virtual void visitRequires(JCRequires that)
            {
                visitTree(that);
            }
            public virtual void visitUses(JCUses that)
            {
                visitTree(that);
            }
            public virtual void visitLetExpr(LetExpr that)
            {
                visitTree(that);
            }

            public virtual void visitTree(JCTree that)
            {
                Assert.error();
            }
        }

    }
}
