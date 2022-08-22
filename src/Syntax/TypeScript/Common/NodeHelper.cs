using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

using GrapeCity.Syntax.Converter.Source.TypeScript.Builders;

namespace TypeScript.Syntax
{
    public class NodeHelper
    {
        public static Node CreateNode(NodeKind kind, string text = null)
        {
            if (text != null)
            {
                return CreateNode(
                    "{ " +
                        "kind: \"" + kind.ToString() + "\", " +
                        "text: \"" + text + "\" " +
                    "}");
            }
            else
            {
                return CreateNode(
                    "{ " +
                        "kind: \"" + kind.ToString() + "\" " +
                    "}");
            }
        }

        public static Node CreateNode(string json)
        {
            return CreateNode(JObject.Parse(json));
        }

        public static Node CreateNode(JObject nodeJson)
        {
            var node = new AbstractSyntaxTreeBuilder().Build(nodeJson);
            if (node != null)
            {
                node.Pos = 0;
                node.End = 0;
            }

            return node;
        }
    }
}
