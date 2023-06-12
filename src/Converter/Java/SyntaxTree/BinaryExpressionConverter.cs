using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TypeScript.Syntax;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;
using com.sun.tools.javac.code;

namespace TypeScript.Converter.Java
{
    public class BinaryExpressionConverter : NodeConverter
    {
        private static readonly Name COLLECTION_SET_INDEXER_NAME = Names.fromString("set");
        private static readonly Name HASHMAP_SET_INDEXER_NAME = Names.fromString("put");

        public JCTree Convert(BinaryExpression node)
        {
            // Type Operator
            if (IsCustomTypeOperator(node, customeType: out TypeOperator customType))
            {
                return ConvertCustomTypeBinaryOperator(node, customType);
            }

            // Enum Operator
            if (IsEnumOperator(node))
            {
                return ConvertEnumBinaryOperator(node);
            }

            // Binary Assign Operator
            if (IsBinaryAssignOperator(node))
            {
                return ConvertBinaryAssignOperator(node);
            }

            // Binary Operator
            if (IsBinaryOperator(node))
            {
                return ConvertBinaryOperator(node);
            }

            // =
            if (IsEqualsAssignOperator(node))
            {
                return ConvertEqualsAssignOperator(node);
            }

            // instanceof is
            if (IsTypeTestOperator(node))
            {
                return ConvertTypeTestOperator(node);
            }

            // throw new NotSupportedException(node.OperatorToken.Text);            
            //TODO: 
            return TreeMaker.Literal(TypeTag.UNINITIALIZED_OBJECT, "XXX" + node.Text + "XXX");
        }

