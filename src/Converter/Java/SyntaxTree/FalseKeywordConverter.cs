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
    public class FalseKeywordConverter : NodeConverter
    {
        public JCTree Convert(FalseKeyword node)
        {
            return TreeMaker.Literal(TypeTag.BOOLEAN, false);
        }
    }
}

