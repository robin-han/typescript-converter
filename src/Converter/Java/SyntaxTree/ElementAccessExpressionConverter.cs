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
    public class ElementAccessExpressionConverter : NodeConverter
    {
        private static readonly Name STRING_GET_INDEXER_NAME = Names.fromString("charAt");
        private static readonly Name COLLECTION_GET_INDEXER_METHOD = Names.fromString("get");

        public JCTree Convert(ElementAccessExpression node)
        {
            // get((int)i);
            JCExpression arg = node.ArgumentExpression.ToJavaSyntaxTree<JCExpression>();
            if (ShouldCastArgumentToInt(node))
            {
                Node argType = TypeHelper.GetNodeType(node.ArgumentExpression);
                if (argType != null && (argType.Kind == NodeKind.UnionType || IsGenericTypeArgument(argType)))
                {
                    arg = TreeMaker.TypeCast(TreeMaker.TypeIdent(TypeTag.DOUBLE), arg);
                }
                arg = TreeMaker.TypeCast(TreeMaker.TypeIdent(TypeTag.INT), arg);
            }

            if (IsStringElementAccess(node))
            {
                // Always change to StringExtension.charAt(str, x) method
                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(TreeMaker.Ident(Names.fromString("StringExtension")), STRING_GET_INDEXER_NAME),
                    new List<JCExpression>()
                    {
                        node.Expression.ToJavaSyntaxTree<JCExpression>(),
                        arg
                    }
                );
            }
            else if (IsCollectionElementAccess(node))
            {
                // Always change to xxx.get(x) method
                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(node.Expression.ToJavaSyntaxTree<JCExpression>(), COLLECTION_GET_INDEXER_METHOD),
                    new List<JCExpression>() { arg }
                );
            }
            else
            {
                return TreeMaker.Indexed(
                    node.Expression.ToJavaSyntaxTree<JCExpression>(),
                    node.ArgumentExpression.ToJavaSyntaxTree<JCExpression>()
                );
            }
        }

        private bool IsStringElementAccess(ElementAccessExpression node)
        {
            Node exprType = TypeHelper.GetNodeType(node.Expression);
            return TypeHelper.IsStringType(exprType);
        }

        private bool IsCollectionElementAccess(ElementAccessExpression node)
        {
            Node exprType = TypeHelper.GetNodeType(node.Expression);
            if (TypeHelper.IsArrayType(exprType) && exprType.Parent is Parameter param)
            {
                return !param.IsVariable;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Indicates should cast argument to int.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal static bool ShouldCastArgumentToInt(ElementAccessExpression node)
        {
            Node expr = node.Expression;
            Node argExpr = node.ArgumentExpression;

            return argExpr.Kind != NodeKind.NumericLiteral
                && !TypeHelper.IsIntType(TypeHelper.GetNodeType(argExpr))
                && TypeHelper.IsArrayType(TypeHelper.GetNodeType(expr)
            );
        }
    }
}
