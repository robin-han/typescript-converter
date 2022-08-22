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
    public class DeleteExpressionConverter : NodeConverter
    {
        public JCTree Convert(DeleteExpression node)
        {
            if (node.Expression.Kind == NodeKind.ElementAccessExpression)
            {
                ElementAccessExpression elementAccessNode = node.Expression as ElementAccessExpression;

                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(elementAccessNode.Expression.ToJavaSyntaxTree<JCExpression>(), Names.fromString("remove")),
                    elementAccessNode.ArgumentExpression.ToJavaSyntaxTrees<JCExpression>());
            }
            return null;
        }
    }
}

