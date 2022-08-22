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
    public class ExpressionWithTypeArgumentsConverter : NodeConverter
    {
        public JCTree Convert(ExpressionWithTypeArguments node)
        {
            if (node.TypeArguments.Count > 0)
            {
                return TreeMaker.TypeApply(
                    node.Expression.ToJavaSyntaxTree<JCExpression>(),
                    node.TypeArguments.ToJavaSyntaxTrees<JCExpression>()
                );
            }
            else
            {
                return node.Expression.ToJavaSyntaxTree<JCTree>();
            }
        }
    }
}

