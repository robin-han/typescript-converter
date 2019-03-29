using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class IgnoreTypeNormalizer: Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.ArrowFunction:
                    this.NormalizeArrowFunctionParameterType(node as ArrowFunction);
                    break;

                default:
                    break;
            }
        }

        private void NormalizeArrowFunctionParameterType(ArrowFunction arrowFunNode)
        {
            foreach (Parameter parameter in arrowFunNode.Parameters)
            {
                parameter.IgnoreType = true;
            }
        }
    }
}
