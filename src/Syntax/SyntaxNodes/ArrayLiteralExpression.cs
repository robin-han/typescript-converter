using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ArrayLiteralExpression : Expression
    {
        private Node _type;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ArrayLiteralExpression; }
        }

        public bool MultiLine
        {
            get;
            private set;
        }

        public List<Node> Elements
        {
            get;
            private set;
        }

        public Node Type
        {
            get
            {
                if (this._type == null)
                {
                    this._type = this.InferType();
                }
                return this._type;
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Elements = new List<Node>();

            JToken jsonMultiLine = jsonObj["multiLine"];
            this.MultiLine = jsonMultiLine == null ? false : jsonMultiLine.ToObject<bool>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "elements":
                    this.Elements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override Node InferType()
        {
            Node parent = this.Parent;
            switch (this.Parent.Kind)
            {
                case NodeKind.VariableDeclaration:
                    return (parent as VariableDeclarationNode).Type;

                case NodeKind.PropertyDeclaration:
                    return (parent as PropertyDeclaration).Type;

                case NodeKind.Parameter:
                    return (parent as Parameter).Type;

                case NodeKind.BinaryExpression:
                    BinaryExpression binaryExpr = parent as BinaryExpression;
                    if (binaryExpr.OperatorToken.Kind == NodeKind.EqualsToken) //assign
                    {
                        //TODO: find left(propertyAccess.Type)
                    }
                    return this.CreateNode(NodeKind.ObjectKeyword);

                default:
                    //TODO:
                    return this.CreateNode(NodeKind.ObjectKeyword);
            }
        }

    }
}

