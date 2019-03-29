using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ClassDeclaration : Declaration
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ClassDeclaration; }
        }

        public List<Node> JsDoc
        {
            get;
            private set;
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public Node Name
        {
            get;
            private set;
        }

        public List<Node> TypeParameters
        {
            get;
            private set;
        }

        public List<Node> HeritageClauses
        {
            get;
            private set;
        }

        public List<Node> Members
        {
            get;
            private set;
        }

        public List<Node> BaseTypes
        {
            get
            {
                bool hasBaseClass = false;
                List<Node> types = new List<Node>();
                foreach (HeritageClause heritage in this.HeritageClauses)
                {
                    if (heritage.Text.Contains("extends"))
                    {
                        hasBaseClass = true;
                        types.InsertRange(0, heritage.Types);
                    }
                    else
                    {
                        types.AddRange(heritage.Types);
                    }
                }

                //make all class has the base object
                if (!hasBaseClass)
                {
                    types.Insert(0, this.CreateNode("{ " +
                        "kind: \"ExpressionWithTypeArguments \", " +
                        "expression: { " +
                           "kind: \"Identifier\", " +
                           "text: \"Object\"" +
                        "}" +
                    "}"));
                }

                return types;
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
            this.Name = null;
            this.TypeParameters = new List<Node>();
            this.HeritageClauses = new List<Node>();
            this.Members = new List<Node>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                case "typeParameters":
                    this.TypeParameters.Add(childNode);
                    break;

                case "heritageClauses":
                    this.HeritageClauses.Add(childNode);
                    break;

                case "members":
                    this.Members.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
      
    }
}

