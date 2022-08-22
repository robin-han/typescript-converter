using System.Collections.Generic;

/*
 * Copyright (c) 2001, 2020, Oracle and/or its affiliates. All rights reserved.
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
    using com.sun.tools.javac.util;

    /// <summary>
    /// A subclass of Tree.Visitor, this class defines
    ///  a general tree scanner pattern. Translation proceeds recursively in
    ///  left-to-right order down a tree. There is one visitor method in this class
    ///  for every possible kind of tree node.  To obtain a specific
    ///  scanner, it suffices to override those visitor methods which
    ///  do some interesting work. The scanner class itself takes care of all
    ///  navigational aspects.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class TreeScanner : JCTree.Visitor
    {
        /// <summary>
        /// Visitor method: Scan a single node.
        /// </summary>
        public virtual void scan(JCTree tree)
        {
            if (tree != null)
            {
                tree.accept(this);
            }
        }

        /// <summary>
        /// Visitor method: scan a list of nodes.
        /// </summary>
        public virtual void scan<T1>(IList<T1> trees) where T1 : JCTree
        {
            if (trees != null)
            {
                //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
                //ORIGINAL LINE: for (List<? extends JCTree> l = trees; l.nonEmpty(); l = l.tail)
                //for (IList<JCTree> l = trees; l.nonEmpty(); l = l.tail)
                //{
                //    scan(l.head);
                //}
                foreach (var item in trees)
                {
                    scan(item);
                }
            }
        }


        /* ***************************************************************************
         * Visitor methods
         ****************************************************************************/

        public override void visitTopLevel(JCTree.JCCompilationUnit tree)
        {
            scan(tree.defs);
        }

        public override void visitPackageDef(JCTree.JCPackageDecl tree)
        {
            scan(tree.annotations);
            scan(tree.pid);
        }

        public override void visitModuleDef(JCTree.JCModuleDecl tree)
        {
            scan(tree.mods);
            scan(tree.qualId);
            scan(tree.directives);
        }

        public override void visitExports(JCTree.JCExports tree)
        {
            scan(tree.qualid);
            scan(tree.moduleNames);
        }

        public override void visitOpens(JCTree.JCOpens tree)
        {
            scan(tree.qualid);
            scan(tree.moduleNames);
        }

        public override void visitProvides(JCTree.JCProvides tree)
        {
            scan(tree.serviceName);
            scan(tree.implNames);
        }

        public override void visitRequires(JCTree.JCRequires tree)
        {
            scan(tree.moduleName);
        }

        public override void visitUses(JCTree.JCUses tree)
        {
            scan(tree.qualid);
        }

        public override void visitImport(JCTree.JCImport tree)
        {
            scan(tree.qualid);
        }

        public override void visitClassDef(JCTree.JCClassDecl tree)
        {
            scan(tree.mods);
            scan(tree.typarams);
            scan(tree.extending);
            scan(tree.implementing);
            scan(tree.permitting);
            scan(tree.defs);
        }

        public override void visitMethodDef(JCTree.JCMethodDecl tree)
        {
            scan(tree.mods);
            scan(tree.restype);
            scan(tree.typarams);
            scan(tree.recvparam);
            scan(tree.@params);
            scan(tree.thrown);
            scan(tree.defaultValue);
            scan(tree.body);
        }

        public override void visitVarDef(JCTree.JCVariableDecl tree)
        {
            scan(tree.mods);
            scan(tree.vartype);
            scan(tree.nameexpr);
            scan(tree.init);
        }

        public override void visitSkip(JCTree.JCSkip tree)
        {
        }

        public override void visitBlock(JCTree.JCBlock tree)
        {
            scan(tree.stats);
        }

        public override void visitDoLoop(JCTree.JCDoWhileLoop tree)
        {
            scan(tree.body);
            scan(tree.cond);
        }

        public override void visitWhileLoop(JCTree.JCWhileLoop tree)
        {
            scan(tree.cond);
            scan(tree.body);
        }

        public override void visitForLoop(JCTree.JCForLoop tree)
        {
            scan(tree.init);
            scan(tree.cond);
            scan(tree.step);
            scan(tree.body);
        }

        public override void visitForeachLoop(JCTree.JCEnhancedForLoop tree)
        {
            scan(tree.@var);
            scan(tree.expr);
            scan(tree.body);
        }

        public override void visitLabelled(JCTree.JCLabeledStatement tree)
        {
            scan(tree.body);
        }

        public override void visitSwitch(JCTree.JCSwitch tree)
        {
            scan(tree.selector);
            scan(tree.cases);
        }

        public override void visitCase(JCTree.JCCase tree)
        {
            scan(tree.pats);
            scan(tree.stats);
        }

        public override void visitSwitchExpression(JCTree.JCSwitchExpression tree)
        {
            scan(tree.selector);
            scan(tree.cases);
        }

        public override void visitSynchronized(JCTree.JCSynchronized tree)
        {
            scan(tree.@lock);
            scan(tree.body);
        }

        public override void visitTry(JCTree.JCTry tree)
        {
            scan(tree.resources);
            scan(tree.body);
            scan(tree.catchers);
            scan(tree.finalizer);
        }

        public override void visitCatch(JCTree.JCCatch tree)
        {
            scan(tree.param);
            scan(tree.body);
        }

        public override void visitConditional(JCTree.JCConditional tree)
        {
            scan(tree.cond);
            scan(tree.truepart);
            scan(tree.falsepart);
        }

        public override void visitIf(JCTree.JCIf tree)
        {
            scan(tree.cond);
            scan(tree.thenpart);
            scan(tree.elsepart);
        }

        public override void visitExec(JCTree.JCExpressionStatement tree)
        {
            scan(tree.expr);
        }

        public override void visitBreak(JCTree.JCBreak tree)
        {
        }

        public override void visitYield(JCTree.JCYield tree)
        {
            scan(tree.value);
        }

        public override void visitContinue(JCTree.JCContinue tree)
        {
        }

        public override void visitReturn(JCTree.JCReturn tree)
        {
            scan(tree.expr);
        }

        public override void visitThrow(JCTree.JCThrow tree)
        {
            scan(tree.expr);
        }

        public override void visitAssert(JCTree.JCAssert tree)
        {
            scan(tree.cond);
            scan(tree.detail);
        }

        public override void visitApply(JCTree.JCMethodInvocation tree)
        {
            scan(tree.typeargs);
            scan(tree.meth);
            scan(tree.args);
        }

        public override void visitNewClass(JCTree.JCNewClass tree)
        {
            scan(tree.encl);
            scan(tree.typeargs);
            scan(tree.clazz);
            scan(tree.args);
            scan(tree.def);
        }

        public override void visitNewArray(JCTree.JCNewArray tree)
        {
            scan(tree.annotations);
            scan(tree.elemtype);
            scan(tree.dims);
            foreach (IList<JCTree.JCAnnotation> annos in tree.dimAnnotations)
            {
                scan(annos);
            }
            scan(tree.elems);
        }

        public override void visitLambda(JCTree.JCLambda tree)
        {
            scan(tree.body);
            scan(tree.@params);
        }

        public override void visitParens(JCTree.JCParens tree)
        {
            scan(tree.expr);
        }

        public override void visitAssign(JCTree.JCAssign tree)
        {
            scan(tree.lhs);
            scan(tree.rhs);
        }

        public override void visitAssignop(JCTree.JCAssignOp tree)
        {
            scan(tree.lhs);
            scan(tree.rhs);
        }

        public override void visitUnary(JCTree.JCUnary tree)
        {
            scan(tree.arg);
        }

        public override void visitBinary(JCTree.JCBinary tree)
        {
            scan(tree.lhs);
            scan(tree.rhs);
        }

        public override void visitTypeCast(JCTree.JCTypeCast tree)
        {
            scan(tree.clazz);
            scan(tree.expr);
        }

        public override void visitTypeTest(JCTree.JCInstanceOf tree)
        {
            scan(tree.expr);
            scan(tree.pattern);
        }

        public override void visitBindingPattern(JCTree.JCBindingPattern tree)
        {
            scan(tree.@var);
        }

        public override void visitIndexed(JCTree.JCArrayAccess tree)
        {
            scan(tree.indexed);
            scan(tree.index);
        }

        public override void visitSelect(JCTree.JCFieldAccess tree)
        {
            scan(tree.selected);
        }

        public override void visitReference(JCTree.JCMemberReference tree)
        {
            scan(tree.expr);
            scan(tree.typeargs);
        }

        public override void visitIdent(JCTree.JCIdent tree)
        {
        }

        public override void visitLiteral(JCTree.JCLiteral tree)
        {
        }

        public override void visitTypeIdent(JCTree.JCPrimitiveTypeTree tree)
        {
        }

        public override void visitTypeArray(JCTree.JCArrayTypeTree tree)
        {
            scan(tree.elemtype);
        }

        public override void visitTypeApply(JCTree.JCTypeApply tree)
        {
            scan(tree.clazz);
            scan(tree.arguments);
        }

        public override void visitTypeUnion(JCTree.JCTypeUnion tree)
        {
            scan(tree.alternatives);
        }

        public override void visitTypeIntersection(JCTree.JCTypeIntersection tree)
        {
            scan(tree.bounds);
        }

        public override void visitTypeParameter(JCTree.JCTypeParameter tree)
        {
            scan(tree.annotations);
            scan(tree.bounds);
        }

        public override void visitWildcard(JCTree.JCWildcard tree)
        {
            scan(tree.kind);
            if (tree.inner != null)
            {
                scan(tree.inner);
            }
        }

        public override void visitTypeBoundKind(JCTree.TypeBoundKind that)
        {
        }

        public override void visitModifiers(JCTree.JCModifiers tree)
        {
            scan(tree.annotations);
        }

        public override void visitAnnotation(JCTree.JCAnnotation tree)
        {
            scan(tree.annotationType);
            scan(tree.args);
        }

        public override void visitAnnotatedType(JCTree.JCAnnotatedType tree)
        {
            scan(tree.annotations);
            scan(tree.underlyingType);
        }

        public override void visitErroneous(JCTree.JCErroneous tree)
        {
        }

        public override void visitLetExpr(JCTree.LetExpr tree)
        {
            scan(tree.defs);
            scan(tree.expr);
        }

        public override void visitTree(JCTree tree)
        {
            Assert.error();
        }
    }

}