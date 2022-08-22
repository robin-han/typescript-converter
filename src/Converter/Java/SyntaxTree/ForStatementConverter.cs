using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TypeScript.Syntax;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;
using System.Linq;

namespace TypeScript.Converter.Java
{
    public class ForStatementConverter : NodeConverter
    {
        public JCTree Convert(ForStatement node)
        {
            List<JCStatement> init = new List<JCStatement>();
            foreach (var jcTreeNode in node.Initializers.ToJavaSyntaxTrees<JCTree>())
            {
                if (jcTreeNode is JCStatement stat)
                {
                    init.Add(stat);
                }
                else if (jcTreeNode is JCExpression expr)
                {
                    init.Add(TreeMaker.Exec(expr));
                }
            }

            return TreeMaker.ForLoop(
                init,
                node.Condition.ToJavaSyntaxTree<JCExpression>(),
                node.Incrementors.ToJavaSyntaxTrees<JCExpression>().Select(expr => TreeMaker.Exec(expr)).ToList(),
                node.Statement.ToJavaSyntaxTree<JCStatement>()
            );
        }
    }
}

