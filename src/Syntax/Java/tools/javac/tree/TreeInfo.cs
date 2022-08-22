using System;
using System.Collections;
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
    using com.sun.tools.javac.util;
    using com.sun.source.tree;
    using static com.sun.tools.javac.tree.JCTree;
    using Name = com.sun.tools.javac.util.Name;
    using com.sun.tools.javac.code;

    //    using Tree = com.sun.source.tree.Tree;
    //    using TreePath = com.sun.source.util.TreePath;
    //    using com.sun.tools.javac.code;
    //    using AttrContext = com.sun.tools.javac.comp.AttrContext;
    //    using Env = com.sun.tools.javac.comp.Env;
    //    using com.sun.tools.javac.util;
    //    using DiagnosticPosition = com.sun.tools.javac.util.JCDiagnostic.DiagnosticPosition;

    ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    ////    import static com.sun.tools.javac.code.Flags.*;
    ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    ////    import static com.sun.tools.javac.code.Kinds.Kind.*;
    //    using VarSymbol = com.sun.tools.javac.code.Symbol.VarSymbol;
    ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    ////    import static com.sun.tools.javac.code.TypeTag.BOT;
    ////    import static com.sun2.tools.javac.tree.JCTree.Tag.*;
    using static com.sun.tools.javac.tree.JCTree.Tag;
    ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    ////    import static JCTree.Tag.BLOCK;
    ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    ////    import static JCTree.Tag.SYNCHRONIZED;


    ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    ////    import static JCTree.JCOperatorExpression.OperandPos.LEFT;
    ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    ////    import static JCTree.JCOperatorExpression.OperandPos.RIGHT;

    /// <summary>
    /// Utility class containing inspector methods for trees.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class TreeInfo
    {

        //        public static IList<JCTree.JCExpression> args(JCTree t)
        //        {
        //            switch (t.getTag())
        //            {
        //                case APPLY:
        //                    return ((JCTree.JCMethodInvocation)t).args;
        //                case NEWCLASS:
        //                    return ((JCTree.JCNewClass)t).args;
        //                default:
        //                    return null;
        //            }
        //        }

        //        /// <summary>
        //        /// Is tree a constructor declaration?
        //        /// </summary>
        //        public static bool isConstructor(JCTree tree)
        //        {
        //            if (tree.hasTag(METHODDEF))
        //            {
        //                Name name = ((JCTree.JCMethodDecl) tree).name;
        //                return name == name.table.names.init;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }

        //        public static bool isCanonicalConstructor(JCTree tree)
        //        {
        //            // the record flag is only set to the canonical constructor
        //            return isConstructor(tree) && (((JCTree.JCMethodDecl)tree).sym.flags_field & RECORD) != 0;
        //        }

        //        public static bool isCompactConstructor(JCTree tree)
        //        {
        //            // the record flag is only set to the canonical constructor
        //            return isCanonicalConstructor(tree) && (((JCTree.JCMethodDecl)tree).sym.flags_field & COMPACT_RECORD_CONSTRUCTOR) != 0;
        //        }

        //        public static bool isReceiverParam(JCTree tree)
        //        {
        //            if (tree.hasTag(VARDEF))
        //            {
        //                return ((JCTree.JCVariableDecl)tree).nameexpr != null;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }

        //        /// <summary>
        //        /// Is there a constructor declaration in the given list of trees?
        //        /// </summary>
        //        public static bool hasConstructors(IList<JCTree> trees)
        //        {
        //            for (IList<JCTree> l = trees; l.nonEmpty(); l = l.tail)
        //            {
        //                if (isConstructor(l.head))
        //                {
        //                    return true;
        //                }
        //            }
        //            return false;
        //        }

        //        /// <summary>
        //        /// Is there a constructor invocation in the given list of trees?
        //        /// </summary>
        //        public static Name getConstructorInvocationName<T1>(IList<T1> trees, Names names) where T1 : JCTree
        //        {
        //            foreach (JCTree tree in trees)
        //            {
        //                if (tree.hasTag(EXEC))
        //                {
        //                    JCTree.JCExpressionStatement stat = (JCTree.JCExpressionStatement)tree;
        //                    if (stat.expr.hasTag(APPLY))
        //                    {
        //                        JCTree.JCMethodInvocation apply = (JCTree.JCMethodInvocation)stat.expr;
        //                        Name methName = TreeInfo.name(apply.meth);
        //                        if (methName == names._this || methName == names._super)
        //                        {
        //                            return methName;
        //                        }
        //                    }
        //                }
        //            }
        //            return names.empty;
        //        }

        //        public static bool isMultiCatch(JCTree.JCCatch catchClause)
        //        {
        //            return catchClause.param.vartype.hasTag(TYPEUNION);
        //        }

        //        /// <summary>
        //        /// Is statement an initializer for a synthetic field?
        //        /// </summary>
        //        public static bool isSyntheticInit(JCTree stat)
        //        {
        //            if (stat.hasTag(EXEC))
        //            {
        //                JCTree.JCExpressionStatement exec = (JCTree.JCExpressionStatement)stat;
        //                if (exec.expr.hasTag(ASSIGN))
        //                {
        //                    JCTree.JCAssign assign = (JCTree.JCAssign)exec.expr;
        //                    if (assign.lhs.hasTag(SELECT))
        //                    {
        //                        JCTree.JCFieldAccess select = (JCTree.JCFieldAccess)assign.lhs;
        //                        if (select.sym != null && (select.sym.flags() & SYNTHETIC) != 0)
        //                        {
        //                            Name selected = name(select.selected);
        //                            if (selected != null && selected == selected.table.names._this)
        //                            {
        //                                return true;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            return false;
        //        }

        //        /// <summary>
        //        /// If the expression is a method call, return the method name, null
        //        ///  otherwise. 
        //        /// </summary>
        //        public static Name calledMethodName(JCTree tree)
        //        {
        //            if (tree.hasTag(EXEC))
        //            {
        //                JCTree.JCExpressionStatement exec = (JCTree.JCExpressionStatement)tree;
        //                if (exec.expr.hasTag(APPLY))
        //                {
        //                    Name mname = TreeInfo.name(((JCTree.JCMethodInvocation) exec.expr).meth);
        //                    return mname;
        //                }
        //            }
        //            return null;
        //        }

        //        /// <summary>
        //        /// Is this a call to this or super?
        //        /// </summary>
        //        public static bool isSelfCall(JCTree tree)
        //        {
        //            Name name = calledMethodName(tree);
        //            if (name != null)
        //            {
        //                Names names = name.table.names;
        //                return name == names._this || name == names._super;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }

        //        /// <summary>
        //        /// Is this tree a 'this' identifier?
        //        /// </summary>
        //        public static bool isThisQualifier(JCTree tree)
        //        {
        //            switch (tree.getTag())
        //            {
        //                case PARENS:
        //                    return isThisQualifier(skipParens(tree));
        //                case IDENT:
        //                {
        //                    JCTree.JCIdent id = (JCTree.JCIdent)tree;
        //                    return id.name == id.name.table.names._this;
        //                }
        //                default:
        //                    return false;
        //            }
        //        }

        //        /// <summary>
        //        /// Is this tree an identifier, possibly qualified by 'this'?
        //        /// </summary>
        //        public static bool isIdentOrThisDotIdent(JCTree tree)
        //        {
        //            switch (tree.getTag())
        //            {
        //                case PARENS:
        //                    return isIdentOrThisDotIdent(skipParens(tree));
        //                case IDENT:
        //                    return true;
        //                case SELECT:
        //                    return isThisQualifier(((JCTree.JCFieldAccess)tree).selected);
        //                default:
        //                    return false;
        //            }
        //        }

        //        /// <summary>
        //        /// Is this a call to super?
        //        /// </summary>
        //        public static bool isSuperCall(JCTree tree)
        //        {
        //            Name name = calledMethodName(tree);
        //            if (name != null)
        //            {
        //                Names names = name.table.names;
        //                return name == names._super;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }

        //        public static IList<JCTree.JCVariableDecl> recordFields(JCTree.JCClassDecl tree)
        //        {
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java stream collectors are not converted by JAVA to C# Converter Cracked By X-Cracker:
        //            return tree.defs.Where(t => t.hasTag(VARDEF)).Select(t => (JCTree.JCVariableDecl)t).Where(vd => (vd.getModifiers().flags & (Flags.RECORD)) == RECORD).collect(IList.collector());
        //        }

        //        public static IList<Type> recordFieldTypes(JCTree.JCClassDecl tree)
        //        {
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java stream collectors are not converted by JAVA to C# Converter Cracked By X-Cracker:
        //            return recordFields(tree).Select(vd => vd.type).collect(IList.collector());
        //        }

        //        /// <summary>
        //        /// Is this a constructor whose first (non-synthetic) statement is not
        //        ///  of the form this(...)?
        //        /// </summary>
        //        public static bool isInitialConstructor(JCTree tree)
        //        {
        //            JCTree.JCMethodInvocation app = firstConstructorCall(tree);
        //            if (app == null)
        //            {
        //                return false;
        //            }
        //            Name meth = name(app.meth);
        //            return meth == null || meth != meth.table.names._this;
        //        }

        //        /// <summary>
        //        /// Return the first call in a constructor definition. </summary>
        //        public static JCTree.JCMethodInvocation firstConstructorCall(JCTree tree)
        //        {
        //            if (!tree.hasTag(METHODDEF))
        //            {
        //                return null;
        //            }
        //            JCTree.JCMethodDecl md = (JCTree.JCMethodDecl) tree;
        //            Names names = md.name.table.names;
        //            if (md.name != names.init)
        //            {
        //                return null;
        //            }
        //            if (md.body == null)
        //            {
        //                return null;
        //            }
        //            IList<JCTree.JCStatement> stats = md.body.stats;
        //            // Synthetic initializations can appear before the super call.
        //            while (stats.nonEmpty() && isSyntheticInit(stats.head))
        //            {
        //                stats = stats.tail;
        //            }
        //            if (stats.Count == 0)
        //            {
        //                return null;
        //            }
        //            if (!stats.head.hasTag(EXEC))
        //            {
        //                return null;
        //            }
        //            JCTree.JCExpressionStatement exec = (JCTree.JCExpressionStatement) stats.head;
        //            if (!exec.expr.hasTag(APPLY))
        //            {
        //                return null;
        //            }
        //            return (JCTree.JCMethodInvocation)exec.expr;
        //        }

        //        /// <summary>
        //        /// Return true if a tree represents a diamond new expr. </summary>
        //        public static bool isDiamond(JCTree tree)
        //        {
        //            switch (tree.getTag())
        //            {
        //                case TYPEAPPLY:
        //                    return ((JCTree.JCTypeApply)tree).getTypeArguments().isEmpty();
        //                case NEWCLASS:
        //                    return isDiamond(((JCTree.JCNewClass)tree).clazz);
        //                case ANNOTATED_TYPE:
        //                    return isDiamond(((JCTree.JCAnnotatedType)tree).underlyingType);
        //                default:
        //                    return false;
        //            }
        //        }

        //        public static bool isEnumInit(JCTree tree)
        //        {
        //            switch (tree.getTag())
        //            {
        //                case VARDEF:
        //                    return (((JCTree.JCVariableDecl)tree).mods.flags & ENUM) != 0;
        //                default:
        //                    return false;
        //            }
        //        }

        //        /// <summary>
        //        /// set 'polyKind' on given tree </summary>
        //        public static void setPolyKind(JCTree tree, PolyKind pkind)
        //        {
        //            switch (tree.getTag())
        //            {
        //                case APPLY:
        //                    ((JCTree.JCMethodInvocation)tree).polyKind = pkind;
        //                    break;
        //                case NEWCLASS:
        //                    ((JCTree.JCNewClass)tree).polyKind = pkind;
        //                    break;
        //                case REFERENCE:
        //                    ((JCTree.JCMemberReference)tree).refPolyKind = pkind;
        //                    break;
        //                default:
        //                    throw new AssertionError("Unexpected tree: " + tree);
        //            }
        //        }

        //        /// <summary>
        //        /// set 'varargsElement' on given tree </summary>
        //        public static void setVarargsElement(JCTree tree, Type varargsElement)
        //        {
        //            switch (tree.getTag())
        //            {
        //                case APPLY:
        //                    ((JCTree.JCMethodInvocation)tree).varargsElement = varargsElement;
        //                    break;
        //                case NEWCLASS:
        //                    ((JCTree.JCNewClass)tree).varargsElement = varargsElement;
        //                    break;
        //                case REFERENCE:
        //                    ((JCTree.JCMemberReference)tree).varargsElement = varargsElement;
        //                    break;
        //                default:
        //                    throw new AssertionError("Unexpected tree: " + tree);
        //            }
        //        }

        //        /// <summary>
        //        /// Return true if the tree corresponds to an expression statement </summary>
        //        public static bool isExpressionStatement(JCTree.JCExpression tree)
        //        {
        //            switch (tree.getTag())
        //            {
        //                case PREINC:
        //            case PREDEC:
        //                case POSTINC:
        //            case POSTDEC:
        //                case ASSIGN:
        //                case BITOR_ASG:
        //            case BITXOR_ASG:
        //        case BITAND_ASG:
        //                case SL_ASG:
        //            case SR_ASG:
        //        case USR_ASG:
        //                case PLUS_ASG:
        //            case MINUS_ASG:
        //                case MUL_ASG:
        //            case DIV_ASG:
        //        case MOD_ASG:
        //                case APPLY:
        //            case NEWCLASS:
        //                case ERRONEOUS:
        //                    return true;
        //                default:
        //                    return false;
        //            }
        //        }

        //        /// <summary>
        //        /// Return true if the tree corresponds to a statement </summary>
        //        public static bool isStatement(JCTree tree)
        //        {
        //            return (tree is JCTree.JCStatement) && !tree.hasTag(CLASSDEF) && !tree.hasTag(Tag.BLOCK) && !tree.hasTag(METHODDEF);
        //        }

        //        /// <summary>
        //        /// Return true if the AST corresponds to a static select of the kind A.B
        //        /// </summary>
        //        public static bool isStaticSelector(JCTree @base, Names names)
        //        {
        //            if (@base == null)
        //            {
        //                return false;
        //            }
        //            switch (@base.getTag())
        //            {
        //                case IDENT:
        //                    JCTree.JCIdent id = (JCTree.JCIdent)@base;
        //                    return id.name != names._this && id.name != names._super && isStaticSym(@base);
        //                case SELECT:
        //                    return isStaticSym(@base) && isStaticSelector(((JCTree.JCFieldAccess)@base).selected, names);
        //                case TYPEAPPLY:
        //                case TYPEARRAY:
        //                    return true;
        //                case ANNOTATED_TYPE:
        //                    return isStaticSelector(((JCTree.JCAnnotatedType)@base).underlyingType, names);
        //                default:
        //                    return false;
        //            }
        //        }
        //        //where
        //            private static bool isStaticSym(JCTree tree)
        //            {
        //                Symbol sym = symbol(tree);
        //                return (sym.kind == TYP || sym.kind == PCK);
        //            }

        //        /// <summary>
        //        /// Return true if a tree represents the null literal. </summary>
        //        public static bool isNull(JCTree tree)
        //        {
        //            if (!tree.hasTag(LITERAL))
        //            {
        //                return false;
        //            }
        //            JCTree.JCLiteral lit = (JCTree.JCLiteral) tree;
        //            return (lit.typetag == BOT);
        //        }

        //        /// <summary>
        //        /// Return true iff this tree is a child of some annotation. </summary>
        //        public static bool isInAnnotation<T1>(Env<T1> env, JCTree tree)
        //        {
        //            TreePath tp = TreePath.getPath(env.toplevel, tree);
        //            if (tp != null)
        //            {
        //                foreach (Tree t in tp)
        //                {
        //                    if (t.getKind() == Kind.ANNOTATION)
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //            return false;
        //        }

        //        public static string getCommentText<T1>(Env<T1> env, JCTree tree)
        //        {
        //            DocCommentTable docComments = (tree.hasTag(TOPLEVEL)) ? ((JCTree.JCCompilationUnit) tree).docComments : env.toplevel.docComments;
        //            return (docComments == null) ? null : docComments.getCommentText(tree);
        //        }

        //        public static DCTree.DCDocComment getCommentTree<T1>(Env<T1> env, JCTree tree)
        //        {
        //            DocCommentTable docComments = (tree.hasTag(TOPLEVEL)) ? ((JCTree.JCCompilationUnit) tree).docComments : env.toplevel.docComments;
        //            return (docComments == null) ? null : docComments.getCommentTree(tree);
        //        }

        //        /// <summary>
        //        /// The position of the first statement in a block, or the position of
        //        ///  the block itself if it is empty.
        //        /// </summary>
        //        public static int firstStatPos(JCTree tree)
        //        {
        //            if (tree.hasTag(BLOCK) && ((JCTree.JCBlock) tree).stats.nonEmpty())
        //            {
        //                return ((JCTree.JCBlock) tree).stats.head.pos;
        //            }
        //            else
        //            {
        //                return tree.pos_Renamed;
        //            }
        //        }

        /// <summary>
        /// The end position of given tree, if it is a block with
        ///  defined endpos.
        /// </summary>
        public static int endPos(JCTree tree)
        {
            if (tree.hasTag(BLOCK) && ((JCTree.JCBlock)tree).endpos != Position.NOPOS)
            {
                return ((JCTree.JCBlock)tree).endpos;
            }
            else if (tree.hasTag(SYNCHRONIZED))
            {
                return endPos(((JCTree.JCSynchronized)tree).body);
            }
            else if (tree.hasTag(TRY))
            {
                JCTree.JCTry t = (JCTree.JCTry)tree;
                return endPos((t.finalizer != null) ? t.finalizer : (t.catchers.nonEmpty() ? t.catchers.last().body : t.body));
            }
            else if (tree.hasTag(SWITCH_EXPRESSION) && ((JCTree.JCSwitchExpression)tree).endpos != Position.NOPOS)
            {
                return ((JCTree.JCSwitchExpression)tree).endpos;
            }
            else
            {
                return tree.pos;
            }
        }


        /// <summary>
        /// Get the start position for a tree node.  The start position is
        /// defined to be the position of the first character of the first
        /// token of the node's source text. </summary>
        /// <param name="tree">  The tree node </param>
        public static int getStartPos(JCTree tree)
        {
            if (tree == null)
            {
                return Position.NOPOS;
            }

            switch (tree.getTag().innerEnumValue)
            {
                case JCTree.Tag.InnerEnum.MODULEDEF:
                    {
                        JCTree.JCModuleDecl md = (JCTree.JCModuleDecl)tree;
                        return md.mods.annotations.isEmpty() ? md.pos : md.mods.annotations[0].pos;
                    }
                case JCTree.Tag.InnerEnum.PACKAGEDEF:
                    {
                        JCTree.JCPackageDecl pd = (JCTree.JCPackageDecl)tree;
                        return pd.annotations.isEmpty() ? pd.pos : pd.annotations[0].pos;
                    }
                case JCTree.Tag.InnerEnum.APPLY:
                    return getStartPos(((JCTree.JCMethodInvocation)tree).meth);
                case JCTree.Tag.InnerEnum.ASSIGN:
                    return getStartPos(((JCTree.JCAssign)tree).lhs);
                case JCTree.Tag.InnerEnum.BITOR_ASG:
                case JCTree.Tag.InnerEnum.BITXOR_ASG:
                case JCTree.Tag.InnerEnum.BITAND_ASG:
                case JCTree.Tag.InnerEnum.SL_ASG:
                case JCTree.Tag.InnerEnum.SR_ASG:
                case JCTree.Tag.InnerEnum.USR_ASG:
                case JCTree.Tag.InnerEnum.PLUS_ASG:
                case JCTree.Tag.InnerEnum.MINUS_ASG:
                case JCTree.Tag.InnerEnum.MUL_ASG:
                case JCTree.Tag.InnerEnum.DIV_ASG:
                case JCTree.Tag.InnerEnum.MOD_ASG:
                case JCTree.Tag.InnerEnum.OR:
                case JCTree.Tag.InnerEnum.AND:
                case JCTree.Tag.InnerEnum.BITOR:
                case JCTree.Tag.InnerEnum.BITXOR:
                case JCTree.Tag.InnerEnum.BITAND:
                case JCTree.Tag.InnerEnum.EQ:
                case JCTree.Tag.InnerEnum.NE:
                case JCTree.Tag.InnerEnum.LT:
                case JCTree.Tag.InnerEnum.GT:
                case JCTree.Tag.InnerEnum.LE:
                case JCTree.Tag.InnerEnum.GE:
                case JCTree.Tag.InnerEnum.SL:
                case JCTree.Tag.InnerEnum.SR:
                case JCTree.Tag.InnerEnum.USR:
                case JCTree.Tag.InnerEnum.PLUS:
                case JCTree.Tag.InnerEnum.MINUS:
                case JCTree.Tag.InnerEnum.MUL:
                case JCTree.Tag.InnerEnum.DIV:
                case JCTree.Tag.InnerEnum.MOD:
                case JCTree.Tag.InnerEnum.POSTINC:
                case JCTree.Tag.InnerEnum.POSTDEC:
                    return getStartPos(((JCTree.JCOperatorExpression)tree).getOperand(JCTree.JCOperatorExpression.OperandPos.LEFT));
                case JCTree.Tag.InnerEnum.CLASSDEF:
                    {
                        JCTree.JCClassDecl node = (JCTree.JCClassDecl)tree;
                        if (node.mods.pos != Position.NOPOS)
                        {
                            return node.mods.pos;
                        }
                        break;
                    }
                case JCTree.Tag.InnerEnum.CONDEXPR:
                    return getStartPos(((JCTree.JCConditional)tree).cond);
                case JCTree.Tag.InnerEnum.EXEC:
                    return getStartPos(((JCTree.JCExpressionStatement)tree).expr);
                case JCTree.Tag.InnerEnum.INDEXED:
                    return getStartPos(((JCTree.JCArrayAccess)tree).indexed);
                case JCTree.Tag.InnerEnum.METHODDEF:
                    {
                        JCTree.JCMethodDecl node = (JCTree.JCMethodDecl)tree;
                        if (node.mods.pos != Position.NOPOS)
                        {
                            return node.mods.pos;
                        }
                        if (node.typarams.nonEmpty()) // List.nil() used for no typarams
                        {
                            return getStartPos(node.typarams[0]);
                        }
                        return node.restype == null ? node.pos : getStartPos(node.restype);
                    }
                case JCTree.Tag.InnerEnum.SELECT:
                    return getStartPos(((JCTree.JCFieldAccess)tree).selected);
                case JCTree.Tag.InnerEnum.TYPEAPPLY:
                    return getStartPos(((JCTree.JCTypeApply)tree).clazz);
                case JCTree.Tag.InnerEnum.TYPEARRAY:
                    return getStartPos(((JCTree.JCArrayTypeTree)tree).elemtype);
                case JCTree.Tag.InnerEnum.TYPETEST:
                    return getStartPos(((JCTree.JCInstanceOf)tree).expr);
                case JCTree.Tag.InnerEnum.ANNOTATED_TYPE:
                    {
                        JCTree.JCAnnotatedType node = (JCTree.JCAnnotatedType)tree;
                        if (node.annotations.nonEmpty())
                        {
                            if (node.underlyingType.hasTag(TYPEARRAY) || node.underlyingType.hasTag(SELECT))
                            {
                                return getStartPos(node.underlyingType);
                            }
                            else
                            {
                                return getStartPos(node.annotations[0]);
                            }
                        }
                        else
                        {
                            return getStartPos(node.underlyingType);
                        }
                    }
                case JCTree.Tag.InnerEnum.NEWCLASS:
                    {
                        JCTree.JCNewClass node = (JCTree.JCNewClass)tree;
                        if (node.encl != null)
                        {
                            return getStartPos(node.encl);
                        }
                        break;
                    }
                case JCTree.Tag.InnerEnum.VARDEF:
                    {
                        JCTree.JCVariableDecl node = (JCTree.JCVariableDecl)tree;
                        if (node.startPos != Position.NOPOS)
                        {
                            return node.startPos;
                        }
                        else if (node.mods.pos != Position.NOPOS)
                        {
                            return node.mods.pos;
                        }
                        else if (node.vartype == null || node.vartype.pos == Position.NOPOS)
                        {
                            //if there's no type (partially typed lambda parameter)
                            //simply return node position
                            return node.pos;
                        }
                        else
                        {
                            return getStartPos(node.vartype);
                        }
                    }
                case JCTree.Tag.InnerEnum.BINDINGPATTERN:
                    {
                        JCTree.JCBindingPattern node = (JCTree.JCBindingPattern)tree;
                        return getStartPos(node.@var);
                    }
                case JCTree.Tag.InnerEnum.ERRONEOUS:
                    {
                        JCTree.JCErroneous node = (JCTree.JCErroneous)tree;
                        if (node.errs != null && node.errs.nonEmpty())
                        {
                            return getStartPos(node.errs[0]);
                        }
                    }
                    break;
            }
            return tree.pos;
        }

        /// <summary>
        /// The end position of given tree, given  a table of end positions generated by the parser
        /// </summary>
        public static int getEndPos(JCTree tree, EndPosTable endPosTable)
        {
            if (tree == null)
            {
                return Position.NOPOS;
            }

            if (endPosTable == null)
            {
                // fall back on limited info in the tree
                return endPos(tree);
            }

            int mapPos = endPosTable.getEndPos(tree);
            if (mapPos != Position.NOPOS)
            {
                return mapPos;
            }

            switch (tree.getTag().innerEnumValue)
            {
                case JCTree.Tag.InnerEnum.BITOR_ASG:
                case JCTree.Tag.InnerEnum.BITXOR_ASG:
                case JCTree.Tag.InnerEnum.BITAND_ASG:
                case JCTree.Tag.InnerEnum.SL_ASG:
                case JCTree.Tag.InnerEnum.SR_ASG:
                case JCTree.Tag.InnerEnum.USR_ASG:
                case JCTree.Tag.InnerEnum.PLUS_ASG:
                case JCTree.Tag.InnerEnum.MINUS_ASG:
                case JCTree.Tag.InnerEnum.MUL_ASG:
                case JCTree.Tag.InnerEnum.DIV_ASG:
                case JCTree.Tag.InnerEnum.MOD_ASG:
                case JCTree.Tag.InnerEnum.OR:
                case JCTree.Tag.InnerEnum.AND:
                case JCTree.Tag.InnerEnum.BITOR:
                case JCTree.Tag.InnerEnum.BITXOR:
                case JCTree.Tag.InnerEnum.BITAND:
                case JCTree.Tag.InnerEnum.EQ:
                case JCTree.Tag.InnerEnum.NE:
                case JCTree.Tag.InnerEnum.LT:
                case JCTree.Tag.InnerEnum.GT:
                case JCTree.Tag.InnerEnum.LE:
                case JCTree.Tag.InnerEnum.GE:
                case JCTree.Tag.InnerEnum.SL:
                case JCTree.Tag.InnerEnum.SR:
                case JCTree.Tag.InnerEnum.USR:
                case JCTree.Tag.InnerEnum.PLUS:
                case JCTree.Tag.InnerEnum.MINUS:
                case JCTree.Tag.InnerEnum.MUL:
                case JCTree.Tag.InnerEnum.DIV:
                case JCTree.Tag.InnerEnum.MOD:
                case JCTree.Tag.InnerEnum.POS:
                case JCTree.Tag.InnerEnum.NEG:
                case JCTree.Tag.InnerEnum.NOT:
                case JCTree.Tag.InnerEnum.COMPL:
                case JCTree.Tag.InnerEnum.PREINC:
                case JCTree.Tag.InnerEnum.PREDEC:
                    return getEndPos(((JCTree.JCOperatorExpression)tree).getOperand(JCTree.JCOperatorExpression.OperandPos.RIGHT), endPosTable);
                case JCTree.Tag.InnerEnum.CASE:
                    return getEndPos(((JCTree.JCCase)tree).stats.last(), endPosTable);
                case JCTree.Tag.InnerEnum.CATCH:
                    return getEndPos(((JCTree.JCCatch)tree).body, endPosTable);
                case JCTree.Tag.InnerEnum.CONDEXPR:
                    return getEndPos(((JCTree.JCConditional)tree).falsepart, endPosTable);
                case JCTree.Tag.InnerEnum.FORLOOP:
                    return getEndPos(((JCTree.JCForLoop)tree).body, endPosTable);
                case JCTree.Tag.InnerEnum.FOREACHLOOP:
                    return getEndPos(((JCTree.JCEnhancedForLoop)tree).body, endPosTable);
                case JCTree.Tag.InnerEnum.IF:
                    {
                        JCTree.JCIf node = (JCTree.JCIf)tree;
                        if (node.elsepart == null)
                        {
                            return getEndPos(node.thenpart, endPosTable);
                        }
                        else
                        {
                            return getEndPos(node.elsepart, endPosTable);
                        }
                    }
                case JCTree.Tag.InnerEnum.LABELLED:
                    return getEndPos(((JCTree.JCLabeledStatement)tree).body, endPosTable);
                case JCTree.Tag.InnerEnum.MODIFIERS:
                    return getEndPos(((JCTree.JCModifiers)tree).annotations.last(), endPosTable);
                case JCTree.Tag.InnerEnum.SYNCHRONIZED:
                    return getEndPos(((JCTree.JCSynchronized)tree).body, endPosTable);
                case JCTree.Tag.InnerEnum.TOPLEVEL:
                    return getEndPos(((JCTree.JCCompilationUnit)tree).defs.last(), endPosTable);
                case JCTree.Tag.InnerEnum.TRY:
                    {
                        JCTree.JCTry node = (JCTree.JCTry)tree;
                        if (node.finalizer != null)
                        {
                            return getEndPos(node.finalizer, endPosTable);
                        }
                        else if (!node.catchers.isEmpty())
                        {
                            return getEndPos(node.catchers.last(), endPosTable);
                        }
                        else
                        {
                            return getEndPos(node.body, endPosTable);
                        }
                    }
                case JCTree.Tag.InnerEnum.WILDCARD:
                    return getEndPos(((JCTree.JCWildcard)tree).inner, endPosTable);
                case JCTree.Tag.InnerEnum.TYPECAST:
                    return getEndPos(((JCTree.JCTypeCast)tree).expr, endPosTable);
                case JCTree.Tag.InnerEnum.TYPETEST:
                    return getEndPos(((JCTree.JCInstanceOf)tree).pattern, endPosTable);
                case JCTree.Tag.InnerEnum.WHILELOOP:
                    return getEndPos(((JCTree.JCWhileLoop)tree).body, endPosTable);
                case JCTree.Tag.InnerEnum.ANNOTATED_TYPE:
                    return getEndPos(((JCTree.JCAnnotatedType)tree).underlyingType, endPosTable);
                case JCTree.Tag.InnerEnum.ERRONEOUS:
                    {
                        JCTree.JCErroneous node = (JCTree.JCErroneous)tree;
                        if (node.errs != null && node.errs.nonEmpty())
                        {
                            return getEndPos(node.errs.last(), endPosTable);
                        }
                    }
                    break;
            }
            return Position.NOPOS;
        }


        //        /// <summary>
        //        /// A DiagnosticPosition with the preferred position set to the
        //        ///  end position of given tree, if it is a block with
        //        ///  defined endpos.
        //        /// </summary>
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public static com.sun.tools.javac.util.JCDiagnostic.DiagnosticPosition diagEndPos(final JCTree tree)
        //        public static DiagnosticPosition diagEndPos(JCTree tree)
        //        {
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: The original Java variable was marked 'final':
        ////ORIGINAL LINE: final int endPos = TreeInfo.endPos(tree);
        //            int endPos = TreeInfo.endPos(tree);
        //            return new DiagnosticPositionAnonymousInnerClass(tree, endPos);
        //        }

        //        private class DiagnosticPositionAnonymousInnerClass : DiagnosticPosition
        //        {
        //            private JCTree tree;
        //            private int endPos;

        //            public DiagnosticPositionAnonymousInnerClass(JCTree tree, int endPos)
        //            {
        //                this.tree = tree;
        //                this.endPos = endPos;
        //            }

        //            public virtual JCTree getTree()
        //            {
        //                return tree;
        //            }
        //            public virtual int getStartPosition()
        //            {
        //                return TreeInfo.getStartPos(tree);
        //            }
        //            public virtual int getPreferredPosition()
        //            {
        //                return endPos;
        //            }
        //            public virtual int getEndPosition(EndPosTable endPosTable)
        //            {
        //                return TreeInfo.getEndPos(tree, endPosTable);
        //            }
        //        }

        //        public sealed class PosKind
        //        {
        //            public static readonly PosKind START_POS = new PosKind("START_POS", InnerEnum.START_POS, TreeInfo.getStartPos);
        //            public static readonly PosKind FIRST_STAT_POS = new PosKind("FIRST_STAT_POS", InnerEnum.FIRST_STAT_POS, TreeInfo.firstStatPos);
        //            public static readonly PosKind END_POS = new PosKind("END_POS", InnerEnum.END_POS, TreeInfo.endPos);

        //            private static readonly IList<PosKind> valueList = new List<PosKind>();

        //            static PosKind()
        //            {
        //                valueList.Add(START_POS);
        //                valueList.Add(FIRST_STAT_POS);
        //                valueList.Add(END_POS);
        //            }

        //            public enum InnerEnum
        //            {
        //                START_POS,
        //                FIRST_STAT_POS,
        //                END_POS
        //            }

        //            public readonly InnerEnum innerEnumValue;
        //            private readonly string nameValue;
        //            private readonly int ordinalValue;
        //            private static int nextOrdinal = 0;

        //            internal readonly System.Func<JCTree, int> posFunc;

        //            internal PosKind(string name, InnerEnum innerEnum, System.Func<JCTree, int> posFunc)
        //            {
        //                this.posFunc = posFunc;

        //                nameValue = name;
        //                ordinalValue = nextOrdinal++;
        //                innerEnumValue = innerEnum;
        //            }

        //            internal int toPos(JCTree tree)
        //            {
        //                return posFunc.applyAsInt(tree);
        //            }

        //            public static IList<PosKind> values()
        //            {
        //                return valueList;
        //            }

        //            public int ordinal()
        //            {
        //                return ordinalValue;
        //            }

        //            public override string ToString()
        //            {
        //                return nameValue;
        //            }

        //            public static PosKind valueOf(string name)
        //            {
        //                foreach (PosKind enumInstance in PosKind.valueList)
        //                {
        //                    if (enumInstance.nameValue == name)
        //                    {
        //                        return enumInstance;
        //                    }
        //                }
        //                throw new System.ArgumentException(name);
        //            }
        //        }

        //        /// <summary>
        //        /// The position of the finalizer of given try/synchronized statement.
        //        /// </summary>
        //        public static int finalizerPos(JCTree tree, PosKind posKind)
        //        {
        //            if (tree.hasTag(TRY))
        //            {
        //                JCTree.JCTry t = (JCTree.JCTry) tree;
        //                Assert.checkNonNull(t.finalizer);
        //                return posKind.toPos(t.finalizer);
        //            }
        //            else if (tree.hasTag(SYNCHRONIZED))
        //            {
        //                return endPos(((JCTree.JCSynchronized) tree).body);
        //            }
        //            else
        //            {
        //                throw new AssertionError();
        //            }
        //        }

        //        /// <summary>
        //        /// Find the position for reporting an error about a symbol, where
        //        ///  that symbol is defined somewhere in the given tree. 
        //        /// </summary>
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public static int positionFor(final Symbol sym, final JCTree tree)
        //        public static int positionFor(Symbol sym, JCTree tree)
        //        {
        //            JCTree decl = declarationFor(sym, tree);
        //            return ((decl != null) ? decl : tree).pos_Renamed;
        //        }

        //        /// <summary>
        //        /// Find the position for reporting an error about a symbol, where
        //        ///  that symbol is defined somewhere in the given tree. 
        //        /// </summary>
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public static com.sun.tools.javac.util.JCDiagnostic.DiagnosticPosition diagnosticPositionFor(final Symbol sym, final JCTree tree)
        //        public static DiagnosticPosition diagnosticPositionFor(Symbol sym, JCTree tree)
        //        {
        //            return diagnosticPositionFor(sym, tree, false);
        //        }

        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public static com.sun.tools.javac.util.JCDiagnostic.DiagnosticPosition diagnosticPositionFor(final Symbol sym, final JCTree tree, boolean returnNullIfNotFound)
        //        public static DiagnosticPosition diagnosticPositionFor(Symbol sym, JCTree tree, bool returnNullIfNotFound)
        //        {
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Local classes are not converted by Java to C# Converter:
        ////            class DiagScanner extends DeclScanner
        //    //        {
        //    //            DiagScanner(Symbol sym)
        //    //            {
        //    //                base(sym);
        //    //            }
        //    //
        //    //            public void visitIdent(JCIdent that)
        //    //            {
        //    //                if (that.sym == sym)
        //    //                    result = that;
        //    //                else
        //    //                    base.visitIdent(that);
        //    //            }
        //    //            public void visitSelect(JCFieldAccess that)
        //    //            {
        //    //                if (that.sym == sym)
        //    //                    result = that;
        //    //                else
        //    //                    base.visitSelect(that);
        //    //            }
        //    //        }
        //            DiagScanner s = new DiagScanner(sym);
        //            tree.accept(s);
        //            JCTree decl = s.result;
        //            if (decl == null && returnNullIfNotFound)
        //            {
        //                return null;
        //            }
        //            return ((decl != null) ? decl : tree).pos();
        //        }

        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public static com.sun.tools.javac.util.JCDiagnostic.DiagnosticPosition diagnosticPositionFor(final Symbol sym, final List<? extends JCTree> trees)
        //        public static DiagnosticPosition diagnosticPositionFor<T1>(Symbol sym, IList<T1> trees) where T1 : JCTree
        //        {
        //            return trees.Select(t => TreeInfo.diagnosticPositionFor(sym, t)).Where(t => t != null).First().get();
        //        }

        //        private class DeclScanner : TreeScanner
        //        {
        //            internal readonly Symbol sym;

        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: DeclScanner(final Symbol sym)
        //            internal DeclScanner(Symbol sym)
        //            {
        //                this.sym = sym;
        //            }

        //            internal JCTree result = null;
        //            public override void scan(JCTree tree)
        //            {
        //                if (tree != null && result == null)
        //                {
        //                    tree.accept(this);
        //                }
        //            }
        //            public override void visitTopLevel(JCTree.JCCompilationUnit that)
        //            {
        //                if (that.packge == sym)
        //                {
        //                    result = that;
        //                }
        //                else
        //                {
        //                    base.visitTopLevel(that);
        //                }
        //            }
        //            public override void visitModuleDef(JCTree.JCModuleDecl that)
        //            {
        //                if (that.sym == sym)
        //                {
        //                    result = that;
        //                }
        //                // no need to scan within module declaration
        //            }
        //            public override void visitPackageDef(JCTree.JCPackageDecl that)
        //            {
        //                if (that.packge == sym)
        //                {
        //                    result = that;
        //                }
        //                else
        //                {
        //                    base.visitPackageDef(that);
        //                }
        //            }
        //            public override void visitClassDef(JCTree.JCClassDecl that)
        //            {
        //                if (that.sym == sym)
        //                {
        //                    result = that;
        //                }
        //                else
        //                {
        //                    base.visitClassDef(that);
        //                }
        //            }
        //            public override void visitMethodDef(JCTree.JCMethodDecl that)
        //            {
        //                if (that.sym == sym)
        //                {
        //                    result = that;
        //                }
        //                else
        //                {
        //                    base.visitMethodDef(that);
        //                }
        //            }
        //            public override void visitVarDef(JCTree.JCVariableDecl that)
        //            {
        //                if (that.sym == sym)
        //                {
        //                    result = that;
        //                }
        //                else
        //                {
        //                    base.visitVarDef(that);
        //                }
        //            }
        //            public override void visitTypeParameter(JCTree.JCTypeParameter that)
        //            {
        //                if (that.type != null && that.type.tsym == sym)
        //                {
        //                    result = that;
        //                }
        //                else
        //                {
        //                    base.visitTypeParameter(that);
        //                }
        //            }
        //        }

        //        /// <summary>
        //        /// Find the declaration for a symbol, where
        //        ///  that symbol is defined somewhere in the given tree. 
        //        /// </summary>
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public static JCTree declarationFor(final Symbol sym, final JCTree tree)
        //        public static JCTree declarationFor(Symbol sym, JCTree tree)
        //        {
        //            DeclScanner s = new DeclScanner(sym);
        //            tree.accept(s);
        //            return s.result;
        //        }

        //        public static Env<AttrContext> scopeFor(JCTree node, JCTree.JCCompilationUnit unit)
        //        {
        //            return scopeFor(pathFor(node, unit));
        //        }

        //        public static Env<AttrContext> scopeFor(IList<JCTree> path)
        //        {
        //            // TODO: not implemented yet
        //            throw new System.NotSupportedException("not implemented yet");
        //        }

        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public static List<JCTree> pathFor(final JCTree node, final JCCompilationUnit unit)
        //        public static IList<JCTree> pathFor(JCTree node, JCTree.JCCompilationUnit unit)
        //        {
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Local classes are not converted by Java to C# Converter:
        ////            class Result extends Error
        //    //        {
        //    //            static final long serialVersionUID = -5942088234594905625L;
        //    //            List<JCTree> path;
        //    //            Result(List<JCTree> path)
        //    //            {
        //    //                this.path = path;
        //    //            }
        //    //        }
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Local classes are not converted by Java to C# Converter:
        ////            class PathFinder extends TreeScanner
        //    //        {
        //    //            List<JCTree> path = List.nil();
        //    //            public void scan(JCTree tree)
        //    //            {
        //    //                if (tree != null)
        //    //                {
        //    //                    path = path.prepend(tree);
        //    //                    if (tree == node)
        //    //                        throw new Result(path);
        //    //                    base.scan(tree);
        //    //                    path = path.tail;
        //    //                }
        //    //            }
        //    //        }
        //            try
        //            {
        //                (new PathFinder()).scan(unit);
        //            }
        //            catch (Result result)
        //            {
        //                return result.path;
        //            }
        //            return IList.nil();
        //        }

        //        /// <summary>
        //        /// Return the statement referenced by a label.
        //        ///  If the label refers to a loop or switch, return that switch
        //        ///  otherwise return the labelled statement itself
        //        /// </summary>
        //        public static JCTree referencedStatement(JCTree.JCLabeledStatement tree)
        //        {
        //            JCTree t = tree;
        //            do
        //            {
        //                t = ((JCTree.JCLabeledStatement) t).body;
        //            } while (t.hasTag(LABELLED));
        //            switch (t.getTag())
        //            {
        //            case DOLOOP:
        //        case WHILELOOP:
        //    case FORLOOP:
        //    case FOREACHLOOP:
        //    case SWITCH:
        //                return t;
        //            default:
        //                return tree;
        //            }
        //        }

        //        /// <summary>
        //        /// Skip parens and return the enclosed expression
        //        /// </summary>
        //        public static JCTree.JCExpression skipParens(JCTree.JCExpression tree)
        //        {
        //            while (tree.hasTag(PARENS))
        //            {
        //                tree = ((JCTree.JCParens) tree).expr;
        //            }
        //            return tree;
        //        }

        /// <summary>
        /// Skip parens and return the enclosed expression
        /// </summary>
        public static JCTree skipParens(JCTree tree)
        {
            if (tree.hasTag(PARENS))
            {
                return skipParens((JCTree.JCParens)tree);
            }
            else
            {
                return tree;
            }
        }

        //        /// <summary>
        //        /// Return the types of a list of trees.
        //        /// </summary>
        //        public static IList<Type> types<T1>(IList<T1> trees) where T1 : JCTree
        //        {
        //            ListBuffer<Type> ts = new ListBuffer<Type>();
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        ////ORIGINAL LINE: for (List<? extends JCTree> l = trees; l.nonEmpty(); l = l.tail)
        //            for (IList<JCTree> l = trees; l.nonEmpty(); l = l.tail)
        //            {
        //                ts.append(l.head.type);
        //            }
        //            return ts.toList();
        //        }

        /// <summary>
        /// If this tree is an identifier or a field or a parameterized type,
        ///  return its name, otherwise return null.
        /// </summary>
        public static Name name(JCTree tree)
        {
            switch (tree.getTag().innerEnumValue)
            {
                case JCTree.Tag.InnerEnum.IDENT:
                    return ((JCTree.JCIdent)tree).name;
                case JCTree.Tag.InnerEnum.SELECT:
                    return ((JCTree.JCFieldAccess)tree).name;
                case JCTree.Tag.InnerEnum.TYPEAPPLY:
                    return name(((JCTree.JCTypeApply)tree).clazz);
                default:
                    return null;
            }
        }

        //        /// <summary>
        //        /// If this tree is a qualified identifier, its return fully qualified name,
        //        ///  otherwise return null.
        //        /// </summary>
        //        public static Name fullName(JCTree tree)
        //        {
        //            tree = skipParens(tree);
        //            switch (tree.getTag())
        //            {
        //            case IDENT:
        //                return ((JCTree.JCIdent) tree).name;
        //            case SELECT:
        //                Name sname = fullName(((JCTree.JCFieldAccess) tree).selected);
        //                return sname == null ? null : sname.append('.', name(tree));
        //            default:
        //                return null;
        //            }
        //        }

        //        public static Symbol symbolFor(JCTree node)
        //        {
        //            Symbol sym = symbolForImpl(node);

        //            return sym != null ? sym.baseSymbol() : null;
        //        }

        //        private static Symbol symbolForImpl(JCTree node)
        //        {
        //            node = skipParens(node);
        //            switch (node.getTag())
        //            {
        //            case TOPLEVEL:
        //                JCTree.JCCompilationUnit cut = (JCTree.JCCompilationUnit) node;
        //                JCTree.JCModuleDecl moduleDecl = cut.getModuleDecl();
        //                if (isModuleInfo(cut) && moduleDecl != null)
        //                {
        //                    return symbolFor(moduleDecl);
        //                }
        //                return cut.packge;
        //            case MODULEDEF:
        //                return ((JCTree.JCModuleDecl) node).sym;
        //            case PACKAGEDEF:
        //                return ((JCTree.JCPackageDecl) node).packge;
        //            case CLASSDEF:
        //                return ((JCTree.JCClassDecl) node).sym;
        //            case METHODDEF:
        //                return ((JCTree.JCMethodDecl) node).sym;
        //            case VARDEF:
        //                return ((JCTree.JCVariableDecl) node).sym;
        //            case IDENT:
        //                return ((JCTree.JCIdent) node).sym;
        //            case SELECT:
        //                return ((JCTree.JCFieldAccess) node).sym;
        //            case REFERENCE:
        //                return ((JCTree.JCMemberReference) node).sym;
        //            case NEWCLASS:
        //                return ((JCTree.JCNewClass) node).constructor;
        //            case APPLY:
        //                return symbolFor(((JCTree.JCMethodInvocation) node).meth);
        //            case TYPEAPPLY:
        //                return symbolFor(((JCTree.JCTypeApply) node).clazz);
        //            case ANNOTATION:
        //            case TYPE_ANNOTATION:
        //            case TYPEPARAMETER:
        //                if (node.type != null)
        //                {
        //                    return node.type.tsym;
        //                }
        //                return null;
        //            default:
        //                return null;
        //            }
        //        }

        //        public static bool isDeclaration(JCTree node)
        //        {
        //            node = skipParens(node);
        //            switch (node.getTag())
        //            {
        //            case PACKAGEDEF:
        //            case CLASSDEF:
        //            case METHODDEF:
        //            case VARDEF:
        //                return true;
        //            default:
        //                return false;
        //            }
        //        }

        /// <summary>
        /// If this tree is an identifier or a field, return its symbol,
        ///  otherwise return null.
        /// </summary>
        public static Symbol symbol(JCTree tree)
        {
            tree = skipParens(tree);
            switch (tree.getTag().innerEnumValue)
            {
                case JCTree.Tag.InnerEnum.IDENT:
                    return ((JCTree.JCIdent)tree).sym;
                case JCTree.Tag.InnerEnum.SELECT:
                    return ((JCTree.JCFieldAccess)tree).sym;
                case JCTree.Tag.InnerEnum.TYPEAPPLY:
                    return symbol(((JCTree.JCTypeApply)tree).clazz);
                case JCTree.Tag.InnerEnum.ANNOTATED_TYPE:
                    return symbol(((JCTree.JCAnnotatedType)tree).underlyingType);
                case JCTree.Tag.InnerEnum.REFERENCE:
                    return ((JCTree.JCMemberReference)tree).sym;
                default:
                    return null;
            }
        }

        //        /// <summary>
        //        /// If this tree has a modifiers field, return it otherwise return null
        //        /// </summary>
        //        public static JCTree.JCModifiers getModifiers(JCTree tree)
        //        {
        //            tree = skipParens(tree);
        //            switch (tree.getTag())
        //            {
        //                case VARDEF:
        //                    return ((JCTree.JCVariableDecl) tree).mods;
        //                case METHODDEF:
        //                    return ((JCTree.JCMethodDecl) tree).mods;
        //                case CLASSDEF:
        //                    return ((JCTree.JCClassDecl) tree).mods;
        //                case MODULEDEF:
        //                    return ((JCTree.JCModuleDecl) tree).mods;
        //            default:
        //                return null;
        //            }
        //        }

        //        /// <summary>
        //        /// Return true if this is a nonstatic selection. </summary>
        //        public static bool nonstaticSelect(JCTree tree)
        //        {
        //            tree = skipParens(tree);
        //            if (!tree.hasTag(SELECT))
        //            {
        //                return false;
        //            }
        //            JCTree.JCFieldAccess s = (JCTree.JCFieldAccess) tree;
        //            Symbol e = symbol(s.selected);
        //            return e == null || (e.kind != PCK && e.kind != TYP);
        //        }

        //        /// <summary>
        //        /// If this tree is an identifier or a field, set its symbol, otherwise skip.
        //        /// </summary>
        //        public static void setSymbol(JCTree tree, Symbol sym)
        //        {
        //            tree = skipParens(tree);
        //            switch (tree.getTag())
        //            {
        //            case IDENT:
        //                ((JCTree.JCIdent) tree).sym = sym;
        //                break;
        //            case SELECT:
        //                ((JCTree.JCFieldAccess) tree).sym = sym;
        //                break;
        //            default:
        //        break;
        //            }
        //        }

        //        /// <summary>
        //        /// If this tree is a declaration or a block, return its flags field,
        //        ///  otherwise return 0.
        //        /// </summary>
        //        public static long flags(JCTree tree)
        //        {
        //            switch (tree.getTag())
        //            {
        //            case VARDEF:
        //                return ((JCTree.JCVariableDecl) tree).mods.flags;
        //            case METHODDEF:
        //                return ((JCTree.JCMethodDecl) tree).mods.flags;
        //            case CLASSDEF:
        //                return ((JCTree.JCClassDecl) tree).mods.flags;
        //            case BLOCK:
        //                return ((JCTree.JCBlock) tree).flags;
        //            default:
        //                return 0;
        //            }
        //        }

        //        /// <summary>
        //        /// Return first (smallest) flag in `flags':
        //        ///  pre: flags != 0
        //        /// </summary>
        //        public static long firstFlag(long flags)
        //        {
        //            long flag = 1;
        //            while ((flag & flags & ExtendedStandardFlags) == 0)
        //            {
        //                flag = flag << 1;
        //            }
        //            return flag;
        //        }

        /// <summary>
        /// Return flags as a string, separated by " ".
        /// </summary>
        public static string flagNames(long flags)
        {
            return Flags.ToString(flags & Flags.ExtendedStandardFlags).Trim();
        }

        /// <summary>
        /// Operator precedences values.
        /// </summary>
        public const int
            notExpression = -1,  // not an expression
            noPrec = 0,          // no enclosing expression 
            assignPrec = 1,
            assignopPrec = 2,
            condPrec = 3,
            orPrec = 4,
            andPrec = 5,
            bitorPrec = 6,
            bitxorPrec = 7,
            bitandPrec = 8,
            eqPrec = 9,
            ordPrec = 10,
            shiftPrec = 11,
            addPrec = 12,
            mulPrec = 13,
            prefixPrec = 14,
            postfixPrec = 15,
            precCount = 16;


        /// <summary>
        /// Map operators to their precedence levels.
        /// </summary>
        public static int opPrec(JCTree.Tag op)
        {
            switch (op.innerEnumValue)
            {
                case JCTree.Tag.InnerEnum.POS:
                case JCTree.Tag.InnerEnum.NEG:
                case JCTree.Tag.InnerEnum.NOT:
                case JCTree.Tag.InnerEnum.COMPL:
                case JCTree.Tag.InnerEnum.PREINC:
                case JCTree.Tag.InnerEnum.PREDEC:
                    return prefixPrec;
                case JCTree.Tag.InnerEnum.POSTINC:
                case JCTree.Tag.InnerEnum.POSTDEC:
                case JCTree.Tag.InnerEnum.NULLCHK:
                    return postfixPrec;
                case JCTree.Tag.InnerEnum.ASSIGN:
                    return assignPrec;
                case JCTree.Tag.InnerEnum.BITOR_ASG:
                case JCTree.Tag.InnerEnum.BITXOR_ASG:
                case JCTree.Tag.InnerEnum.BITAND_ASG:
                case JCTree.Tag.InnerEnum.SL_ASG:
                case JCTree.Tag.InnerEnum.SR_ASG:
                case JCTree.Tag.InnerEnum.USR_ASG:
                case JCTree.Tag.InnerEnum.PLUS_ASG:
                case JCTree.Tag.InnerEnum.MINUS_ASG:
                case JCTree.Tag.InnerEnum.MUL_ASG:
                case JCTree.Tag.InnerEnum.DIV_ASG:
                case JCTree.Tag.InnerEnum.MOD_ASG:
                    return assignopPrec;
                case JCTree.Tag.InnerEnum.OR:
                    return orPrec;
                case JCTree.Tag.InnerEnum.AND:
                    return andPrec;
                case JCTree.Tag.InnerEnum.EQ:
                case JCTree.Tag.InnerEnum.NE:
                    return eqPrec;
                case JCTree.Tag.InnerEnum.LT:
                case JCTree.Tag.InnerEnum.GT:
                case JCTree.Tag.InnerEnum.LE:
                case JCTree.Tag.InnerEnum.GE:
                    return ordPrec;
                case JCTree.Tag.InnerEnum.BITOR:
                    return bitorPrec;
                case JCTree.Tag.InnerEnum.BITXOR:
                    return bitxorPrec;
                case JCTree.Tag.InnerEnum.BITAND:
                    return bitandPrec;
                case JCTree.Tag.InnerEnum.SL:
                case JCTree.Tag.InnerEnum.SR:
                case JCTree.Tag.InnerEnum.USR:
                    return shiftPrec;
                case JCTree.Tag.InnerEnum.PLUS:
                case JCTree.Tag.InnerEnum.MINUS:
                    return addPrec;
                case JCTree.Tag.InnerEnum.MUL:
                case JCTree.Tag.InnerEnum.DIV:
                case JCTree.Tag.InnerEnum.MOD:
                    return mulPrec;
                case JCTree.Tag.InnerEnum.TYPETEST:
                    return ordPrec;
                default:
                    throw new AssertionError();
            }
        }

        internal static Kind tagToKind(JCTree.Tag tag)
        {
            switch (tag.innerEnumValue)
            {
                // Postfix expressions
                case JCTree.Tag.InnerEnum.POSTINC: // _ ++
                    return Kind.POSTFIX_INCREMENT;
                case JCTree.Tag.InnerEnum.POSTDEC: // _ --
                    return Kind.POSTFIX_DECREMENT;

                // Unary operators
                case JCTree.Tag.InnerEnum.PREINC: // ++ _
                    return Kind.PREFIX_INCREMENT;
                case JCTree.Tag.InnerEnum.PREDEC: // -- _
                    return Kind.PREFIX_DECREMENT;
                case JCTree.Tag.InnerEnum.POS: // +
                    return Kind.UNARY_PLUS;
                case JCTree.Tag.InnerEnum.NEG: // -
                    return Kind.UNARY_MINUS;
                case JCTree.Tag.InnerEnum.COMPL: // ~
                    return Kind.BITWISE_COMPLEMENT;
                case JCTree.Tag.InnerEnum.NOT: // !
                    return Kind.LOGICAL_COMPLEMENT;

                // Binary operators

                // Multiplicative operators
                case JCTree.Tag.InnerEnum.MUL: // *
                    return Kind.MULTIPLY;
                case JCTree.Tag.InnerEnum.DIV: // /
                    return Kind.DIVIDE;
                case JCTree.Tag.InnerEnum.MOD: // %
                    return Kind.REMAINDER;

                // Additive operators
                case JCTree.Tag.InnerEnum.PLUS: // +
                    return Kind.PLUS;
                case JCTree.Tag.InnerEnum.MINUS: // -
                    return Kind.MINUS;

                // Shift operators
                case JCTree.Tag.InnerEnum.SL: // <<
                    return Kind.LEFT_SHIFT;
                case JCTree.Tag.InnerEnum.SR: // >>
                    return Kind.RIGHT_SHIFT;
                case JCTree.Tag.InnerEnum.USR: // >>>
                    return Kind.UNSIGNED_RIGHT_SHIFT;

                // Relational operators
                case JCTree.Tag.InnerEnum.LT: // <
                    return Kind.LESS_THAN;
                case JCTree.Tag.InnerEnum.GT: // >
                    return Kind.GREATER_THAN;
                case JCTree.Tag.InnerEnum.LE: // <=
                    return Kind.LESS_THAN_EQUAL;
                case JCTree.Tag.InnerEnum.GE: // >=
                    return Kind.GREATER_THAN_EQUAL;

                // Equality operators
                case JCTree.Tag.InnerEnum.EQ: // ==
                    return Kind.EQUAL_TO;
                case JCTree.Tag.InnerEnum.NE: // !=
                    return Kind.NOT_EQUAL_TO;

                // Bitwise and logical operators
                case JCTree.Tag.InnerEnum.BITAND: // &
                    return Kind.AND;
                case JCTree.Tag.InnerEnum.BITXOR: // ^
                    return Kind.XOR;
                case JCTree.Tag.InnerEnum.BITOR: // |
                    return Kind.OR;

                // Conditional operators
                case JCTree.Tag.InnerEnum.AND: // &&
                    return Kind.CONDITIONAL_AND;
                case JCTree.Tag.InnerEnum.OR: // ||
                    return Kind.CONDITIONAL_OR;

                // Assignment operators
                case JCTree.Tag.InnerEnum.MUL_ASG: // *=
                    return Kind.MULTIPLY_ASSIGNMENT;
                case JCTree.Tag.InnerEnum.DIV_ASG: // /=
                    return Kind.DIVIDE_ASSIGNMENT;
                case JCTree.Tag.InnerEnum.MOD_ASG: // %=
                    return Kind.REMAINDER_ASSIGNMENT;
                case JCTree.Tag.InnerEnum.PLUS_ASG: // +=
                    return Kind.PLUS_ASSIGNMENT;
                case JCTree.Tag.InnerEnum.MINUS_ASG: // -=
                    return Kind.MINUS_ASSIGNMENT;
                case JCTree.Tag.InnerEnum.SL_ASG: // <<=
                    return Kind.LEFT_SHIFT_ASSIGNMENT;
                case JCTree.Tag.InnerEnum.SR_ASG: // >>=
                    return Kind.RIGHT_SHIFT_ASSIGNMENT;
                case JCTree.Tag.InnerEnum.USR_ASG: // >>>=
                    return Kind.UNSIGNED_RIGHT_SHIFT_ASSIGNMENT;
                case JCTree.Tag.InnerEnum.BITAND_ASG: // &=
                    return Kind.AND_ASSIGNMENT;
                case JCTree.Tag.InnerEnum.BITXOR_ASG: // ^=
                    return Kind.XOR_ASSIGNMENT;
                case JCTree.Tag.InnerEnum.BITOR_ASG: // |=
                    return Kind.OR_ASSIGNMENT;

                // Null check (implementation detail), for example, __.getClass()
                case JCTree.Tag.InnerEnum.NULLCHK:
                    return Kind.OTHER;

                case JCTree.Tag.InnerEnum.ANNOTATION:
                    return Kind.ANNOTATION;
                case JCTree.Tag.InnerEnum.TYPE_ANNOTATION:
                    return Kind.TYPE_ANNOTATION;

                case JCTree.Tag.InnerEnum.EXPORTS:
                    return Kind.EXPORTS;
                case JCTree.Tag.InnerEnum.OPENS:
                    return Kind.OPENS;

                default:
                    // return null;
                    throw new System.NotSupportedException();
            }
        }

        //        /// <summary>
        //        /// Returns the underlying type of the tree if it is an annotated type,
        //        /// or the tree itself otherwise.
        //        /// </summary>
        //        public static JCTree.JCExpression typeIn(JCTree.JCExpression tree)
        //        {
        //            switch (tree.getTag())
        //            {
        //            case ANNOTATED_TYPE:
        //                return ((JCTree.JCAnnotatedType)tree).underlyingType;
        //            case IDENT: // simple names
        //            case TYPEIDENT: // primitive name
        //            case SELECT: // qualified name
        //            case TYPEARRAY: // array types
        //            case WILDCARD: // wild cards
        //            case TYPEPARAMETER: // type parameters
        //            case TYPEAPPLY: // parameterized types
        //            case ERRONEOUS: // error tree TODO: needed for BadCast JSR308 test case. Better way?
        //                return tree;
        //            default:
        //                throw new AssertionError("Unexpected type tree: " + tree);
        //            }
        //        }

        /* Return the inner-most type of a type tree.
         * For an array that contains an annotated type, return that annotated type.
         * TODO: currently only used by Pretty. Describe behavior better.
         */
        public static JCTree innermostType(JCTree type, bool skipAnnos)
        {
            JCTree lastAnnotatedType = null;
            JCTree cur = type;
            while (true)
            {
                switch (cur.getTag().innerEnumValue)
                {
                    case JCTree.Tag.InnerEnum.TYPEARRAY:
                        lastAnnotatedType = null;
                        cur = ((JCTree.JCArrayTypeTree)cur).elemtype;
                        break;
                    case JCTree.Tag.InnerEnum.WILDCARD:
                        lastAnnotatedType = null;
                        cur = ((JCTree.JCWildcard)cur).inner;
                        break;
                    case JCTree.Tag.InnerEnum.ANNOTATED_TYPE:
                        lastAnnotatedType = cur;
                        cur = ((JCTree.JCAnnotatedType)cur).underlyingType;
                        break;
                    default:
                        goto loopBreak;
                }
            }
        loopBreak:
            if (!skipAnnos && lastAnnotatedType != null)
            {
                return lastAnnotatedType;
            }
            else
            {
                return cur;
            }
        }

        //        private class TypeAnnotationFinder : TreeScanner
        //        {
        //            public bool foundTypeAnno = false;

        //            public override void scan(JCTree tree)
        //            {
        //                if (foundTypeAnno || tree == null)
        //                {
        //                    return;
        //                }
        //                base.scan(tree);
        //            }

        //            public override void visitAnnotation(JCTree.JCAnnotation tree)
        //            {
        //                foundTypeAnno = foundTypeAnno || tree.hasTag(TYPE_ANNOTATION);
        //            }
        //        }

        //        public static bool containsTypeAnnotation(JCTree e)
        //        {
        //            TypeAnnotationFinder finder = new TypeAnnotationFinder();
        //            finder.scan(e);
        //            return finder.foundTypeAnno;
        //        }

        //        public static bool isModuleInfo(JCTree.JCCompilationUnit tree)
        //        {
        //            return tree.sourcefile.isNameCompatible("module-info", JavaFileObject.Kind.SOURCE) && tree.getModuleDecl() != null;
        //        }

        //        public static JCTree.JCModuleDecl getModule(JCTree.JCCompilationUnit t)
        //        {
        //            if (t.defs.nonEmpty())
        //            {
        //                JCTree def = t.defs.head;
        //                if (def.hasTag(MODULEDEF))
        //                {
        //                    return (JCTree.JCModuleDecl) def;
        //                }
        //            }
        //            return null;
        //        }

        //        public static bool isPackageInfo(JCTree.JCCompilationUnit tree)
        //        {
        //            return tree.sourcefile.isNameCompatible("package-info", JavaFileObject.Kind.SOURCE);
        //        }

    }

}