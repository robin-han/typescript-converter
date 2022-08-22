namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.Parameter)]
    public class Parameter : LocalVariableDeclaration
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

        public Node QuestionToken
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
                return this.DotDotDotToken != null;
            }
        }

        public Node VariableType
        {
            get
            {
                if (this.IsVariable)
                {
                    return TypeHelper.GetArrayElementType(this.Type);
                }
                return null;
            }
        }
        #endregion

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

        public static Node CreateInitializer(Node type)
        {
            if (TypeHelper.IsNativeBool(type) || type.Kind == NodeKind.BooleanKeyword)
            {
                return NodeHelper.CreateNode(NodeKind.FalseKeyword);
            }
            else if (TypeHelper.IsNativeNumber(type) || type.Kind == NodeKind.NumberKeyword)
            {
                return NodeHelper.CreateNode(NodeKind.NumericLiteral, "0");
            }
            else
            {
                return NodeHelper.CreateNode(NodeKind.NullKeyword);
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
