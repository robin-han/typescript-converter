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
    public class GetSetAccessorConverter : NodeConverter
    {
        public List<JCTree> Convert(GetSetAccessor node)
        {
            return new List<JCTree>()
            {
                node.GetAccessor.ToJavaSyntaxTree<JCTree>(),
                node.SetAccessor.ToJavaSyntaxTree<JCTree>()
            };
        }
    }
}

