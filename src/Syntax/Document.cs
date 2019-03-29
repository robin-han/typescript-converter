using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class Document
    {
        #region Constructor
        public Document()
        {
            this.Root = null;
            this.Path = string.Empty;
        }
        #endregion

        #region Properties
        public Node Root
        {
            get;
            internal set;
        }

        public string Path
        {
            get;
            internal set;
        }

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
                return n.Kind == NodeKind.ClassDeclaration || n.Kind == NodeKind.InterfaceDeclaration || n.Kind == NodeKind.EnumDeclaration;
            });
        }

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
