using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TypeScript.Syntax;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;
using com.sun.tools.javac.code;

namespace TypeScript.Converter.Java
{
    public class TemplateExpressionConverter : NodeConverter
    {
        public JCTree Convert(TemplateExpression node)
        {
            //Change To: MessageFormat.format(String pattern, Object... arguments);
            string pattern = node.Head.Text + "{"; // Header: `xxx${;

            List<string> arguments = new List<string>();
            for (int i = 0, count = node.TemplateSpans.Count; i < count; i++)
            {
                TemplateSpan span = (TemplateSpan)node.TemplateSpans[i];
                arguments.Add(span.Expression.Text);

                if (span.Literal.Kind == NodeKind.TemplateMiddle) // Middler: }xxxx${
                {
                    pattern += (i + "}" + span.Literal.Text + "{");
                }
                else // TemplateTail: }xxxxx`
                {
                    pattern += (i + "}" + span.Literal.Text);
                }
            }

            var fn = TreeMaker.Select(TreeMaker.Ident(Names.fromString("MessageFormat")), Names.fromString("format"));
            var args = new List<JCExpression>() { TreeMaker.Literal(TypeTag.NONE, pattern) };
            args.AddRange(arguments.Select(arg => TreeMaker.Ident(Names.fromString(arg))));
            return TreeMaker.Apply(Nil<JCExpression>(), fn, args);
        }
    }
}

