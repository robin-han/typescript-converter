using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TypeScript.Syntax.Analysis
{
    public class CallExpressionNormalizer : Normalizer
    {
        public override int Priority
        {
            get { return 5; }
        }

        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.CallExpression:
                    this.NormalizeEnum(node as CallExpression);
                    break;

                default:
                    break;
            }
        }

        private void NormalizeEnum(CallExpression callExpr)
        {
            if (callExpr.TypeArguments.Count == 0 &&
                callExpr.Arguments.Count >= 2 &&
                callExpr.Expression.Kind == NodeKind.PropertyAccessExpression &&
               (callExpr.Expression as PropertyAccessExpression).Name.Text == "asEnum")
            {
                string[] typeParts = callExpr.Arguments[1].Text.Split('.');
                string typeText = typeParts[typeParts.Length - 1];
                Node typeArgument = NodeHelper.CreateNode(
                "{ " +
                    "kind: \"TypeReference \", " +
                    "typeName: { " +
                        "kind: \"Identifier\", " +
                        "text: \"" + typeText + "\", " +
                    "}" +
                "}");

                callExpr.RemoveArgumentAt(1);
                List<Node> newArguments = new List<Node>();
                foreach (var arg in callExpr.Arguments)
                {
                    newArguments.Add(NodeHelper.CreateNode(arg.TsNode));
                }
                callExpr.ClearArguments();
                callExpr.AddArguments(newArguments);
                callExpr.AddTypeArgument(typeArgument);
            }
        }
    }
}
