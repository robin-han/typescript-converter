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
    public class DoStatementConverter : NodeConverter
    {
        public JCTree Convert(DoStatement node)
        {
            return TreeMaker.DoLoop(
                node.Statement.ToJavaSyntaxTree<JCStatement>(),
                node.Expression.ToJavaSyntaxTree<JCExpression>()
            );
        }
    }
}

