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
        private string _nodeName = null;
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

                    Type propType = prop.PropertyType;
                    if (TypeHelper.IsNodeType(propType))
                    {
                        if (prop.GetValue(this) is Node node && !children.Contains(node))
                        {
                            if (node.Parent != this) { node.Parent = this; };
                            children.Add(node);
                        }
                    }
                    else if (TypeHelper.IsNodeListType(propType))
                    {
                        if (prop.GetValue(this) is List<Node> nodes && nodes.Count > 0)
                        {
                            nodes.ForEach(n => { if (n.Parent != this) { n.Parent = this; } });
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
            private set;
        }

        public string NodeName
        {
            get
            {
                if (this._nodeName != null)
                {
                    return this._nodeName;
                }

                string name = this.Path.Split(".").Last();
                Match match = Regex.Match(name, @"(.*)\[\d+\]"); ;
                if (match.Success)
                {
                    name = match.Groups[1].Value;
                }
                return name;
            }
            internal set
            {
                this._nodeName = value;
            }
        }

        public string Text
        {
            get
            {
                if (this._text != null)
                {
                    return this._text;
                }

                return this.GetText();
            }
            internal set
            {
                this._text = value;
            }
        }

        public int Pos
        {
            get;
            protected set;
        }

        public int End
        {
            get;
            protected set;
        }

        public int Flags
        {
            get;
            protected set;
        }

        public Dictionary<string, Object> Pocket
        {
            get;
            protected set;
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
                if (this._document != null)
                {
                    return this._document;
                }

                Node parent = this;
                while ((parent = parent.Parent) != null)
                {
                    if (parent._document != null)
                    {
                        this._document = parent._document;
                        break;
                    }
                }
                return this._document;
            }
            set
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

        public virtual bool IsValidChild(Node childNode)
        {
            return true;
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

        public virtual void AddChild(Node childNode)
        {
            childNode.Parent = this;
        }

        public void AddChild(JObject tsNode)
        {
            this.AddChild(NodeHelper.CreateNode(tsNode));
        }

        public void RemoveChild(Node childNode)
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
            return this.DescendantsImpl(match, false);
        }

        public List<Node> DescendantsOnce(Predicate<Node> match = null)
        {
            return this.DescendantsImpl(match, true);
        }

        private List<Node> DescendantsImpl(Predicate<Node> match, bool once)
        {
            List<Node> nodes = new List<Node>();
            Queue<Node> queue = new Queue<Node>(this.Children);
            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                if (match == null || match.Invoke(node))
                {
                    nodes.Add(node);
                    if (match != null && once)
                    {
                        continue;
                    }
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
            return this.DescendantsAndSelfImpl(match, false);
        }

        public List<Node> DescendantsAndSelfOnce(Predicate<Node> match = null)
        {
            return this.DescendantsAndSelfImpl(match, true);
        }

        private List<Node> DescendantsAndSelfImpl(Predicate<Node> match, bool once)
        {
            List<Node> nodes = new List<Node>();
            if (match == null || match.Invoke(this))
            {
                nodes.Add(this);
                if (match != null && once)
                {
                    return nodes;
                }
            }
            nodes.AddRange(this.Descendants(match));
            return nodes;
        }

        public Node Ancestor(NodeKind kind)
        {
            Node parent = this;
            while ((parent = parent.Parent) != null)
            {
                if (parent.Kind == kind)
                {
                    return parent;
                }
            }
            return null;
        }

        #endregion

        #region Internal and Private Methods
        /// <summary>
        /// Get node's original text.
        /// </summary>
        protected virtual string GetText()
        {
            Node root = this.Root;
            if (root.Kind == NodeKind.SourceFile)
            {
                return root.Text.Substring(this.Pos, this.End - this.Pos);
            }
            return string.Empty;
        }

        protected bool IsTypeAliasType(Node node)
        {
            if (node.Kind == NodeKind.TypeAliasDeclaration)
            {
                return ((TypeAliasDeclaration)node).Type.Kind != NodeKind.FunctionType;
            }
            return false;
        }

        protected bool IsTypeNode(Node node)
        {
            switch (node.Kind)
            {
                case NodeKind.ClassDeclaration:
                case NodeKind.InterfaceDeclaration:
                case NodeKind.EnumDeclaration:
                    return true;

                case NodeKind.TypeAliasDeclaration:
                    return !this.IsTypeAliasType(node);

                default:
                    return false;
            }
        }

        private bool IsIgnoredProperty(string propName)
        {
            return (propName == "Parent" || propName == "JsDoc");
        }
        #endregion

        [Conditional("DEBUG")]
        protected void ProcessUnknownNode(Node child)
        {
            Console.WriteLine(string.Format("WARNING: {0} does not process child node {1}, Code: {2}", child.Parent.Kind, child.Kind, child.Text));
        }

    }
}
