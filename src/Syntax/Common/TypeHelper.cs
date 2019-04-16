using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    internal class TypeHelper
    {
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

        public static Node GetNodeType(Node node)
        {
            switch (node.Kind)
            {
                case NodeKind.StringLiteral:
                    return NodeHelper.CreateNode(NodeKind.StringKeyword);

                case NodeKind.NumericLiteral:
                    return NodeHelper.CreateNode(NodeKind.NumberKeyword);

                case NodeKind.PrefixUnaryExpression:
                    PrefixUnaryExpression prefixExpr = node as PrefixUnaryExpression;
                    if (prefixExpr.Operand.Kind == NodeKind.NumericLiteral)
                    {
                        return NodeHelper.CreateNode(NodeKind.NumberKeyword);
                    }
                    return NodeHelper.CreateNode(NodeKind.AnyKeyword);

                case NodeKind.TrueKeyword:
                case NodeKind.FalseKeyword:
                    return NodeHelper.CreateNode(NodeKind.BooleanKeyword);

                case NodeKind.NewExpression:
                    return (node as NewExpression).Type;

                case NodeKind.Identifier:
                    Node identifierType = GetIndentifierType(node as Identifier);
                    return identifierType ?? NodeHelper.CreateNode(NodeKind.AnyKeyword);

                case NodeKind.PropertyAccessExpression:
                case NodeKind.CallExpression:
                    return GetPropertyAccessType(node);

                case NodeKind.ArrayLiteralExpression:
                    return GetArrayLiteralType(node as ArrayLiteralExpression);

                case NodeKind.ObjectLiteralExpression:
                    return GetObjectLiteralType(node as ObjectLiteralExpression);

                case NodeKind.ElementAccessExpression:
                    return GetElementAccessType(node as ElementAccessExpression);

                default:
                    return node.GetValue("Type") as Node;
            }
        }

        private static Node GetElementAccessType(ElementAccessExpression elementAccess)
        {
            //TODO:
            Node type = GetNodeType(elementAccess.Expression);
            if (type != null)
            {
                return type.Kind == NodeKind.ArrayType ? (type as ArrayType).ElementType : type;
            }
            return null;
        }

        private static Node GetIndentifierType(Identifier identifier)
        {
            return GetIndentifierType(identifier.Text, identifier);
        }

        private static Node GetIndentifierType(string name, Node startNode)
        {
            Block block = GetBlockParent(startNode);
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

            MethodDeclaration methodDeclaration = GetMethodDeclarationParent(startNode);
            if (methodDeclaration != null && (methodDeclaration.Parameters.Find(p => (p as Parameter).Name.Text == name) is Parameter methodParameter))
            {
                return methodParameter.Type;
            }

            Constructor ctor = GetConstructorParent(startNode);
            if (ctor != null && (ctor.Parameters.Find(p => (p as Parameter).Name.Text == name) is Parameter ctorParameter))
            {
                return ctorParameter.Type;
            }
            return null;
        }

        private static Node GetPropertyAccessType(Node accessNode)
        {
            Document document = accessNode.Document;
            Project project = document?.Project;

            ClassDeclaration classNode = null;
            string[] accessNames = GetNameParts(accessNode.Text);
            for (int i = 0; i < accessNames.Length; i++)
            {
                string className = accessNames[i];

                if (i == 0 && className == "this")
                {
                    classNode = GetClassDeclarationParent(accessNode);
                }
                else
                {
                    Node type = i == 0 ? GetIndentifierType(className, accessNode) : GetClassMemberType(classNode, className);
                    if (type == null)
                    {
                        return null;
                    }
                    if (i == accessNames.Length - 1)
                    {
                        return type;
                    }
                    string[] typeParts = GetNameParts(type.Text);
                    className = typeParts[typeParts.Length - 1];
                    classNode = project?.GetClass(className);
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
            arrayType.AddNode(valueType);
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
                    Node elementType = GetNodeType(prop.Initializer);
                    elementType = elementType ?? NodeHelper.CreateNode(NodeKind.AnyKeyword);
                    elementType.Path = "type";

                    Node propSignature = NodeHelper.CreateNode(NodeKind.PropertySignature);
                    propSignature.AddNode(prop.Name.TsNode);
                    propSignature.AddNode(elementType);

                    typeLiteral.Members.Add(propSignature);
                }
                return typeLiteral;
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
            PropertyDeclaration propertyParent = node.Parent as PropertyDeclaration;
            if (propertyParent != null)
            {
                return propertyParent.Type;
            }
            //
            ReturnStatement returnParent = node.Parent as ReturnStatement;
            if (returnParent != null)
            {
                MethodDeclaration method = GetMethodDeclarationParent(returnParent);
                return method?.Type;
            }
            //
            BinaryExpression binaryParent = node.Parent as BinaryExpression;
            if (binaryParent != null && binaryParent.OperatorToken.Kind == NodeKind.EqualsToken) //assign
            {
                switch (binaryParent.Left.Kind)
                {
                    case NodeKind.Identifier:
                        return GetIndentifierType(binaryParent.Left as Identifier);

                    case NodeKind.PropertyAccessExpression:
                    case NodeKind.CallExpression:
                        return GetPropertyAccessType(binaryParent.Left);

                    default:
                        return null;
                }
            }
            return null;
        }

        private static ClassDeclaration GetClassDeclarationParent(Node node)
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

        private static MethodDeclaration GetMethodDeclarationParent(Node node)
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

        private static Constructor GetConstructorParent(Node node)
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

        private static Block GetBlockParent(Node node)
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

        private static Node GetClassMemberType(ClassDeclaration classNode, string name)
        {
            foreach (var memeber in classNode.Members)
            {
                switch (memeber.Kind)
                {
                    case NodeKind.PropertyDeclaration:
                        PropertyDeclaration propertyDeclaration = memeber as PropertyDeclaration;
                        if (propertyDeclaration.Name.Text == name)
                        {
                            return propertyDeclaration.Type;
                        }
                        break;

                    case NodeKind.GetAccessor:
                        GetAccessor getAccess = memeber as GetAccessor;
                        if (getAccess.Name.Text == name)
                        {
                            return getAccess.Type;
                        }
                        break;

                    case NodeKind.GetSetAccessor:
                        GetSetAccessor getSetAccess = memeber as GetSetAccessor;
                        if (getSetAccess.Name.Text == name)
                        {
                            return getSetAccess.Type;
                        }
                        break;

                    case NodeKind.MethodDeclaration:
                        MethodDeclaration method = memeber as MethodDeclaration;
                        if (method.Name.Text == name)
                        {
                            return method.Type;
                        }
                        break;

                    default:
                        break;
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
