using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class VariableDeclarationNode : Declaration
    {
        private Node _type;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.VariableDeclaration; }
        }

        public Node VariableDeclaration
        {
            get;
            private set;
        }

        public Node Name
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

        public Node Initializer
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Name = null;
            this.Initializer = null;

            this._type = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "name":
                    this.Name = childNode;
                    break;

                case "type":
                    this._type = childNode;
                    break;

                case "initializer":
                    this.Initializer = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override Node InferType()
        {
            Node initValue = this.Initializer;
            if (initValue != null)
            {
                return this.GetValueType(initValue);
            }

            if (this.Parent.Kind == NodeKind.CatchClause)
            {
                return this.CreateNode(NodeKind.Identifier, "Exception");
            }
            return this.CreateNode(NodeKind.AnyKeyword);
        }

        private Node GetValueType(Node value)
        {
            switch (value.Kind)
            {
                case NodeKind.StringLiteral:
                    return this.CreateNode(NodeKind.StringKeyword);

                case NodeKind.NumericLiteral:
                    NumberKeyword numNode = this.CreateNode(NodeKind.NumberKeyword) as NumberKeyword;
                    string originalText = value.GetText();
                    if (originalText.IndexOf('.') == 0)
                    {
                        numNode.Integer = true;
                    }
                    return numNode;

                case NodeKind.TrueKeyword:
                case NodeKind.FalseKeyword:
                    return this.CreateNode(NodeKind.BooleanKeyword);

                case NodeKind.NewExpression:
                    return this.CreateNode((value as NewExpression).Type.TsNode);

                //case NodeKind.Identifier:
                //    //TODO: find the variablelist
                //    return null;

                //case NodeKind.PropertyAccessExpression:
                //    //TODO: find the method defined Utils.Const = 'abc';
                //    return null;

                //case NodeKind.CallExpression:
                //    //TODO: find global method's type
                //    return null;

                case NodeKind.ArrayLiteralExpression:
                    List<Node> elements = (value as ArrayLiteralExpression).Elements;
                    if (elements.Count == 0)
                    {
                        return this.CreateNode(NodeKind.ObjectKeyword);
                    }

                    Node arrayType = this.CreateNode(NodeKind.ArrayType);
                    Node elementType = this.GetValueType(elements[0]);
                    elementType.Path = "elementType";
                    arrayType.AddNode(elementType);//only check first element
                    return arrayType;

                case NodeKind.ObjectLiteralExpression:
                    List<Node> properties = (value as ObjectLiteralExpression).Properties;
                    if (properties.Count == 0)
                    {
                        return this.CreateNode(NodeKind.ObjectKeyword);
                    }

                    TypeLiteral typeLiteral = this.CreateNode(NodeKind.TypeLiteral) as TypeLiteral;
                    foreach (PropertyAssignment prop in properties)
                    {
                        Node type = this.GetValueType(prop.Initializer);
                        type.Path = "type";

                        Node propSignature = this.CreateNode(NodeKind.PropertySignature);
                        propSignature.AddNode(prop.Name.TsNode);
                        propSignature.AddNode(type);

                        typeLiteral.Members.Add(propSignature);
                    }
                    return typeLiteral;

                default:
                    return this.CreateNode(NodeKind.ObjectKeyword);
            }
        }

    }
}

