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
    public class CaseBlockConverter : NodeConverter
    {
        public List<JCCase> Convert(CaseBlock node)
        {
            List<JCCase> clauses = node.Clauses.ToJavaSyntaxTrees<JCCase>();

            bool hasDefault = node.Clauses.Find(c => c.Kind == NodeKind.DefaultClause) != null;
            if (!hasDefault)
            {
                JCCase defaultCase = TreeMaker.Case(
                    CaseKind.STATEMENT,
                    Nil<JCExpression>(),
                    new List<JCStatement>() { TreeMaker.Break(null) },
                    null
                );

                clauses.Add(defaultCase);
            }

            return clauses;
        }
    }
}

