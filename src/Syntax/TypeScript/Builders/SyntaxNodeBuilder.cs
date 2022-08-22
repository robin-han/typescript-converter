using System;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json.Linq;

using TypeScript.Syntax;

namespace GrapeCity.Syntax.Converter.Source.TypeScript.Builders
{
    public class SyntaxNodeBuilder
    {
        private Dictionary<string, Type> syntaxNodeNameAndTypes = SyntaxNodeNameAndTypeDictionaryBuilder.DefaultSyntaxNodeTypeListBuilder.Build();
        private List<string> ignoredProperties;
        public static SyntaxNodeBuilder DefaultSyntaxNodeBuilder = new SyntaxNodeBuilder();

        public SyntaxNodeBuilder(List<string> ignoredProperties = null)
        {
            if (ignoredProperties == null)
            {
                this.ignoredProperties = new List<string> {
                    "amdDependencies",
                    "bindDiagnostics",
                    "hasNoDefaultLib",
                    "identifiers",
                    "identifierCount",
                    "isBracketed",
                    "isDeclarationFile",
                    "isNameFirst",
                    "kind",
                    "languageVariant",
                    "languageVersion",
                    "libReferenceDirectives",
                    "modifierFlagsCache",
                    "nodeCount",
                    "numericLiteralFlags",
                    "originalKeywordKind",
                    "parseDiagnostics",
                    "pragmas",
                    "referencedFiles",
                    "scriptKind",
                    "transformFlags",
                    "typeReferenceDirectives",
                };
            }
            else
            {
                this.ignoredProperties = ignoredProperties;
            }
        }

        public Node BuildNode(Node parent, JObject json, INodeVisitor visitor = null)
        {
            Node node = this.CreateNode(json);
            if (node == null)
            {
                return null;
            }

            this.SetProperty(node, "TsNode", json);
            this.SetProperty(node, "parent", parent);
            foreach (var item in json.Children<JProperty>())
            {
                var name = item.Name;
                var value = item.Value;

                if (this.ignoredProperties.Contains(name))
                {
                    continue;
                }

                if (value.Type == JTokenType.Array)
                {
                    var arrayValue = value as JArray;
                    var propertyValue = this.BuildNodes(node, arrayValue, visitor);
                    if (propertyValue != null)
                    {
                        this.SetProperty(node, name, propertyValue);
                        continue;
                    }
                }
                if (value.Type == JTokenType.Object)
                {
                    var objectValue = value as JObject;
                    var propertyValue = this.BuildNode(node, objectValue, visitor);
                    if (propertyValue != null)
                    {
                        this.SetProperty(node, name, propertyValue);
                        continue;
                    }
                }
                if (value.Type == JTokenType.Integer)
                {
                    var propertyValue = value.ToObject<int>();
                    this.SetProperty(node, name, propertyValue);
                    continue;
                }
                if (value.Type == JTokenType.String)
                {
                    var propertyValue = value.ToObject<string>();
                    this.SetProperty(node, name, propertyValue);
                    continue;
                }
                if (value.Type == JTokenType.Boolean)
                {
                    var propertyValue = value.ToObject<bool>();
                    this.SetProperty(node, name, propertyValue);
                    continue;
                }
                throw new NotSupportSyntaxNodeContentException();
            }

            if (visitor != null)
            {
                visitor.Visit(node);
            }

            return node;
        }
        protected List<Node> BuildNodes(Node parent, JArray json, INodeVisitor visitor = null)
        {
            var nodes = new List<Node>();
            foreach (var item in json.Children())
            {
                if (item.Type == JTokenType.Array)
                {
                    throw new NotSupportSyntaxNodeContentException();
                }
                if (item.Type == JTokenType.Object)
                {
                    var node = this.BuildNode(parent, item as JObject, visitor);
                    if (node != null)
                    {
                        nodes.Add(node);
                        continue;
                    }
                }
                throw new NotSupportSyntaxNodeContentException();
            }
            return nodes;
        }
        protected void SetProperty(Node node, string name, object value)
        {
            foreach (var propertyInfo in node.GetType().GetProperties())
            {
                if (propertyInfo.CanWrite && (propertyInfo.Name.ToLower() == name.ToLower()))
                {
                    propertyInfo.SetValue(node, value);
                    return;
                }
            }
            Console.WriteLine("The '{0}' field is ignored in node '{1}'", name, node.GetType().ToString());
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
            var nodeType = this.syntaxNodeNameAndTypes[nodeKind.ToString()];
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
    }

}
