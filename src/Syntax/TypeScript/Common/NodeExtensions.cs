using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TypeScript.Syntax
{
    #region Node Extension
    public static class NodeExtension
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

        public static List<Node> Descendants(this Node node, Predicate<Node> match = null)
        {
            return node.DescendantsImpl(match, false);
        }

        public static List<Node> DescendantsOnce(this Node node, Predicate<Node> match = null)
        {
            return node.DescendantsImpl(match, true);
        }

        private static List<Node> DescendantsImpl(this Node node, Predicate<Node> match, bool once)
        {
            List<Node> nodes = new List<Node>();
            Queue<Node> queue = new Queue<Node>(node.Children);
            while (queue.Count > 0)
            {
                Node nd = queue.Dequeue();
                if (match == null || match.Invoke(nd))
                {
                    nodes.Add(nd);
                    if (match != null && once)
                    {
                        continue;
                    }
                }
                foreach (Node child in nd.Children)
                {
                    queue.Enqueue(child);
                }
            }
            return nodes;
        }

        public static List<Node> DescendantsAndSelf(this Node node, Predicate<Node> match = null)
        {
            return node.DescendantsAndSelfImpl(match, false);
        }

        public static List<Node> DescendantsAndSelfOnce(this Node node, Predicate<Node> match = null)
        {
            return node.DescendantsAndSelfImpl(match, true);
        }

        private static List<Node> DescendantsAndSelfImpl(this Node node, Predicate<Node> match, bool once)
        {
            List<Node> nodes = new List<Node>();
            if (match == null || match.Invoke(node))
            {
                nodes.Add(node);
                if (match != null && once)
                {
                    return nodes;
                }
            }
            nodes.AddRange(node.Descendants(match));
            return nodes;
        }

        public static T Ancestor<T>(this Node node) where T : Node
        {
            Node parent = node.Parent;
            if (parent is T result)
            {
                return result;
            }
            else if (parent == null)
            {
                return null;
            }
            else
            {
                return parent.Ancestor<T>();
            }
        }

        public static bool IsTypeAliasType(this Node node)
        {
            if (node.Kind == NodeKind.TypeAliasDeclaration)
            {
                return ((TypeAliasDeclaration)node).Type.Kind != NodeKind.FunctionType;
            }
            return false;
        }

        public static bool IsTypeDeclaration(this Node node)
        {
            switch (node.Kind)
            {
                case NodeKind.ClassDeclaration:
                case NodeKind.InterfaceDeclaration:
                case NodeKind.EnumDeclaration:
                    return true;

                case NodeKind.TypeAliasDeclaration:
                    return !node.IsTypeAliasType();

                default:
                    return false;
            }
        }

        public static bool HasProperty(this Node node, string name)
        {
            return node.GetType().GetProperty(name) != null;
        }

        public static void AddChild(this Node node, JObject tsNode)
        {
            node.AddChild(NodeHelper.CreateNode(tsNode));
        }

        public static void RemoveChild(this Node node, Node childNode)
        {
            PropertyInfo[] properties = node.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo prop in properties)
            {
                if (!prop.CanWrite || IsIgnoredProperty(prop.Name))
                {
                    continue;
                }

                System.Type propType = prop.PropertyType;
                bool isNodeType = TypeHelper.IsNodeType(propType);
                bool isNodeListType = TypeHelper.IsNodeListType(propType);
                if (!isNodeType && !isNodeListType)
                {
                    continue;
                }

                if (isNodeType && object.Equals(prop.GetValue(node), childNode))
                {
                    prop.SetValue(node, null);
                }
                if (isNodeListType)
                {
                    System.Type itemType = propType.GetGenericArguments()[0];
                    System.Type removeType = childNode.GetType();
                    if (removeType.Equals(itemType) || removeType.IsSubclassOf(itemType))
                    {
                        MethodInfo removeMethod = propType.GetMethod("Remove");
                        removeMethod.Invoke(prop.GetValue(node), new object[] { childNode });
                    }
                }
            }
        }

        public static List<Node> GetChildren(this Node node)
        {
            List<Node> children = new List<Node>();

            PropertyInfo[] properties = node.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo prop in properties)
            {
                if (!prop.CanWrite || IsIgnoredProperty(prop.Name))
                {
                    continue;
                }

                Type propType = prop.PropertyType;
                if (TypeHelper.IsNodeType(propType))
                {
                    if (prop.GetValue(node) is Node nd && !children.Contains(nd))
                    {
                        if (nd.Parent == node)
                        {
                            children.Add(nd);
                        }
                    }
                }
                else if (TypeHelper.IsNodeListType(propType))
                {
                    if (prop.GetValue(node) is List<Node> nodes && nodes.Count > 0)
                    {
                        nodes.ForEach(n => { if (n.Parent == node) { children.Add(n); } });
                    }
                }
            }
            return children;
        }

        private static bool IsIgnoredProperty(string propName)
        {
            return (propName == "Parent" || propName == "JsDoc");
        }

        public static object GetValue(this Node node, string propName)
        {
            PropertyInfo prop = node.GetType().GetProperty(propName);
            if (prop != null && prop.CanRead)
            {
                return prop.GetValue(node);
            }
            return null;
        }

        public static void SetValue(this Node node, string propName, object value)
        {
            PropertyInfo prop = node.GetType().GetProperty(propName);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(node, value);
            }
        }

        public static string GetName(this Node node)
        {
            object nameText = node.GetValue("NameText");
            if (nameText != null)
            {
                return (string)nameText;
            }
            return null;
        }
    }
    #endregion
}
