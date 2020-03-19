using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class Parameter : Node
    {
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
            get;
            private set;
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

        public void SetType(Node type, bool changeParent = true)
        {
            this.Type = type;
            if (changeParent && this.Type != null)
            {
                this.Type.Parent = this;
            }
        }

        public void SetQuestionToken(Node questionToken, bool changeParent = true)
        {
            this.QuestionToken = questionToken;
            if (changeParent && this.QuestionToken != null)
            {
                this.QuestionToken.Parent = this;
            }
        }

        public void SetInitializer(Node initializer, bool changeParent = true)
        {
            this.Initializer = initializer;
            if (changeParent && this.Initializer != null)
            {
                this.Initializer.Parent = this;
            }
        }
    }
}

