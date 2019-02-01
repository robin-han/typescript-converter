using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class Node
    {
        private string _text = null;

        #region Properties
        public virtual NodeKind Kind
        {
            get { return NodeKind.Unknown; }
        }

        public Node Parent
        {
            get;
            private set;
        }

        public Node Root
        {
            get
            {
                Node parent = this;
                while (parent.Parent != null)
                {
                    parent = parent.Parent;
                }
                return parent;
            }
        }

        public List<Node> Children
        {
            get;
            private set;
        }

        public string Path
        {
            get;
            internal set;
        }

        public string NodeName
        {
            get
            {
                string name = this.Path.Split(".").Last();
                Match match = Regex.Match(name, @"(.*)\[\d+\]"); ;
                if (match.Success)
                {
                    name = match.Groups[1].Value;
                }
                return name;
            }
        }

        public string Text
        {
            get
            {
                if (this._text == null)
                {
                    this._text = this.GetText();
                }
                return this._text;
            }
            internal set
            {
                this._text = value;
            }
        }

        public int Pos
        {
            get;
            internal set;
        }

        public int End
        {
            get;
            internal set;
        }

        public int Flags
        {
            get;
            private set;
        }

        public Dictionary<string, Object> Pocket
        {
            get;
            private set;
        }

        public JObject TsNode
        {
            get;
            private set;
        }
        #endregion

        public virtual void Init(JObject jsonObj)
        {
            this.TsNode = jsonObj;

            this.Parent = null;
            this.Children = new List<Node>();

            this.Pocket = new Dictionary<string, object>();
            this.Path = jsonObj.Path;

            JToken jsonText = jsonObj["text"];
            this.Text = jsonText == null ? null: jsonText.ToObject<string>();

            JToken jsonPos = jsonObj["pos"];
            this.Pos = jsonPos == null ? 0 : jsonPos.ToObject<int>();

            JToken jsonEnd = jsonObj["end"];
            this.End = jsonEnd == null ? 0 : jsonEnd.ToObject<int>();

            JToken jsonFlags = jsonObj["flags"];
            this.Flags = jsonFlags == null ? 0 : jsonFlags.ToObject<int>();
        }

        public bool HasProperty(string name)
        {
            return this.GetType().GetProperty(name) != null;
        }

        public object GetValue(string propName)
        {
            PropertyInfo prop = this.GetType().GetProperty(propName);
            if (prop != null && prop.CanRead)
            {
                return prop.GetValue(this);
            }
            return null;
        }

        public void SetValue(string propName, object value)
        {
            PropertyInfo prop = this.GetType().GetProperty(propName);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(this, value);
            }
        }

        public virtual void AddNode(Node childNode)
        {
            childNode.Parent = this;
            this.Children.Add(childNode);
        }

        internal void AddNode(JObject tsNode)
        {
            this.AddNode(this.CreateNode(tsNode));
        }

        public void Remove(Node childNode)
        {
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Type NodeType = typeof(Node);
            Type removeType = childNode.GetType();

            foreach (PropertyInfo prop in properties)
            {
                if (!prop.CanWrite)
                {
                    continue;
                }

                Type propType = prop.PropertyType;
                Type itemType = propType;
                bool isNodeType = propType.Equals(NodeType) || propType.IsSubclassOf(NodeType);
                bool isNodeListType = false;

                if (propType.IsGenericType)
                {
                    itemType = propType.GetGenericArguments()[0];
                    isNodeListType = itemType.Equals(NodeType) || itemType.IsSubclassOf(NodeType);
                }

                if (!isNodeType && !isNodeListType)
                {
                    continue;
                }
                if (prop.Name == "Parent" || prop.Name == "Children")
                {
                    continue;
                }

                object propValue = prop.GetValue(this);
                if (isNodeType && object.Equals(propValue, childNode))
                {
                    prop.SetValue(this, null);
                }
                if (isNodeListType && (removeType.Equals(itemType) || removeType.IsSubclassOf(itemType)))
                {
                    MethodInfo removeMethod = propType.GetMethod("Remove");
                    removeMethod.Invoke(propValue, new object[] { childNode });
                }
            }
        }

        public List<Node> Descendants(Predicate<Node> match = null)
        {
            List<Node> nodes = new List<Node>();

            Queue<Node> queue = new Queue<Node>(this.Children);
            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                if (match == null || match.Invoke(node))
                {
                    nodes.Add(node);
                }

                foreach (Node nd in node.Children)
                {
                    queue.Enqueue(nd);
                }
            }
            return nodes;
        }

        public List<Node> DescendantsAndSelf(Predicate<Node> match = null)
        {
            List<Node> nodes = new List<Node> { this };
            nodes.AddRange(this.Descendants(match));

            return nodes;
        }

        public void Normalize(bool onlySelf = false)
        {
            List<Node> nodes = onlySelf ? new List<Node> { this } : this.DescendantsAndSelf();
            foreach (Node node in nodes)
            {
                node.NormalizeImp();
            }
        }
        protected virtual void NormalizeImp()
        {
        }

        protected virtual Node InferType()
        {
            return null;
        }

        /// <summary>
        /// Get node's original text.
        /// </summary>
        /// <returns></returns>
        internal virtual string GetText()
        {
            Node root = this.Root;
            if (root.Kind == NodeKind.SourceFile)
            {
                return root.Text.Substring(this.Pos, this.End - this.Pos);
            }
            return string.Empty;
        }

        protected Node CreateNode(NodeKind kind, string text = "")
        {
            return this.CreateNode(
                "{ " +
                    "kind: \"" + kind.ToString() + "\", " +
                    "text: \"" + text + "\" " +
                "}");
        }
        protected Node CreateNode(string json)
        {
            return this.CreateNode(JObject.Parse(json));
        }
        protected Node CreateNode(JObject nodeJson)
        {
            Node node = new TsAstBuilder().Build(nodeJson);
            node.Parent = this;

            return node;
        }

        protected Node PropertyAccessExpressionToQualifiedName(Node expression)
        {
            if (expression.Kind != NodeKind.PropertyAccessExpression)
            {
                return expression;
            }

            string jsonString = expression.TsNode.ToString();

            jsonString = jsonString
                .Replace("PropertyAccessExpression", "QualifiedName")
                .Replace("\"expression\":", "\"left\":")
                .Replace("\"name\":", "\"right\":");

            return this.CreateNode(jsonString);
        }


        [Conditional("DEBUG")]
        protected void ProcessUnknownNode(Node child)
        {
            Console.WriteLine(string.Format("WARNING: {0} does not process child node {1}, Code: {2}", child.Parent.Kind, child.Kind, child.Text));
        }

    }
}
