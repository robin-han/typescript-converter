using System.Collections.Generic;

/*
 * Copyright (c) 2005, 2020, Oracle and/or its affiliates. All rights reserved.
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
    /// Common interface for all nodes in an abstract syntax tree.
    /// 
    /// <para><b>WARNING:</b> This interface and its sub-interfaces are
    /// subject to change as the Java programming language evolves.
    /// These interfaces are implemented by the JDK Java compiler (javac)
    /// and should not be implemented either directly or indirectly by
    /// other applications.
    /// 
    /// @author Peter von der Ah&eacute;
    /// @author Jonathan Gibbons
    /// 
    /// @since 1.6
    /// </para>
    /// </summary>
    public interface Tree
    {

        /// <summary>
        /// Enumerates all kinds of trees.
        /// </summary>

        /// <summary>
        /// Returns the kind of this tree.
        /// </summary>
        /// <returns> the kind of this tree </returns>
        Kind getKind();

        /// <summary>
        /// Accept method used to implement the visitor pattern.  The
        /// visitor pattern is used to implement operations on trees.
        /// </summary>
        /// @param <R> the result type of this operation </param>
        /// @param <D> the type of additional data </param>
        /// <param name="visitor"> the visitor to be called </param>
        /// <param name="data"> a value to be passed to the visitor </param>
        /// <returns> the result returned from calling the visitor </returns>
        R accept<R, D>(TreeVisitor<R, D> visitor, D data);
    }


    public enum Kind
    {
        ANNOTATED_TYPE,
        ANNOTATION,
        TYPE_ANNOTATION,
        ARRAY_ACCESS,
        ARRAY_TYPE,
        ASSERT,
        ASSIGNMENT,
        BLOCK,
        BREAK,
        CASE,
        CATCH,
        CLASS,
        COMPILATION_UNIT,
        CONDITIONAL_EXPRESSION,
        CONTINUE,
        DO_WHILE_LOOP,
        ENHANCED_FOR_LOOP,
        EXPRESSION_STATEMENT,
        MEMBER_SELECT,
        MEMBER_REFERENCE,
        FOR_LOOP,
        IDENTIFIER,
        IF,
        IMPORT,
        INSTANCE_OF,
        LABELED_STATEMENT,
        METHOD,
        METHOD_INVOCATION,
        MODIFIERS,
        NEW_ARRAY,
        NEW_CLASS,
        LAMBDA_EXPRESSION,
        PACKAGE,
        PARENTHESIZED,
        BINDING_PATTERN,
        PRIMITIVE_TYPE,
        RETURN,
        EMPTY_STATEMENT,
        SWITCH,
        SWITCH_EXPRESSION,
        SYNCHRONIZED,
        THROW,
        TRY,
        PARAMETERIZED_TYPE,
        UNION_TYPE,
        INTERSECTION_TYPE,
        TYPE_CAST,
        TYPE_PARAMETER,
        VARIABLE,
        WHILE_LOOP,
        POSTFIX_INCREMENT,
        POSTFIX_DECREMENT,
        PREFIX_INCREMENT,
        PREFIX_DECREMENT,
        UNARY_PLUS,
        UNARY_MINUS,
        BITWISE_COMPLEMENT,
        LOGICAL_COMPLEMENT,
        MULTIPLY,
        DIVIDE,
        REMAINDER,
        PLUS,
        MINUS,
        LEFT_SHIFT,
        RIGHT_SHIFT,
        UNSIGNED_RIGHT_SHIFT,
        LESS_THAN,
        GREATER_THAN,
        LESS_THAN_EQUAL,
        GREATER_THAN_EQUAL,
        EQUAL_TO,
        NOT_EQUAL_TO,
        AND,
        XOR,
        OR,
        CONDITIONAL_AND,
        CONDITIONAL_OR,
        MULTIPLY_ASSIGNMENT,
        DIVIDE_ASSIGNMENT,
        REMAINDER_ASSIGNMENT,
        PLUS_ASSIGNMENT,
        MINUS_ASSIGNMENT,
        LEFT_SHIFT_ASSIGNMENT,
        RIGHT_SHIFT_ASSIGNMENT,
        UNSIGNED_RIGHT_SHIFT_ASSIGNMENT,
        AND_ASSIGNMENT,
        XOR_ASSIGNMENT,
        OR_ASSIGNMENT,
        INT_LITERAL,
        LONG_LITERAL,
        FLOAT_LITERAL,
        DOUBLE_LITERAL,
        BOOLEAN_LITERAL,
        CHAR_LITERAL,
        STRING_LITERAL,
        NULL_LITERAL,
        UNBOUNDED_WILDCARD,
        EXTENDS_WILDCARD,
        SUPER_WILDCARD,
        ERRONEOUS,
        INTERFACE,
        ENUM,
        ANNOTATION_TYPE,
        MODULE,
        EXPORTS,
        OPENS,
        PROVIDES,
        RECORD,
        REQUIRES,
        USES,
        OTHER,
        YIELD
    }


    //public sealed class Tree_Kind
    //{
    //    /// <summary>
    //    /// Used for instances of <seealso cref="AnnotatedTypeTree"/>
    //    /// representing annotated types.
    //    /// </summary>
    //    public static readonly Tree_Kind ANNOTATED_TYPE = new Tree_Kind("ANNOTATED_TYPE", InnerEnum.ANNOTATED_TYPE, typeof(AnnotatedTypeTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="AnnotationTree"/>
    //    /// representing declaration annotations.
    //    /// </summary>
    //    public static readonly Tree_Kind ANNOTATION = new Tree_Kind("ANNOTATION", InnerEnum.ANNOTATION, typeof(AnnotationTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="AnnotationTree"/>
    //    /// representing type annotations.
    //    /// </summary>
    //    public static readonly Tree_Kind TYPE_ANNOTATION = new Tree_Kind("TYPE_ANNOTATION", InnerEnum.TYPE_ANNOTATION, typeof(AnnotationTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ArrayAccessTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind ARRAY_ACCESS = new Tree_Kind("ARRAY_ACCESS", InnerEnum.ARRAY_ACCESS, typeof(ArrayAccessTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ArrayTypeTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind ARRAY_TYPE = new Tree_Kind("ARRAY_TYPE", InnerEnum.ARRAY_TYPE, typeof(ArrayTypeTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="AssertTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind ASSERT = new Tree_Kind("ASSERT", InnerEnum.ASSERT, typeof(AssertTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="AssignmentTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind ASSIGNMENT = new Tree_Kind("ASSIGNMENT", InnerEnum.ASSIGNMENT, typeof(AssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BlockTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind BLOCK = new Tree_Kind("BLOCK", InnerEnum.BLOCK, typeof(BlockTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BreakTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind BREAK = new Tree_Kind("BREAK", InnerEnum.BREAK, typeof(BreakTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CaseTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind CASE = new Tree_Kind("CASE", InnerEnum.CASE, typeof(CaseTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CatchTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind CATCH = new Tree_Kind("CATCH", InnerEnum.CATCH, typeof(CatchTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ClassTree"/> representing classes.
    //    /// </summary>
    //    public static readonly Tree_Kind CLASS = new Tree_Kind("CLASS", InnerEnum.CLASS, typeof(ClassTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompilationUnitTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind COMPILATION_UNIT = new Tree_Kind("COMPILATION_UNIT", InnerEnum.COMPILATION_UNIT, typeof(CompilationUnitTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ConditionalExpressionTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind CONDITIONAL_EXPRESSION = new Tree_Kind("CONDITIONAL_EXPRESSION", InnerEnum.CONDITIONAL_EXPRESSION, typeof(ConditionalExpressionTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ContinueTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind CONTINUE = new Tree_Kind("CONTINUE", InnerEnum.CONTINUE, typeof(ContinueTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="DoWhileLoopTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind DO_WHILE_LOOP = new Tree_Kind("DO_WHILE_LOOP", InnerEnum.DO_WHILE_LOOP, typeof(DoWhileLoopTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="EnhancedForLoopTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind ENHANCED_FOR_LOOP = new Tree_Kind("ENHANCED_FOR_LOOP", InnerEnum.ENHANCED_FOR_LOOP, typeof(EnhancedForLoopTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ExpressionStatementTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind EXPRESSION_STATEMENT = new Tree_Kind("EXPRESSION_STATEMENT", InnerEnum.EXPRESSION_STATEMENT, typeof(ExpressionStatementTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="MemberSelectTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind MEMBER_SELECT = new Tree_Kind("MEMBER_SELECT", InnerEnum.MEMBER_SELECT, typeof(MemberSelectTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="MemberReferenceTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind MEMBER_REFERENCE = new Tree_Kind("MEMBER_REFERENCE", InnerEnum.MEMBER_REFERENCE, typeof(MemberReferenceTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ForLoopTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind FOR_LOOP = new Tree_Kind("FOR_LOOP", InnerEnum.FOR_LOOP, typeof(ForLoopTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="IdentifierTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind IDENTIFIER = new Tree_Kind("IDENTIFIER", InnerEnum.IDENTIFIER, typeof(IdentifierTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="IfTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind IF = new Tree_Kind("IF", InnerEnum.IF, typeof(IfTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ImportTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind IMPORT = new Tree_Kind("IMPORT", InnerEnum.IMPORT, typeof(ImportTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="InstanceOfTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind INSTANCE_OF = new Tree_Kind("INSTANCE_OF", InnerEnum.INSTANCE_OF, typeof(InstanceOfTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="LabeledStatementTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind LABELED_STATEMENT = new Tree_Kind("LABELED_STATEMENT", InnerEnum.LABELED_STATEMENT, typeof(LabeledStatementTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="MethodTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind METHOD = new Tree_Kind("METHOD", InnerEnum.METHOD, typeof(MethodTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="MethodInvocationTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind METHOD_INVOCATION = new Tree_Kind("METHOD_INVOCATION", InnerEnum.METHOD_INVOCATION, typeof(MethodInvocationTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ModifiersTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind MODIFIERS = new Tree_Kind("MODIFIERS", InnerEnum.MODIFIERS, typeof(ModifiersTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="NewArrayTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind NEW_ARRAY = new Tree_Kind("NEW_ARRAY", InnerEnum.NEW_ARRAY, typeof(NewArrayTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="NewClassTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind NEW_CLASS = new Tree_Kind("NEW_CLASS", InnerEnum.NEW_CLASS, typeof(NewClassTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="LambdaExpressionTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind LAMBDA_EXPRESSION = new Tree_Kind("LAMBDA_EXPRESSION", InnerEnum.LAMBDA_EXPRESSION, typeof(LambdaExpressionTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="PackageTree"/>.
    //    /// @since 9
    //    /// </summary>
    //    public static readonly Tree_Kind PACKAGE = new Tree_Kind("PACKAGE", InnerEnum.PACKAGE, typeof(PackageTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ParenthesizedTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind PARENTHESIZED = new Tree_Kind("PARENTHESIZED", InnerEnum.PARENTHESIZED, typeof(ParenthesizedTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BindingPatternTree"/>.
    //    /// 
    //    /// @since 16
    //    /// </summary>
    //    public static readonly Tree_Kind BINDING_PATTERN = new Tree_Kind("BINDING_PATTERN", InnerEnum.BINDING_PATTERN, typeof(BindingPatternTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="PrimitiveTypeTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind PRIMITIVE_TYPE = new Tree_Kind("PRIMITIVE_TYPE", InnerEnum.PRIMITIVE_TYPE, typeof(PrimitiveTypeTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ReturnTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind RETURN = new Tree_Kind("RETURN", InnerEnum.RETURN, typeof(ReturnTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="EmptyStatementTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind EMPTY_STATEMENT = new Tree_Kind("EMPTY_STATEMENT", InnerEnum.EMPTY_STATEMENT, typeof(EmptyStatementTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="SwitchTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind SWITCH = new Tree_Kind("SWITCH", InnerEnum.SWITCH, typeof(SwitchTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="SwitchExpressionTree"/>.
    //    /// 
    //    /// @since 12
    //    /// </summary>
    //    public static readonly Tree_Kind SWITCH_EXPRESSION = new Tree_Kind("SWITCH_EXPRESSION", InnerEnum.SWITCH_EXPRESSION, typeof(SwitchExpressionTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="SynchronizedTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind SYNCHRONIZED = new Tree_Kind("SYNCHRONIZED", InnerEnum.SYNCHRONIZED, typeof(SynchronizedTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ThrowTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind THROW = new Tree_Kind("THROW", InnerEnum.THROW, typeof(ThrowTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="TryTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind TRY = new Tree_Kind("TRY", InnerEnum.TRY, typeof(TryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ParameterizedTypeTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind PARAMETERIZED_TYPE = new Tree_Kind("PARAMETERIZED_TYPE", InnerEnum.PARAMETERIZED_TYPE, typeof(ParameterizedTypeTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="UnionTypeTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind UNION_TYPE = new Tree_Kind("UNION_TYPE", InnerEnum.UNION_TYPE, typeof(UnionTypeTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="IntersectionTypeTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind INTERSECTION_TYPE = new Tree_Kind("INTERSECTION_TYPE", InnerEnum.INTERSECTION_TYPE, typeof(IntersectionTypeTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="TypeCastTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind TYPE_CAST = new Tree_Kind("TYPE_CAST", InnerEnum.TYPE_CAST, typeof(TypeCastTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="TypeParameterTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind TYPE_PARAMETER = new Tree_Kind("TYPE_PARAMETER", InnerEnum.TYPE_PARAMETER, typeof(TypeParameterTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="VariableTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind VARIABLE = new Tree_Kind("VARIABLE", InnerEnum.VARIABLE, typeof(VariableTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="WhileLoopTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind WHILE_LOOP = new Tree_Kind("WHILE_LOOP", InnerEnum.WHILE_LOOP, typeof(WhileLoopTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="UnaryTree"/> representing postfix
    //    /// increment operator {@code ++}.
    //    /// </summary>
    //    public static readonly Tree_Kind POSTFIX_INCREMENT = new Tree_Kind("POSTFIX_INCREMENT", InnerEnum.POSTFIX_INCREMENT, typeof(UnaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="UnaryTree"/> representing postfix
    //    /// decrement operator {@code --}.
    //    /// </summary>
    //    public static readonly Tree_Kind POSTFIX_DECREMENT = new Tree_Kind("POSTFIX_DECREMENT", InnerEnum.POSTFIX_DECREMENT, typeof(UnaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="UnaryTree"/> representing prefix
    //    /// increment operator {@code ++}.
    //    /// </summary>
    //    public static readonly Tree_Kind PREFIX_INCREMENT = new Tree_Kind("PREFIX_INCREMENT", InnerEnum.PREFIX_INCREMENT, typeof(UnaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="UnaryTree"/> representing prefix
    //    /// decrement operator {@code --}.
    //    /// </summary>
    //    public static readonly Tree_Kind PREFIX_DECREMENT = new Tree_Kind("PREFIX_DECREMENT", InnerEnum.PREFIX_DECREMENT, typeof(UnaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="UnaryTree"/> representing unary plus
    //    /// operator {@code +}.
    //    /// </summary>
    //    public static readonly Tree_Kind UNARY_PLUS = new Tree_Kind("UNARY_PLUS", InnerEnum.UNARY_PLUS, typeof(UnaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="UnaryTree"/> representing unary minus
    //    /// operator {@code -}.
    //    /// </summary>
    //    public static readonly Tree_Kind UNARY_MINUS = new Tree_Kind("UNARY_MINUS", InnerEnum.UNARY_MINUS, typeof(UnaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="UnaryTree"/> representing bitwise
    //    /// complement operator {@code ~}.
    //    /// </summary>
    //    public static readonly Tree_Kind BITWISE_COMPLEMENT = new Tree_Kind("BITWISE_COMPLEMENT", InnerEnum.BITWISE_COMPLEMENT, typeof(UnaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="UnaryTree"/> representing logical
    //    /// complement operator {@code !}.
    //    /// </summary>
    //    public static readonly Tree_Kind LOGICAL_COMPLEMENT = new Tree_Kind("LOGICAL_COMPLEMENT", InnerEnum.LOGICAL_COMPLEMENT, typeof(UnaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// multiplication {@code *}.
    //    /// </summary>
    //    public static readonly Tree_Kind MULTIPLY = new Tree_Kind("MULTIPLY", InnerEnum.MULTIPLY, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// division {@code /}.
    //    /// </summary>
    //    public static readonly Tree_Kind DIVIDE = new Tree_Kind("DIVIDE", InnerEnum.DIVIDE, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// remainder {@code %}.
    //    /// </summary>
    //    public static readonly Tree_Kind REMAINDER = new Tree_Kind("REMAINDER", InnerEnum.REMAINDER, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// addition or string concatenation {@code +}.
    //    /// </summary>
    //    public static readonly Tree_Kind PLUS = new Tree_Kind("PLUS", InnerEnum.PLUS, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// subtraction {@code -}.
    //    /// </summary>
    //    public static readonly Tree_Kind MINUS = new Tree_Kind("MINUS", InnerEnum.MINUS, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// left shift {@code <<}.
    //    /// </summary>
    //    public static readonly Tree_Kind LEFT_SHIFT = new Tree_Kind("LEFT_SHIFT", InnerEnum.LEFT_SHIFT, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// right shift {@code >>}.
    //    /// </summary>
    //    public static readonly Tree_Kind RIGHT_SHIFT = new Tree_Kind("RIGHT_SHIFT", InnerEnum.RIGHT_SHIFT, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// unsigned right shift {@code >>>}.
    //    /// </summary>
    //    public static readonly Tree_Kind UNSIGNED_RIGHT_SHIFT = new Tree_Kind("UNSIGNED_RIGHT_SHIFT", InnerEnum.UNSIGNED_RIGHT_SHIFT, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// less-than {@code <}.
    //    /// </summary>
    //    public static readonly Tree_Kind LESS_THAN = new Tree_Kind("LESS_THAN", InnerEnum.LESS_THAN, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// greater-than {@code >}.
    //    /// </summary>
    //    public static readonly Tree_Kind GREATER_THAN = new Tree_Kind("GREATER_THAN", InnerEnum.GREATER_THAN, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// less-than-equal {@code <=}.
    //    /// </summary>
    //    public static readonly Tree_Kind LESS_THAN_EQUAL = new Tree_Kind("LESS_THAN_EQUAL", InnerEnum.LESS_THAN_EQUAL, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// greater-than-equal {@code >=}.
    //    /// </summary>
    //    public static readonly Tree_Kind GREATER_THAN_EQUAL = new Tree_Kind("GREATER_THAN_EQUAL", InnerEnum.GREATER_THAN_EQUAL, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// equal-to {@code ==}.
    //    /// </summary>
    //    public static readonly Tree_Kind EQUAL_TO = new Tree_Kind("EQUAL_TO", InnerEnum.EQUAL_TO, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// not-equal-to {@code !=}.
    //    /// </summary>
    //    public static readonly Tree_Kind NOT_EQUAL_TO = new Tree_Kind("NOT_EQUAL_TO", InnerEnum.NOT_EQUAL_TO, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// bitwise and logical "and" {@code &}.
    //    /// </summary>
    //    public static readonly Tree_Kind AND = new Tree_Kind("AND", InnerEnum.AND, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// bitwise and logical "xor" {@code ^}.
    //    /// </summary>
    //    public static readonly Tree_Kind XOR = new Tree_Kind("XOR", InnerEnum.XOR, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// bitwise and logical "or" {@code |}.
    //    /// </summary>
    //    public static readonly Tree_Kind OR = new Tree_Kind("OR", InnerEnum.OR, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// conditional-and {@code &&}.
    //    /// </summary>
    //    public static readonly Tree_Kind CONDITIONAL_AND = new Tree_Kind("CONDITIONAL_AND", InnerEnum.CONDITIONAL_AND, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="BinaryTree"/> representing
    //    /// conditional-or {@code ||}.
    //    /// </summary>
    //    public static readonly Tree_Kind CONDITIONAL_OR = new Tree_Kind("CONDITIONAL_OR", InnerEnum.CONDITIONAL_OR, typeof(BinaryTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// multiplication assignment {@code *=}.
    //    /// </summary>
    //    public static readonly Tree_Kind MULTIPLY_ASSIGNMENT = new Tree_Kind("MULTIPLY_ASSIGNMENT", InnerEnum.MULTIPLY_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// division assignment {@code /=}.
    //    /// </summary>
    //    public static readonly Tree_Kind DIVIDE_ASSIGNMENT = new Tree_Kind("DIVIDE_ASSIGNMENT", InnerEnum.DIVIDE_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// remainder assignment {@code %=}.
    //    /// </summary>
    //    public static readonly Tree_Kind REMAINDER_ASSIGNMENT = new Tree_Kind("REMAINDER_ASSIGNMENT", InnerEnum.REMAINDER_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// addition or string concatenation assignment {@code +=}.
    //    /// </summary>
    //    public static readonly Tree_Kind PLUS_ASSIGNMENT = new Tree_Kind("PLUS_ASSIGNMENT", InnerEnum.PLUS_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// subtraction assignment {@code -=}.
    //    /// </summary>
    //    public static readonly Tree_Kind MINUS_ASSIGNMENT = new Tree_Kind("MINUS_ASSIGNMENT", InnerEnum.MINUS_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// left shift assignment {@code <<=}.
    //    /// </summary>
    //    public static readonly Tree_Kind LEFT_SHIFT_ASSIGNMENT = new Tree_Kind("LEFT_SHIFT_ASSIGNMENT", InnerEnum.LEFT_SHIFT_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// right shift assignment {@code >>=}.
    //    /// </summary>
    //    public static readonly Tree_Kind RIGHT_SHIFT_ASSIGNMENT = new Tree_Kind("RIGHT_SHIFT_ASSIGNMENT", InnerEnum.RIGHT_SHIFT_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// unsigned right shift assignment {@code >>>=}.
    //    /// </summary>
    //    public static readonly Tree_Kind UNSIGNED_RIGHT_SHIFT_ASSIGNMENT = new Tree_Kind("UNSIGNED_RIGHT_SHIFT_ASSIGNMENT", InnerEnum.UNSIGNED_RIGHT_SHIFT_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// bitwise and logical "and" assignment {@code &=}.
    //    /// </summary>
    //    public static readonly Tree_Kind AND_ASSIGNMENT = new Tree_Kind("AND_ASSIGNMENT", InnerEnum.AND_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// bitwise and logical "xor" assignment {@code ^=}.
    //    /// </summary>
    //    public static readonly Tree_Kind XOR_ASSIGNMENT = new Tree_Kind("XOR_ASSIGNMENT", InnerEnum.XOR_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="CompoundAssignmentTree"/> representing
    //    /// bitwise and logical "or" assignment {@code |=}.
    //    /// </summary>
    //    public static readonly Tree_Kind OR_ASSIGNMENT = new Tree_Kind("OR_ASSIGNMENT", InnerEnum.OR_ASSIGNMENT, typeof(CompoundAssignmentTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="LiteralTree"/> representing
    //    /// an integral literal expression of type {@code int}.
    //    /// </summary>
    //    public static readonly Tree_Kind INT_LITERAL = new Tree_Kind("INT_LITERAL", InnerEnum.INT_LITERAL, typeof(LiteralTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="LiteralTree"/> representing
    //    /// an integral literal expression of type {@code long}.
    //    /// </summary>
    //    public static readonly Tree_Kind LONG_LITERAL = new Tree_Kind("LONG_LITERAL", InnerEnum.LONG_LITERAL, typeof(LiteralTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="LiteralTree"/> representing
    //    /// a floating-point literal expression of type {@code float}.
    //    /// </summary>
    //    public static readonly Tree_Kind FLOAT_LITERAL = new Tree_Kind("FLOAT_LITERAL", InnerEnum.FLOAT_LITERAL, typeof(LiteralTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="LiteralTree"/> representing
    //    /// a floating-point literal expression of type {@code double}.
    //    /// </summary>
    //    public static readonly Tree_Kind DOUBLE_LITERAL = new Tree_Kind("DOUBLE_LITERAL", InnerEnum.DOUBLE_LITERAL, typeof(LiteralTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="LiteralTree"/> representing
    //    /// a boolean literal expression of type {@code boolean}.
    //    /// </summary>
    //    public static readonly Tree_Kind BOOLEAN_LITERAL = new Tree_Kind("BOOLEAN_LITERAL", InnerEnum.BOOLEAN_LITERAL, typeof(LiteralTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="LiteralTree"/> representing
    //    /// a character literal expression of type {@code char}.
    //    /// </summary>
    //    public static readonly Tree_Kind CHAR_LITERAL = new Tree_Kind("CHAR_LITERAL", InnerEnum.CHAR_LITERAL, typeof(LiteralTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="LiteralTree"/> representing
    //    /// a string literal expression of type <seealso cref="String"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind STRING_LITERAL = new Tree_Kind("STRING_LITERAL", InnerEnum.STRING_LITERAL, typeof(LiteralTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="LiteralTree"/> representing
    //    /// the use of {@code null}.
    //    /// </summary>
    //    public static readonly Tree_Kind NULL_LITERAL = new Tree_Kind("NULL_LITERAL", InnerEnum.NULL_LITERAL, typeof(LiteralTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="WildcardTree"/> representing
    //    /// an unbounded wildcard type argument.
    //    /// </summary>
    //    public static readonly Tree_Kind UNBOUNDED_WILDCARD = new Tree_Kind("UNBOUNDED_WILDCARD", InnerEnum.UNBOUNDED_WILDCARD, typeof(WildcardTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="WildcardTree"/> representing
    //    /// an extends bounded wildcard type argument.
    //    /// </summary>
    //    public static readonly Tree_Kind EXTENDS_WILDCARD = new Tree_Kind("EXTENDS_WILDCARD", InnerEnum.EXTENDS_WILDCARD, typeof(WildcardTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="WildcardTree"/> representing
    //    /// a super bounded wildcard type argument.
    //    /// </summary>
    //    public static readonly Tree_Kind SUPER_WILDCARD = new Tree_Kind("SUPER_WILDCARD", InnerEnum.SUPER_WILDCARD, typeof(WildcardTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ErroneousTree"/>.
    //    /// </summary>
    //    public static readonly Tree_Kind ERRONEOUS = new Tree_Kind("ERRONEOUS", InnerEnum.ERRONEOUS, typeof(ErroneousTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ClassTree"/> representing interfaces.
    //    /// </summary>
    //    public static readonly Tree_Kind INTERFACE = new Tree_Kind("INTERFACE", InnerEnum.INTERFACE, typeof(ClassTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ClassTree"/> representing enums.
    //    /// </summary>
    //    public static readonly Tree_Kind ENUM = new Tree_Kind("ENUM", InnerEnum.ENUM, typeof(ClassTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ClassTree"/> representing annotation types.
    //    /// </summary>
    //    public static readonly Tree_Kind ANNOTATION_TYPE = new Tree_Kind("ANNOTATION_TYPE", InnerEnum.ANNOTATION_TYPE, typeof(ClassTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ModuleTree"/> representing module declarations.
    //    /// </summary>
    //    public static readonly Tree_Kind MODULE = new Tree_Kind("MODULE", InnerEnum.MODULE, typeof(ModuleTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ExportsTree"/> representing
    //    /// exports directives in a module declaration.
    //    /// </summary>
    //    public static readonly Tree_Kind EXPORTS = new Tree_Kind("EXPORTS", InnerEnum.EXPORTS, typeof(ExportsTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ExportsTree"/> representing
    //    /// opens directives in a module declaration.
    //    /// </summary>
    //    public static readonly Tree_Kind OPENS = new Tree_Kind("OPENS", InnerEnum.OPENS, typeof(OpensTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ProvidesTree"/> representing
    //    /// provides directives in a module declaration.
    //    /// </summary>
    //    public static readonly Tree_Kind PROVIDES = new Tree_Kind("PROVIDES", InnerEnum.PROVIDES, typeof(ProvidesTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="ClassTree"/> representing records.
    //    /// @since 16
    //    /// </summary>
    //    public static readonly Tree_Kind RECORD = new Tree_Kind("RECORD", InnerEnum.RECORD, typeof(ClassTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="RequiresTree"/> representing
    //    /// requires directives in a module declaration.
    //    /// </summary>
    //    public static readonly Tree_Kind REQUIRES = new Tree_Kind("REQUIRES", InnerEnum.REQUIRES, typeof(RequiresTree));

    //    /// <summary>
    //    /// Used for instances of <seealso cref="UsesTree"/> representing
    //    /// uses directives in a module declaration.
    //    /// </summary>
    //    public static readonly Tree_Kind USES = new Tree_Kind("USES", InnerEnum.USES, typeof(UsesTree));

    //    /// <summary>
    //    /// An implementation-reserved node. This is the not the node
    //    /// you are looking for.
    //    /// </summary>
    //    public static readonly Tree_Kind OTHER = new Tree_Kind("OTHER", InnerEnum.OTHER, null);

    //    /// <summary>
    //    /// Used for instances of <seealso cref="YieldTree"/>.
    //    /// 
    //    /// @since 13
    //    /// </summary>
    //    public static readonly Tree_Kind YIELD = new Tree_Kind("YIELD", InnerEnum.YIELD, typeof(YieldTree));

    //    private static readonly IList<Tree_Kind> valueList = new List<Tree_Kind>();

    //    static Tree_Kind()
    //    {
    //        valueList.Add(ANNOTATED_TYPE);
    //        valueList.Add(ANNOTATION);
    //        valueList.Add(TYPE_ANNOTATION);
    //        valueList.Add(ARRAY_ACCESS);
    //        valueList.Add(ARRAY_TYPE);
    //        valueList.Add(ASSERT);
    //        valueList.Add(ASSIGNMENT);
    //        valueList.Add(BLOCK);
    //        valueList.Add(BREAK);
    //        valueList.Add(CASE);
    //        valueList.Add(CATCH);
    //        valueList.Add(CLASS);
    //        valueList.Add(COMPILATION_UNIT);
    //        valueList.Add(CONDITIONAL_EXPRESSION);
    //        valueList.Add(CONTINUE);
    //        valueList.Add(DO_WHILE_LOOP);
    //        valueList.Add(ENHANCED_FOR_LOOP);
    //        valueList.Add(EXPRESSION_STATEMENT);
    //        valueList.Add(MEMBER_SELECT);
    //        valueList.Add(MEMBER_REFERENCE);
    //        valueList.Add(FOR_LOOP);
    //        valueList.Add(IDENTIFIER);
    //        valueList.Add(IF);
    //        valueList.Add(IMPORT);
    //        valueList.Add(INSTANCE_OF);
    //        valueList.Add(LABELED_STATEMENT);
    //        valueList.Add(METHOD);
    //        valueList.Add(METHOD_INVOCATION);
    //        valueList.Add(MODIFIERS);
    //        valueList.Add(NEW_ARRAY);
    //        valueList.Add(NEW_CLASS);
    //        valueList.Add(LAMBDA_EXPRESSION);
    //        valueList.Add(PACKAGE);
    //        valueList.Add(PARENTHESIZED);
    //        valueList.Add(BINDING_PATTERN);
    //        valueList.Add(PRIMITIVE_TYPE);
    //        valueList.Add(RETURN);
    //        valueList.Add(EMPTY_STATEMENT);
    //        valueList.Add(SWITCH);
    //        valueList.Add(SWITCH_EXPRESSION);
    //        valueList.Add(SYNCHRONIZED);
    //        valueList.Add(THROW);
    //        valueList.Add(TRY);
    //        valueList.Add(PARAMETERIZED_TYPE);
    //        valueList.Add(UNION_TYPE);
    //        valueList.Add(INTERSECTION_TYPE);
    //        valueList.Add(TYPE_CAST);
    //        valueList.Add(TYPE_PARAMETER);
    //        valueList.Add(VARIABLE);
    //        valueList.Add(WHILE_LOOP);
    //        valueList.Add(POSTFIX_INCREMENT);
    //        valueList.Add(POSTFIX_DECREMENT);
    //        valueList.Add(PREFIX_INCREMENT);
    //        valueList.Add(PREFIX_DECREMENT);
    //        valueList.Add(UNARY_PLUS);
    //        valueList.Add(UNARY_MINUS);
    //        valueList.Add(BITWISE_COMPLEMENT);
    //        valueList.Add(LOGICAL_COMPLEMENT);
    //        valueList.Add(MULTIPLY);
    //        valueList.Add(DIVIDE);
    //        valueList.Add(REMAINDER);
    //        valueList.Add(PLUS);
    //        valueList.Add(MINUS);
    //        valueList.Add(LEFT_SHIFT);
    //        valueList.Add(RIGHT_SHIFT);
    //        valueList.Add(UNSIGNED_RIGHT_SHIFT);
    //        valueList.Add(LESS_THAN);
    //        valueList.Add(GREATER_THAN);
    //        valueList.Add(LESS_THAN_EQUAL);
    //        valueList.Add(GREATER_THAN_EQUAL);
    //        valueList.Add(EQUAL_TO);
    //        valueList.Add(NOT_EQUAL_TO);
    //        valueList.Add(AND);
    //        valueList.Add(XOR);
    //        valueList.Add(OR);
    //        valueList.Add(CONDITIONAL_AND);
    //        valueList.Add(CONDITIONAL_OR);
    //        valueList.Add(MULTIPLY_ASSIGNMENT);
    //        valueList.Add(DIVIDE_ASSIGNMENT);
    //        valueList.Add(REMAINDER_ASSIGNMENT);
    //        valueList.Add(PLUS_ASSIGNMENT);
    //        valueList.Add(MINUS_ASSIGNMENT);
    //        valueList.Add(LEFT_SHIFT_ASSIGNMENT);
    //        valueList.Add(RIGHT_SHIFT_ASSIGNMENT);
    //        valueList.Add(UNSIGNED_RIGHT_SHIFT_ASSIGNMENT);
    //        valueList.Add(AND_ASSIGNMENT);
    //        valueList.Add(XOR_ASSIGNMENT);
    //        valueList.Add(OR_ASSIGNMENT);
    //        valueList.Add(INT_LITERAL);
    //        valueList.Add(LONG_LITERAL);
    //        valueList.Add(FLOAT_LITERAL);
    //        valueList.Add(DOUBLE_LITERAL);
    //        valueList.Add(BOOLEAN_LITERAL);
    //        valueList.Add(CHAR_LITERAL);
    //        valueList.Add(STRING_LITERAL);
    //        valueList.Add(NULL_LITERAL);
    //        valueList.Add(UNBOUNDED_WILDCARD);
    //        valueList.Add(EXTENDS_WILDCARD);
    //        valueList.Add(SUPER_WILDCARD);
    //        valueList.Add(ERRONEOUS);
    //        valueList.Add(INTERFACE);
    //        valueList.Add(ENUM);
    //        valueList.Add(ANNOTATION_TYPE);
    //        valueList.Add(MODULE);
    //        valueList.Add(EXPORTS);
    //        valueList.Add(OPENS);
    //        valueList.Add(PROVIDES);
    //        valueList.Add(RECORD);
    //        valueList.Add(REQUIRES);
    //        valueList.Add(USES);
    //        valueList.Add(OTHER);
    //        valueList.Add(YIELD);
    //    }

    //    public enum InnerEnum
    //    {
    //        ANNOTATED_TYPE,
    //        ANNOTATION,
    //        TYPE_ANNOTATION,
    //        ARRAY_ACCESS,
    //        ARRAY_TYPE,
    //        ASSERT,
    //        ASSIGNMENT,
    //        BLOCK,
    //        BREAK,
    //        CASE,
    //        CATCH,
    //        CLASS,
    //        COMPILATION_UNIT,
    //        CONDITIONAL_EXPRESSION,
    //        CONTINUE,
    //        DO_WHILE_LOOP,
    //        ENHANCED_FOR_LOOP,
    //        EXPRESSION_STATEMENT,
    //        MEMBER_SELECT,
    //        MEMBER_REFERENCE,
    //        FOR_LOOP,
    //        IDENTIFIER,
    //        IF,
    //        IMPORT,
    //        INSTANCE_OF,
    //        LABELED_STATEMENT,
    //        METHOD,
    //        METHOD_INVOCATION,
    //        MODIFIERS,
    //        NEW_ARRAY,
    //        NEW_CLASS,
    //        LAMBDA_EXPRESSION,
    //        PACKAGE,
    //        PARENTHESIZED,
    //        BINDING_PATTERN,
    //        PRIMITIVE_TYPE,
    //        RETURN,
    //        EMPTY_STATEMENT,
    //        SWITCH,
    //        SWITCH_EXPRESSION,
    //        SYNCHRONIZED,
    //        THROW,
    //        TRY,
    //        PARAMETERIZED_TYPE,
    //        UNION_TYPE,
    //        INTERSECTION_TYPE,
    //        TYPE_CAST,
    //        TYPE_PARAMETER,
    //        VARIABLE,
    //        WHILE_LOOP,
    //        POSTFIX_INCREMENT,
    //        POSTFIX_DECREMENT,
    //        PREFIX_INCREMENT,
    //        PREFIX_DECREMENT,
    //        UNARY_PLUS,
    //        UNARY_MINUS,
    //        BITWISE_COMPLEMENT,
    //        LOGICAL_COMPLEMENT,
    //        MULTIPLY,
    //        DIVIDE,
    //        REMAINDER,
    //        PLUS,
    //        MINUS,
    //        LEFT_SHIFT,
    //        RIGHT_SHIFT,
    //        UNSIGNED_RIGHT_SHIFT,
    //        LESS_THAN,
    //        GREATER_THAN,
    //        LESS_THAN_EQUAL,
    //        GREATER_THAN_EQUAL,
    //        EQUAL_TO,
    //        NOT_EQUAL_TO,
    //        AND,
    //        XOR,
    //        OR,
    //        CONDITIONAL_AND,
    //        CONDITIONAL_OR,
    //        MULTIPLY_ASSIGNMENT,
    //        DIVIDE_ASSIGNMENT,
    //        REMAINDER_ASSIGNMENT,
    //        PLUS_ASSIGNMENT,
    //        MINUS_ASSIGNMENT,
    //        LEFT_SHIFT_ASSIGNMENT,
    //        RIGHT_SHIFT_ASSIGNMENT,
    //        UNSIGNED_RIGHT_SHIFT_ASSIGNMENT,
    //        AND_ASSIGNMENT,
    //        XOR_ASSIGNMENT,
    //        OR_ASSIGNMENT,
    //        INT_LITERAL,
    //        LONG_LITERAL,
    //        FLOAT_LITERAL,
    //        DOUBLE_LITERAL,
    //        BOOLEAN_LITERAL,
    //        CHAR_LITERAL,
    //        STRING_LITERAL,
    //        NULL_LITERAL,
    //        UNBOUNDED_WILDCARD,
    //        EXTENDS_WILDCARD,
    //        SUPER_WILDCARD,
    //        ERRONEOUS,
    //        INTERFACE,
    //        ENUM,
    //        ANNOTATION_TYPE,
    //        MODULE,
    //        EXPORTS,
    //        OPENS,
    //        PROVIDES,
    //        RECORD,
    //        REQUIRES,
    //        USES,
    //        OTHER,
    //        YIELD
    //    }

    //    public readonly InnerEnum innerEnumValue;
    //    private readonly string nameValue;
    //    private readonly int ordinalValue;
    //    private static int nextOrdinal = 0;


    //    internal Tree_Kind(string name, InnerEnum innerEnum, System.Type intf)
    //    {
    //        associatedInterface = intf;

    //        nameValue = name;
    //        ordinalValue = nextOrdinal++;
    //        innerEnumValue = innerEnum;
    //    }

    //    /// <summary>
    //    /// Returns the associated interface type that uses this kind. </summary>
    //    /// <returns> the associated interface </returns>
    //    public System.Type asInterface()
    //    {
    //        return associatedInterface;
    //    }

    //    internal readonly System.Type associatedInterface;

    //    public static IList<Tree_Kind> values()
    //    {
    //        return valueList;
    //    }

    //    public int ordinal()
    //    {
    //        return ordinalValue;
    //    }

    //    public override string ToString()
    //    {
    //        return nameValue;
    //    }

    //    public static Tree_Kind valueOf(string name)
    //    {
    //        foreach (Tree_Kind enumInstance in Tree_Kind.valueList)
    //        {
    //            if (enumInstance.nameValue == name)
    //            {
    //                return enumInstance;
    //            }
    //        }
    //        throw new System.ArgumentException(name);
    //    }
    //}

}