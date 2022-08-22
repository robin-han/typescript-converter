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
    using Name = com.sun.tools.javac.util.Name;

    // using CaseTree = com.sun.source.tree.CaseTree;
    // using ModuleKind = com.sun.source.tree.ModuleTree.ModuleKind;
    using com.sun.tools.javac.code;
    // using UnresolvedClass = com.sun.tools.javac.code.Attribute.UnresolvedClass;
    //using com.sun.tools.javac.code.Symbol;
    //using com.sun.tools.javac.code.Type;
    using com.sun.tools.javac.util;
    using com.sun.source.tree;
    using static com.sun.tools.javac.tree.JCTree;

    // using DiagnosticPosition = com.sun.tools.javac.util.JCDiagnostic.DiagnosticPosition;

    //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //    import static com.sun.tools.javac.code.Flags.*;
    using static com.sun.tools.javac.code.Flags;
    //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //    import static com.sun.tools.javac.code.Kinds.Kind.*;
    //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //    import static com.sun.tools.javac.code.TypeTag.*;
    using static com.sun.tools.javac.code.TypeTag;


    /// <summary>
    /// Factory class for trees.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class TreeMaker : JCTree.Factory
    {
        private bool InstanceFieldsInitialized = false;

        private void InitializeInstanceFields()
        {
            // annotationBuilder = new AnnotationBuilder(this);
        }


        /// <summary>
        /// The context key for the tree factory. </summary>
        protected internal static readonly Context.Key<TreeMaker> treeMakerKey = new Context.Key<TreeMaker>();

        /// <summary>
        /// Get the TreeMaker instance. </summary>
        public static TreeMaker instance(Context context)
        {
            TreeMaker instance = context.get(treeMakerKey);
            if (instance == null)
            {
                instance = new TreeMaker(context);
            }
            return instance;
        }

        /// <summary>
        /// The position at which subsequent trees will be created.
        /// </summary>
        public int pos = Position.NOPOS;

        /// <summary>
        /// The toplevel tree to which created trees belong.
        /// </summary>
        public JCTree.JCCompilationUnit toplevel;

        /// <summary>
        /// The current name table. </summary>
        internal Names names;

        // internal Types types;

        ///// <summary>
        ///// The current symbol table. </summary>
        //internal Symtab syms;

        /// <summary>
        /// Create a tree maker with null toplevel and NOPOS as initial position.
        /// </summary>
        protected internal TreeMaker(Context context)
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
            context.put(treeMakerKey, this);
            this.pos = Position.NOPOS;
            this.toplevel = null;
            this.names = Names.instance(context);
            //this.syms = Symtab.instance(context);
            //this.types = Types.instance(context);
        }

        /// <summary>
        /// Create a tree maker with a given toplevel and FIRSTPOS as initial position.
        /// </summary>
        protected internal TreeMaker(JCTree.JCCompilationUnit toplevel, Names names) //, Types types, Symtab syms)
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
            this.pos = Position.FIRSTPOS;
            this.toplevel = toplevel;
            this.names = names;
            //this.types = types;
            //this.syms = syms;
        }

        /// <summary>
        /// Create a new tree maker for a given toplevel.
        /// </summary>
        public virtual TreeMaker forToplevel(JCTree.JCCompilationUnit toplevel)
        {
            return new TreeMaker(toplevel, names); //, types, syms);
        }

        /// <summary>
        /// Reassign current position.
        /// </summary>
        public virtual TreeMaker at(int pos)
        {
            this.pos = pos;
            return this;
        }

        ///// <summary>
        ///// Reassign current position.
        ///// </summary>
        //public virtual TreeMaker at(DiagnosticPosition pos)
        //{
        //    this.pos = (pos == null ? Position.NOPOS : pos.getStartPosition());
        //    return this;
        //}

        /// <summary>
        /// Create given tree node at current position. </summary>
        /// <param name="defs"> a list of PackageDef, ClassDef, Import, and Skip </param>
        public virtual JCTree.JCCompilationUnit TopLevel(List<JCTree> defs)
        {
            foreach (JCTree node in defs)
            {
                util.Assert.check(node is JCTree.JCClassDecl
                    || node is JCTree.JCPackageDecl
                    || node is JCTree.JCImport
                    || node is JCTree.JCModuleDecl
                    || node is JCTree.JCSkip
                    || node is JCTree.JCErroneous
                    || (node is JCTree.JCExpressionStatement && ((JCTree.JCExpressionStatement)node).expr is JCTree.JCErroneous),
                    () => node.GetType().Name);
            }
            JCTree.JCCompilationUnit tree = new JCTree.JCCompilationUnit(defs);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCPackageDecl PackageDecl(List<JCTree.JCAnnotation> annotations, JCTree.JCExpression pid)
        {
            util.Assert.checkNonNull(annotations);
            util.Assert.checkNonNull(pid);
            JCTree.JCPackageDecl tree = new JCTree.JCPackageDecl(annotations, pid);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCImport Import(JCTree qualid, bool importStatic)
        {
            JCTree.JCImport tree = new JCTree.JCImport(qualid, importStatic);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCClassDecl ClassDef(
                                JCTree.JCModifiers mods,
                                Name name,
                                List<JCTree.JCTypeParameter> typarams,
                                JCTree.JCExpression extending,
                                List<JCTree.JCExpression> implementing,
                                List<JCTree> defs)
        {
            // return ClassDef(mods, name, typarams, extending, implementing, IList.nil(), defs);
            return ClassDef(mods, name, typarams, extending, implementing, new List<JCTree.JCExpression>(), defs);
        }

        public virtual JCTree.JCClassDecl ClassDef(
                                JCTree.JCModifiers mods,
                                Name name,
                                List<JCTree.JCTypeParameter> typarams,
                                JCTree.JCExpression extending,
                                List<JCTree.JCExpression> implementing,
                                List<JCTree.JCExpression> permitting,
                                List<JCTree> defs)
        {
            JCTree.JCClassDecl tree = new JCTree.JCClassDecl(mods, name, typarams, extending, implementing, permitting, defs, null);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCMethodDecl MethodDef(
                                JCTree.JCModifiers mods,
                                Name name,
                                JCTree.JCExpression restype,
                                List<JCTree.JCTypeParameter> typarams,
                                List<JCTree.JCVariableDecl> @params,
                                List<JCTree.JCExpression> thrown,
                                JCTree.JCBlock body,
                                JCTree.JCExpression defaultValue)
        {
            return MethodDef(mods, name, restype, typarams, null, @params, thrown, body, defaultValue);
        }

        public virtual JCTree.JCMethodDecl MethodDef(
                                JCTree.JCModifiers mods,
                                Name name,
                                JCTree.JCExpression restype,
                                List<JCTree.JCTypeParameter> typarams,
                                JCTree.JCVariableDecl recvparam,
                                List<JCTree.JCVariableDecl> @params,
                                List<JCTree.JCExpression> thrown,
                                JCTree.JCBlock body,
                                JCTree.JCExpression defaultValue)
        {
            JCTree.JCMethodDecl tree = new JCTree.JCMethodDecl(
                                            mods,
                                            name,
                                            restype,
                                            typarams,
                                            recvparam,
                                            @params,
                                            thrown,
                                            body,
                                            defaultValue,
                                            null);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCVariableDecl VarDef(JCTree.JCModifiers mods, Name name, JCTree.JCExpression vartype, JCTree.JCExpression init)
        {
            JCTree.JCVariableDecl tree = new JCTree.JCVariableDecl(mods, name, vartype, init, null);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCVariableDecl ReceiverVarDef(JCTree.JCModifiers mods, JCTree.JCExpression name, JCTree.JCExpression vartype)
        {
            JCTree.JCVariableDecl tree = new JCTree.JCVariableDecl(mods, name, vartype);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCSkip Skip()
        {
            JCTree.JCSkip tree = new JCTree.JCSkip();
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCBlock Block(long flags, List<JCTree.JCStatement> stats)
        {
            JCTree.JCBlock tree = new JCTree.JCBlock(flags, stats);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCDoWhileLoop DoLoop(JCTree.JCStatement body, JCTree.JCExpression cond)
        {
            JCTree.JCDoWhileLoop tree = new JCTree.JCDoWhileLoop(body, cond);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCWhileLoop WhileLoop(JCTree.JCExpression cond, JCTree.JCStatement body)
        {
            JCTree.JCWhileLoop tree = new JCTree.JCWhileLoop(cond, body);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCForLoop ForLoop(
                                List<JCTree.JCStatement> init,
                                JCTree.JCExpression cond,
                                List<JCTree.JCExpressionStatement> step,
                                JCTree.JCStatement body)
        {
            JCTree.JCForLoop tree = new JCTree.JCForLoop(init, cond, step, body);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCEnhancedForLoop ForeachLoop(JCTree.JCVariableDecl @var, JCTree.JCExpression expr, JCTree.JCStatement body)
        {
            JCTree.JCEnhancedForLoop tree = new JCTree.JCEnhancedForLoop(@var, expr, body);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCLabeledStatement Labelled(Name label, JCTree.JCStatement body)
        {
            JCTree.JCLabeledStatement tree = new JCTree.JCLabeledStatement(label, body);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCSwitch Switch(JCTree.JCExpression selector, List<JCTree.JCCase> cases)
        {
            JCTree.JCSwitch tree = new JCTree.JCSwitch(selector, cases);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCCase Case(CaseKind caseKind, List<JCTree.JCExpression> pats, List<JCTree.JCStatement> stats, JCTree body)
        {
            JCTree.JCCase tree = new JCTree.JCCase(caseKind, pats, stats, body);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCSwitchExpression SwitchExpression(JCTree.JCExpression selector, List<JCTree.JCCase> cases)
        {
            JCTree.JCSwitchExpression tree = new JCTree.JCSwitchExpression(selector, cases);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCSynchronized Synchronized(JCTree.JCExpression @lock, JCTree.JCBlock body)
        {
            JCTree.JCSynchronized tree = new JCTree.JCSynchronized(@lock, body);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCTry Try(JCTree.JCBlock body, List<JCTree.JCCatch> catchers, JCTree.JCBlock finalizer)
        {
            // return Try(IList.nil(), body, catchers, finalizer);
            return Try(new List<JCTree>(), body, catchers, finalizer);
        }

        public virtual JCTree.JCTry Try(List<JCTree> resources, JCTree.JCBlock body, List<JCTree.JCCatch> catchers, JCTree.JCBlock finalizer)
        {
            JCTree.JCTry tree = new JCTree.JCTry(resources, body, catchers, finalizer);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCCatch Catch(JCTree.JCVariableDecl param, JCTree.JCBlock body)
        {
            JCTree.JCCatch tree = new JCTree.JCCatch(param, body);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCConditional Conditional(JCTree.JCExpression cond, JCTree.JCExpression thenpart, JCTree.JCExpression elsepart)
        {
            JCTree.JCConditional tree = new JCTree.JCConditional(cond, thenpart, elsepart);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCIf If(JCTree.JCExpression cond, JCTree.JCStatement thenpart, JCTree.JCStatement elsepart)
        {
            JCTree.JCIf tree = new JCTree.JCIf(cond, thenpart, elsepart);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCExpressionStatement Exec(JCTree.JCExpression expr)
        {
            JCTree.JCExpressionStatement tree = new JCTree.JCExpressionStatement(expr);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCBreak Break(Name label)
        {
            JCTree.JCBreak tree = new JCTree.JCBreak(label, null);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCYield Yield(JCTree.JCExpression value)
        {
            JCTree.JCYield tree = new JCTree.JCYield(value, null);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCContinue Continue(Name label)
        {
            JCTree.JCContinue tree = new JCTree.JCContinue(label, null);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCReturn Return(JCTree.JCExpression expr)
        {
            JCTree.JCReturn tree = new JCTree.JCReturn(expr);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCThrow Throw(JCTree.JCExpression expr)
        {
            JCTree.JCThrow tree = new JCTree.JCThrow(expr);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCAssert Assert(JCTree.JCExpression cond, JCTree.JCExpression detail)
        {
            JCTree.JCAssert tree = new JCTree.JCAssert(cond, detail);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCMethodInvocation Apply(List<JCTree.JCExpression> typeargs, JCTree.JCExpression fn, List<JCTree.JCExpression> args)
        {
            JCTree.JCMethodInvocation tree = new JCTree.JCMethodInvocation(typeargs, fn, args);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCNewClass NewClass(
                                JCTree.JCExpression encl,
                                List<JCTree.JCExpression> typeargs,
                                JCTree.JCExpression clazz,
                                List<JCTree.JCExpression> args,
                                JCTree.JCClassDecl def)
        {
            return SpeculativeNewClass(encl, typeargs, clazz, args, def, false);
        }

        public virtual JCTree.JCNewClass SpeculativeNewClass(
                                JCTree.JCExpression encl,
                                List<JCTree.JCExpression> typeargs,
                                JCTree.JCExpression clazz,
                                List<JCTree.JCExpression> args,
                                JCTree.JCClassDecl def,
                                bool classDefRemoved)
        {
            JCTree.JCNewClass tree = classDefRemoved ?
                new JCNewClassAnonymousInnerClass(this, encl, typeargs, clazz, args, def) :
                new JCTree.JCNewClass(encl, typeargs, clazz, args, def);
            tree.pos = pos;
            return tree;
        }

        private class JCNewClassAnonymousInnerClass : JCTree.JCNewClass
        {
            private readonly TreeMaker outerInstance;

            public JCNewClassAnonymousInnerClass(
                                    TreeMaker outerInstance,
                                    JCTree.JCExpression encl,
                                    List<JCTree.JCExpression> typeargs,
                                    JCTree.JCExpression clazz,
                                    List<JCTree.JCExpression> args,
                                    JCTree.JCClassDecl def) : base(encl, typeargs, clazz, args, def)
            {
                this.outerInstance = outerInstance;
            }

            public override bool classDeclRemoved()
            {
                return true;
            }
        }

        public virtual JCTree.JCNewArray NewArray(JCTree.JCExpression elemtype, List<JCTree.JCExpression> dims, List<JCTree.JCExpression> elems)
        {
            JCTree.JCNewArray tree = new JCTree.JCNewArray(elemtype, dims, elems);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCLambda Lambda(List<JCTree.JCVariableDecl> @params, JCTree body)
        {
            JCTree.JCLambda tree = new JCTree.JCLambda(@params, body);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCParens Parens(JCTree.JCExpression expr)
        {
            JCTree.JCParens tree = new JCTree.JCParens(expr);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCAssign Assign(JCTree.JCExpression lhs, JCTree.JCExpression rhs)
        {
            JCTree.JCAssign tree = new JCTree.JCAssign(lhs, rhs);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCAssignOp Assignop(JCTree.Tag opcode, JCTree lhs, JCTree rhs)
        {
            JCTree.JCAssignOp tree = new JCTree.JCAssignOp(opcode, lhs, rhs, null);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCUnary Unary(JCTree.Tag opcode, JCTree.JCExpression arg)
        {
            JCTree.JCUnary tree = new JCTree.JCUnary(opcode, arg);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCBinary Binary(JCTree.Tag opcode, JCTree.JCExpression lhs, JCTree.JCExpression rhs)
        {
            JCTree.JCBinary tree = new JCTree.JCBinary(opcode, lhs, rhs, null);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCTypeCast TypeCast(JCTree clazz, JCTree.JCExpression expr)
        {
            JCTree.JCTypeCast tree = new JCTree.JCTypeCast(clazz, expr);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCInstanceOf TypeTest(JCTree.JCExpression expr, JCTree clazz)
        {
            JCTree.JCInstanceOf tree = new JCTree.JCInstanceOf(expr, clazz);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCBindingPattern BindingPattern(JCTree.JCVariableDecl @var)
        {
            JCTree.JCBindingPattern tree = new JCTree.JCBindingPattern(@var);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCArrayAccess Indexed(JCTree.JCExpression indexed, JCTree.JCExpression index)
        {
            JCTree.JCArrayAccess tree = new JCTree.JCArrayAccess(indexed, index);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCFieldAccess Select(JCTree.JCExpression selected, Name selector)
        {
            JCTree.JCFieldAccess tree = new JCTree.JCFieldAccess(selected, selector, null);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCMemberReference Reference(ReferenceMode mode, Name name, JCTree.JCExpression expr, List<JCTree.JCExpression> typeargs)
        {
            JCTree.JCMemberReference tree = new JCTree.JCMemberReference(mode, name, expr, typeargs);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCIdent Ident(Name name)
        {
            JCTree.JCIdent tree = new JCTree.JCIdent(name, null);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCLiteral Literal(TypeTag tag, object value)
        {
            JCTree.JCLiteral tree = new JCTree.JCLiteral(tag, value);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCPrimitiveTypeTree TypeIdent(TypeTag typetag)
        {
            JCTree.JCPrimitiveTypeTree tree = new JCTree.JCPrimitiveTypeTree(typetag);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCArrayTypeTree TypeArray(JCTree.JCExpression elemtype)
        {
            JCTree.JCArrayTypeTree tree = new JCTree.JCArrayTypeTree(elemtype);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCTypeApply TypeApply(JCTree.JCExpression clazz, List<JCTree.JCExpression> arguments)
        {
            JCTree.JCTypeApply tree = new JCTree.JCTypeApply(clazz, arguments);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCTypeUnion TypeUnion(List<JCTree.JCExpression> components)
        {
            JCTree.JCTypeUnion tree = new JCTree.JCTypeUnion(components);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCTypeIntersection TypeIntersection(List<JCTree.JCExpression> components)
        {
            JCTree.JCTypeIntersection tree = new JCTree.JCTypeIntersection(components);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCTypeParameter TypeParameter(Name name, List<JCTree.JCExpression> bounds)
        {
            // return TypeParameter(name, bounds, IList.nil());
            return TypeParameter(name, bounds, new List<JCTree.JCAnnotation>());
        }

        public virtual JCTree.JCTypeParameter TypeParameter(Name name, List<JCTree.JCExpression> bounds, List<JCTree.JCAnnotation> annos)
        {
            JCTree.JCTypeParameter tree = new JCTree.JCTypeParameter(name, bounds, annos);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCWildcard Wildcard(JCTree.TypeBoundKind kind, JCTree type)
        {
            JCTree.JCWildcard tree = new JCTree.JCWildcard(kind, type);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.TypeBoundKind TypeBoundKind(BoundKind kind)
        {
            JCTree.TypeBoundKind tree = new JCTree.TypeBoundKind(kind);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCAnnotation Annotation(JCTree annotationType, List<JCTree.JCExpression> args)
        {
            JCTree.JCAnnotation tree = new JCTree.JCAnnotation(JCTree.Tag.ANNOTATION, annotationType, args);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCAnnotation TypeAnnotation(JCTree annotationType, List<JCTree.JCExpression> args)
        {
            JCTree.JCAnnotation tree = new JCTree.JCAnnotation(JCTree.Tag.TYPE_ANNOTATION, annotationType, args);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCModifiers Modifiers(long flags, List<JCTree.JCAnnotation> annotations)
        {
            JCTree.JCModifiers tree = new JCTree.JCModifiers(flags, annotations);
            bool noFlags = (flags & (Flags.ModifierFlags | (long)Flags.ANNOTATION)) == 0;
            tree.pos = (noFlags && annotations.Count == 0) ? Position.NOPOS : pos;
            return tree;
        }

        public virtual JCTree.JCModifiers Modifiers(long flags)
        {
            // return Modifiers(flags, IList.nil());
            return Modifiers(flags, new List<JCTree.JCAnnotation>());
        }

        public virtual JCTree.JCModuleDecl ModuleDef(JCTree.JCModifiers mods, ModuleKind kind, JCTree.JCExpression qualid, List<JCTree.JCDirective> directives)
        {
            JCTree.JCModuleDecl tree = new JCTree.JCModuleDecl(mods, kind, qualid, directives);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCExports Exports(JCTree.JCExpression qualId, List<JCTree.JCExpression> moduleNames)
        {
            JCTree.JCExports tree = new JCTree.JCExports(qualId, moduleNames);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCOpens Opens(JCTree.JCExpression qualId, List<JCTree.JCExpression> moduleNames)
        {
            JCTree.JCOpens tree = new JCTree.JCOpens(qualId, moduleNames);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCProvides Provides(JCTree.JCExpression serviceName, List<JCTree.JCExpression> implNames)
        {
            JCTree.JCProvides tree = new JCTree.JCProvides(serviceName, implNames);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCRequires Requires(bool isTransitive, bool isStaticPhase, JCTree.JCExpression qualId)
        {
            JCTree.JCRequires tree = new JCTree.JCRequires(isTransitive, isStaticPhase, qualId);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCUses Uses(JCTree.JCExpression qualId)
        {
            JCTree.JCUses tree = new JCTree.JCUses(qualId);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCAnnotatedType AnnotatedType(List<JCTree.JCAnnotation> annotations, JCTree.JCExpression underlyingType)
        {
            JCTree.JCAnnotatedType tree = new JCTree.JCAnnotatedType(annotations, underlyingType);
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.JCErroneous Erroneous()
        {
            // return Erroneous(IList.nil());
            return Erroneous(new List<JCTree>());
        }

        public virtual JCTree.JCErroneous Erroneous<T1>(List<T1> errs) where T1 : JCTree
        {
            JCTree.JCErroneous tree = new JCTree.JCErroneous(new List<JCTree>(errs));
            tree.pos = pos;
            return tree;
        }

        public virtual JCTree.LetExpr LetExpr(List<JCTree.JCStatement> defs, JCTree.JCExpression expr)
        {
            JCTree.LetExpr tree = new JCTree.LetExpr(defs, expr);
            tree.pos = pos;
            return tree;
        }

        /* ***************************************************************************
         * Derived building blocks.
         ****************************************************************************/

        public virtual JCTree.JCClassDecl AnonymousClassDef(JCTree.JCModifiers mods, List<JCTree> defs)
        {
            // return ClassDef(mods, names.empty, IList.nil(), null, IList.nil(), defs);
            return ClassDef(mods, names.empty, new List<JCTree.JCTypeParameter>(), null, new List<JCTree.JCExpression>(), defs);
        }

        public virtual JCTree.LetExpr LetExpr(JCTree.JCVariableDecl def, JCTree.JCExpression expr)
        {
            // JCTree.LetExpr tree = new JCTree.LetExpr(IList.of(def), expr);
            JCTree.LetExpr tree = new JCTree.LetExpr(new List<JCTree.JCStatement>() { def }, expr);
            tree.pos = pos;
            return tree;
        }

        ///// <summary>
        ///// Create an identifier from a symbol.
        ///// </summary>
        //public virtual JCTree.JCIdent Ident(Symbol sym)
        //{
        //    return (JCTree.JCIdent)(new JCTree.JCIdent((sym.name != names.empty) ? sym.name : sym.flatName(), sym)).setPos(pos).setType(sym.type);
        //}

        ///// <summary>
        ///// Create a selection node from a qualifier tree and a symbol. </summary>
        /////  <param name="base">   The qualifier tree. </param>
        //public virtual JCTree.JCExpression Select(JCTree.JCExpression @base, Symbol sym)
        //{
        //    return (new JCTree.JCFieldAccess(@base, sym.name, sym)).setPos(pos).setType(sym.type);
        //}

        ///// <summary>
        ///// Create a qualified identifier from a symbol, adding enough qualifications
        /////  to make the reference unique.
        ///// </summary>
        //public virtual JCTree.JCExpression QualIdent(Symbol sym)
        //{
        //    return isUnqualifiable(sym) ? Ident(sym) : Select(QualIdent(sym.owner), sym);
        //}

        ///// <summary>
        ///// Create an identifier that refers to the variable declared in given variable
        /////  declaration.
        ///// </summary>
        //public virtual JCTree.JCExpression Ident(JCTree.JCVariableDecl param)
        //{
        //    return Ident(param.sym);
        //}

        ///// <summary>
        ///// Create a list of identifiers referring to the variables declared
        /////  in given list of variable declarations.
        ///// </summary>
        //public virtual List<JCTree.JCExpression> Idents(List<JCTree.JCVariableDecl> @params)
        //{
        //    ListBuffer<JCTree.JCExpression> ids = new ListBuffer<JCTree.JCExpression>();
        //    for (List<JCTree.JCVariableDecl> l = @params; l.nonEmpty(); l = l.tail)
        //    {
        //        ids.append(Ident(l.head));
        //    }
        //    return ids.toList();
        //}

        ///// <summary>
        ///// Create a tree representing `this', given its type.
        ///// </summary>
        //public virtual JCTree.JCExpression This(Type t)
        //{
        //    return Ident(new VarSymbol(FINAL, names._this, t, t.tsym));
        //}

        ///// <summary>
        ///// Create a tree representing qualified `this' given its type
        ///// </summary>
        //public virtual JCTree.JCExpression QualThis(Type t)
        //{
        //    return Select(Type(t), new VarSymbol(FINAL, names._this, t, t.tsym));
        //}

        ///// <summary>
        ///// Create a tree representing a class literal.
        ///// </summary>
        //public virtual JCTree.JCExpression ClassLiteral(ClassSymbol clazz)
        //{
        //    return ClassLiteral(clazz.type);
        //}

        ///// <summary>
        ///// Create a tree representing a class literal.
        ///// </summary>
        //public virtual JCTree.JCExpression ClassLiteral(Type t)
        //{
        //    VarSymbol lit = new VarSymbol(STATIC | PUBLIC | FINAL, names._class, t, t.tsym);
        //    return Select(Type(t), lit);
        //}

        ///// <summary>
        ///// Create a tree representing `super', given its type and owner.
        ///// </summary>
        //public virtual JCTree.JCIdent Super(Type t, TypeSymbol owner)
        //{
        //    return Ident(new VarSymbol(FINAL, names._super, t, owner));
        //}

        ///// <summary>
        ///// Create a method invocation from a method tree and a list of
        ///// argument trees.
        ///// </summary>
        //public virtual JCTree.JCMethodInvocation App(JCTree.JCExpression meth, List<JCTree.JCExpression> args)
        //{
        //    return Apply(null, meth, args).setType(meth.type.getReturnType());
        //}

        ///// <summary>
        ///// Create a no-arg method invocation from a method tree
        ///// </summary>
        //public virtual JCTree.JCMethodInvocation App(JCTree.JCExpression meth)
        //{
        //    return Apply(null, meth, IList.nil()).setType(meth.type.getReturnType());
        //}

        ///// <summary>
        ///// Create a method invocation from a method tree and a list of argument trees.
        ///// </summary>
        //public virtual JCTree.JCExpression Create(Symbol ctor, List<JCTree.JCExpression> args)
        //{
        //    Type t = ctor.owner.erasure(types);
        //    JCTree.JCNewClass newclass = NewClass(null, null, Type(t), args, null);
        //    newclass.constructor = ctor;
        //    newclass.setType(t);
        //    return newclass;
        //}

        ///// <summary>
        ///// Create a tree representing given type.
        ///// </summary>
        //public virtual JCTree.JCExpression Type(Type t)
        //{
        //    if (t == null)
        //    {
        //        return null;
        //    }
        //    JCTree.JCExpression tp;
        //    switch (t.getTag())
        //    {
        //        case BYTE:
        //        case CHAR:
        //        case SHORT:
        //        case INT:
        //        case LONG:
        //        case FLOAT:
        //        case DOUBLE:
        //        case BOOLEAN:
        //        case VOID:
        //            tp = TypeIdent(t.getTag());
        //            break;
        //        case TYPEVAR:
        //            tp = Ident(t.tsym);
        //            break;
        //        case WILDCARD:
        //            {
        //                WildcardType a = ((WildcardType)t);
        //                tp = Wildcard(TypeBoundKind(a.kind), a.kind == BoundKind.UNBOUND ? null : Type(a.type));
        //                break;
        //            }
        //        case CLASS:
        //            switch (t.getKind())
        //            {
        //                case UNION:
        //                    {
        //                        UnionClassType tu = (UnionClassType)t;
        //                        ListBuffer<JCTree.JCExpression> la = new ListBuffer<JCTree.JCExpression>();
        //                        foreach (Type ta in tu.getAlternativeTypes())
        //                        {
        //                            la.add(Type(ta));
        //                        }
        //                        tp = TypeUnion(la.toList());
        //                        break;
        //                    }
        //                case INTERSECTION:
        //                    {
        //                        IntersectionClassType it = (IntersectionClassType)t;
        //                        ListBuffer<JCTree.JCExpression> la = new ListBuffer<JCTree.JCExpression>();
        //                        foreach (Type ta in it.getExplicitComponents())
        //                        {
        //                            la.add(Type(ta));
        //                        }
        //                        tp = TypeIntersection(la.toList());
        //                        break;
        //                    }
        //                default:
        //                    {
        //                        Type outer = t.getEnclosingType();
        //                        JCTree.JCExpression clazz = outer.hasTag(CLASS) && t.tsym.owner.kind == TYP ? Select(Type(outer), t.tsym) : QualIdent(t.tsym);
        //                        tp = t.getTypeArguments().isEmpty() ? clazz : TypeApply(clazz, Types(t.getTypeArguments()));
        //                        break;
        //                    }
        //            }
        //            break;
        //        case ARRAY:
        //            tp = TypeArray(Type(types.elemtype(t)));
        //            break;
        //        case ERROR:
        //            tp = TypeIdent(ERROR);
        //            break;
        //        default:
        //            throw new AssertionError("unexpected type: " + t);
        //    }
        //    return tp.setType(t);
        //}

        ///// <summary>
        ///// Create a list of trees representing given list of types.
        ///// </summary>
        //public virtual List<JCTree.JCExpression> Types(List<Type> ts)
        //{
        //    ListBuffer<JCTree.JCExpression> lb = new ListBuffer<JCTree.JCExpression>();
        //    for (List<Type> l = ts; l.nonEmpty(); l = l.tail)
        //    {
        //        lb.append(Type(l.head));
        //    }
        //    return lb.toList();
        //}

        ///// <summary>
        ///// Create a variable definition from a variable symbol and an initializer
        /////  expression.
        ///// </summary>
        //public virtual JCTree.JCVariableDecl VarDef(VarSymbol v, JCTree.JCExpression init)
        //{
        //    return (JCTree.JCVariableDecl)(new JCTree.JCVariableDecl(Modifiers(v.flags(), Annotations(v.getRawAttributes())), v.name, Type(v.type), init, v)).setPos(pos).setType(v.type);
        //}

        ///// <summary>
        ///// Create annotation trees from annotations.
        ///// </summary>
        //public virtual List<JCTree.JCAnnotation> Annotations(List<Attribute.Compound> attributes)
        //{
        //    if (attributes == null)
        //    {
        //        return IList.nil();
        //    }
        //    ListBuffer<JCTree.JCAnnotation> result = new ListBuffer<JCTree.JCAnnotation>();
        //    for (List<Attribute.Compound> i = attributes; i.nonEmpty(); i = i.tail)
        //    {
        //        Attribute a = i.head;
        //        result.append(Annotation(a));
        //    }
        //    return result.toList();
        //}

        //public virtual JCTree.JCLiteral Literal(object value)
        //{
        //    JCTree.JCLiteral result = null;
        //    if (value is string)
        //    {
        //        result = Literal(CLASS, value).setType(syms.stringType.constType(value));
        //    }
        //    else if (value is int?)
        //    {
        //        result = Literal(INT, value).setType(syms.intType.constType(value));
        //    }
        //    else if (value is long?)
        //    {
        //        result = Literal(LONG, value).setType(syms.longType.constType(value));
        //    }
        //    else if (value is sbyte?)
        //    {
        //        result = Literal(BYTE, value).setType(syms.byteType.constType(value));
        //    }
        //    else if (value is char?)
        //    {
        //        int v = (int)(((char?)value).ToString()[0]);
        //        result = Literal(CHAR, v).setType(syms.charType.constType(v));
        //    }
        //    else if (value is double?)
        //    {
        //        result = Literal(DOUBLE, value).setType(syms.doubleType.constType(value));
        //    }
        //    else if (value is float?)
        //    {
        //        result = Literal(FLOAT, value).setType(syms.floatType.constType(value));
        //    }
        //    else if (value is short?)
        //    {
        //        result = Literal(SHORT, value).setType(syms.shortType.constType(value));
        //    }
        //    else if (value is bool?)
        //    {
        //        int v = ((bool?)value) ? 1 : 0;
        //        result = Literal(BOOLEAN, v).setType(syms.booleanType.constType(v));
        //    }
        //    else
        //    {
        //        throw new AssertionError(value);
        //    }
        //    return result;
        //}

        //internal class AnnotationBuilder : Attribute.Visitor
        //{
        //    private readonly TreeMaker outerInstance;

        //    public AnnotationBuilder(TreeMaker outerInstance)
        //    {
        //        this.outerInstance = outerInstance;
        //    }

        //    internal JCTree.JCExpression result = null;
        //    public virtual void visitConstant(Attribute.Constant v)
        //    {
        //        result = outerInstance.Literal(v.type.getTag(), v.value);
        //    }
        //    public virtual void visitClass(Attribute.Class clazz)
        //    {
        //        result = outerInstance.ClassLiteral(clazz.classType).setType(syms.classType);
        //    }
        //    public virtual void visitEnum(Attribute.Enum e)
        //    {
        //        result = outerInstance.QualIdent(e.value);
        //    }
        //    public virtual void visitError(Attribute.Error e)
        //    {
        //        if (e is UnresolvedClass)
        //        {
        //            result = outerInstance.ClassLiteral(((UnresolvedClass)e).classType).setType(syms.classType);
        //        }
        //        else
        //        {
        //            result = outerInstance.Erroneous();
        //        }
        //    }
        //    public virtual void visitCompound(Attribute.Compound compound)
        //    {
        //        if (compound is Attribute.TypeCompound)
        //        {
        //            result = visitTypeCompoundInternal((Attribute.TypeCompound)compound);
        //        }
        //        else
        //        {
        //            result = visitCompoundInternal(compound);
        //        }
        //    }
        //    public virtual JCTree.JCAnnotation visitCompoundInternal(Attribute.Compound compound)
        //    {
        //        ListBuffer<JCTree.JCExpression> args = new ListBuffer<JCTree.JCExpression>();
        //        for (List<Pair<Symbol.MethodSymbol, Attribute>> values = compound.values; values.nonEmpty(); values = values.tail)
        //        {
        //            Pair<MethodSymbol, Attribute> pair = values.head;
        //            JCTree.JCExpression valueTree = translate(pair.snd);
        //            args.append(outerInstance.Assign(outerInstance.Ident(pair.fst), valueTree).setType(valueTree.type));
        //        }
        //        return outerInstance.Annotation(outerInstance.Type(compound.type), args.toList());
        //    }
        //    public virtual JCTree.JCAnnotation visitTypeCompoundInternal(Attribute.TypeCompound compound)
        //    {
        //        ListBuffer<JCTree.JCExpression> args = new ListBuffer<JCTree.JCExpression>();
        //        for (List<Pair<Symbol.MethodSymbol, Attribute>> values = compound.values; values.nonEmpty(); values = values.tail)
        //        {
        //            Pair<MethodSymbol, Attribute> pair = values.head;
        //            JCTree.JCExpression valueTree = translate(pair.snd);
        //            args.append(outerInstance.Assign(outerInstance.Ident(pair.fst), valueTree).setType(valueTree.type));
        //        }
        //        return outerInstance.TypeAnnotation(outerInstance.Type(compound.type), args.toList());
        //    }
        //    public virtual void visitArray(Attribute.Array array)
        //    {
        //        ListBuffer<JCTree.JCExpression> elems = new ListBuffer<JCTree.JCExpression>();
        //        for (int i = 0; i < array.values.length; i++)
        //        {
        //            elems.append(translate(array.values[i]));
        //        }
        //        result = outerInstance.NewArray(null, IList.nil(), elems.toList()).setType(array.type);
        //    }
        //    internal virtual JCTree.JCExpression translate(Attribute a)
        //    {
        //        a.accept(this);
        //        return result;
        //    }
        //    internal virtual JCTree.JCAnnotation translate(Attribute.Compound a)
        //    {
        //        return visitCompoundInternal(a);
        //    }
        //    internal virtual JCTree.JCAnnotation translate(Attribute.TypeCompound a)
        //    {
        //        return visitTypeCompoundInternal(a);
        //    }
        //}

        //internal AnnotationBuilder annotationBuilder;

        ///// <summary>
        ///// Create an annotation tree from an attribute.
        ///// </summary>
        //public virtual JCTree.JCAnnotation Annotation(Attribute a)
        //{
        //    return annotationBuilder.translate((Attribute.Compound)a);
        //}

        //public virtual JCTree.JCAnnotation TypeAnnotation(Attribute a)
        //{
        //    return annotationBuilder.translate((Attribute.TypeCompound)a);
        //}

        ///// <summary>
        ///// Create a method definition from a method symbol and a method body.
        ///// </summary>
        //public virtual JCTree.JCMethodDecl MethodDef(MethodSymbol m, JCTree.JCBlock body)
        //{
        //    return MethodDef(m, m.type, body);
        //}

        ///// <summary>
        ///// Create a method definition from a method symbol, method type
        /////  and a method body.
        ///// </summary>
        //public virtual JCTree.JCMethodDecl MethodDef(MethodSymbol m, Type mtype, JCTree.JCBlock body)
        //{
        //    return (JCTree.JCMethodDecl)(new JCTree.JCMethodDecl(Modifiers(m.flags(), Annotations(m.getRawAttributes())), m.name, Type(mtype.getReturnType()), TypeParams(mtype.getTypeArguments()), null, Params(mtype.getParameterTypes(), m), Types(mtype.getThrownTypes()), body, null, m)).setPos(pos).setType(mtype); // receiver type
        //}

        ///// <summary>
        ///// Create a type parameter tree from its name and type.
        ///// </summary>
        //public virtual JCTree.JCTypeParameter TypeParam(Name name, TypeVar tvar)
        //{
        //    return (JCTree.JCTypeParameter)TypeParameter(name, Types(types.getBounds(tvar))).setPos(pos).setType(tvar);
        //}

        ///// <summary>
        ///// Create a list of type parameter trees from a list of type variables.
        ///// </summary>
        //public virtual List<JCTree.JCTypeParameter> TypeParams(List<Type> typarams)
        //{
        //    ListBuffer<JCTree.JCTypeParameter> tparams = new ListBuffer<JCTree.JCTypeParameter>();
        //    for (List<Type> l = typarams; l.nonEmpty(); l = l.tail)
        //    {
        //        tparams.append(TypeParam(l.head.tsym.name, (TypeVar)l.head));
        //    }
        //    return tparams.toList();
        //}

        ///// <summary>
        ///// Create a value parameter tree from its name, type, and owner.
        ///// </summary>
        //public virtual JCTree.JCVariableDecl Param(Name name, Type argtype, Symbol owner)
        //{
        //    return VarDef(new VarSymbol(PARAMETER, name, argtype, owner), null);
        //}

        ///// <summary>
        ///// Create a a list of value parameter trees x0, ..., xn from a list of
        /////  their types and an their owner.
        ///// </summary>
        //public virtual List<JCTree.JCVariableDecl> Params(List<Type> argtypes, Symbol owner)
        //{
        //    ListBuffer<JCTree.JCVariableDecl> @params = new ListBuffer<JCTree.JCVariableDecl>();
        //    MethodSymbol mth = (owner.kind == MTH) ? ((MethodSymbol)owner) : null;
        //    if (mth != null && mth.@params != null && argtypes.length() == mth.@params.length())
        //    {
        //        foreach (VarSymbol param in ((MethodSymbol)owner).@params)
        //        {
        //            @params.append(VarDef(param, null));
        //        }
        //    }
        //    else
        //    {
        //        int i = 0;
        //        for (List<Type> l = argtypes; l.nonEmpty(); l = l.tail)
        //        {
        //            @params.append(Param(paramName(i++), l.head, owner));
        //        }
        //    }
        //    return @params.toList();
        //}

        ///// <summary>
        ///// Wrap a method invocation in an expression statement or return statement,
        /////  depending on whether the method invocation expression's type is void.
        ///// </summary>
        //public virtual JCTree.JCStatement Call(JCTree.JCExpression apply)
        //{
        //    return apply.type.hasTag(VOID) ? Exec(apply) : Return(apply);
        //}

        ///// <summary>
        ///// Construct an assignment from a variable symbol and a right hand side.
        ///// </summary>
        //public virtual JCTree.JCStatement Assignment(Symbol v, JCTree.JCExpression rhs)
        //{
        //    return Exec(Assign(Ident(v), rhs).setType(v.type));
        //}

        ///// <summary>
        ///// Construct an index expression from a variable and an expression.
        ///// </summary>
        //public virtual JCTree.JCArrayAccess Indexed(Symbol v, JCTree.JCExpression index)
        //{
        //    JCTree.JCArrayAccess tree = new JCTree.JCArrayAccess(QualIdent(v), index);
        //    tree.type = ((ArrayType)v.type).elemtype;
        //    return tree;
        //}

        ///// <summary>
        ///// Make an attributed type cast expression.
        ///// </summary>
        //public virtual JCTree.JCTypeCast TypeCast(Type type, JCTree.JCExpression expr)
        //{
        //    return (JCTree.JCTypeCast)TypeCast(Type(type), expr).setType(type);
        //}

        /* ***************************************************************************
         * Helper methods.
         ****************************************************************************/

        ///// <summary>
        ///// Can given symbol be referred to in unqualified form?
        ///// </summary>
        //internal virtual bool isUnqualifiable(Symbol sym)
        //{
        //    if (sym.name == names.empty || sym.owner == null || sym.owner == syms.rootPackage || sym.owner.kind == MTH || sym.owner.kind == VAR)
        //    {
        //        return true;
        //    }
        //    else if (sym.kind == TYP && toplevel != null)
        //    {
        //        IEnumerator<Symbol> it = toplevel.namedImportScope.getSymbolsByName(sym.name).GetEnumerator();
        //        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //        if (it.hasNext())
        //        {
        //            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //            Symbol s = it.next();
        //            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //            return s == sym && !it.hasNext();
        //        }
        //        it = toplevel.packge.members().getSymbolsByName(sym.name).GetEnumerator();
        //        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //        if (it.hasNext())
        //        {
        //            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //            Symbol s = it.next();
        //            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //            return s == sym && !it.hasNext();
        //        }
        //        it = toplevel.starImportScope.getSymbolsByName(sym.name).GetEnumerator();
        //        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //        if (it.hasNext())
        //        {
        //            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //            Symbol s = it.next();
        //            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //            return s == sym && !it.hasNext();
        //        }
        //    }
        //    return false;
        //}

        /// <summary>
        /// The name of synthetic parameter number `i'.
        /// </summary>
        public virtual Name paramName(int i)
        {
            return names.fromString("x" + i);
        }

        /// <summary>
        /// The name of synthetic type parameter number `i'.
        /// </summary>
        public virtual Name typaramName(int i)
        {
            return names.fromString("A" + i);
        }
    }

}