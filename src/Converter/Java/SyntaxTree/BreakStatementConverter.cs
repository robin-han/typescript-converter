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
    public class BreakStatementConverter : NodeConverter
    {
        public JCTree Convert(BreakStatement node)
        {
            return TreeMaker.Break(null);
        }
    }
}

