using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TypeScript.Syntax;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;
using com.sun.tools.javac.code;

namespace TypeScript.Converter.Java
{
    public class NumericLiteralConverter : NodeConverter
    {
        public JCTree Convert(NumericLiteral node)
        {
            string text = node.Text;
            if (Zeroize(node))
            {
                text += ".0";
            }

            return TreeMaker.Literal(TypeTag.DOUBLE, text);
        }

        private bool Zeroize(NumericLiteral node)
        {
            string text = node.Text.ToLower();
            bool isDouble = text.Contains('.') || text.Contains('e');
            if (isDouble)
            {
                return false;
            }

            Node parent = node.Parent;
            if (parent == null)
            {
                return false;
            }

            while (parent.Kind == NodeKind.PrefixUnaryExpression)
            {
                parent = parent.Parent;
            }

            //
            if (parent.Kind == NodeKind.BinaryExpression)
            {
                // 1/2
                BinaryExpression expr = (BinaryExpression)parent;
                if (expr.OperatorToken.Kind == NodeKind.SlashToken)
                {
                    return true;
                }

                // xxx = 1;
                bool isRightLeaf = expr.Right.DescendantsAndSelfOnce(r => r == node).Count > 0;
                if (expr.OperatorToken.Kind == NodeKind.EqualsToken && isRightLeaf)
                {
                    Node leftType = TypeHelper.GetNodeType(expr.Left);
                    return !TypeHelper.IsIntType(leftType);
                }
            }
            else if (parent.Kind == NodeKind.ArrayLiteralExpression)
            {
                return true;
            }
            else if (parent.Kind == NodeKind.CallExpression)
            {
                // TODO: check method declaration parameter type
                return true;
            }
            else if (parent.Kind == NodeKind.NewExpression)
            {
                // TODO: check constructor parameter type
                return true;
            }
            else if (parent.Kind == NodeKind.PropertyAssignment)
            {
                return true;
            }
            else if (parent.Kind == NodeKind.VariableDeclaration)
            {
                return !TypeHelper.IsIntType(((VariableDeclaration)parent).Type);
            }
            else if (parent.Kind == NodeKind.PropertyDeclaration)
            {
                return !TypeHelper.IsIntType(((PropertyDeclaration)parent).Type);
            }
            else if (parent.Kind == NodeKind.ReturnStatement)
            {
                Node returnType = TypeHelper.GetReturnType((ReturnStatement)parent);
                if (returnType != null && TypeHelper.IsIntType(returnType))
                {
                    return false;
                }
                return true;
            }
            else if (parent.Kind == NodeKind.ArrowFunction) //() => 1 + 2;
            {
                return !TypeHelper.IsIntType(((ArrowFunction)parent).Type);
            }
            else if (parent.Kind == NodeKind.ConditionalExpression)
            {
                ConditionalExpression cond = (ConditionalExpression)parent;
                Node declarationType = TypeHelper.GetDeclarationType(cond);
                if (declarationType != null)
                {
                    return !TypeHelper.IsIntType(declarationType);
                }

                Node type = TypeHelper.GetConditionalExpressionType(cond);
                return !TypeHelper.IsIntType(type);
            }

            return false;
        }
    }
}

