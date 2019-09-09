using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TypeScript.Syntax
{
    public class Node
    {
        #region Fields
        private string _text = null;
        private Document _document = null;
        #endregion

        #region Ignored Properties
        protected int modifierFlagsCache { get; set; }
        protected int TransformFlags { get; set; }
        #endregion

        #region Properties
        public virtual NodeKind Kind
        {
            get { return NodeKind.Unknown; }
        }

        public Node Parent
        {
            get;
            internal set;
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

        public List<Node> OriginalChildren
        {
            get;
            private set;
        }

        public List<Node> Children
        {
            get
            {
                List<Node> children = new List<Node>();

                PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in properties)
                {
                    if (!prop.CanWrite || this.IsIgnoredProperty(prop.Name))
                    {
                        continue;
                    }

                    System.Type propType = prop.PropertyType;
                    if (TypeHelper.IsNodeType(propType))
                    {
                        Node node = prop.GetValue(this) as Node;
                        if (node != null && !children.Contains(node))
                        {
                            children.Add(node);
                        }
                    }
                    else if (TypeHelper.IsNodeListType(propType))
                    {
                        List<Node> nodes = prop.GetValue(this) as List<Node>;
                        if (nodes != null && nodes.Count > 0)
                        {
                            children.AddRange(nodes);
                        }
                    }
                }
                return children;
            }
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
            internal set;
        }

        public Document Document
        {
            get
            {
                Node root = this.Root;
                if (root == this)
                {
                    return this._document;
                }
                return root._document;
            }
            internal set
            {
                this._document = value;
            }
        }

        public Project Project
        {
            get
            {
                return this.Document?.Project;
            }
        }

        #endregion

        #region Public Methods
        public virtual void Init(JObject jsonObj)
        {
            this.TsNode = jsonObj;

            this.Parent = null;
            this.OriginalChildren = new List<Node>();

            this.Pocket = new Dictionary<string, object>();
            this.Path = jsonObj.Path;

            JToken jsonText = jsonObj["text"];
            this.Text = jsonText?.ToObject<string>();

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
            this.OriginalChildren.Add(childNode);
        }

        public void AddNode(JObject tsNode)
        {
            this.AddNode(NodeHelper.CreateNode(tsNode));
        }

        public void Remove(Node childNode)
        {
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo prop in properties)
            {
                if (!prop.CanWrite || this.IsIgnoredProperty(prop.Name))
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

                if (isNodeType && object.Equals(prop.GetValue(this), childNode))
                {
                    prop.SetValue(this, null);
                }
                if (isNodeListType)
                {
                    System.Type itemType = propType.GetGenericArguments()[0];
                    System.Type removeType = childNode.GetType();
                    if (removeType.Equals(itemType) || removeType.IsSubclassOf(itemType))
                    {
                        MethodInfo removeMethod = propType.GetMethod("Remove");
                        removeMethod.Invoke(prop.GetValue(this), new object[] { childNode });
                    }
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
            List<Node> nodes = new List<Node>();
            if (match == null || match.Invoke(this))
            {
                nodes.Add(this);
            }

            nodes.AddRange(this.Descendants(match));

            return nodes;
        }

        public Node GetAncestor(NodeKind kind)
        {
            Node parent = this.Parent;
            while (parent != null)
            {
                if (parent.Kind == kind)
                {
                    return parent;
                }

                parent = parent.Parent;
            }
            return null;
        }

        #endregion

        #region Internal and Private Methods
        /// <summary>
        /// Get node's original text.
        /// </summary>
        internal virtual string GetText()
        {
            Node root = this.Root;
            if (root.Kind == NodeKind.SourceFile)
            {
                return root.Text.Substring(this.Pos, this.End - this.Pos);
            }
            return string.Empty;
        }

        private bool IsIgnoredProperty(string propName)
        {
            return (propName == "Parent" || propName == "OriginalChildren" || propName == "jsDoc");
        }
        #endregion

        [Conditional("DEBUG")]
        protected void ProcessUnknownNode(Node child)
        {
            Console.WriteLine(string.Format("WARNING: {0} does not process child node {1}, Code: {2}", child.Parent.Kind, child.Kind, child.Text));
        }

    }
}
