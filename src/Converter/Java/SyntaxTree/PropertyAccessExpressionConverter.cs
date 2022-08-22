using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TypeScript.Syntax;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;

namespace TypeScript.Converter.Java
{
    public class PropertyAccessExpressionConverter : NodeConverter
    {
        private static readonly Name STRING_LENGTH_NAME = Names.fromString("length");
        private static readonly Name ARRAY_LENGTH_NAME = Names.fromString("size");
        private static readonly Name HASHMAP_LENGTH_NAME = Names.fromString("size");

        public JCTree Convert(PropertyAccessExpression node)
        {
            JCExpression exprTree = ConvertToExpressionTree(node);

            if (!string.IsNullOrEmpty(node.As))
            {
                string asType = NormalizeTypeName(TrimTypeName(node.As));
                exprTree = AsExpressionConverter.AsType(exprTree, asType);
            }

            return exprTree;
        }

        private JCExpression ConvertToExpressionTree(PropertyAccessExpression node)
        {
            Name name = Names.fromString(node.Name.Text);

            // remove this ...
            if (ShouldRemoveThisExpression(node))
            {
                if (ShouldConvertToMethod(node))
                {
                    name = GetMethodName(node);
                    return TreeMaker.Apply(
                        Nil<JCExpression>(),
                        TreeMaker.Ident(name),
                        Nil<JCExpression>()
                    );
                }
                else
                {
                    return TreeMaker.Ident(name);
                }
            }

            // omit names(dv. core.)
            if (this.IsQualifiedName(node.Expression.Text))
            {
                return TreeMaker.Ident(Names.fromString(NormalizeTypeName(node.Name.Text)));
            }

            // get method
            if (ShouldConvertToMethod(node)) //convert to get
            {
                name = GetMethodName(node);
                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(node.Expression.ToJavaSyntaxTree<JCExpression>(), name),
                    Nil<JCExpression>()
                );
            }

            return TreeMaker.Select(node.Expression.ToJavaSyntaxTree<JCExpression>(), name);
        }

        /// <summary>
        /// The method name when property should convert to method.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Name GetMethodName(PropertyAccessExpression node)
        {
            string name = node.Name.Text;
            if (name == "length")
            {
                Node exprType = TypeHelper.GetNodeType(node.Expression);
                if (TypeHelper.IsStringType(exprType))
                {
                    return STRING_LENGTH_NAME;
                }
                else if (TypeHelper.IsArrayType(exprType))
                {
                    return ARRAY_LENGTH_NAME;
                }
                else if (TypeHelper.IsDictionaryType(exprType))
                {
                    return HASHMAP_LENGTH_NAME;
                }
                else
                {
                    return Names.fromString(name);
                }
            }

            return Names.fromString(name.ToGetMethodName());
        }

        /// <summary>
        /// Indicate whether remove this from this.xxx
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ShouldRemoveThisExpression(PropertyAccessExpression node)
        {
            // this.xxx
            if (node.Expression.Kind == NodeKind.ThisKeyword)
            {
                Node parent = node.Parent;
                while (parent != null)
                {
                    if (parent.Kind == NodeKind.ArrowFunction)
                    {
                        return true;
                    }
                    if (parent.Kind == NodeKind.MethodDeclaration)
                    {
                        return ((MethodDeclaration)parent).IsStatic;
                    }
                    if (parent.Kind == NodeKind.ClassDeclaration)
                    {
                        return false;
                    }
                    if (parent.Kind == NodeKind.ObjectLiteralExpression)
                    {
                        return ObjectLiteralExpressionConverter.ShouldConvertToAnonymousInnerClass((ObjectLiteralExpression)parent);
                    }

                    parent = parent.Parent;
                }
            }
            return false;
        }

        /// <summary>
        /// Indicate whether convert a.b to a.b();
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ShouldConvertToMethod(PropertyAccessExpression node)
        {
            // a.b();
            if (node.Parent.Kind == NodeKind.CallExpression && ((CallExpression)node.Parent).Expression == node)
            {
                return false;
            }

            // assign a.b = 100
            if (node.Parent.Kind == NodeKind.BinaryExpression)
            {
                BinaryExpression binary = (BinaryExpression)node.Parent;
                if (binary.Left == node && binary.OperatorToken.Kind == NodeKind.EqualsToken)
                {
                    return false;
                }
            }

            // this.xxx
            if (node.Expression.Kind == NodeKind.ThisKeyword)
            {
                var @class = node.Ancestor<ClassDeclaration>();
                var field = @class?.GetField(node.Name.Text);
                return (field == null || (field.IsPublic && !field.IsStatic));
            }

            // AA.field
            string expressionText = node.Expression.Text;
            int index = expressionText.LastIndexOf('.');
            if (index >= 0 && this.IsQualifiedName(expressionText.Substring(0, index))) // separate
            {
                expressionText = expressionText.Substring(index + 1);
            }
            if (TypeHelper.IsMatchTypeName(expressionText))
            {
                var project = node.Document.Project;
                // enum
                if (project.GetEnum(expressionText) != null)
                {
                    return false;
                }

                string name = node.Name.Text;
                //native type(Math, Number etc)
                if ((expressionText == "Math" || expressionText == "Number") && name == name.ToUpper())
                {
                    return false;
                }

                // class static field
                var @class = project.GetClass(expressionText);
                var field = @class?.GetField(name);
                if (field != null && (field.IsStatic || field.IsReadonly))
                {
                    return false;
                }
            }

            // field: xxx.xxx
            Node member = TypeHelper.GetPropertyAccessMember(node);
            if (member is PropertyDeclaration prop && (!prop.IsPublic || prop.IsStatic))
            {
                return false;
            }

            // Function(...args: boolean[])
            Node exprType = TypeHelper.GetNodeType(node.Expression);
            if (exprType != null && exprType.Parent is Parameter param)
            {
                return !param.IsVariable;
            }

            return true;
        }

    }
}

