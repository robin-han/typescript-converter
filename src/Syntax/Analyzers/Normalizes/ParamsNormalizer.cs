using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class ParamsNormalizer : Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.Parameter:
                    this.NormalizeParameter(node as Parameter);
                    break;

                default:
                    break;
            }
        }

        private void NormalizeParameter(Parameter parameterNode)
        {
            Node type = parameterNode.Type;

            if (parameterNode.IsOptional && parameterNode.Initializer == null)
            {
                parameterNode.Initializer = NodeHelper.CreateNode(NodeKind.NullKeyword);
            }

            if (parameterNode.IsVariable)
            {
                if (type.Kind == NodeKind.ArrayType)
                {
                    (type as ArrayType).IsParams = true;
                }
                else if (type.Kind == NodeKind.TypeReference)
                {
                    (type as TypeReference).IsParams = true;
                }
            }
        }
    }
}
