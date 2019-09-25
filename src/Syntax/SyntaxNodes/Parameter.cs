using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class Parameter : Node
    {
        private Node _type;
        private Node _questionToken;
        private Node _initializer;

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
                return this._type;
            }
            internal set
            {
                this._type = value;
                if (this._type != null)
                {
                    this._type.Parent = this;
                }
            }
        }

        public Node QuestionToken
        {
            get
            {
                return this._questionToken;
            }
            internal set
            {
                this._questionToken = value;
                if (this._questionToken != null)
                {
                    this._questionToken.Parent = this;
                }
            }
        }

        public Node Initializer
        {
            get
            {
                return this._initializer;
            }
            internal set
            {
                this._initializer = value;
                if (this._initializer != null)
                {
                    this._initializer.Parent = this;
                }
            }
        }

        public bool IsOptional
        {
            get
            {
                return this.QuestionToken != null;
            }
        }

        public bool IsVariable
        {
            get
            {
                if (this.DotDotDotToken == null)
                {
                    return false;
                }

                Node type = this.Type;
                if (type.Kind == NodeKind.ArrayType)
                {
                    ArrayType arrayType = type as ArrayType;
                    if (arrayType.ElementType.Kind == NodeKind.ArrayType)
                    {
                        return false;
                    }
                }

                if (type.Kind == NodeKind.TypeReference)
                {
                    TypeReference typeReference = type as TypeReference;
                    if (typeReference.TypeName.Text == "Array" && typeReference.TypeArguments.Count > 0)
                    {
                        Node typeArgument = typeReference.TypeArguments[0];
                        if (typeArgument.Kind == NodeKind.ArrayType)
                        {
                            return false;
                        }
                        if (typeArgument.Kind == NodeKind.TypeReference && (typeArgument as TypeReference).TypeName.Text == "Array")
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        public bool IgnoreType
        {
            get;
            internal set;
        }
        #endregion


        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.DotDotDotToken = null;
            this.Name = null;
            this.Type = null;
            this.QuestionToken = null;
            this.Initializer = null;
            this.IgnoreType = false;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

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
                    this.Type = childNode;
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

    }
}

