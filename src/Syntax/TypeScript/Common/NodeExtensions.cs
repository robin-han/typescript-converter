using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax
{
    #region Node Extension
    public static class NodeExtensions
    {
        internal static Node ToQualifiedName(this Node node)
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

        public static bool HasJsDocTag(this Node node, string tagName)
        {
            List<Node> jsDoc = node.GetValue("JsDoc") as List<Node>;
            if (jsDoc != null && jsDoc.Count > 0)
            {
                JSDocComment docComment = jsDoc[0] as JSDocComment;
                if (docComment != null)
                {
                    return docComment.Tags.Find(tag => tag.Kind == NodeKind.JSDocTag && (tag as JSDocTag).TagName.Text == tagName) != null;
                }
            }
            return false;
        }

        public static bool HasModify(this Node node, NodeKind modify)
        {
            List<Node> modifiers = node.GetValue("Modifiers") as List<Node>;
            if (modifiers == null)
            {
                return false;
            }
            return modifiers.Exists(n => n.Kind == modify);
        }

        public static void AddModify(this Node node, NodeKind modify)
        {
            List<Node> modifiers = node.GetValue("Modifiers") as List<Node>;
            if (modifiers == null)
            {
                return;
            }
            if (modifiers.Exists(n => n.Kind == modify))
            {
                return;
            }
            modifiers.Add(NodeHelper.CreateNode(modify));
        }

        public static void RemoveModify(this Node node, NodeKind modify)
        {
            List<Node> modifiers = node.GetValue("Modifiers") as List<Node>;
            if (modifiers == null)
            {
                return;
            }
            int index = modifiers.FindIndex(n => n.Kind == modify);
            if (index >= 0)
            {
                modifiers.RemoveAt(index);
            }
        }

    }
    #endregion
}
