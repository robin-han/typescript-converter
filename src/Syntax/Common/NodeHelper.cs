using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax
{
    internal class NodeHelper
    {
        public static Node CreateNode(NodeKind kind, string text = "")
        {
            return CreateNode(
                "{ " +
                    "kind: \"" + kind.ToString() + "\", " +
                    "text: \"" + text + "\" " +
                "}");
        }

        public static Node CreateNode(string json)
        {
            return CreateNode(JObject.Parse(json));
        }

        public static Node CreateNode(JObject nodeJson)
        {
            return new AstBuilder().Build(nodeJson);
        }
    }
}
