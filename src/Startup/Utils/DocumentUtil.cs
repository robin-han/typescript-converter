using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TypeScript.Syntax;
using GrapeCity.Syntax.Converter.Source.TypeScript.Builders;

using Newtonsoft.Json.Linq;

namespace TypeScript.Converter
{
    class DocumentUtil
    {
        private static Dictionary<string, Type> nodeKinds = SyntaxNodeNameAndTypeDictionaryBuilder.DefaultSyntaxNodeTypeListBuilder.Build();

        /// <summary>
        /// Gets the lost nodes of the document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetLostNodes(Document doc)
        {
            Dictionary<string, string> lost = new Dictionary<string, string>();

            foreach (Node node in doc.Source.DescendantsAndSelf())
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
            foreach (var doc in tsDocs)
            {
                string docText = doc.Source.Text;
                var notImplementedNodeKinds = GetNotImplementNodeKind(doc);
                foreach (var item in notImplementedNodeKinds)
                {
                    var texts = item.Value.Select(v =>
                    {
                        JToken jsonPos = v["pos"];
                        int pos = jsonPos == null ? 0 : jsonPos.ToObject<int>();
                        JToken jsonEnd = v["end"];
                        int end = jsonEnd == null ? 0 : jsonEnd.ToObject<int>();
                        if (pos + end <= docText.Length)
                        {
                            return docText.Substring(pos, end - pos);
                        }
                        return "";
                    });
                    ret.Add($"{doc.Path} : {item.Key} : {string.Join(',', texts)}");
                }
            }
            return ret;
        }

        /// <summary>
        /// Gets all NodeKinds of the document.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, List<JToken>> GetNotImplementNodeKind(Document doc)
        {
            var implementedNodeKinds = nodeKinds.Keys;
            var notImplementNodeKinds = new Dictionary<string, List<JToken>>();
            Queue<JToken> queue = new Queue<JToken>(doc.Source.TsNode);

            while (queue.Count > 0)
            {
                JToken jsonToken = queue.Dequeue();
                if (jsonToken.HasValues && jsonToken.Type == JTokenType.Object)
                {
                    string nodeKind = AbstractSyntaxTreeBuilder.GetSyntaxNodeKey((JObject)jsonToken);
                    if (!implementedNodeKinds.Contains(nodeKind))
                    {
                        if (notImplementNodeKinds.ContainsKey(nodeKind))
                        {
                            notImplementNodeKinds[nodeKind].Add(jsonToken);
                        }
                        else
                        {
                            notImplementNodeKinds.Add(nodeKind, new List<JToken>() { jsonToken });
                        }
                    }
                }
                foreach (var token in jsonToken)
                {
                    queue.Enqueue(token);
                }
            }

            return notImplementNodeKinds;
        }
    }
}
