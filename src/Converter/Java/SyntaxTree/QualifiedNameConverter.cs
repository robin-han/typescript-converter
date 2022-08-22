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
    public class QualifiedNameConverter : NodeConverter
    {
        public JCTree Convert(QualifiedName node)
        {
            Name name = Names.fromString(NormalizeTypeName(node.Right.Text));

            // omit names(dv. core.)
            if (this.IsQualifiedName(node.Left.Text))
            {
                return TreeMaker.Ident(name);
            }

            return TreeMaker.Select(node.Left.ToJavaSyntaxTree<JCExpression>(), name);
        }
    }
}

