using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace TypeScript.Syntax
{
    public static class TypeHelper
    {
        // ITypeDeclaration: native type should contains: Name, MemberDeclarations, TypeParameters, GetMember(name), BaseTypeDeclarations
        // IMemberDeclaration: Name, TypeParameters, ParameterTypes, ReturnType.
        private static readonly List<string> MATH_MEMBER = new List<string>()
        {
            "E", "LN10", "LN2", "LOG2E", "LOG10E", "PI", "SQRT1_2", "SQRT2",
            "abs", "acos", "asin", "atan", "atan2", "ceil", "cos", "exp", "floor",
            "log", "max", "min", "pow", "random", "round", "sin", "sqrt", "tan"
        };

        public static bool IsMatchTypeName(string name)
        {
            return Regex.IsMatch(name, "^_*[A-Z]+[_A-Za-z0-9]*$");
        }

        public static bool IsSameType(Node type1, Node type2)
        {
            if (type1 == null && type2 == null)
            {
                return true;
            }
            if (type1 == null || type2 == null)
            {
                return false;
            }

            type1 = TrimType(type1);
            type2 = TrimType(type2);
            string name1 = GetNormalizedTypeName(type1);
            string name2 = GetNormalizedTypeName(type2);
            if (name1 == name2)
            {
                return true;
            }
            if (Regex.IsMatch(name1, "^T\\d*$|TKey|<.*>$") || Regex.IsMatch(name2, "^T\\d*$|TKey|<.*>$")) //TODO: compare generic type
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Trim null type of UnionType and ParenthesizedType.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Node TrimType(Node type)
        {
            if (type != null)
            {
                if (type.Kind == NodeKind.ParenthesizedType)
                {
                    type = ((ParenthesizedType)type).Type;
                }

                if (type.Kind == NodeKind.UnionType)
                {
                    List<Node> types = (type as UnionType).Types;
                    if (types.Count == 2)
                    {
                        if (types[0].Kind == NodeKind.NullKeyword)
                        {
                            type = types[1];
                        }
                        else if (types[1].Kind == NodeKind.NullKeyword)
                        {
                            type = types[0];
                        }
                    }
                }
            }
            return type;
        }

        public static bool IsNullableType(Node type)
        {
            if (type != null && type.Kind == NodeKind.UnionType)
            {
                List<Node> types = ((UnionType)type).Types;
                return (types.Count == 2 && (types[0].Kind == NodeKind.NullKeyword || types[1].Kind == NodeKind.NullKeyword));
            }
            return false;
        }

        public static string GetTypeName(Node type)
        {
            if (type == null)
            {
                return null;
            }

            type = TrimType(type);
            if (type.IsTypeDeclaration())
            {
                return type.GetName();
            }
            if (IsStringType(type))
            {
                return "string";
            }
            if (IsNumberType(type))
            {
                return "number";
            }
            if (IsBoolType(type))
            {
                return "boolean";
            }

            string typeName = type.Text;
            return ToShortName(typeName);
        }

        public static string GetTypeDeclarationName(Node type)
        {
            if (type == null)
            {
                return null;
            }

            type = TrimType(type);
            string name = ToShortName(type.Text); // name or alias name
            Node declaration = type.Document?.GetTypeDeclaration(name);
            if (declaration != null)
            {
                name = declaration.GetName();
            }
            return name;
        }

        public static string ToShortName(string typeName)
        {
            string name = typeName;
            if (!string.IsNullOrEmpty(name))
            {
                int index = typeName.IndexOf("<");
                if (index >= 0)
                {
                    name = typeName.Substring(0, index);
                }
                index = name.LastIndexOf('.');
                if (index >= 0)
                {
                    return name.Substring(index + 1);
                }
            }
            return name;
        }

        public static bool IsNativeNumber(Node type)
        {
            if (type == null)
            {
                return false;
            }

            type = TrimType(type);
            if (type.Kind == NodeKind.IntKeyword)
            {
                return true;
            }
            if (type.Kind == NodeKind.LongKeyword)
            {
                return true;
            }
            return IsNativeNumber(type.Text);
        }

        public static bool IsNativeNumber(string type)
        {
            if (type == NativeTypes.Double || type == NativeTypes.Int || type == NativeTypes.Long)
            {
                return true;
            }
            return false;
        }

        public static bool IsNativeBool(Node type)
        {
            if (type == null)
            {
                return false;
            }

            type = TrimType(type);
            return IsNativeBool(type.Text);
        }

        public static bool IsNativeBool(string type)
        {
            return type == NativeTypes.Bool;
        }

        public static bool IsBoolType(Node type)
        {
            if (type == null)
            {
                return false;
            }

            type = TrimType(type);
            if (type.Kind == NodeKind.BooleanKeyword)
            {
                return true;
            }

            return IsNativeBool(type);
        }

        public static bool IsIntType(Node type)
        {
            if (type == null)
            {
                return false;
            }

            type = TrimType(type);
            if (type.Kind == NodeKind.IntKeyword)
            {
                return true;
            }
            return type.Text == NativeTypes.Int;
        }

        public static bool IsNumberType(Node type)
        {
            if (type == null)
            {
                return false;
            }

            type = TrimType(type);
            if (type.Kind == NodeKind.NumberKeyword)
            {
                return true;
            }

            return IsNativeNumber(type);
        }

        public static bool IsStringType(Node type)
        {
            if (type == null)
            {
                return false;
            }

            type = TrimType(type);
            if (type.Kind == NodeKind.StringKeyword)
            {
                return true;
            }

            return IsStringType(type.Text);
        }

        public static bool IsStringType(string typeName)
        {
            if (typeName == "string" || typeName == NativeTypes.String)
            {
                return true;
            }
            return false;
        }

        public static bool IsDictionaryType(Node type)
        {
            if (type == null)
            {
                return false;
            }

            type = TrimType(type);
            if (type.Kind == NodeKind.IndexSignature)
            {
                return true;
            }
            if (type.Kind == NodeKind.TypeLiteral && ((TypeLiteral)type).IsIndexSignature)
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

            type = TrimType(type);
            if (type.Kind == NodeKind.ArrayType)
            {
                return true;
            }

            string typeText = "";
            if (type.Kind == NodeKind.TypeReference)
            {
                typeText = (type as TypeReference).TypeName.Text;
            }
            return IsArrayType(typeText);
        }

        public static bool IsArrayType(string typeText)
        {
            return (typeText == "Array" || typeText == "RegExpExecArray" || typeText == "RegExpMatchArray");
        }

        public static bool IsGenericType(Node type)
        {
            if (type == null)
            {
                return false;
            }

            type = TrimType(type);
            List<Node> tArgs = type.GetValue("TypeArguments") as List<Node>;
            return (tArgs != null && tArgs.Count > 0);
        }

        public static bool IsObjectType(Node type)
        {
            if (type == null)
            {
                return false;
            }

            type = TrimType(type);
            if (type.Kind == NodeKind.ObjectKeyword)
            {
                return true;
            }
            return type.Text == NativeTypes.AnyObject;
        }

        public static bool IsEnumType(Node type)
        {
            if (type == null)
            {
                return false;
            }

            type = TrimType(type);
            if (type.Kind == NodeKind.EnumDeclaration)
            {
                return true;
            }

            string typeName = GetTypeName(type);
            if (!string.IsNullOrEmpty(typeName))
            {
                var project = type.Document?.Project;
                return project?.GetEnum(typeName) != null;
            }
            return false;
        }

        public static Node GetArrayElementType(Node arrayType)
        {
            if (arrayType == null)
            {
                return null;
            }

            arrayType = TrimType(arrayType);
            if (arrayType.Kind == NodeKind.ArrayType)
            {
                return ((ArrayType)arrayType).ElementType;
            }
            if (arrayType.Kind == NodeKind.TypeReference)
            {
                string typeText = ((TypeReference)arrayType).TypeName.Text;
                if (typeText == "RegExpExecArray" || typeText == "RegExpMatchArray")
                {
                    return NodeHelper.CreateNode(NodeKind.StringKeyword);
                }
                else
                {
                    return ((TypeReference)arrayType).TypeArguments[0];
                }
            }

            return null;
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
                    if (node.Text.Contains('.') || node.Text.Contains('e'))
                    {
                        return NodeHelper.CreateNode(NodeKind.NumberKeyword);
                    }
                    else
                    {
                        return NodeHelper.CreateNode(NodeKind.IntKeyword);
                    }

                case NodeKind.PrefixUnaryExpression:
                    return NodeHelper.CreateNode(NodeKind.NumberKeyword);

                case NodeKind.BinaryExpression:
                    return GetBinaryExpressionType(node as BinaryExpression);

                case NodeKind.TrueKeyword:
                case NodeKind.FalseKeyword:
                    return NodeHelper.CreateNode(NodeKind.BooleanKeyword);

                case NodeKind.NewExpression:
                    return ((NewExpression)node).Type;

                case NodeKind.Identifier:
                    return GetIdentifierType((Identifier)node);

                case NodeKind.PropertyAccessExpression:
                    return GetPropertyAccessType((PropertyAccessExpression)node);

                case NodeKind.CallExpression:
                    return GetCallExpressionType((CallExpression)node);

                case NodeKind.ArrayLiteralExpression:
                    return GetArrayLiteralType((ArrayLiteralExpression)node);

                case NodeKind.ObjectLiteralExpression:
                    return GetObjectLiteralType((ObjectLiteralExpression)node);

                case NodeKind.ElementAccessExpression:
                    return GetElementAccessType((ElementAccessExpression)node);

                case NodeKind.ParenthesizedExpression:
                    return GetNodeType(((ParenthesizedExpression)node).Expression);

                case NodeKind.AsExpression:
                    return ((AsExpression)node).Type;

                case NodeKind.VariableDeclaration:
                    return GetVariableDeclarationType((VariableDeclaration)node);

                case NodeKind.TypeOfExpression:
                    return NodeHelper.CreateNode(NodeKind.StringKeyword);

                case NodeKind.ConditionalExpression:
                    return GetConditionalExpressionType((ConditionalExpression)node);

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

            switch (type.Kind)
            {
                case NodeKind.ArrayType:
                    return GetArrayElementType(type);

                case NodeKind.TypeLiteral:
                    TypeLiteral typeLiteral = (TypeLiteral)type;
                    if (typeLiteral.IsIndexSignature)
                    {
                        return ((IndexSignature)typeLiteral.Members[0]).Type;
                    }
                    return null;

                default:
                    return null;
            }
        }

        private static Node GetIdentifierType(Identifier identifier)
        {
            return GetIdentifierTypeByName(identifier.Text, identifier);
        }

        private static Node GetIdentifierTypeByName(string name, Node startNode)
        {
            var project = startNode.Document.Project;
            if (name == "this")
            {
                return startNode.Ancestor<ClassDeclaration>();
            }
            if (name == "super")
            {
                ClassDeclaration thisClassNode = startNode.Ancestor<ClassDeclaration>();
                return (thisClassNode == null ? null : project.GetBaseClass(thisClassNode));
            }
            if (name == "NaN" || name == "Infinity")
            {
                return NodeHelper.CreateNode(NodeKind.NumberKeyword);
            }
            // xxx as View
            if (name.Contains(" as "))
            {
                string[] parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return project.GetTypeDeclaration(parts[parts.Length - 1]);
            }

            Node parent = startNode.Parent;
            while (parent != null)
            {
                switch (parent.Kind)
                {
                    case NodeKind.Block:
                        Block block = parent as Block;
                        Node bvType = GetVariableStatementType(block.Statements, name);
                        if (bvType != null)
                        {
                            return bvType;
                        }

                        break;

                    case NodeKind.CaseClause:
                        CaseClause caseClause = parent as CaseClause;
                        Node ccType = GetVariableStatementType(caseClause.Statements, name);
                        if (ccType != null)
                        {
                            return ccType;
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
                            return GetForOfStatementType(forOfStatement);
                        }
                        break;

                    case NodeKind.ForStatement:
                        ForStatement forStatement = parent as ForStatement;
                        if (CanGetForStatementType(forStatement, name, out Node forType))
                        {
                            return forType;
                        }
                        break;

                    case NodeKind.MethodDeclaration:
                    case NodeKind.Constructor:
                    case NodeKind.ArrowFunction:
                    case NodeKind.FunctionDeclaration:
                    case NodeKind.FunctionExpression:
                    case NodeKind.SetAccessor:
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
                        Node typeDeclaration = project.GetTypeDeclaration(name);
                        if (typeDeclaration != null)
                        {
                            return typeDeclaration;
                        }
                        return project.GetFunctionDeclaration(name);

                    default:
                        break;
                }
                parent = parent.Parent;
            }

            return null;
        }

        private static Node GetVariableStatementType(List<Node> statements, string name)
        {
            foreach (var statement in statements)
            {
                if (statement.Kind != NodeKind.VariableStatement)
                {
                    continue;
                }

                VariableDeclarationList declarationList = (statement as VariableStatement).DeclarationList as VariableDeclarationList;
                VariableDeclaration declarationNode = declarationList.Declarations[0] as VariableDeclaration;
                if (declarationNode.Name.Text == name)
                {
                    return GetVariableDeclarationType(declarationNode);
                }
            }
            return null;
        }

        public static Node GetVariableDeclarationType(VariableDeclaration node)
        {
            if (node.Type != null)
            {
                return node.Type;
            }

            Node type = null;
            if (node.Initializer != null)
            {
                type = GetNodeType(node.Initializer);
                if (type == null && node.Initializer.Kind == NodeKind.ConditionalExpression)
                {
                    ConditionalExpression cond = (ConditionalExpression)node.Initializer;
                    type = GetConditionalExpressionType(cond);
                }
            }
            else if (node.Parent.Kind == NodeKind.CatchClause)
            {
                type = NodeHelper.CreateNode(NodeKind.Identifier, "Exception");
            }
            else if (node.Parent.Parent.Kind == NodeKind.ForOfStatement)
            {
                type = GetForOfStatementType((ForOfStatement)node.Parent.Parent);
            }
            return type ?? NodeHelper.CreateNode(NodeKind.AnyKeyword);
        }

        public static Node GetConditionalExpressionType(ConditionalExpression cond)
        {
            Node trueType = GetNodeType(cond.WhenTrue);
            Node falseType = GetNodeType(cond.WhenFalse);
            return GetTypeOfBinaryType(trueType, falseType);
        }

        private static bool CanGetForStatementType(ForStatement forStatement, string varName, out Node type)
        {
            type = null;
            foreach (var init in forStatement.Initializers)
            {
                VariableDeclaration varDeclaraion = null;
                if (init is VariableDeclarationList varList)
                {
                    varDeclaraion = (VariableDeclaration)varList.Declarations[0];
                }
                else if (init is VariableDeclaration var)
                {
                    varDeclaraion = var;
                }

                if (varDeclaraion != null && varDeclaraion.Name.Text == varName)
                {
                    type = GetNodeType(varDeclaraion);
                    return true;
                }
            }
            return false;
        }

        private static Node GetForOfStatementType(ForOfStatement forOfStatement)
        {
            Node forOfType = GetNodeType(forOfStatement.Expression);
            forOfType = TrimType(forOfType);
            if (IsArrayType(forOfType))
            {
                return GetArrayElementType(forOfType);
            }
            return null;
        }

        private static ClassDeclaration GetThisType(ThisKeyword @this)
        {
            return @this.Ancestor<ClassDeclaration>();
        }

        private static ClassDeclaration GetSuperType(SuperKeyword superNode)
        {
            ClassDeclaration thisClassNode = superNode.Ancestor<ClassDeclaration>();
            return thisClassNode == null ? null : thisClassNode.Document.Project.GetBaseClass(thisClassNode);
        }

        public static List<Node> GetParameters(NewExpression newExpr)
        {
            List<Node> parameters = new List<Node>();
            string name = ToShortName(newExpr.Expression.Text);
            ClassDeclaration classDeclaration = newExpr.Document.Project.GetClass(name);
            if (classDeclaration != null)
            {
                parameters = classDeclaration.GetConstructor().Parameters;
            }
            return parameters;
        }

        public static List<Node> GetParameters(CallExpression callExpr) // TODO: generic
        {
            List<Node> parameters = new List<Node>();

            Node member = GetCallExpressionMember(callExpr);
            if (member != null && member.IsTypeDeclaration())
            {
                if (member.Kind == NodeKind.ClassDeclaration)
                {
                    member = ((ClassDeclaration)member).GetConstructor();
                }
                else if (member.Kind == NodeKind.TypeAliasDeclaration)
                {
                    member = ((TypeAliasDeclaration)member).Type;
                }
            }
            if (member != null)
            {
                if (member.Kind == NodeKind.MethodDeclaration)
                {
                    parameters = ((MethodDeclaration)member).Parameters;
                }
                else if (member.Kind == NodeKind.MethodSignature)
                {
                    parameters = ((MethodSignature)member).Parameters;
                }
                else if (member.Kind == NodeKind.Constructor)
                {
                    parameters = ((Constructor)member).Parameters;
                }
                else if (member.Kind == NodeKind.FunctionType)
                {
                    parameters = ((FunctionType)member).Parameters;
                }
                else if (member.Kind == NodeKind.FunctionDeclaration)
                {
                    parameters = ((FunctionDeclaration)member).Parameters;
                }
            }
            return parameters;
        }

        private static Node GetCallExpressionType(CallExpression callExpr)
        {
            Node tailMember = GetCallExpressionMember(callExpr);
            if (tailMember != null)
            {
                if (tailMember.Kind == NodeKind.Identifier)
                {
                    string text = tailMember.Text;
                    switch (text)
                    {
                        case "isNaN":
                        case "isFinite":
                            return NodeHelper.CreateNode(NodeKind.BooleanKeyword);

                        case "parseInt":
                        case "parseFloat":
                            return NodeHelper.CreateNode(NodeKind.NumberKeyword);

                        default:
                            return null;
                    }
                }
                else if (tailMember.IsTypeDeclaration())
                {
                    return tailMember.GetValue("Name") as Node;
                }
                else if (IsArrayType(tailMember) || IsStringType(tailMember))
                {
                    return tailMember;
                }
                else
                {
                    return tailMember.GetValue("Type") as Node;
                }
            }

            // Hard code here:
            if (callExpr.Expression.Kind == NodeKind.PropertyAccessExpression)
            {
                List<string> parts = callExpr.Parts;
                if (parts.Count == 2 && parts[0] == "Math" && MATH_MEMBER.Contains(parts[1]))
                {
                    return NodeHelper.CreateNode(NodeKind.NumberKeyword);
                }

                switch (parts[parts.Count - 1])
                {
                    case "indexOf":
                    case "lastIndexOf":
                        return NodeHelper.CreateNode(NodeKind.IntKeyword);

                    case "substr":
                    case "substring":
                        return NodeHelper.CreateNode(NodeKind.StringKeyword);

                    default:
                        return null;
                }

            }

            return null;
        }

        private static Node GetPropertyAccessType(PropertyAccessExpression accessNode)
        {
            Node propAccessType = null;

            Node tailMember = GetPropertyAccessMember(accessNode);
            if (tailMember != null)
            {
                if (IsArrayType(tailMember) || IsStringType(tailMember)) //todo native type
                {
                    propAccessType = tailMember;
                }
                else if (tailMember.GetValue("Type") is Node accessType)
                {
                    propAccessType = accessType;
                }
                else if (tailMember.IsTypeDeclaration())
                {
                    return tailMember.GetValue("Name") as Node;
                }
            }

            // native object's property.
            if (propAccessType == null)
            {
                List<string> parts = accessNode.Parts;
                if (parts[parts.Count - 1] == "length")
                {
                    Node exprType = GetNodeType(accessNode.Expression);
                    if (IsArrayType(exprType) || IsStringType(exprType))
                    {
                        propAccessType = NodeHelper.CreateNode(NodeKind.IntKeyword);
                    }
                }
                else if (parts.Count == 2 && parts[0] == "Math" && MATH_MEMBER.Contains(parts[1]))
                {
                    propAccessType = NodeHelper.CreateNode(NodeKind.NumberKeyword);
                }
            }

            return propAccessType;
        }

        private static Node GetCallExpressionMember(CallExpression callExpr)
        {
            Node expression = callExpr.Expression;
            switch (expression.Kind)
            {
                case NodeKind.SuperKeyword:
                    return GetSuperType((SuperKeyword)expression);

                case NodeKind.ThisKeyword:
                    return GetThisType((ThisKeyword)expression);

                case NodeKind.Identifier:
                    Node type = GetIdentifierType((Identifier)expression);
                    if (type != null)
                    {
                        if (type.IsTypeDeclaration() || type.Kind == NodeKind.FunctionDeclaration)
                        {
                            return type;
                        }
                        else
                        {
                            return type.Document.Project.GetTypeDeclaration(type.Text);
                        }
                    }
                    return expression;

                case NodeKind.PropertyAccessExpression:
                    return GetPropertyAccessMember((PropertyAccessExpression)expression);

                default:
                    return null;
            }
        }

        public static Node GetPropertyAccessMember(PropertyAccessExpression accessNode)
        {
            Document document = accessNode.Document;
            Project project = document?.Project;

            List<string> accessNames = accessNode.Parts;
            if (project.Converter != null)
            {
                string fullName = string.Join('.', accessNames);
                fullName = project.Converter.Context.TrimTypeName(fullName);
                accessNames = fullName.Split('.').ToList();
            }

            Node classNode = null;
            for (int i = 0; i < accessNames.Count; i++)
            {
                string memberName = accessNames[i];

                int elementAccessIndex = memberName.IndexOf('[');
                if (elementAccessIndex >= 0)
                {
                    memberName = memberName.Substring(0, elementAccessIndex);
                }
                if (memberName.Length > 0 && memberName[0] == '(' && memberName[memberName.Length - 1] == ')')
                {
                    memberName = memberName.Substring(1, memberName.Length - 2);
                }

                Node type;
                Node member;
                if (i == 0)
                {
                    type = GetIdentifierTypeByName(memberName, accessNode);
                    member = type;
                }
                else
                {
                    member = GetClassInterfaceMember(classNode, memberName);
                    type = GetClassInterfaceMemberType(member);
                }

                //  Get array type...
                if (IsArrayType(type))
                {
                    for (int j = i + 1; j < accessNames.Count; j++)
                    {
                        if (accessNames[j] == "concat"
                         || accessNames[j] == "reverse"
                         || accessNames[j] == "slice"
                         || accessNames[j] == "filter"
                         || accessNames[j] == "sort"
                         || accessNames[j] == "splice")
                        {
                            i = j;
                            continue;
                        }
                        break;
                    }
                }
                //~

                if (i == accessNames.Count - 1)
                {
                    return member;
                }

                // normalize type
                if (type == null)
                {
                    return null;
                }
                if (IsArrayType(type) && elementAccessIndex >= 0)
                {
                    type = GetArrayElementType(type);
                }
                type = TrimType(type);

                // class node
                if (type.IsTypeDeclaration())
                {
                    classNode = type;
                }
                else
                {
                    string[] typeParts = GetNameParts(type.Text);
                    classNode = project.GetTypeDeclaration(typeParts[typeParts.Length - 1]);
                }
                if (classNode == null || classNode.Kind == NodeKind.EnumDeclaration)
                {
                    return classNode;
                }
            }

            return null;
        }
        private static Node GetClassInterfaceMemberType(Node member)
        {
            if (member != null)
            {
                if (member.GetValue("Type") is Node type)
                {
                    return type;
                }

                if (member.Kind == NodeKind.PropertyDeclaration)
                {
                    PropertyDeclaration propDeclaration = ((PropertyDeclaration)member);
                    if (propDeclaration.Initializer != null)
                    {
                        return GetNodeType(propDeclaration.Initializer);
                    }
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

            if (arrayLiteral.Elements.Find(n => n.Kind == NodeKind.SpreadElement) is SpreadElement spreadElement)
            {
                type = GetNodeType(spreadElement.Expression);
                if (type != null)
                {
                    return type;
                }
            }

            ArrayType arrayType = (ArrayType)NodeHelper.CreateNode(NodeKind.ArrayType);
            Node elementType = GetItemType(arrayLiteral);
            if (elementType != null)
            {
                bool shouldClone = elementType.Parent != null;
                if (shouldClone)
                {
                    elementType = elementType.TsNode != null ? NodeHelper.CreateNode(elementType.TsNode) : NodeHelper.CreateNode(elementType.Kind);
                }
            }
            else
            {
                elementType = NodeHelper.CreateNode(NodeKind.AnyKeyword);
            }
            arrayType.SetElementType(elementType, elementType.Parent == null);
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
                    elementType.NodeName = "type";

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
                case NodeKind.LessThanToken:
                case NodeKind.LessThanEqualsToken:
                case NodeKind.GreaterThanToken:
                case NodeKind.GreaterThanEqualsToken:
                case NodeKind.EqualsEqualsToken:
                case NodeKind.EqualsEqualsEqualsToken:
                case NodeKind.ExclamationEqualsToken:
                case NodeKind.ExclamationEqualsEqualsToken:
                    return NodeHelper.CreateNode(NodeKind.BooleanKeyword);

                case NodeKind.MinusToken:
                case NodeKind.AsteriskToken:
                case NodeKind.SlashToken:
                case NodeKind.PercentToken:
                case NodeKind.PlusToken:
                case NodeKind.PlusEqualsToken: // +=
                case NodeKind.MinusEqualsToken: // -=
                case NodeKind.AsteriskEqualsToken: // *=
                case NodeKind.SlashEqualsToken: // /=
                case NodeKind.PercentEqualsToken: // %=
                    Node leftType = GetNodeType(binary.Left);
                    Node rightType = GetNodeType(binary.Right);
                    return GetTypeOfBinaryType(leftType, rightType);

                default:
                    return null;
            }
        }
        private static Node GetTypeOfBinaryType(Node leftType, Node rightType)
        {
            // string
            if (IsStringType(leftType) || IsStringType(rightType))
            {
                return NodeHelper.CreateNode(NodeKind.StringKeyword);
            }

            // integer
            if (IsIntType(leftType) && IsIntType(rightType))
            {
                return leftType;
            }
            // number
            else if (IsIntType(leftType) && IsNumberType(rightType))
            {
                return rightType;
            }
            else if (IsNumberType(leftType) && IsIntType(rightType))
            {
                return leftType;
            }
            else if (IsNumberType(leftType))
            {
                return leftType;
            }
            else if (IsNumberType(rightType))
            {
                return rightType;
            }
            else
            {
                return leftType != null ? leftType : rightType;
            }
        }

        private static Node GetItemType(ArrayLiteralExpression value)
        {
            Node elementType = null;

            List<Node> elements = value.Elements;
            if (value.Parent.Kind == NodeKind.ArrayLiteralExpression)
            {
                Node arrarrType = GetDeclarationType((ArrayLiteralExpression)value.Parent);
                if (arrarrType != null)
                {
                    elementType = ((arrarrType as ArrayType).ElementType as ArrayType).ElementType;
                }
            }
            else if (elements.Find(n => n.Kind == NodeKind.StringLiteral) != null)
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
            elementType.NodeName = "elementType";

            return elementType;
        }

        public static Node GetReturnType(ReturnStatement returnStatement)
        {
            ArrowFunction arrowFn = returnStatement.Ancestor<ArrowFunction>();
            if (arrowFn != null && arrowFn.Type != null)
            {
                return arrowFn.Type;
            }

            MethodDeclaration method = returnStatement.Ancestor<MethodDeclaration>();
            if (method != null && method.Type != null)
            {
                return method.Type;
            }
            return null;
        }

        public static Node GetDeclarationType(Node node)
        {
            Node parent = node?.Parent;
            if (parent == null)
            {
                return null;
            }

            switch (parent.Kind)
            {
                case NodeKind.VariableDeclaration:
                    return ((VariableDeclaration)parent).Type;

                case NodeKind.Parameter:
                    return ((Parameter)parent).Type;

                case NodeKind.PropertyDeclaration:
                    return ((PropertyDeclaration)parent).Type;

                case NodeKind.CallExpression:
                    CallExpression callExpressionParent = (CallExpression)parent;
                    List<Node> parameters = GetParameters(callExpressionParent);
                    int index = callExpressionParent.Arguments.IndexOf(node);
                    if (0 <= index && index < parameters.Count)
                    {
                        return (parameters[index] as Parameter).Type;
                    }
                    return null;

                case NodeKind.ReturnStatement:
                    return GetReturnType((ReturnStatement)parent);

                case NodeKind.BinaryExpression:
                    BinaryExpression binaryParent = (BinaryExpression)parent;
                    if (binaryParent.OperatorToken.Kind == NodeKind.EqualsToken && binaryParent.Right == node) //assign
                    {
                        return GetNodeType(binaryParent.Left);
                    }
                    return null;

                case NodeKind.ConditionalExpression:
                    return GetDeclarationType(parent);

                case NodeKind.NewExpression:
                    NewExpression newParent = (NewExpression)parent;
                    int idx = newParent.Arguments.IndexOf(node);
                    string clsName = TypeHelper.ToShortName(newParent.Type.Text);
                    Project project = newParent.Document.Project;
                    ClassDeclaration classDeclaration = project?.GetClass(clsName);
                    Constructor ctor = classDeclaration?.GetConstructor();
                    if (idx >= 0 && ctor != null)
                    {
                        return (ctor.Parameters[idx] as Parameter).Type;
                    }
                    return null;

                case NodeKind.PropertyAssignment:
                    PropertyAssignment propertyAssignParent = (PropertyAssignment)parent; //{a: [], b: ''}
                    ObjectLiteralExpression objLiteralParent = propertyAssignParent?.Parent as ObjectLiteralExpression;
                    if (objLiteralParent != null)
                    {
                        string memberName = propertyAssignParent.Name.Text;
                        Node objLiteralType = GetNodeType(objLiteralParent);
                        if (objLiteralType != null && objLiteralType.Kind == NodeKind.TypeLiteral)
                        {
                            PropertySignature propSignatureMember = (objLiteralType as TypeLiteral).Members.Find(n => (n as PropertySignature).Name.Text == memberName) as PropertySignature;
                            if (propSignatureMember != null)
                            {
                                return propSignatureMember.Type;
                            }
                        }
                    }
                    return null;

                case NodeKind.ParenthesizedExpression:
                    return GetDeclarationType(parent);

                default:
                    return null;
            }
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
                        if (member.Kind == NodeKind.GetSetAccessor)
                        {
                            member = ((GetSetAccessor)member).GetAccessor;
                        }
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
                accessNames[i] = name;
            }
            return accessNames;
        }

        private static string GetNormalizedTypeName(Node type)
        {
            string name = "";
            if (type.Kind == NodeKind.TypeLiteral)
            {
                List<string> itemTypes = new List<string>();
                foreach (Node item in (type as TypeLiteral).Members)
                {
                    if (item.Kind == NodeKind.PropertySignature)
                    {
                        PropertySignature psItem = item as PropertySignature;
                        string questionToken = psItem.QuestionToken != null ? "?" : "";
                        itemTypes.Add($"{psItem.Name.Text}{questionToken}:{psItem.Type.Text}");
                    }
                    else
                    {
                        itemTypes.Add(item.Text);
                    }
                }
                name = string.Join(',', itemTypes);
            }
            else
            {
                name = type.Text;
            }

            string ret = "";
            string[] parts = name.TrimEnd(';').Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                int index = part.LastIndexOf('.');
                if (index >= 0)
                {
                    ret += part.Substring(index + 1);
                }
                else
                {
                    ret += part;
                }
            }
            return ret.Replace(" ", "");
        }
    }
}
