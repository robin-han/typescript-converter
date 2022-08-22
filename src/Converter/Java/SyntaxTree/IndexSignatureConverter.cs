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
    public class IndexSignatureConverter : NodeConverter
    {
        public JCTree Convert(IndexSignature node)
        {
            // Name name = Context.TypeScriptType ? Names.fromString("Hashtable") : Names.fromString("HashMap");
            Name name = Names.fromString("HashMap");

            JCExpression clazz = TreeMaker.Ident(name);
            List<JCExpression> arguments = new List<JCExpression>() {
                node.KeyType.ToJavaSyntaxTree<JCExpression>(),
                node.Type.ToJavaSyntaxTree<JCExpression>()
            };

            return TreeMaker.TypeApply(clazz, arguments);
        }
    }
}

