/*
 * Copyright (c) 2005, 2019, Oracle and/or its affiliates. All rights reserved.
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

namespace com.sun.source.tree
{
    /// <summary>
    /// A visitor of trees, in the style of the visitor design pattern.
    /// Classes implementing this interface are used to operate
    /// on a tree when the kind of tree is unknown at compile time.
    /// When a visitor is passed to an tree's {@link Tree#accept
    /// accept} method, the <code>visit<i>Xyz</i></code> method most applicable
    /// to that tree is invoked.
    /// 
    /// <para> Classes implementing this interface may or may not throw a
    /// {@code NullPointerException} if the additional parameter {@code p}
    /// is {@code null}; see documentation of the implementing class for
    /// details.
    /// 
    /// </para>
    /// <para> <b>WARNING:</b> It is possible that methods will be added to
    /// this interface to accommodate new, currently unknown, language
    /// structures added to future versions of the Java programming
    /// language.  Therefore, visitor classes directly implementing this
    /// interface may be source incompatible with future versions of the
    /// platform.
    /// 
    /// </para>
    /// </summary>
    /// @param <R> the return type of this visitor's methods.  Use {@link
    ///            Void} for visitors that do not need to return results. </param>
    /// @param <P> the type of the additional parameter to this visitor's
    ///            methods.  Use {@code Void} for visitors that do not need an
    ///            additional parameter.
    /// 
    /// @author Peter von der Ah&eacute;
    /// @author Jonathan Gibbons
    /// 
    /// @since 1.6 </param>
    public interface TreeVisitor<R, P>
    {
        /// <summary>
        /// Visits an AnnotatedTypeTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitAnnotatedType(AnnotatedTypeTree node, P p);

        /// <summary>
        /// Visits an AnnotatedTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitAnnotation(AnnotationTree node, P p);

        /// <summary>
        /// Visits a MethodInvocationTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitMethodInvocation(MethodInvocationTree node, P p);

        /// <summary>
        /// Visits an AssertTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitAssert(AssertTree node, P p);

        /// <summary>
        /// Visits an AssignmentTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitAssignment(AssignmentTree node, P p);

        /// <summary>
        /// Visits a CompoundAssignmentTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitCompoundAssignment(CompoundAssignmentTree node, P p);

        /// <summary>
        /// Visits a BinaryTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitBinary(BinaryTree node, P p);

        /// <summary>
        /// Visits a BlockTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitBlock(BlockTree node, P p);

        /// <summary>
        /// Visits a BreakTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitBreak(BreakTree node, P p);

        /// <summary>
        /// Visits a CaseTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitCase(CaseTree node, P p);

        /// <summary>
        /// Visits a CatchTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitCatch(CatchTree node, P p);

        /// <summary>
        /// Visits a ClassTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitClass(ClassTree node, P p);

        /// <summary>
        /// Visits a ConditionalExpressionTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitConditionalExpression(ConditionalExpressionTree node, P p);

        /// <summary>
        /// Visits a ContinueTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitContinue(ContinueTree node, P p);

        /// <summary>
        /// Visits a DoWhileTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitDoWhileLoop(DoWhileLoopTree node, P p);

        /// <summary>
        /// Visits an ErroneousTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitErroneous(ErroneousTree node, P p);

        /// <summary>
        /// Visits an ExpressionStatementTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitExpressionStatement(ExpressionStatementTree node, P p);

        /// <summary>
        /// Visits an EnhancedForLoopTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitEnhancedForLoop(EnhancedForLoopTree node, P p);

        /// <summary>
        /// Visits a ForLoopTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitForLoop(ForLoopTree node, P p);

        /// <summary>
        /// Visits an IdentifierTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitIdentifier(IdentifierTree node, P p);

        /// <summary>
        /// Visits an IfTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitIf(IfTree node, P p);

        /// <summary>
        /// Visits an ImportTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitImport(ImportTree node, P p);

        /// <summary>
        /// Visits an ArrayAccessTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitArrayAccess(ArrayAccessTree node, P p);

        /// <summary>
        /// Visits a LabeledStatementTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitLabeledStatement(LabeledStatementTree node, P p);

        /// <summary>
        /// Visits a LiteralTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitLiteral(LiteralTree node, P p);

        /// <summary>
        /// Visits an BindingPattern node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value
        /// @since 16 </returns>
        R visitBindingPattern(BindingPatternTree node, P p);

        /// <summary>
        /// Visits a MethodTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitMethod(MethodTree node, P p);

        /// <summary>
        /// Visits a ModifiersTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitModifiers(ModifiersTree node, P p);

        /// <summary>
        /// Visits a NewArrayTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitNewArray(NewArrayTree node, P p);

        /// <summary>
        /// Visits a NewClassTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitNewClass(NewClassTree node, P p);

        /// <summary>
        /// Visits a LambdaExpressionTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitLambdaExpression(LambdaExpressionTree node, P p);

        /// <summary>
        /// Visits a PackageTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitPackage(PackageTree node, P p);

        /// <summary>
        /// Visits a ParenthesizedTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitParenthesized(ParenthesizedTree node, P p);

        /// <summary>
        /// Visits a ReturnTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitReturn(ReturnTree node, P p);

        /// <summary>
        /// Visits a MemberSelectTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitMemberSelect(MemberSelectTree node, P p);

        /// <summary>
        /// Visits a MemberReferenceTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitMemberReference(MemberReferenceTree node, P p);

        /// <summary>
        /// Visits an EmptyStatementTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitEmptyStatement(EmptyStatementTree node, P p);

        /// <summary>
        /// Visits a SwitchTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitSwitch(SwitchTree node, P p);

        /// <summary>
        /// Visits a SwitchExpressionTree node.
        /// </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value
        /// @since 12 </returns>
        R visitSwitchExpression(SwitchExpressionTree node, P p);

        /// <summary>
        /// Visits a SynchronizedTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitSynchronized(SynchronizedTree node, P p);

        /// <summary>
        /// Visits a ThrowTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitThrow(ThrowTree node, P p);

        /// <summary>
        /// Visits a CompilationUnitTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitCompilationUnit(CompilationUnitTree node, P p);

        /// <summary>
        /// Visits a TryTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitTry(TryTree node, P p);

        /// <summary>
        /// Visits a ParameterizedTypeTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitParameterizedType(ParameterizedTypeTree node, P p);

        /// <summary>
        /// Visits a UnionTypeTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitUnionType(UnionTypeTree node, P p);

        /// <summary>
        /// Visits an IntersectionTypeTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitIntersectionType(IntersectionTypeTree node, P p);

        /// <summary>
        /// Visits an ArrayTypeTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitArrayType(ArrayTypeTree node, P p);

        /// <summary>
        /// Visits a TypeCastTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitTypeCast(TypeCastTree node, P p);

        /// <summary>
        /// Visits a PrimitiveTypeTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitPrimitiveType(PrimitiveTypeTree node, P p);

        /// <summary>
        /// Visits a TypeParameterTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitTypeParameter(TypeParameterTree node, P p);

        /// <summary>
        /// Visits an InstanceOfTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitInstanceOf(InstanceOfTree node, P p);

        /// <summary>
        /// Visits a UnaryTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitUnary(UnaryTree node, P p);

        /// <summary>
        /// Visits a VariableTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitVariable(VariableTree node, P p);

        /// <summary>
        /// Visits a WhileLoopTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitWhileLoop(WhileLoopTree node, P p);

        /// <summary>
        /// Visits a WildcardTypeTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitWildcard(WildcardTree node, P p);

        /// <summary>
        /// Visits a ModuleTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitModule(ModuleTree node, P p);

        /// <summary>
        /// Visits an ExportsTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitExports(ExportsTree node, P p);

        /// <summary>
        /// Visits an OpensTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitOpens(OpensTree node, P p);

        /// <summary>
        /// Visits a ProvidesTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitProvides(ProvidesTree node, P p);

        /// <summary>
        /// Visits a RequiresTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitRequires(RequiresTree node, P p);

        /// <summary>
        /// Visits a UsesTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitUses(UsesTree node, P p);

        /// <summary>
        /// Visits an unknown type of Tree node.
        /// This can occur if the language evolves and new kinds
        /// of nodes are added to the {@code Tree} hierarchy. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value </returns>
        R visitOther(Tree node, P p);

        /// <summary>
        /// Visits a YieldTree node. </summary>
        /// <param name="node"> the node being visited </param>
        /// <param name="p"> a parameter value </param>
        /// <returns> a result value
        /// @since 13 </returns>
        R visitYield(YieldTree node, P p);
    }

}