using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
#region Node Extension
    internal static class NodeExtensions
    {
        public static Node ToQualifiedName(this Node node)
        {
            if (node.Kind != NodeKind.PropertyAccessExpression)
            {
                return node;
            }

            PropertyAccessExpression expression = node as PropertyAccessExpression;
            //PropertyAccessExpression
            string jsonString = expression.TsNode.ToString();

            jsonString = jsonString
                .Replace("PropertyAccessExpression", "QualifiedName")
                .Replace("\"expression\":", "\"left\":")
                .Replace("\"name\":", "\"right\":");

            return NodeHelper.CreateNode(jsonString);
        }
    }
    #endregion
}
