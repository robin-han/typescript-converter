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
    public class RegularExpressionLiteralConverter : NodeConverter
    {
        public JCTree Convert(RegularExpressionLiteral node)
        {
            List<JCExpression> args = new List<JCExpression>();
            args.Add(TreeMaker.Literal(TypeTag.NONE, node.Pattern));
            if (!string.IsNullOrEmpty(node.SearchFlags))
            {
                args.Add(TreeMaker.Literal(TypeTag.NONE, node.SearchFlags));
            }

            return TreeMaker.NewClass(
                null,
                Nil<JCExpression>(),
                TreeMaker.Ident(Names.fromString("RegExp")),
                args,
                null
             );
        }
    }
}

