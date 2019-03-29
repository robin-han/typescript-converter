using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class RegularExpressionNormalizer : Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.RegularExpressionLiteral:

                    break;
                default:
                    break;
            }
        }

        private void Normalize(RegularExpressionLiteral regexNode)
        {
            int lastIndex = regexNode.Text.LastIndexOf('/');
            regexNode.Pattern = regexNode.Text.Substring(1, lastIndex - 1);

            string regOption = regexNode.Text.Substring(lastIndex);
            if (regOption.Contains('i'))
            {
                regexNode.IgnoreCase = true;
            }
            if (regOption.Contains('m'))
            {
                regexNode.Multiline = true;
            }
        }
    }
}
