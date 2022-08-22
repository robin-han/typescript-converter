using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

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

        public SourceFile Root
        {
            get
            {
                if (this.Parent != null)
                {
                    return this.Parent.Root;
                }
                else if (this is SourceFile)
                {
                    return this as SourceFile;
                }
                else
                {
                    throw new UnexpectedSyntaxNodeException();
                }
            }
        }

        public List<Node> Children
        {
            get
            {
                return this.GetChildren();
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

        public virtual string Text
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
            protected set;
        }

        public Dictionary<string, object> Pocket
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
            internal set
            {
                this._document = value;
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

        public virtual void AddChild(Node childNode)
        {
            childNode.Parent = this;
        }
        #endregion

        #region Internal and Private Methods
        /// <summary>
        /// Get node's original text.
        /// </summary>
        protected virtual string GetText()
        {
            if (this.Pos == this.End)
            {
                return string.Empty;
            }

            string text = this.Root.Text;
            if (this.Pos >= text.Length || this.End > text.Length)
            {
                return string.Empty;
            }
            return text.Substring(this.Pos, this.End - this.Pos).Trim();
        }
        #endregion

        [Conditional("DEBUG")]
        protected void ProcessUnknownNode(Node child)
        {
            Console.WriteLine(string.Format("WARNING: {0} does not process child node {1}, Code: {2}", child.Parent.Kind, child.Kind, child.Text));
        }
    }
}
