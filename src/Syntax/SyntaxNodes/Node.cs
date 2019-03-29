using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class Node
    {
        private string _text = null;

        #region Properties
        public virtual NodeKind Kind
        {
            get { return NodeKind.Unknown; }
        }

        public Node Parent
        {
            get;
            private set;
        }

        public Node Root
        {
            get
            {
                Node parent = this;
                while (parent.Parent != null)
                {
                    parent = parent.Parent;
                }
                return parent;
            }
        }

        public List<Node> OriginalChildren
        {
            get;
            private set;
        }

        public List<Node> Children
        {
            get
            {
                List<Node> children = new List<Node>();

                PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in properties)
                {
                    if (!prop.CanWrite || this.IsIgnoredProperty(prop.Name))
                    {
                        continue;
                    }

                    Type propType = prop.PropertyType;
                    if (this.IsNodeType(propType))
                    {
                        Node node = prop.GetValue(this) as Node;
                        if (node != null && !children.Contains(node))
                        {
                            children.Add(node);
                        }
                    }
                    else if (this.IsNodeListType(propType))
                    {
                        List<Node> nodes = prop.GetValue(this) as List<Node>;
                        if (nodes != null && nodes.Count > 0)
                        {
                            children.AddRange(nodes);
                        }
                    }
                }
                return children;
            }
        }

        public string Path
        {
            get;
            internal set;
        }

        public string NodeName
        {
            get
            {
                string name = this.Path.Split(".").Last();
                Match match = Regex.Match(name, @"(.*)\[\d+\]"); ;
                if (match.Success)
                {
                    name = match.Groups[1].Value;
                }
                return name;
            }
        }

        public string Text
        {
            get
            {
                if (this._text == null)
                {
                    this._text = this.GetText();
                }
                return this._text;
            }
            internal set
            {
                this._text = value;
            }
        }

        public int Pos
        {
            get;
            internal set;
        }

        public int End
        {
            get;
            internal set;
        }

        public int Flags
        {
            get;
            private set;
        }

        public Dictionary<string, Object> Pocket
        {
            get;
            private set;
        }

        public JObject TsNode
        {
            get;
            internal set;
        }
        #endregion

        #region Public Methods
        public virtual void Init(JObject jsonObj)
        {
            this.TsNode = jsonObj;

            this.Parent = null;
            this.OriginalChildren = new List<Node>();

            this.Pocket = new Dictionary<string, object>();
            this.Path = jsonObj.Path;

            JToken jsonText = jsonObj["text"];
            this.Text = jsonText?.ToObject<string>();

            JToken jsonPos = jsonObj["pos"];
            this.Pos = jsonPos == null ? 0 : jsonPos.ToObject<int>();

            JToken jsonEnd = jsonObj["end"];
            this.End = jsonEnd == null ? 0 : jsonEnd.ToObject<int>();

            JToken jsonFlags = jsonObj["flags"];
            this.Flags = jsonFlags == null ? 0 : jsonFlags.ToObject<int>();
        }

        public bool HasProperty(string name)
        {
            return this.GetType().GetProperty(name) != null;
        }

        public object GetValue(string propName)
        {
            PropertyInfo prop = this.GetType().GetProperty(propName);
            if (prop != null && prop.CanRead)
            {
                return prop.GetValue(this);
            }
            return null;
        }

        public void SetValue(string propName, object value)
        {
            PropertyInfo prop = this.GetType().GetProperty(propName);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(this, value);
            }
        }

        public virtual void AddNode(Node childNode)
        {
            childNode.Parent = this;
            this.OriginalChildren.Add(childNode);
        }

        internal void AddNode(JObject tsNode)
        {
            this.AddNode(this.CreateNode(tsNode));
        }

        public void Remove(Node childNode)
        {
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo prop in properties)
            {
                if (!prop.CanWrite || this.IsIgnoredProperty(prop.Name))
                {
                    continue;
                }

                Type propType = prop.PropertyType;
                bool isNodeType = this.IsNodeType(propType);
                bool isNodeListType = this.IsNodeListType(propType);
                if (!isNodeType && !isNodeListType)
                {
                    continue;
                }

                if (isNodeType && object.Equals(prop.GetValue(this), childNode))
                {
                    prop.SetValue(this, null);
                }
                if (isNodeListType)
                {
                    Type itemType = propType.GetGenericArguments()[0];
                    Type removeType = childNode.GetType();
                    if (removeType.Equals(itemType) || removeType.IsSubclassOf(itemType))
                    {
                        MethodInfo removeMethod = propType.GetMethod("Remove");
                        removeMethod.Invoke(prop.GetValue(this), new object[] { childNode });
                    }
                }
            }
        }

        public List<Node> Descendants(Predicate<Node> match = null)
        {
            List<Node> nodes = new List<Node>();

            Queue<Node> queue = new Queue<Node>(this.Children);
            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                if (match == null || match.Invoke(node))
                {
                    nodes.Add(node);
                }

                foreach (Node nd in node.Children)
                {
                    queue.Enqueue(nd);
                }
            }
            return nodes;
        }

        public List<Node> DescendantsAndSelf(Predicate<Node> match = null)
        {
            List<Node> nodes = new List<Node>();
            if (match == null || match.Invoke(this))
            {
                nodes.Add(this);
            }
            nodes.AddRange(this.Descendants(match));

            return nodes;
        }
        #endregion

        #region Internal and Private Methods
        /// <summary>
        /// Get node's original text.
        /// </summary>
        /// <returns></returns>
        internal virtual string GetText()
        {
            Node root = this.Root;
            if (root.Kind == NodeKind.SourceFile)
            {
                return root.Text.Substring(this.Pos, this.End - this.Pos);
            }
            return string.Empty;
        }

        public Node CreateNode(NodeKind kind, string text = "")
        {
            return this.CreateNode(
                "{ " +
                    "kind: \"" + kind.ToString() + "\", " +
                    "text: \"" + text + "\" " +
                "}");
        }

        public Node CreateNode(string json)
        {
            return this.CreateNode(JObject.Parse(json));
        }

        public Node CreateNode(JObject nodeJson)
        {
            Node node = new AstBuilder().Build(nodeJson);
            node.Parent = this;

            return node;
        }

        protected Node ToQualifiedName(PropertyAccessExpression expression)
        {
            string jsonString = expression.TsNode.ToString();

            jsonString = jsonString
                .Replace("PropertyAccessExpression", "QualifiedName")
                .Replace("\"expression\":", "\"left\":")
                .Replace("\"name\":", "\"right\":");

            return this.CreateNode(jsonString);
        }

        public Node GetNodeType()
        {
            return this.GetNodeType(this);
        }

        public Node GetNodeType(Node node)
        {
            switch (node.Kind)
            {
                case NodeKind.StringLiteral:
                    return this.CreateNode(NodeKind.StringKeyword);

                case NodeKind.NumericLiteral:
                    return this.CreateNode(NodeKind.NumberKeyword);

                case NodeKind.PrefixUnaryExpression:
                    PrefixUnaryExpression prefixExpr = node as PrefixUnaryExpression;
                    if (prefixExpr.Operand.Kind == NodeKind.NumericLiteral)
                    {
                        return this.CreateNode(NodeKind.NumberKeyword);
                    }
                    return this.CreateNode(NodeKind.ObjectKeyword);

                case NodeKind.TrueKeyword:
                case NodeKind.FalseKeyword:
                    return this.CreateNode(NodeKind.BooleanKeyword);

                case NodeKind.NewExpression:
                    return (node as NewExpression).Type;

                case NodeKind.Identifier:
                    Node identifierType = this.GetIndentifierType(node as Identifier);
                    return identifierType ?? this.CreateNode(NodeKind.ObjectKeyword);

                case NodeKind.PropertyAccessExpression:
                    Node propertyAccessType = this.GetPropertyAccessType(node as PropertyAccessExpression);
                    return propertyAccessType ?? this.CreateNode(NodeKind.ObjectKeyword);

                //case NodeKind.CallExpression:
                //    //TODO: find global method's type
                //    return null;

                case NodeKind.ArrayLiteralExpression:
                    return this.GetArrayLiteralType(node as ArrayLiteralExpression);

                case NodeKind.ObjectLiteralExpression:
                    return this.GetObjectLiteralType(node as ObjectLiteralExpression);

                default:
                    return node.GetValue("Type") as Node;
            }
        }

        protected Node GetIndentifierType(Identifier identifier)
        {
            string name = identifier.Text;

            Block block = this.GetBlockParent(identifier);
            while (block != null)
            {
                foreach (var statement in block.Statements)
                {
                    if (statement.Kind != NodeKind.VariableStatement)
                    {
                        continue;
                    }

                    VariableDeclarationList declarationList = (statement as VariableStatement).DeclarationList as VariableDeclarationList;
                    VariableDeclarationNode declarationNode = declarationList.Declarations[0] as VariableDeclarationNode;
                    if (declarationNode.Name.Text == name)
                    {
                        return declarationNode.Type;
                    }
                }
                block = GetBlockParent(block);
            }

            MethodDeclaration methodDeclaration = this.GetMethodDeclarationParent(identifier);
            if (methodDeclaration != null && (methodDeclaration.Parameters.Find(p => (p as Parameter).Name.Text == name) is Parameter methodParameter))
            {
                return methodParameter.Type;
            }

            Constructor ctor = this.GetConstructorParent(identifier);
            if (ctor != null && (ctor.Parameters.Find(p => (p as Parameter).Name.Text == name) is Parameter ctorParameter))
            {
                return ctorParameter.Type;
            }

            return null;
        }

        protected Node GetPropertyAccessType(PropertyAccessExpression propertyAccess)
        {
            ClassDeclaration classDeclaration = this.GetClassDeclarationParent(propertyAccess);
            if (classDeclaration != null)
            {
                string name = propertyAccess.Name.Text;
                foreach (var memeber in classDeclaration.Members)
                {
                    if (memeber.Kind != NodeKind.PropertyDeclaration)
                    {
                        continue;
                    }

                    PropertyDeclaration propertyDeclaration = memeber as PropertyDeclaration;
                    if (propertyDeclaration.Name.Text == name)
                    {
                        return propertyDeclaration.Type;
                    }
                }
            }
            return null;
        }

        private Node GetArrayLiteralType(ArrayLiteralExpression arrayLiteral)
        {
            Node type = this.GetDeclarationType(arrayLiteral);
            if (type != null)
            {
                return type;
            }
            Node valueType = this.GetItemType(arrayLiteral);
            Node arrayType = this.CreateNode(NodeKind.ArrayType);
            arrayType.AddNode(valueType);
            return arrayType;
        }

        private Node GetObjectLiteralType(ObjectLiteralExpression objectLiteral)
        {
            Node type = this.GetDeclarationType(objectLiteral);
            if (type != null)
            {
                return type;
            }

            List<Node> properties = objectLiteral.Properties;
            if (properties.Count == 0)
            {
                TypeLiteral typeLiteral = this.CreateNode(NodeKind.TypeLiteral) as TypeLiteral;
                typeLiteral.Members.Add(this.CreateNode(NodeKind.IndexSignature));
                return typeLiteral;
            }
            else
            {
                TypeLiteral typeLiteral = this.CreateNode(NodeKind.TypeLiteral) as TypeLiteral;
                foreach (PropertyAssignment prop in properties)
                {
                    Node elementType = this.GetNodeType(prop.Initializer);
                    elementType = elementType ?? this.CreateNode(NodeKind.ObjectKeyword);
                    elementType.Path = "type";

                    Node propSignature = this.CreateNode(NodeKind.PropertySignature);
                    propSignature.AddNode(prop.Name.TsNode);
                    propSignature.AddNode(elementType);

                    typeLiteral.Members.Add(propSignature);
                }
                return typeLiteral;
            }
        }

        private Node GetItemType(ArrayLiteralExpression value)
        {
            Node elementType = null;

            List<Node> elements = value.Elements;
            if (elements.Find(n => n.Kind == NodeKind.StringLiteral) != null)
            {
                elementType = this.CreateNode(NodeKind.StringKeyword);
            }
            else if (elements.Find(n => n.Kind == NodeKind.NumericLiteral || (n.Kind == NodeKind.PrefixUnaryExpression && (n as PrefixUnaryExpression).Operand.Kind == NodeKind.NumericLiteral)) != null)
            {
                elementType = this.CreateNode(NodeKind.NumberKeyword);
            }
            else
            {
                foreach (Node element in elements)
                {
                    if (element.Kind == NodeKind.Identifier || element.Kind == NodeKind.PropertyAccessExpression)
                    {
                        elementType = this.GetNodeType(element);
                        break;
                    }
                }
            }

            elementType = elementType ?? this.CreateNode(NodeKind.ObjectKeyword);
            elementType.Path = "elementType";
            return elementType;
        }

        private Node GetDeclarationType(Node node)
        {
            VariableDeclarationNode variableParent = node.Parent as VariableDeclarationNode;
            if (variableParent != null)
            {
                return variableParent.Type;
            }
            //
            Parameter paramterParent = node.Parent as Parameter;
            if (paramterParent != null)
            {
                return paramterParent.Type;
            }
            //
            PropertyDeclaration propertyParent = node.Parent as PropertyDeclaration;
            if (propertyParent != null)
            {
                return propertyParent.Type;
            }
            //
            BinaryExpression binaryParent = node.Parent as BinaryExpression;
            if (binaryParent != null && binaryParent.OperatorToken.Kind == NodeKind.EqualsToken) //assign
            {
                switch (binaryParent.Left.Kind)
                {
                    case NodeKind.Identifier:
                        return this.GetIndentifierType(binaryParent.Left as Identifier);

                    case NodeKind.PropertyAccessExpression:
                        return this.GetPropertyAccessType(binaryParent.Left as PropertyAccessExpression);

                    default:
                        return null;
                }
            }
            return null;
        }

        private ClassDeclaration GetClassDeclarationParent(Node node)
        {
            Node parent = node.Parent;
            while (parent != null && parent.Kind != NodeKind.ClassDeclaration)
            {
                if (parent.Kind == NodeKind.ModuleBlock)
                {
                    break;
                }

                parent = parent.Parent;
            }
            return parent as ClassDeclaration;
        }

        private MethodDeclaration GetMethodDeclarationParent(Node node)
        {
            Node parent = node.Parent;
            while (parent != null && parent.Kind != NodeKind.MethodDeclaration)
            {
                if (parent.Kind == NodeKind.ClassDeclaration)
                {
                    break;
                }

                parent = parent.Parent;
            }
            return parent as MethodDeclaration;
        }

        private Constructor GetConstructorParent(Node node)
        {
            Node parent = node.Parent;
            while (parent != null && parent.Kind != NodeKind.Constructor)
            {
                if (parent.Kind == NodeKind.ClassDeclaration)
                {
                    break;
                }

                parent = parent.Parent;
            }
            return parent as Constructor;
        }

        private Block GetBlockParent(Node node)
        {
            Node parent = node.Parent;
            while (parent != null && parent.Kind != NodeKind.Block)
            {
                if (parent.Kind == NodeKind.MethodDeclaration)
                {
                    break;
                }
                parent = parent.Parent;
            }
            return parent as Block;
        }

        private bool IsNodeType(Type type)
        {
            Type nodeType = typeof(Node);
            return type.Equals(nodeType) || type.IsSubclassOf(nodeType);
        }

        private bool IsNodeListType(Type type)
        {
            if (type.IsGenericType)
            {
                Type nodeType = typeof(Node);
                Type itemType = type.GetGenericArguments()[0];
                return itemType.Equals(nodeType) || itemType.IsSubclassOf(nodeType);
            }
            return false;
        }

        private bool IsIgnoredProperty(string propName)
        {
            return (propName == "Parent" || propName == "OriginalChildren");
        }
        #endregion


        internal bool HasJsDocTag(string tagName)
        {
            List<Node> jsDoc = this.GetValue("JsDoc") as List<Node>;
            if (jsDoc != null && jsDoc.Count > 0)
            {
                JSDocComment docComment = jsDoc[0] as JSDocComment;
                if (docComment != null)
                {
                    return docComment.Tags.Find(tag => tag.Kind == NodeKind.JSDocTag && (tag as JSDocTag).TagName.Text == tagName) != null;
                }
            }
            return false;
        }


        [Conditional("DEBUG")]
        protected void ProcessUnknownNode(Node child)
        {
            Console.WriteLine(string.Format("WARNING: {0} does not process child node {1}, Code: {2}", child.Parent.Kind, child.Kind, child.Text));
        }

    }
}
