using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class Parameter : Node
    {
        private Node _type;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.Parameter; }
        }

        public Node DotDotDotToken
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

        public Node QuestionToken
        {
            get;
            private set;
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public bool IsOptional
        {
            get
            {
                return this.QuestionToken != null;
            }
        }
        #endregion


        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.DotDotDotToken = null;
            this.Name = null;
            this._type = null;
            this.QuestionToken = null;
            this.Initializer = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "dotDotDotToken":
                    this.DotDotDotToken = childNode;
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                case "type":
                    this._type = childNode;
                    break;

                case "questionToken":
                    this.QuestionToken = childNode;
                    break;

                case "initializer":
                    this.Initializer = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override void NormalizeImp()
        {
            Node type = this.Type;

            if (this.QuestionToken != null)
            {
                switch (type.Kind)
                {
                    case NodeKind.NumberKeyword:
                        (type as NumberKeyword).Nullable = true;
                        break;

                    case NodeKind.BooleanKeyword:
                        (type as BooleanKeyword).Nullable = true;
                        break;

                    //TODO: enum  NodeKind.TypeReference
                    default:
                        break;
                }
                if (this.Initializer == null)
                {
                    this.Initializer = this.CreateNode(NodeKind.NullKeyword);
                }
            }

            if (this.DotDotDotToken != null && type.Kind == NodeKind.ArrayType)
            {
                (type as ArrayType).IsList = false;
            }
        }

        protected override Node InferType()
        {
            return this.CreateNode(NodeKind.AnyKeyword);
        }

    }
}

