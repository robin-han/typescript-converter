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
    public class CatchClauseConverter : NodeConverter
    {
        public JCTree Convert(CatchClause node)
        {
            JCVariableDecl param;
            if (node.VariableDeclaration != null)
            {
                param = node.VariableDeclaration?.ToJavaSyntaxTree<JCVariableDecl>();
            }
            else
            {
                param = TreeMaker.VarDef(TreeMaker.Modifiers(0), Names.fromString("ex"), TreeMaker.Ident(Names.fromString("Exception")), null);
            }

            return TreeMaker.Catch(
                param,
                node.Block.ToJavaSyntaxTree<JCBlock>()
            );
        }
    }
}

