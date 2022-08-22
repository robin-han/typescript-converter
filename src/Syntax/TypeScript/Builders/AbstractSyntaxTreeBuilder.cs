using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Newtonsoft.Json.Linq;
using TypeScript.Syntax;


namespace GrapeCity.Syntax.Converter.Source.TypeScript.Builders
{
    public class AbstractSyntaxTreeBuilder
    {
        private SyntaxNodeBuilder syntaxNodeBuilder = new SyntaxNodeBuilder();

        private Dictionary<string, Type> syntaxNodeTypes = SyntaxNodeNameAndTypeDictionaryBuilder.DefaultSyntaxNodeTypeListBuilder.Build();
        public Document Build(string path, INodeVisitor visiter = null)
        {
            var root = this.syntaxNodeBuilder.BuildNode(null, JObject.Parse(File.ReadAllText(path)), visiter);
            return new Document(path, (SourceFile)root);
        }

        public Node Build(JToken json)
        {
            return this.BuildNode(null, json);
        }

        protected Node BuildNode(Node parent, JToken json)
        {
            if (!json.HasValues)
            {
                return null;
            }

            if (json.Type == JTokenType.Object)
            {
                var objectValue = json as JObject;
                Node node = this.CreateNode(objectValue);
                if (node == null)
                {
                    return parent;
                }
                node.Init(objectValue);
                if (parent == null)
                {
                    parent = node;
                }
                else
                {
                    parent.AddChild(node);
                }
                foreach (var item in json)
                {
                    this.BuildNode(node, item);
                }

                return parent;
            }
            else
            {
                foreach (var item in json)
                {
                    this.BuildNode(parent, item);
                }
                return parent;
            }
        }

        protected Node CreateNode(JObject json)
        {
            if (!json.ContainsKey("kind"))
            {
                return null;
            }

            var kind = json["kind"];
            if (kind == null)
            {
                return null;
            }

            var kindValue = kind.ToObject<string>();
            if (string.IsNullOrWhiteSpace(kindValue))
            {
                return null;
            }

            var nodeKind = Enum.Parse<NodeKind>(kindValue);
            var nodeType = this.syntaxNodeTypes[nodeKind.ToString()];
            if (nodeType == null)
            {
                throw new NotSupportSyntaxNodeContentException();
            }
            ConstructorInfo constructorInfo = nodeType.GetConstructor(Type.EmptyTypes);
            if (constructorInfo == null)
            {
                throw new NoDefaultConstructorException();
            }

            return constructorInfo.Invoke(new object[] { }) as Node;
        }

        #region Static Methods
        public static string GetSyntaxNodeKey(JObject json)
        {
            string kind = json["kind"].ToString();
            if (Enum.TryParse(typeof(NodeKind), kind, out object result))
            {
                kind = result.ToString();
            }
            return kind;
        }

        #endregion
    }

}

