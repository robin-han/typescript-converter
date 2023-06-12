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
    public class VariableDeclarationConverter : NodeConverter
    {
        public JCTree Convert(VariableDeclaration node)
        {
            Name name = Names.fromString(node.Name.Text);
            JCExpression type = node.Type?.ToJavaSyntaxTree<JCExpression>();
            JCExpression init = node.Initializer?.ToJavaSyntaxTree<JCExpression>();
            JCModifiers mods = IsConst(node) ? TreeMaker.Modifiers(Flags.FINAL) : TreeMaker.Modifiers(0);

            // (DataValueType)xxx
            if (node.Type != null && node.Initializer != null)
            {
                if (IsEnumToNumberOperator(node))
                {
                    init = CreateEnumToNumberOperator(node);
                }
                else
                {
                    string fromType = TypeHelper.GetTypeName(TypeHelper.GetNodeType(node.Initializer));
                    string toType = TypeHelper.GetTypeName(node.Type);
                    var implicitOperator = OperatorConfig.ImplicitOperators.Find(@operator => @operator.From == fromType && @operator.To == toType);
                    if (implicitOperator != null)
                    {
                        init = CreateImplicitOperatorTree(implicitOperator, init);
                    }
                }
            }

            return TreeMaker.VarDef(mods, name, type, init);
        }

        private bool IsEnumToNumberOperator(VariableDeclaration node)
        {
            Node initializerType = TypeHelper.GetNodeType(node.Initializer);
            return TypeHelper.IsNumberType(node.Type) && TypeHelper.IsEnumType(initializerType);
        }

        private JCExpression CreateEnumToNumberOperator(VariableDeclaration node)
        {
            return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(node.Initializer.ToJavaSyntaxTree<JCExpression>(), EnumNames.ENUM_VALUE_NAME),
                    Nil<JCExpression>());
        }

        private bool IsConst(VariableDeclaration node)
        {
            VariableDeclarationList parent = node.Parent as VariableDeclarationList;
            if (parent == null)
            {
                return false;
            }
            string[] texts = parent.Text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var text in texts)
            {
                if (text.TrimStart().StartsWith("const "))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
