using TypeScript.Syntax;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace TypeScript.Converter
{
    class DocumentUtil
    {
        /// <summary>
        /// Gets the lost nodes of the document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetLostNodes(Document doc)
        {
            Dictionary<string, string> lost = new Dictionary<string, string>();

            foreach (Node node in doc.Root.DescendantsAndSelf())
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
        /// Gets the not implement node types of all the document.
        /// </summary>
        /// <param name="tsDocs">The documents.</param>
        /// <returns>Not implement node types.</returns>
        public static List<string> GetNotImplementNodeTypes(List<Document> tsDocs)
        {
            List<string> ret = new List<string>();
            var implementedNodeTypes = AstBuilder.AllNodeTypes.Keys;
            foreach (var doc in tsDocs)
            {
                List<string> docNodeKinds = GetNodeKinds(doc);
                List<string> notImplementedNodes = docNodeKinds.FindAll((k) => !implementedNodeTypes.Contains(k));
                notImplementedNodes.ForEach(item =>
                {
                    if (!ret.Contains(item))
                    {
                        ret.Add(item);
                    }
                });
            }
            return ret;
        }

        /// <summary>
        /// Gets all NodeKinds of the document.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetNodeKinds(Document doc)
        {
            List<string> kinds = new List<string>();
            JObject jsonObject = JObject.Parse(File.ReadAllText(doc.Path));
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
        /// Gets all type names of the document.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetTypeNames(Syntax.Document doc)
        {
            List<string> ret = new List<string>();
            List<Node> types = doc.TypeNodes;
            foreach (Node type in types)
            {
                string name = TypeHelper.GetTypeName(type);
                if (!string.IsNullOrEmpty(name))
                {
                    ret.Add(name);
                }
            }
            return ret;
        }
    }
}
