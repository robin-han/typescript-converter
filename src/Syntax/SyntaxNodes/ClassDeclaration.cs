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
                    }
                    types.AddRange(heritage.Types);
                }

                //make all class has the base object
                if (!hasBaseClass)
                {
                    types.Add(this.CreateNode("{ " +
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

        protected override void NormalizeImp()
        {
            base.NormalizeImp();

            if (!this.Modifiers.Exists(n => n.Kind == NodeKind.PublicKeyword || n.Kind == NodeKind.PrivateKeyword || n.Kind == NodeKind.ProtectedKeyword))
            {
                this.Modifiers.Add(this.CreateNode(NodeKind.PublicKeyword));
            }

            this.RemoveUnnecessaryMembers();
            this.CombineGetSetAccess();
            this.SeparateMembers();
        }

        private void RemoveUnnecessaryMembers()
        {
            for (int i = 0; i < this.Members.Count; i++)
            {
                Node member = this.Members[i];

                switch (member.Kind)
                {
                    case NodeKind.SemicolonClassElement: //;
                    case NodeKind.IndexSignature: //ley
                        this.Members.RemoveAt(i--);
                        break;

                    default:
                        break;
                }
            }
        }

        private void CombineGetSetAccess()
        {
            List<Node> removedNodes = new List<Node>();

            for (int i = 0; i < this.Members.Count; i++)
            {
                if (this.Members[i].Kind != NodeKind.GetAccessor)
                {
                    continue;
                }

                GetAccessor getAccessor = this.Members[i] as GetAccessor;
                SetAccessor setAccessor = this.Members.Find(c =>
                    (c.Kind == NodeKind.SetAccessor) &&
                    ((c as SetAccessor).Name.Text == getAccessor.Name.Text)) as SetAccessor;

                if (setAccessor != null)
                {
                    removedNodes.Add(getAccessor);
                    removedNodes.Add(setAccessor);

                    Node getSestAccessor = this.CreateNode(NodeKind.GetSetAccessor);
                    getSestAccessor.AddNode(getAccessor.TsNode);
                    getSestAccessor.AddNode(setAccessor.TsNode);
                    this.Members.Insert(i++, getSestAccessor);
                }
            }

            this.Members.RemoveAll(m => removedNodes.IndexOf(m) >= 0);
        }

        //Separate getset method to two method
        private void SeparateMembers()
        {
            for (int i = 0; i < this.Members.Count; i++)
            {
                Node member = this.Members[i];

                if (this.CanSeparate(member))
                {
                    this.Members.RemoveAt(i);
                    this.Members.InsertRange(i++, this.SeparateMember(member));
                }
            }
        }

        private List<Node> SeparateMember(Node member)
        {
            List<Node> newMembers = new List<Node>();

            JObject getMethod = new JObject(member.TsNode);
            getMethod["parameters"][0].Remove();
            getMethod["body"]["statements"] = getMethod["body"]["statements"][0]["thenStatement"]["statements"];
            newMembers.Add(this.CreateNode(getMethod));

            JObject setMethod = new JObject(member.TsNode);
            setMethod.Remove("type");
            (setMethod["parameters"][0] as JObject).Remove("questionToken");
            JToken elseStatement = setMethod["body"]["statements"][0]["elseStatement"];
            if (elseStatement["kind"].ToObject<string>() == "Block")
            {
                setMethod["body"]["statements"] = elseStatement = elseStatement["statements"];
            }
            else
            {
                setMethod["body"]["statements"][0] = elseStatement;
            }
            newMembers.Add(this.CreateNode(setMethod));

            return newMembers;
        }

        private bool CanSeparate(Node node)
        {
            if (node.Kind != NodeKind.MethodDeclaration || (node as MethodDeclaration).Parameters.Count != 1)
            {
                return false;
            }

            MethodDeclaration method = node as MethodDeclaration;
            Parameter parameter = method.Parameters[0] as Parameter;
            Block body = method.Body as Block;
            if (!parameter.IsOptional || body == null || body.Statements.Count != 1 || body.Statements[0].Kind != NodeKind.IfStatement)
            {
                return false;
            }

            IfStatement ifStatement = body.Statements[0] as IfStatement;
            if (ifStatement.ElseStatement == null || ifStatement.Expression.Text.Replace(" ", "") != "arguments.length<=0")
            {
                return false;
            }

            return true;
        }

    }
}

