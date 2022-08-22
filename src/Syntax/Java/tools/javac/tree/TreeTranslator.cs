using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;
using System.Collections.Generic;

/*
 * Copyright (c) 1999, 2019, Oracle and/or its affiliates. All rights reserved.
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

    /// <summary>
    /// A subclass of Tree.Visitor, this class defines
    ///  a general tree translator pattern. Translation proceeds recursively in
    ///  left-to-right order down a tree, constructing translated nodes by
    ///  overwriting existing ones. There is one visitor method in this class
    ///  for every possible kind of tree node.  To obtain a specific
    ///  translator, it suffices to override those visitor methods which
    ///  do some interesting work. The translator class itself takes care of all
    ///  navigational aspects.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class TreeTranslator : JCTree.Visitor
    {

        /// <summary>
        /// Visitor result field: a tree
        /// </summary>
        protected internal JCTree result;

        /// <summary>
        /// Visitor method: Translate a single node.
        /// </summary>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public <T extends JCTree> T translate(T tree)
        public virtual T translate<T>(T tree) where T : JCTree
        {
            if (tree == default(T))
            {
                return default(T);
            }
            else
            {
                tree.accept(this);
                JCTree tmpResult = this.result;
                this.result = null;
                return (T)tmpResult; // XXX cast
            }
        }

        /// <summary>
        /// Visitor method: translate a list of nodes.
        /// </summary>
        public virtual List<T> translate<T>(List<T> trees) where T : JCTree
        {
            if (trees == null)
            {
                return null;
            }
            //for (IList<T> l = trees; l.nonEmpty(); l = l.tail)
            //{
            //    l.head = translate(l.head);
            //}
            for (int i = 0; i < trees.Count; i++)
            {
                trees[i] = translate(trees[i]);
            }
            return trees;
        }

        /// <summary>
        ///  Visitor method: translate a list of variable definitions.
        /// </summary>
        public virtual List<JCTree.JCVariableDecl> translateVarDefs(List<JCTree.JCVariableDecl> trees)
        {
            //for (IList<JCTree.JCVariableDecl> l = trees; l.nonEmpty(); l = l.tail)
            //{
            //    l.head = translate(l.head);
            //}
            for (int i = 0; i < trees.Count; i++)
            {
                trees[i] = translate(trees[i]);
            }
            return trees;
        }

        /// <summary>
        ///  Visitor method: translate a list of type parameters.
        /// </summary>
        public virtual List<JCTree.JCTypeParameter> translateTypeParams(List<JCTree.JCTypeParameter> trees)
        {
            //for (IList<JCTree.JCTypeParameter> l = trees; l.nonEmpty(); l = l.tail)
            //{
            //    l.head = translate(l.head);
            //}
            for (int i = 0; i < trees.Count; i++)
            {
                trees[i] = translate(trees[i]);
            }
            return trees;
        }

        /// <summary>
        ///  Visitor method: translate a list of case parts of switch statements.
        /// </summary>
        public virtual List<JCTree.JCCase> translateCases(List<JCTree.JCCase> trees)
        {
            //for (IList<JCTree.JCCase> l = trees; l.nonEmpty(); l = l.tail)
            //{
            //    l.head = translate(l.head);
            //}
            for (int i = 0; i < trees.Count; i++)
            {
                trees[i] = translate(trees[i]);
            }
            return trees;
        }

        /// <summary>
        ///  Visitor method: translate a list of catch clauses in try statements.
        /// </summary>
        public virtual List<JCTree.JCCatch> translateCatchers(List<JCTree.JCCatch> trees)
        {
            //for (IList<JCTree.JCCatch> l = trees; l.nonEmpty(); l = l.tail)
            //{
            //    l.head = translate(l.head);
            //}
            for (int i = 0; i < trees.Count; i++)
            {
                trees[i] = translate(trees[i]);
            }
            return trees;
        }

        /// <summary>
        ///  Visitor method: translate a list of catch clauses in try statements.
        /// </summary>
        public virtual List<JCTree.JCAnnotation> translateAnnotations(List<JCTree.JCAnnotation> trees)
        {
            //for (IList<JCTree.JCAnnotation> l = trees; l.nonEmpty(); l = l.tail)
            //{
            //    l.head = translate(l.head);
            //}
            for (int i = 0; i < trees.Count; i++)
            {
                trees[i] = translate(trees[i]);
            }
            return trees;
        }

        /* ***************************************************************************
         * Visitor methods
         ****************************************************************************/

        public override void visitTopLevel(JCTree.JCCompilationUnit tree)
        {
            tree.defs = translate(tree.defs);
            result = tree;
        }

        public override void visitPackageDef(JCTree.JCPackageDecl tree)
        {
            tree.annotations = translate(tree.annotations);
            tree.pid = translate(tree.pid);
            result = tree;
        }

        public override void visitImport(JCTree.JCImport tree)
        {
            tree.qualid = translate(tree.qualid);
            result = tree;
        }

        public override void visitClassDef(JCTree.JCClassDecl tree)
        {
            tree.mods = translate(tree.mods);
            tree.typarams = translateTypeParams(tree.typarams);
            tree.extending = translate(tree.extending);
            tree.implementing = translate(tree.implementing);
            tree.defs = translate(tree.defs);
            result = tree;
        }

        public override void visitMethodDef(JCTree.JCMethodDecl tree)
        {
            tree.mods = translate(tree.mods);
            tree.restype = translate(tree.restype);
            tree.typarams = translateTypeParams(tree.typarams);
            tree.recvparam = translate(tree.recvparam);
            tree.@params = translateVarDefs(tree.@params);
            tree.thrown = translate(tree.thrown);
            tree.body = translate(tree.body);
            result = tree;
        }

        public override void visitVarDef(JCTree.JCVariableDecl tree)
        {
            tree.mods = translate(tree.mods);
            tree.nameexpr = translate(tree.nameexpr);
            tree.vartype = translate(tree.vartype);
            tree.init = translate(tree.init);
            result = tree;
        }

        public override void visitSkip(JCTree.JCSkip tree)
        {
            result = tree;
        }

        public override void visitBlock(JCTree.JCBlock tree)
        {
            tree.stats = translate(tree.stats);
            result = tree;
        }

        public override void visitDoLoop(JCTree.JCDoWhileLoop tree)
        {
            tree.body = translate(tree.body);
            tree.cond = translate(tree.cond);
            result = tree;
        }

        public override void visitWhileLoop(JCTree.JCWhileLoop tree)
        {
            tree.cond = translate(tree.cond);
            tree.body = translate(tree.body);
            result = tree;
        }

        public override void visitForLoop(JCTree.JCForLoop tree)
        {
            tree.init = translate(tree.init);
            tree.cond = translate(tree.cond);
            tree.step = translate(tree.step);
            tree.body = translate(tree.body);
            result = tree;
        }

        public override void visitForeachLoop(JCTree.JCEnhancedForLoop tree)
        {
            tree.@var = translate(tree.@var);
            tree.expr = translate(tree.expr);
            tree.body = translate(tree.body);
            result = tree;
        }

        public override void visitLabelled(JCTree.JCLabeledStatement tree)
        {
            tree.body = translate(tree.body);
            result = tree;
        }

        public override void visitSwitch(JCTree.JCSwitch tree)
        {
            tree.selector = translate(tree.selector);
            tree.cases = translateCases(tree.cases);
            result = tree;
        }

        public override void visitCase(JCTree.JCCase tree)
        {
            tree.pats = translate(tree.pats);
            tree.stats = translate(tree.stats);
            result = tree;
        }

        public override void visitSwitchExpression(JCTree.JCSwitchExpression tree)
        {
            tree.selector = translate(tree.selector);
            tree.cases = translateCases(tree.cases);
            result = tree;
        }

        public override void visitSynchronized(JCTree.JCSynchronized tree)
        {
            tree.@lock = translate(tree.@lock);
            tree.body = translate(tree.body);
            result = tree;
        }

        public override void visitTry(JCTree.JCTry tree)
        {
            tree.resources = translate(tree.resources);
            tree.body = translate(tree.body);
            tree.catchers = translateCatchers(tree.catchers);
            tree.finalizer = translate(tree.finalizer);
            result = tree;
        }

        public override void visitCatch(JCTree.JCCatch tree)
        {
            tree.param = translate(tree.param);
            tree.body = translate(tree.body);
            result = tree;
        }

        public override void visitConditional(JCTree.JCConditional tree)
        {
            tree.cond = translate(tree.cond);
            tree.truepart = translate(tree.truepart);
            tree.falsepart = translate(tree.falsepart);
            result = tree;
        }

        public override void visitIf(JCTree.JCIf tree)
        {
            tree.cond = translate(tree.cond);
            tree.thenpart = translate(tree.thenpart);
            tree.elsepart = translate(tree.elsepart);
            result = tree;
        }

        public override void visitExec(JCTree.JCExpressionStatement tree)
        {
            tree.expr = translate(tree.expr);
            result = tree;
        }

        public override void visitBreak(JCTree.JCBreak tree)
        {
            result = tree;
        }

        public override void visitYield(JCTree.JCYield tree)
        {
            tree.value = translate(tree.value);
            result = tree;
        }

        public override void visitContinue(JCTree.JCContinue tree)
        {
            result = tree;
        }

        public override void visitReturn(JCTree.JCReturn tree)
        {
            tree.expr = translate(tree.expr);
            result = tree;
        }

        public override void visitThrow(JCTree.JCThrow tree)
        {
            tree.expr = translate(tree.expr);
            result = tree;
        }

        public override void visitAssert(JCTree.JCAssert tree)
        {
            tree.cond = translate(tree.cond);
            tree.detail = translate(tree.detail);
            result = tree;
        }

        public override void visitApply(JCTree.JCMethodInvocation tree)
        {
            tree.meth = translate(tree.meth);
            tree.args = translate(tree.args);
            result = tree;
        }

        public override void visitNewClass(JCTree.JCNewClass tree)
        {
            tree.encl = translate(tree.encl);
            tree.clazz = translate(tree.clazz);
            tree.args = translate(tree.args);
            tree.def = translate(tree.def);
            result = tree;
        }

        public override void visitLambda(JCTree.JCLambda tree)
        {
            tree.@params = translate(tree.@params);
            tree.body = translate(tree.body);
            result = tree;
        }

        public override void visitNewArray(JCTree.JCNewArray tree)
        {
            tree.annotations = translate(tree.annotations);
            // IList<IList<JCTree.JCAnnotation>> dimAnnos = IList.nil();
            List<IList<JCTree.JCAnnotation>> dimAnnos = new List<IList<JCTree.JCAnnotation>>();
            foreach (List<JCTree.JCAnnotation> origDimAnnos in tree.dimAnnotations)
            {
                // dimAnnos = dimAnnos.append(translate(origDimAnnos));
                dimAnnos.Add(translate(origDimAnnos));
            }
            tree.dimAnnotations = dimAnnos;
            tree.elemtype = translate(tree.elemtype);
            tree.dims = translate(tree.dims);
            tree.elems = translate(tree.elems);
            result = tree;
        }

        public override void visitParens(JCTree.JCParens tree)
        {
            tree.expr = translate(tree.expr);
            result = tree;
        }

        public override void visitAssign(JCTree.JCAssign tree)
        {
            tree.lhs = translate(tree.lhs);
            tree.rhs = translate(tree.rhs);
            result = tree;
        }

        public override void visitAssignop(JCTree.JCAssignOp tree)
        {
            tree.lhs = translate(tree.lhs);
            tree.rhs = translate(tree.rhs);
            result = tree;
        }

        public override void visitUnary(JCTree.JCUnary tree)
        {
            tree.arg = translate(tree.arg);
            result = tree;
        }

        public override void visitBinary(JCTree.JCBinary tree)
        {
            tree.lhs = translate(tree.lhs);
            tree.rhs = translate(tree.rhs);
            result = tree;
        }

        public override void visitTypeCast(JCTree.JCTypeCast tree)
        {
            tree.clazz = translate(tree.clazz);
            tree.expr = translate(tree.expr);
            result = tree;
        }

        public override void visitTypeTest(JCTree.JCInstanceOf tree)
        {
            tree.expr = translate(tree.expr);
            tree.pattern = translate(tree.pattern);
            result = tree;
        }

        public override void visitBindingPattern(JCTree.JCBindingPattern tree)
        {
            tree.@var = translate(tree.@var);
            result = tree;
        }

        public override void visitIndexed(JCTree.JCArrayAccess tree)
        {
            tree.indexed = translate(tree.indexed);
            tree.index = translate(tree.index);
            result = tree;
        }

        public override void visitSelect(JCTree.JCFieldAccess tree)
        {
            tree.selected = translate(tree.selected);
            result = tree;
        }

        public override void visitReference(JCTree.JCMemberReference tree)
        {
            tree.expr = translate(tree.expr);
            result = tree;
        }

        public override void visitIdent(JCTree.JCIdent tree)
        {
            result = tree;
        }

        public override void visitLiteral(JCTree.JCLiteral tree)
        {
            result = tree;
        }

        public override void visitTypeIdent(JCTree.JCPrimitiveTypeTree tree)
        {
            result = tree;
        }

        public override void visitTypeArray(JCTree.JCArrayTypeTree tree)
        {
            tree.elemtype = translate(tree.elemtype);
            result = tree;
        }

        public override void visitTypeApply(JCTree.JCTypeApply tree)
        {
            tree.clazz = translate(tree.clazz);
            tree.arguments = translate(tree.arguments);
            result = tree;
        }

        public override void visitTypeUnion(JCTree.JCTypeUnion tree)
        {
            tree.alternatives = translate(tree.alternatives);
            result = tree;
        }

        public override void visitTypeIntersection(JCTree.JCTypeIntersection tree)
        {
            tree.bounds = translate(tree.bounds);
            result = tree;
        }

        public override void visitTypeParameter(JCTree.JCTypeParameter tree)
        {
            tree.annotations = translate(tree.annotations);
            tree.bounds = translate(tree.bounds);
            result = tree;
        }

        public override void visitWildcard(JCTree.JCWildcard tree)
        {
            tree.kind = translate(tree.kind);
            tree.inner = translate(tree.inner);
            result = tree;
        }

        public override void visitTypeBoundKind(JCTree.TypeBoundKind tree)
        {
            result = tree;
        }

        public override void visitErroneous(JCTree.JCErroneous tree)
        {
            result = tree;
        }

        public override void visitLetExpr(JCTree.LetExpr tree)
        {
            tree.defs = translate(tree.defs);
            tree.expr = translate(tree.expr);
            result = tree;
        }

        public override void visitModifiers(JCTree.JCModifiers tree)
        {
            tree.annotations = translateAnnotations(tree.annotations);
            result = tree;
        }

        public override void visitAnnotation(JCTree.JCAnnotation tree)
        {
            tree.annotationType = translate(tree.annotationType);
            tree.args = translate(tree.args);
            result = tree;
        }

        public override void visitAnnotatedType(JCTree.JCAnnotatedType tree)
        {
            tree.annotations = translate(tree.annotations);
            tree.underlyingType = translate(tree.underlyingType);
            result = tree;
        }

        public override void visitTree(JCTree tree)
        {
            throw new AssertionError(tree);
        }
    }

}