        private bool IsCustomTypeOperator(BinaryExpression node, out TypeOperator customeType)
        {
            customeType = null;
            //Skip Null
            if (node.Left.Kind == NodeKind.NullKeyword || node.Right.Kind == NodeKind.NullKeyword)
            {
                return false;
            }

            string operatorText = node.OperatorToken.Text;
            Node leftType = TypeHelper.GetNodeType(node.Left);
            Node rightType = TypeHelper.GetNodeType(node.Right);
            string leftTypeName = TypeHelper.GetTypeName(leftType) ?? "";
            string rightTypeName = TypeHelper.GetTypeName(rightType) ?? "";
            foreach (var typeOperator in OperatorConfig.BinaryOperators)
            {
                // TODO: refactor for type system is finished
                if (typeOperator.ConvertingOperators.Contains(operatorText))
                {
                    bool isCustomNullableType = typeOperator.Name.EndsWith("?");
                    string leftName = isCustomNullableType ? leftTypeName + "?" : leftTypeName;
                    string rightName = isCustomNullableType ? rightTypeName + "?" : rightTypeName;
                    if (leftName == typeOperator.Name || rightName == typeOperator.Name)
                    {
                        bool isSameType = leftTypeName == rightTypeName;
                        //Skip same type assign
                        bool isAssign = (operatorText == "=" && isSameType);
                        if (isAssign)
                        {
                            return false;
                        }
                        else if ((typeOperator.Name == "number?" || typeOperator.Name == "boolean?") && (!TypeHelper.IsNullableType(leftType) || !TypeHelper.IsNullableType(rightType)))
                        {
                            return false;
                        }
                        else
                        {
                            customeType = typeOperator;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private JCTree ConvertCustomTypeBinaryOperator(BinaryExpression node, TypeOperator customType)
        {
            string operatorText = node.OperatorToken.Text;
            string[] method = customType.ConvertingMethod.Split('.');
            JCExpression fn;
            if (method.Length == 1)
            {
                fn = TreeMaker.Ident(Names.fromString(method[0]));
            }
            else
            {
                fn = TreeMaker.Select(TreeMaker.Ident(Names.fromString(method[0])), Names.fromString(method[1]));
            }

            //
            if (operatorText == "=")
            {
                Node type = TypeHelper.GetNodeType(node.Left);
                JCExpression rightExpression = TreeMaker.Apply(
                   Nil<JCExpression>(),
                   fn,
                   new List<JCExpression>()
                   {
                        node.Right.ToJavaSyntaxTree<JCExpression>(),
                        TreeMaker.Select(type.ToJavaSyntaxTree<JCExpression>(), Names.fromString("class"))
                   }
                );
                return ConvertEqualsAssignOperator(node, rightExpression);
            }
            else
            {
                return TreeMaker.Apply(
                   Nil<JCExpression>(),
                   fn,
                   new List<JCExpression>()
                   {
                    node.Left.ToJavaSyntaxTree<JCExpression>(),
                    TreeMaker.Literal(TypeTag.NONE, operatorText),
                    node.Right.ToJavaSyntaxTree<JCExpression>()
                   }
               );
            }
        }

        private bool IsEnumOperator(BinaryExpression node)
        {
            Node leftType = TypeHelper.GetNodeType(node.Left);
            Node rightType = TypeHelper.GetNodeType(node.Right);
            return (TypeHelper.IsEnumType(leftType) || TypeHelper.IsEnumType(rightType));
        }
        private JCTree ConvertEnumBinaryOperator(BinaryExpression node)
        {
            NodeKind operatorKind = node.OperatorToken.Kind;
            Node left = node.Left;
            Node right = node.Right;
            Node leftType = TypeHelper.GetNodeType(left);
            Node rightType = TypeHelper.GetNodeType(right);

            string enumName = TypeHelper.GetTypeName(leftType);
            if (string.IsNullOrEmpty(enumName))
            {
                enumName = TypeHelper.GetTypeName(rightType);
            }

            switch (operatorKind)
            {
                case NodeKind.EqualsEqualsToken: // ==
                case NodeKind.EqualsEqualsEqualsToken:
                case NodeKind.ExclamationEqualsToken: // !=
                case NodeKind.ExclamationEqualsEqualsToken:
                    JCExpression leftExpression = left.ToJavaSyntaxTree<JCExpression>();
                    JCExpression rightExpression = right.ToJavaSyntaxTree<JCExpression>();
                    if (node.Document.Project.GetEnum(enumName)?.IsBitFieldSet ?? false)
                    {
                        leftExpression = TreeMaker.Apply(
                            Nil<JCExpression>(),
                            TreeMaker.Select(leftExpression, EnumNames.ENUM_VALUE_NAME),
                            Nil<JCExpression>());
                        rightExpression = TreeMaker.Apply(
                            Nil<JCExpression>(),
                            TreeMaker.Select(rightExpression, EnumNames.ENUM_VALUE_NAME),
                            Nil<JCExpression>());
                    }
                    return TreeMaker.Binary(
                        KindOperator2TagOperator(operatorKind),
                        leftExpression,
                        rightExpression
                     );

                case NodeKind.LessThanToken: // <
                case NodeKind.LessThanEqualsToken: // <=
                case NodeKind.GreaterThanToken: // >
                case NodeKind.GreaterThanEqualsToken: // >=
                    return TreeMaker.Binary(
                        KindOperator2TagOperator(operatorKind),
                        TreeMaker.Apply(
                            Nil<JCExpression>(),
                            TreeMaker.Select(node.Left.ToJavaSyntaxTree<JCExpression>(), EnumNames.ENUM_VALUE_NAME),
                            Nil<JCExpression>()),
                        TreeMaker.Apply(
                            Nil<JCExpression>(),
                            TreeMaker.Select(node.Right.ToJavaSyntaxTree<JCExpression>(), EnumNames.ENUM_VALUE_NAME),
                            Nil<JCExpression>())
                    );

                case NodeKind.EqualsToken: // =
                    return ConvertEqualsAssignOperator(node);
                default:
                    throw new NotSupportedException($"Does not support '{node.OperatorToken.Text}' on enum");
            }
        }

        private bool IsBinaryOperator(BinaryExpression node)
        {
            return KindOperator2TagOperator(node.OperatorToken.Kind) != null;
        }
        private JCTree ConvertBinaryOperator(BinaryExpression node)
        {
            return TreeMaker.Binary(
                KindOperator2TagOperator(node.OperatorToken.Kind),
                node.Left.ToJavaSyntaxTree<JCExpression>(),
                node.Right.ToJavaSyntaxTree<JCExpression>()
            );
        }

        private bool IsBinaryAssignOperator(BinaryExpression node)
        {
            return KindAssignOperator2TagAssignOperator(node.OperatorToken.Kind) != null;
        }
        private JCTree ConvertBinaryAssignOperator(BinaryExpression node)
        {
            NodeKind operatorKind = node.OperatorToken.Kind;
            Node left = node.Left;
            Node right = node.Right;
            Tag opcode = KindAssignOperator2TagAssignOperator(operatorKind);

            // a.b += xxx
            if (left.Kind == NodeKind.PropertyAccessExpression && ShouldConvertToSetMethod((PropertyAccessExpression)left))
            {
                var propAccess = (PropertyAccessExpression)left;
                var name = Names.fromString(propAccess.Name.Text.ToSetMethodName());
                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(propAccess.Expression.ToJavaSyntaxTree<JCExpression>(), name),
                    new List<JCExpression>()
                    {
                        TreeMaker.Binary(KindAssignOperator2TagOperator(operatorKind), left.ToJavaSyntaxTree<JCExpression>(), right.ToJavaSyntaxTree<JCExpression>())
                    }
                );
            }

            //a[b] += xxx
            if (left.Kind == NodeKind.ElementAccessExpression)
            {
                ElementAccessExpression elemAccess = (ElementAccessExpression)left;
                JCExpression indexArg = elemAccess.ArgumentExpression.ToJavaSyntaxTree<JCExpression>();
                if (ElementAccessExpressionConverter.ShouldCastArgumentToInt(elemAccess))
                {
                    indexArg = TreeMaker.TypeCast(TreeMaker.TypeIdent(TypeTag.INT), indexArg);
                }

                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(elemAccess.Expression.ToJavaSyntaxTree<JCExpression>(), GetSetMethodName(elemAccess)),
                    new List<JCExpression>()
                    {
                        indexArg,
                        TreeMaker.Binary(KindAssignOperator2TagOperator(operatorKind), left.ToJavaSyntaxTree<JCExpression>(), right.ToJavaSyntaxTree<JCExpression>())
                    }
                );
            }
            return TreeMaker.Assignop(opcode, left.ToJavaSyntaxTree<JCExpression>(), right.ToJavaSyntaxTree<JCExpression>());
        }

        private bool IsEqualsAssignOperator(BinaryExpression node)
        {
            NodeKind operatorKind = node.OperatorToken.Kind;
            return operatorKind == NodeKind.EqualsToken;
        }
        private JCTree ConvertEqualsAssignOperator(BinaryExpression node, JCExpression rightExpression = null)
        {
            Node left = node.Left;
            Node right = node.Right;
            rightExpression = rightExpression != null ? rightExpression : right.ToJavaSyntaxTree<JCExpression>();

            //a=b
            if (left.Kind == NodeKind.PropertyAccessExpression && ShouldConvertToSetMethod((PropertyAccessExpression)left))
            {
                var propAccess = (PropertyAccessExpression)left;
                var name = Names.fromString(propAccess.Name.Text.ToSetMethodName());
                if (propAccess.Name.Text == "length" && TypeHelper.IsArrayType(TypeHelper.GetNodeType(propAccess.Expression)))
                {
                    JCExpression fn = TreeMaker.Select(TreeMaker.Ident(Names.fromString("ArrayExtension")), name);
                    return TreeMaker.Apply(
                        Nil<JCExpression>(),
                        fn,
                        new List<JCExpression>()
                        {
                            propAccess.Expression.ToJavaSyntaxTree<JCExpression>(),
                            rightExpression
                        }
                    );
                }
                else
                {
                    return TreeMaker.Apply(
                        Nil<JCExpression>(),
                        TreeMaker.Select(propAccess.Expression.ToJavaSyntaxTree<JCExpression>(), name),
                        new List<JCExpression>()
                        {
                            rightExpression
                        }
                    );
                }
            }

            //a[b]=xxx
            if (left.Kind == NodeKind.ElementAccessExpression)
            {
                var elemAccess = (ElementAccessExpression)left;
                JCExpression indexArg = elemAccess.ArgumentExpression.ToJavaSyntaxTree<JCExpression>();
                if (ElementAccessExpressionConverter.ShouldCastArgumentToInt(elemAccess))
                {
                    indexArg = TreeMaker.TypeCast(TreeMaker.TypeIdent(TypeTag.INT), indexArg);
                }

                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(elemAccess.Expression.ToJavaSyntaxTree<JCExpression>(), GetSetMethodName(elemAccess)),
                    new List<JCExpression>()
                    {
                        indexArg,
                        rightExpression
                    }
                );
            }

            return TreeMaker.Assign(left.ToJavaSyntaxTree<JCExpression>(), rightExpression);
        }

        private bool IsTypeTestOperator(BinaryExpression node)
        {
            NodeKind operatorKind = node.OperatorToken.Kind;

            return (operatorKind == NodeKind.InstanceOfKeyword
                || operatorKind == NodeKind.IsKeyword
            );
        }
        private JCTree ConvertTypeTestOperator(BinaryExpression node)
        {
            return TreeMaker.TypeTest(node.Left.ToJavaSyntaxTree<JCExpression>(), node.Right.ToJavaSyntaxTree<JCTree>());
        }

        private static Tag KindAssignOperator2TagAssignOperator(NodeKind kind)
        {
            switch (kind)
            {
                case NodeKind.PlusEqualsToken: // +=
                    return Tag.PLUS_ASG;

                case NodeKind.MinusEqualsToken: // -=
                    return Tag.MINUS_ASG;

                case NodeKind.AsteriskEqualsToken: // *=
                    return Tag.MUL_ASG;

                case NodeKind.SlashEqualsToken: // /=
                    return Tag.DIV_ASG;

                case NodeKind.PercentEqualsToken: // %=
                    return Tag.MOD_ASG;

                case NodeKind.LessThanLessThanEqualsToken: // <<=
                    return Tag.SL_ASG;

                case NodeKind.GreaterThanGreaterThanEqualsToken: // >>=
                    return Tag.SR_ASG;

                case NodeKind.AmpersandEqualsToken: // &=
                    return Tag.BITAND_ASG;

                case NodeKind.BarEqualsToken: // |=
                    return Tag.BITOR_ASG;

                case NodeKind.CaretEqualsToken: // ^=
                    return Tag.BITXOR_ASG;

                default:
                    return null;
            }
        }

        private static Tag KindAssignOperator2TagOperator(NodeKind kind)
        {
            switch (kind)
            {
                case NodeKind.PlusEqualsToken: // +=
                    return Tag.PLUS;

                case NodeKind.MinusEqualsToken: // -=
                    return Tag.MINUS;

                case NodeKind.AsteriskEqualsToken: // *=
                    return Tag.MUL;

                case NodeKind.SlashEqualsToken: // /=
                    return Tag.DIV;

                case NodeKind.PercentEqualsToken: // %=
                    return Tag.MOD;

                case NodeKind.LessThanLessThanEqualsToken: // <<=
                    return Tag.SL;

                case NodeKind.GreaterThanGreaterThanEqualsToken: // >>=
                    return Tag.SR;

                case NodeKind.AmpersandEqualsToken: // &=
                    return Tag.BITAND;

                case NodeKind.BarEqualsToken: // |=
                    return Tag.BITOR;

                case NodeKind.CaretEqualsToken: // ^=
                    return Tag.BITXOR;

                default:
                    return null;
            }
        }

        private static Tag KindOperator2TagOperator(NodeKind kind)
        {
            switch (kind)
            {
                // ==Assign
                //case NodeKind.EqualsToken:
                //    return SyntaxKind.SimpleAssignmentExpression;

                //Binary
                case NodeKind.PlusToken: // +
                    return Tag.PLUS;

                case NodeKind.MinusToken: // -
                    return Tag.MINUS;

                case NodeKind.AsteriskToken: // *
                    return Tag.MUL;

                case NodeKind.SlashToken: // /
                    return Tag.DIV;

                case NodeKind.PercentToken: // %
                    return Tag.MOD;

                case NodeKind.LessThanLessThanToken: // <<
                    return Tag.SL;

                case NodeKind.GreaterThanGreaterThanToken: // >>
                case NodeKind.GreaterThanGreaterThanGreaterThanToken:
                    return Tag.SR;

                case NodeKind.BarBarToken: // ||
                    return Tag.OR;

                case NodeKind.AmpersandAmpersandToken: // &&
                    return Tag.AND;

                case NodeKind.BarToken: // |
                    return Tag.BITOR;

                case NodeKind.AmpersandToken: // &
                    return Tag.BITAND;

                case NodeKind.CaretToken: // ^
                    return Tag.BITXOR;

                case NodeKind.EqualsEqualsToken: // ==
                case NodeKind.EqualsEqualsEqualsToken:
                    return Tag.EQ;

                case NodeKind.ExclamationEqualsToken: // !=
                case NodeKind.ExclamationEqualsEqualsToken:
                    return Tag.NE;

                case NodeKind.LessThanToken: // <
                    return Tag.LT;

                case NodeKind.LessThanEqualsToken: // <=
                    return Tag.LE;

                case NodeKind.GreaterThanToken: // >
                    return Tag.GT;

                case NodeKind.GreaterThanEqualsToken: // >=
                    return Tag.GE;

                //case NodeKind.InstanceOfKeyword:
                //case NodeKind.IsKeyword:
                //    return SyntaxKind.IsExpression;

                //case NodeKind.AsKeyword:
                //    return SyntaxKind.AsExpression;

                //case NodeKind.QuestionQuestionToken:
                //    return SyntaxKind.CoalesceExpression;

                default:
                    return null;
            }


            //InKeyword in
            //LessThanSlashToken </
            //EqualsGreaterThanToken =>
            //AsteriskAsteriskToken **
            //PlusPlusToken ++
            //MinusMinusToken --
            //GreaterThanGreaterThanGreaterThanToken >>>
            //ExclamationToken ^
            //TildeToken ~
            //QuestionToken ?
            //ColonToken :
            //AtToken @

        }

        private static bool ShouldConvertToSetMethod(PropertyAccessExpression propAccess)
        {
            //
            // this.xxx
            if (propAccess.Expression.Kind == NodeKind.ThisKeyword)
            {
                var @class = propAccess.Ancestor<ClassDeclaration>();
                var field = @class?.GetField(propAccess.Name.Text);
                return (field == null || (field.IsPublic && !field.IsStatic));
            }

            // private field: xxx.__xxx = 
            Node member = TypeHelper.GetPropertyAccessMember(propAccess);
            if (member is PropertyDeclaration prop && (!prop.IsPublic || prop.IsStatic))
            {
                return false;
            }

            // static AA.field
            if (TypeHelper.IsMatchTypeName(propAccess.Expression.Text))
            {
                var project = propAccess.Document.Project;
                // class
                var @class = project.GetClass(propAccess.Expression.Text);
                var field = @class?.GetField(propAccess.Name.Text);
                if (field != null && field.IsStatic)
                {
                    return false;
                }
            }

            return true;
        }

        private static Name GetSetMethodName(ElementAccessExpression elemAccess)
        {
            Node type = TypeHelper.GetNodeType(elemAccess.Expression);
            if (type != null && TypeHelper.IsDictionaryType(type))
            {
                return HASHMAP_SET_INDEXER_NAME;
            }
            return COLLECTION_SET_INDEXER_NAME;
        }
    }
}


