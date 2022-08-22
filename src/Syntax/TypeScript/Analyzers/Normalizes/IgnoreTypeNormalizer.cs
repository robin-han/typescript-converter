using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class IgnoreTypeNormalizer : Normalizer
    {
        public override int Priority
        {
            get { return 4; }
        }

        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.ArrowFunction:
                    this.NormalizeArrowFunctionParameterType(node as ArrowFunction);
                    break;

                case NodeKind.FunctionExpression:
                    this.NormalizeFunctionExpressionParameterType(node as FunctionExpression);
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

        private void NormalizeFunctionExpressionParameterType(FunctionExpression funExprNode)
        {
            if (funExprNode.Parent.Kind == NodeKind.CallExpression)
            {
                foreach (Parameter parameter in funExprNode.Parameters)
                {
                    parameter.IgnoreType = true;
                }
            }
        }
    }
}
