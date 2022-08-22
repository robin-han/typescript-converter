using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TypeScript.Syntax;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;

namespace TypeScript.Converter.Java
{
    public class TypeAssertionExpressionConverter : NodeConverter
    {
        public JCTree Convert(TypeAssertionExpression node)
        {
            // Enum.value()
            Node exprType = TypeHelper.GetNodeType(node.Expression);
            if (TypeHelper.IsIntType(node.Type) && TypeHelper.IsEnumType(exprType))
            {
                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(node.Expression.ToJavaSyntaxTree<JCExpression>(), EnumNames.ENUM_VALUE_NAME),
                    Nil<JCExpression>());
            }

            // (DataValueType)xxx
            string fromType = TypeHelper.GetTypeName(exprType);
            string toType = TypeHelper.GetTypeName(node.Type);
            var implicitOperator = OperatorConfig.ImplicitOperators.Find(@operator => @operator.From == fromType && @operator.To == toType);
            if (implicitOperator != null)
            {
                return CreateImplicitOperatorTree(implicitOperator, node.Expression);
            }

            //
            JCTree clazz = null;
            bool isPrimitiveBoolean = node.Type.Kind == NodeKind.BooleanKeyword;
            bool isPrimitiveNumber = node.Type.Kind == NodeKind.NumberKeyword;
            if ((isPrimitiveBoolean || isPrimitiveNumber) && TypeHelper.IsObjectType(exprType))
            {
                if (isPrimitiveBoolean)
                {
                    clazz = TreeMaker.Ident(Names.fromString("Boolean"));
                }
                else if (isPrimitiveNumber)
                {
                    clazz = TreeMaker.Ident(Names.fromString("Double"));
                }
            }
            if (clazz == null)
            {
                clazz = node.Type.ToJavaSyntaxTree<JCTree>();
            }

            return TreeMaker.TypeCast(
               clazz,
               node.Expression.ToJavaSyntaxTree<JCExpression>()
           );
        }
    }
}

