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
    public class IdentifierConverter : NodeConverter
    {
        public JCTree Convert(Identifier node)
        {
            string text = NormalizeTypeName(node.Text);
            if (text == "Array" && node.Parent.Kind == NodeKind.NewExpression)
            {
                text = "ArrayList";
            }

            JCIdent ident = TreeMaker.Ident(Names.fromString(text));

            if (string.IsNullOrEmpty(node.As))
            {
                return ident;
            }
            else
            {
                string asType = NormalizeTypeName(TrimTypeName(node.As));
                return AsExpressionConverter.AsType(ident, asType);
            }
        }
    }
}

