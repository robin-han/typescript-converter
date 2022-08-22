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
    public class CaseClauseConverter : NodeConverter
    {
        public JCTree Convert(CaseClause node)
        {
            if (IsEnumMemeberAccess(node.Expression, out string memberText))
            {
                return TreeMaker.Case(
                    CaseKind.STATEMENT,
                    new List<JCExpression>() { TreeMaker.Ident(Names.fromString(memberText)) },
                    node.Statements.ToJavaSyntaxTrees<JCStatement>(),
                    null
                );
            }
            else
            {
                return TreeMaker.Case(
                    CaseKind.STATEMENT,
                    node.Expression.ToJavaSyntaxTrees<JCExpression>(),
                    node.Statements.ToJavaSyntaxTrees<JCStatement>(),
                    null
                );
            }
        }

        private bool IsEnumMemeberAccess(Node caseExpression, out string memberText)
        {
            memberText = null;
            if (caseExpression.Kind == NodeKind.PropertyAccessExpression)
            {
                string text = this.TrimTypeName(caseExpression.Text);
                //
                int index = text.IndexOf('.');
                if (index > 0)
                {
                    string enumName = text.Substring(0, index);
                    Node type = this.Context.Project.GetTypeDeclaration(enumName);
                    if (type != null && type.Kind == NodeKind.EnumDeclaration)
                    {
                        memberText = text.Substring(index + 1);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

