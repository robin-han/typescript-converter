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
    public class TypeParameterConverter : NodeConverter
    {
        public JCTree Convert(TypeParameter node)
        {
            Name name = Names.fromString(node.Name.Text);
            List<JCExpression> bounds = node.Constraint == null ? Nil<JCExpression>() : node.Constraint.ToJavaSyntaxTrees<JCExpression>();

            return TreeMaker.TypeParameter(name, bounds);
        }
    }
}

