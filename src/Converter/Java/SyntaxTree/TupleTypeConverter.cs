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
    public class TupleTypeConverter : NodeConverter
    {
        public JCTree Convert(TupleType node) // let a: [number, string] = [1, "hello"];
        {
            //TODO: 
            return null;
        }
    }
}

