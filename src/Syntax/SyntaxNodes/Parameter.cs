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
            internal set;
        }

        public Node QuestionToken
        {
            get;
            internal set;
        }

        public Node Initializer
        {
            get;
            internal set;
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

