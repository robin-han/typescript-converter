using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TypeScript.Syntax
{
    public class Document
    {
        #region Fields
        private Node _root;
        private string _path;
        private Project _project;
        #endregion

        #region Constructor
        public Document()
        {
            this._root = null;
            this._path = string.Empty;
            this._project = null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the document's root node.
        /// </summary>
        public Node Root
        {
            get
            {
                return this._root;
            }
            internal set
            {
                Node root = value;
                root.Document = this;
                this._root = root;
            }
        }

        /// <summary>
        /// Gets the document's path
        /// </summary>
        public string Path
        {
            get
            {
                return this._path;
            }
            internal set
            {
                this._path = value;
            }
        }

        /// <summary>
        /// Get the document's project.
        /// </summary>
        public Project Project
        {
            get
            {
                return this._project;
            }
            internal set
            {
                this._project = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(this.Path);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetNodeKinds()
        {
            List<string> kinds = new List<string>();

            JObject jsonObject = JObject.Parse(File.ReadAllText(this.Path));
            Queue<JToken> queue = new Queue<JToken>(jsonObject);
            while (queue.Count > 0)
            {
                JToken jsonToken = queue.Dequeue();
                if (jsonToken.HasValues && jsonToken.Type == JTokenType.Object)
                {
                    string syntaxKind = AstBuilder.GetSyntaxNodeKey(jsonToken as JObject);
                    if (!kinds.Contains(syntaxKind))
                    {
                        kinds.Add(syntaxKind);
                    }
                }

                foreach (var token in jsonToken)
                {
                    queue.Enqueue(token);
                }
            }

            return kinds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetLostNodes()
        {
            Dictionary<string, string> lost = new Dictionary<string, string>();
            List<Node> nodes = this.Root.DescendantsAndSelf();

            foreach (Node node in nodes)
            {
                string key = node.Kind.ToString();
                if (lost.ContainsKey(key))
                {
                    continue;
                }

                List<string> ignoredProperties = new List<string>();
                List<string> definedProperties = new List<string>();
                PropertyInfo[] properties = node.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (PropertyInfo p in properties)
                {
                    definedProperties.Add(p.Name.ToLower());
                }

                foreach (var p in node.TsNode.Properties())
                {
                    if (!definedProperties.Contains(p.Name.ToLower()))
                    {
                        ignoredProperties.Add(p.Name);
                    }
                }

                if (ignoredProperties.Count > 0)
                {
                    lost[key] = string.Join(',', ignoredProperties);
                }
            }

            return lost;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetTypeNames()
        {
            List<string> ret = new List<string>();
            List<Node> types = this.GetTypeNodes();

            foreach (Node type in types)
            {
                string name = this.GetTypeName(type);
                if (!string.IsNullOrEmpty(name))
                {
                    ret.Add(name);
                }
            }
            return ret;
        }

        /// <summary>
        /// Get all type nodes(class, interfact, enum etc.) in the document.
        /// </summary>
        /// <returns></returns>
        public List<Node> GetTypeNodes()
        {
            return this.Root.Descendants((n) =>
            {
                return( 
                n.Kind == NodeKind.ClassDeclaration || 
                n.Kind == NodeKind.InterfaceDeclaration ||
                n.Kind == NodeKind.EnumDeclaration ||
                n.Kind == NodeKind.TypeAliasDeclaration);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal string GetTypeName(Node node)
        {
            if (node.GetValue("Name") is Node name)
            {
                return name.Text;
            }
            return null;
        }
        #endregion

    }
}
