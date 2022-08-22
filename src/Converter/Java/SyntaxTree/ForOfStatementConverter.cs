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
    public class ForOfStatementConverter : NodeConverter
    {
        public JCTree Convert(ForOfStatement node)
        {
            Node typeNode = TypeHelper.TrimType(TypeHelper.GetNodeType(node.Expression));
            if (TypeHelper.IsArrayType(typeNode))
            {
                typeNode = TypeHelper.GetArrayElementType(typeNode);
            }

            JCVariableDecl varDef = TreeMaker.VarDef(
                TreeMaker.Modifiers(Flags.FINAL),
                Names.fromString(node.Identifier.Text),
                typeNode?.ToJavaSyntaxTree<JCExpression>(),
                null);

            return TreeMaker.ForeachLoop(
                varDef,
                node.Expression.ToJavaSyntaxTree<JCExpression>(),
                node.Statement.ToJavaSyntaxTree<JCStatement>());
        }
    }
}

