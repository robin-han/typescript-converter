using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
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
                    types.Insert(0, NodeHelper.CreateNode("{ " +
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

        public Constructor GetConstructor()
        {
            return this.Members.Find(m => m.Kind == NodeKind.Constructor) as Constructor;
        }

        public Node GetMember(string name)
        {
            List<Node> members = GetMembers(name);
            if (members.Count > 0)
            {
                return members[0];
            }
            return null;
        }

        public List<Node> GetMembers(string name)
        {
            List<Node> ret = new List<Node>();

            foreach (Node member in this.Members)
            {
                switch (member.Kind)
                {
                    case NodeKind.MethodDeclaration:
                        MethodDeclaration method = member as MethodDeclaration;
                        if (method.Name.Text == name)
                        {
                            ret.Add(method);
                        }
                        break;
                    case NodeKind.GetAccessor:
                        GetAccessor getAccess = member as GetAccessor;
                        if (getAccess.Name.Text == name)
                        {
                            ret.Add(getAccess);
                        }
                        break;
                    case NodeKind.GetSetAccessor:
                        GetSetAccessor getSetAccess = member as GetSetAccessor;
                        if (getSetAccess.Name.Text == name)
                        {
                            ret.Add(getSetAccess);
                        }
                        break;
                    case NodeKind.PropertyDeclaration:
                        PropertyDeclaration prop = member as PropertyDeclaration;
                        if (prop.Name.Text == name)
                        {
                            ret.Add(prop);
                        }
                        break;
                    default:
                        break;
                }
            }

            return ret;
        }

    }
}

