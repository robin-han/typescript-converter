using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax
{
    public static class TypeHelper
    {
        public static Node NormalizeUnionType(Node type)
        {
            if (type.Kind == NodeKind.UnionType)
            {
                List<Node> types = (type as UnionType).Types;
                if (types.Count == 2)
                {
                    if (types[0].Kind == NodeKind.NullKeyword)
                    {
                        return types[1];
                    }
                    else if (types[1].Kind == NodeKind.NullKeyword)
                    {
                        return types[0];
                    }
                }
            }
            return type;
        }

        public static string GetTypeName(Node typeNode)
        {
            if (typeNode.GetValue("Name") is Node name)
            {
                return name.Text;
            }
            return null;
        }

        public static string GetName(string typeName)
        {
            string ret = typeName;
            int genericIndex = typeName.IndexOf("<");
            if (genericIndex >= 0)
            {
                ret = typeName.Substring(0, genericIndex);
            }
            string[] parts = ret.Split('.');
            return parts[parts.Length - 1].Trim();
        }

        public static bool IsNumberType(Node type)
        {
            if (type == null)
            {
                return false;
            }
            return type.Kind == NodeKind.NumberKeyword;
        }

        public static bool IsStringType(Node type)
        {
            if (type == null)
            {
                return false;
            }

            if (type.Kind == NodeKind.StringKeyword)
            {
                return true;
            }

            string text = type.Text.Trim();
            if (text == "string")
            {
                return true;
            }
            return false;
        }

        public static bool IsArrayType(Node type)
        {
            if (type == null)
            {
                return false;
            }

            if (type.Kind == NodeKind.ArrayType)
            {
                return true;
            }
            if (type.Kind == NodeKind.TypeReference)
            {
                return (type as TypeReference).TypeName.Text == "Array";
            }

            string text = type.Text.Trim();
            if (!string.IsNullOrEmpty(text) && text.StartsWith("Array<"))
            {
                return true;
            }
            return false;
        }

        public static bool IsNodeType(System.Type type)
        {
            System.Type nodeType = typeof(Node);
            return type.Equals(nodeType) || type.IsSubclassOf(nodeType);
        }

        public static bool IsNodeListType(System.Type type)
        {
            if (type.IsGenericType)
            {
                System.Type nodeType = typeof(Node);
                System.Type itemType = type.GetGenericArguments()[0];
                return itemType.Equals(nodeType) || itemType.IsSubclassOf(nodeType);
            }
            return false;
        }

        public static Node GetTypeLiteralMemberType(TypeLiteral typeLiteral, string name)
        {
            foreach (Node node in typeLiteral.Members)
            {
                switch (node.Kind)
                {
                    case NodeKind.PropertySignature:
                        PropertySignature prop = node as PropertySignature;
                        if (prop.Name.Text == name)
                        {
                            return prop.Type;
                        }
                        break;

                    case NodeKind.IndexSignature:
                        IndexSignature index = node as IndexSignature;
                        return index.Type;

                    default:
                        break;
                }
            }
            return null;
        }

        public static Node GetNodeType(Node node)
        {
            switch (node.Kind)
            {
                case NodeKind.StringLiteral:
                    return NodeHelper.CreateNode(NodeKind.StringKeyword);

                case NodeKind.NumericLiteral:
                    return NodeHelper.CreateNode(NodeKind.NumberKeyword);

                case NodeKind.PrefixUnaryExpression:
                    return NodeHelper.CreateNode(NodeKind.NumberKeyword);

                case NodeKind.BinaryExpression:
                    return GetBinaryExpressionType(node as BinaryExpression);

                case NodeKind.TrueKeyword:
                case NodeKind.FalseKeyword:
                    return NodeHelper.CreateNode(NodeKind.BooleanKeyword);

                case NodeKind.NewExpression:
                    return (node as NewExpression).Type;

                case NodeKind.Identifier:
                    return GetIdentifierType(node as Identifier);

                case NodeKind.PropertyAccessExpression:
                case NodeKind.CallExpression:
                    return GetPropertyAccessType(node);

                case NodeKind.ArrayLiteralExpression:
                    return GetArrayLiteralType(node as ArrayLiteralExpression);

                case NodeKind.ObjectLiteralExpression:
                    return GetObjectLiteralType(node as ObjectLiteralExpression);

                case NodeKind.ElementAccessExpression:
                    return GetElementAccessType(node as ElementAccessExpression);

                case NodeKind.ParenthesizedExpression:
                    return GetNodeType((node as ParenthesizedExpression).Expression);

                case NodeKind.AsExpression:
                    return (node as AsExpression).Type;

                default:
                    return node.GetValue("Type") as Node;
            }
        }

        private static Node GetElementAccessType(ElementAccessExpression elementAccess)
        {
            //TODO:
            Node type = GetNodeType(elementAccess.Expression);
            if (type == null)
            {
                return null;
            }

            string name = elementAccess.ArgumentExpression.Text;
            switch (type.Kind)
            {
                case NodeKind.ArrayType:
                    return (type as ArrayType).ElementType;

                case NodeKind.TypeLiteral:
                    if (elementAccess.ArgumentExpression.Kind == NodeKind.Identifier)
                    {
                        return GetTypeLiteralMemberType(type as TypeLiteral, elementAccess.ArgumentExpression.Text);
                    }
                    return null;

                default:
                    return null;
            }
        }

        private static Node GetIdentifierType(Identifier identifier)
        {
            return GetIdentifierType(identifier, identifier.Text);
        }
        private static Node GetIdentifierType(Node startNode, string name)
        {
            Node parent = startNode.Parent;

            while (parent != null)
            {
                switch (parent.Kind)
                {
                    case NodeKind.Block:
                        Block block = parent as Block;
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
                        break;

                    case NodeKind.IfStatement:
                        IfStatement ifStatement = parent as IfStatement;
                        List<Node> nodes = ifStatement.Expression.DescendantsAndSelf(n =>
                        {
                            if (n.Kind == NodeKind.BinaryExpression)
                            {
                                BinaryExpression binary = n as BinaryExpression;
                                if (binary.OperatorToken.Kind == NodeKind.InstanceOfKeyword && binary.Left != startNode && binary.Left.Text == name)
                                {
                                    return true;
                                }
                            }
                            return false;
                        });
                        if (nodes.Count > 0)
                        {
                            return (nodes[0] as BinaryExpression).Right;
                        }
                        break;

                    case NodeKind.ForOfStatement:
                        ForOfStatement forOfStatement = parent as ForOfStatement;
                        if (forOfStatement.Identifier.Text == name)
                        {
                            Node forOfType = GetNodeType(forOfStatement.Expression);
                            if (forOfType != null && forOfType.Kind == NodeKind.ArrayType)
                            {
                                return (forOfType as ArrayType).ElementType;
                            }
                        }
                        break;

                    case NodeKind.MethodDeclaration:
                    case NodeKind.Constructor:
                    case NodeKind.ArrowFunction:
                    case NodeKind.FunctionDeclaration:
                    case NodeKind.FunctionExpression:
                        List<Node> parameters = parent.GetValue("Parameters") as List<Node>;
                        if (parameters != null)
                        {
                            if (parameters.Find(p => (p as Parameter).Name.Text == name) is Parameter param)
                            {
                                return param.Type;
                            }
                        }
                        break;

                    case NodeKind.ClassDeclaration:
                    case NodeKind.InterfaceDeclaration:
                        return null;

                    default:
                        break;
                }
                parent = parent.Parent;
            }

            return null;
        }

        private static Node GetPropertyAccessType(Node accessNode)
        {
            Node tailNode = GetPropertyAccessMember(accessNode);
            if (tailNode != null)
            {
                return tailNode.GetValue("Type") as Node;
            }
            return null;
        }

        private static Node GetPropertyAccessMember(Node accessNode)
        {
            string[] accessNames = GetNameParts(accessNode.Text);

            Document document = accessNode.Document;
            Project project = document?.Project;

            Node classNode = null;
            for (int i = 0; i < accessNames.Length; i++)
            {
                string memberName = accessNames[i];

                if (i == 0 && memberName == "this")
                {
                    classNode = accessNode.Ancestor(NodeKind.ClassDeclaration);
                }
                else if (memberName == "super")
                {
                    ClassDeclaration thisClassNode = accessNode.Ancestor(NodeKind.ClassDeclaration) as ClassDeclaration;
                    classNode = thisClassNode == null ? null : project.GetBaseClass(thisClassNode);
                    if (classNode != null && accessNames.Length == 1)
                    {
                        return (classNode as ClassDeclaration).GetConstructor();
                    }
                }
                else
                {
                    Node type = null;
                    Node member = null;
                    if (i == 0)
                    {
                        type = GetIdentifierType(accessNode, memberName);
                    }
                    else
                    {
                        member = GetClassInterfaceMember(classNode, memberName);
                        type = member?.GetValue("Type") as Node;
                    }

                    if (type == null)
                    {
                        return null;
                    }

                    if (i == accessNames.Length - 1)
                    {
                        return member;
                    }

                    string[] typeParts = GetNameParts(type.Text);
                    memberName = typeParts[typeParts.Length - 1];
                    classNode = project?.GetClass(memberName);
                    if (classNode == null)
                    {
                        classNode = project?.GetInterface(memberName);
                    }
                }

                if (classNode == null)
                {
                    return null;
                }
            }
            return null;
        }

        private static Node GetArrayLiteralType(ArrayLiteralExpression arrayLiteral)
        {
            Node type = GetDeclarationType(arrayLiteral);
            if (type != null)
            {
                return type;
            }

            Node valueType = GetItemType(arrayLiteral);
            Node arrayType = NodeHelper.CreateNode(NodeKind.ArrayType);
            arrayType.AddChild(valueType);
            return arrayType;
        }

        private static Node GetObjectLiteralType(ObjectLiteralExpression objectLiteral)
        {
            Node type = GetDeclarationType(objectLiteral);
            if (type != null)
            {
                return type;
            }

            List<Node> properties = objectLiteral.Properties;
            if (properties.Count == 0)
            {
                TypeLiteral typeLiteral = NodeHelper.CreateNode(NodeKind.TypeLiteral) as TypeLiteral;
                typeLiteral.Members.Add(NodeHelper.CreateNode(NodeKind.IndexSignature));
                return typeLiteral;
            }
            else
            {
                TypeLiteral typeLiteral = NodeHelper.CreateNode(NodeKind.TypeLiteral) as TypeLiteral;
                foreach (PropertyAssignment prop in properties)
                {
                    Node initValue = prop.Initializer;
                    Node elementType = null;
                    if (initValue.Kind != NodeKind.ObjectLiteralExpression && initValue.Kind != NodeKind.ArrayLiteralExpression)
                    {
                        elementType = GetNodeType(initValue);
                    }
                    elementType = elementType ?? NodeHelper.CreateNode(NodeKind.AnyKeyword);
                    elementType.Path = "type";

                    Node propSignature = NodeHelper.CreateNode(NodeKind.PropertySignature);
                    propSignature.AddChild(prop.Name.TsNode);
                    propSignature.AddChild(elementType);

                    typeLiteral.Members.Add(propSignature);
                }
                return typeLiteral;
            }
        }

        private static Node GetBinaryExpressionType(BinaryExpression binary)
        {
            switch (binary.OperatorToken.Kind)
            {
                case NodeKind.MinusToken:
                case NodeKind.AsteriskToken:
                case NodeKind.SlashToken:
                    return NodeHelper.CreateNode(NodeKind.NumberKeyword);

                case NodeKind.PlusToken:
                    Node leftType = GetNodeType(binary.Left);
                    Node rightType = GetNodeType(binary.Right);
                    if (IsNumberType(leftType) && IsNumberType(rightType))
                    {
                        return NodeHelper.CreateNode(NodeKind.NumberKeyword);
                    }
                    return NodeHelper.CreateNode(NodeKind.StringKeyword);
                default:
                    return null;
            }
        }

        private static Node GetItemType(ArrayLiteralExpression value)
        {
            Node elementType = null;

            List<Node> elements = value.Elements;
            if (elements.Find(n => n.Kind == NodeKind.StringLiteral) != null)
            {
                elementType = NodeHelper.CreateNode(NodeKind.StringKeyword);
            }
            else if (elements.Find(n => n.Kind == NodeKind.TrueKeyword || n.Kind == NodeKind.FalseKeyword) != null)
            {
                elementType = NodeHelper.CreateNode(NodeKind.BooleanKeyword);
            }
            else if (elements.Find(n => n.Kind == NodeKind.NumericLiteral || (n.Kind == NodeKind.PrefixUnaryExpression && (n as PrefixUnaryExpression).Operand.Kind == NodeKind.NumericLiteral)) != null)
            {
                elementType = NodeHelper.CreateNode(NodeKind.NumberKeyword);
            }
            else if (elements.Count > 0)
            {
                foreach (Node element in elements)
                {
                    Node type = GetNodeType(element);
                    if (type != null)
                    {
                        elementType = type;
                        break;
                    }
                }
            }
            else if (elements.Count == 0 && value.Parent.Kind == NodeKind.ArrayLiteralExpression)
            {
                Node arrarrType = GetNodeType(value.Parent);
                if (arrarrType != null)
                {
                    elementType = ((arrarrType as ArrayType).ElementType as ArrayType).ElementType;
                }
            }

            elementType = elementType ?? NodeHelper.CreateNode(NodeKind.AnyKeyword);
            elementType.Path = "elementType";
            return elementType;
        }

        private static Node GetDeclarationType(Node node)
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
            PropertyDeclaration propertyDeclarationParent = node.Parent as PropertyDeclaration;
            if (propertyDeclarationParent != null)
            {
                return propertyDeclarationParent.Type;
            }
            //
            CallExpression callExpressionParent = node.Parent as CallExpression;
            if (callExpressionParent != null)
            {
                int index = callExpressionParent.Arguments.IndexOf(node);
                Node member = GetPropertyAccessMember(callExpressionParent);
                if (index >= 0 && member != null)
                {
                    List<Node> parameters = new List<Node>();
                    if (member.Kind == NodeKind.MethodDeclaration)
                    {
                        parameters = (member as MethodDeclaration).Parameters;
                    }
                    else if (member.Kind == NodeKind.MethodSignature)
                    {
                        parameters = (member as MethodSignature).Parameters;
                    }
                    else if (member.Kind == NodeKind.Constructor)
                    {
                        parameters = (member as Constructor).Parameters;
                    }

                    if (0 <= index && index < parameters.Count)
                    {
                        return (parameters[index] as Parameter).Type;
                    }
                }
            }
            //
            ReturnStatement returnParent = node.Parent as ReturnStatement;
            if (returnParent != null)
            {
                MethodDeclaration method = returnParent.Ancestor(NodeKind.MethodDeclaration) as MethodDeclaration;
                return method?.Type;
            }
            //
            BinaryExpression binaryParent = node.Parent as BinaryExpression;
            if (binaryParent != null && binaryParent.OperatorToken.Kind == NodeKind.EqualsToken && binaryParent.Right == node) //assign
            {
                return GetNodeType(binaryParent.Left);
            }
            //
            ConditionalExpression conditionalParent = node.Parent as ConditionalExpression;
            if (conditionalParent != null)
            {
                return GetDeclarationType(conditionalParent);
            }
            //
            NewExpression newParent = node.Parent as NewExpression;
            if (newParent != null)
            {
                int index = newParent.Arguments.IndexOf(node);
                string clsName = TypeHelper.GetName(newParent.Type.Text);
                Project project = newParent.Project;
                ClassDeclaration classDeclaration = project?.GetClass(clsName);
                Constructor ctor = classDeclaration?.GetConstructor();
                if (index >= 0 && ctor != null)
                {
                    return (ctor.Parameters[index] as Parameter).Type;
                }
            }
            //
            PropertyAssignment propertyAssignParent = node.Parent as PropertyAssignment; //{a: [], b: ''}
            ObjectLiteralExpression objLiteralParent = propertyAssignParent?.Parent as ObjectLiteralExpression;
            if (objLiteralParent != null)
            {
                string memberName = propertyAssignParent.Name.Text;
                Node objLiteralType = GetNodeType(objLiteralParent);
                if (objLiteralType != null && objLiteralType.Kind == NodeKind.TypeLiteral)
                {
                    PropertySignature member = (objLiteralType as TypeLiteral).Members.Find(n => (n as PropertySignature).Name.Text == memberName) as PropertySignature;
                    if (member != null)
                    {
                        return member.Type;
                    }
                }
            }
            //
            ParenthesizedExpression parentthesizedParent = node.Parent as ParenthesizedExpression;
            if (parentthesizedParent != null)
            {
                return GetDeclarationType(parentthesizedParent);
            }

            return null;
        }

        private static Node GetClassInterfaceMember(Node accessNode, string memberName)
        {
            List<Node> nodes = new List<Node>() { accessNode };

            Project project = accessNode.Document?.Project;
            if (project != null)
            {
                nodes.AddRange(project.GetInherits(accessNode));
            }

            foreach (Node node in nodes)
            {
                if (node.Kind == NodeKind.ClassDeclaration)
                {
                    Node member = (node as ClassDeclaration).GetMember(memberName);
                    if (member != null)
                    {
                        return member;
                    }
                }
                else if (node.Kind == NodeKind.InterfaceDeclaration)
                {
                    Node member = (node as InterfaceDeclaration).GetMember(memberName);
                    if (member != null)
                    {
                        return member;
                    }
                }
            }

            return null;
        }

        private static string[] GetNameParts(string fullName)
        {
            string[] accessNames = fullName.Split(".");
            for (int i = 0; i < accessNames.Length; i++)
            {
                string name = accessNames[i];
                int index = name.IndexOf("(");
                if (index >= 0)
                {
                    name = name.Substring(0, index);
                }
                accessNames[i] = name.Trim();
            }
            return accessNames;
        }
    }
}
