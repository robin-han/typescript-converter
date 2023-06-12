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
    public class CallExpressionConverter : NodeConverter
    {
        private static readonly List<string> ARRAY_EXTENSION_METHODS = new List<string>()
        {
            "push", "pop", "shift", "unshift", "concat", "every", "filter", "forEach", "map", "reduce", "some", "sort", "slice", "reverse", "join", "splice"
        };

        private static readonly List<string> STRING_EXTENSION_METHODS = new List<string>()
        {
            "charAt", "match", "substr", "substring", "split", "slice", "search", "replace", "localeCompare"
        };

        private static readonly List<string> NUMBER_EXTENSION_METHODS = new List<string>()
        {
            "toString", "toFixed"
        };

        private static readonly List<string> STRING_STATIC_METHODS = new List<string>()
        {
            "fromCharCode"
        };

        public JCTree Convert(CallExpression node)
        {
            List<JCExpression> args = Nil<JCExpression>();
            if (node.Arguments.Count > 0)
            {
                List<Node> parameters = TypeHelper.GetParameters(node);
                args = CreateArgumentTreesByParameters<JCExpression>(node.Arguments, parameters);
            }

            if (node.IsTypePredicate(out FunctionDeclaration predicateFunc))
            {
                JCExpression expr = args[0];
                JCTree clazz = ((TypePredicate)predicateFunc.Type).Type.ToJavaSyntaxTree<JCTree>();
                return TreeMaker.TypeTest(expr, clazz);
            }
            else if (IsNumberInvoke(node))
            {
                JCExpression fn = TreeMaker.Ident(Names.fromString("toNumber"));
                return TreeMaker.Apply(Nil<JCExpression>(), fn, args);
            }
            else if (IsDelegateInovke(node))
            {
                return TreeMaker.Apply(
                    node.TypeArguments.ToJavaSyntaxTrees<JCExpression>(),
                    TreeMaker.Select(node.Expression.ToJavaSyntaxTree<JCExpression>(), Names.fromString("invoke")),
                    args
                );
            }
            else if (IsArrayExtensionInvoke(node))
            {
                PropertyAccessExpression propAccess = (PropertyAccessExpression)node.Expression;
                JCExpression fn = TreeMaker.Select(TreeMaker.Ident(Names.fromString("ArrayExtension")), Names.fromString(propAccess.Name.Text));
                args.Insert(0, propAccess.Expression.ToJavaSyntaxTree<JCExpression>());

                return TreeMaker.Apply(Nil<JCExpression>(), fn, args);
            }
            else if (IsStringExtensionInvoke(node))
            {
                PropertyAccessExpression propAccess = (PropertyAccessExpression)node.Expression;
                JCExpression fn = TreeMaker.Select(TreeMaker.Ident(Names.fromString("StringExtension")), Names.fromString(propAccess.Name.Text));
                args.Insert(0, propAccess.Expression.ToJavaSyntaxTree<JCExpression>());

                return TreeMaker.Apply(Nil<JCExpression>(), fn, args);
            }
            else if (IsNumberExtensionInvoke(node))
            {
                PropertyAccessExpression propAccess = (PropertyAccessExpression)node.Expression;
                JCExpression fn = TreeMaker.Select(TreeMaker.Ident(Names.fromString("NumberExtension")), Names.fromString(propAccess.Name.Text));
                args.Insert(0, propAccess.Expression.ToJavaSyntaxTree<JCExpression>());

                return TreeMaker.Apply(Nil<JCExpression>(), fn, args);
            }
            else if (IsStringStaticInvoke(node))
            {
                PropertyAccessExpression propAccess = (PropertyAccessExpression)node.Expression;
                JCExpression fn = TreeMaker.Select(TreeMaker.Ident(Names.fromString("StringExtension")), Names.fromString(propAccess.Name.Text));
                return TreeMaker.Apply(Nil<JCExpression>(), fn, args);
            }
            else if (IsAsEnumInvoke(node))
            {
                args[1] = TreeMaker.Select(args[1], Names.fromString("class"));
                return TreeMaker.Apply(
                   Nil<JCExpression>(),
                   node.Expression.ToJavaSyntaxTree<JCExpression>(),
                   args
               );
            }
            else
            {
                return TreeMaker.Apply(
                    node.TypeArguments.ToJavaSyntaxTrees<JCExpression>(),
                    node.Expression.ToJavaSyntaxTree<JCExpression>(),
                    args
                );
            }
        }

        private bool IsNumberInvoke(CallExpression node)
        {
            return node.Expression.Kind == NodeKind.Identifier && node.Expression.Text == "Number";
        }

        private bool IsDelegateInovke(CallExpression node)
        {
            Node type = TypeHelper.GetNodeType(node.Expression);
            Node declaration = type == null ? null : node.Document.Project.GetTypeDeclaration(TypeHelper.TrimType(type).Text);
            if (declaration != null)
            {
                switch (declaration.Kind)
                {
                    case NodeKind.InterfaceDeclaration:
                        return ((InterfaceDeclaration)declaration).IsDelegate;

                    case NodeKind.TypeAliasDeclaration:
                        return ((TypeAliasDeclaration)declaration).IsDelegate;

                    default:
                        return false;
                }
            }
            return false;
        }

        private bool IsArrayExtensionInvoke(CallExpression node)
        {
            if (node.Expression.Kind == NodeKind.PropertyAccessExpression)
            {
                PropertyAccessExpression propAccess = (PropertyAccessExpression)node.Expression;
                return ARRAY_EXTENSION_METHODS.Contains(propAccess.Name.Text) && TypeHelper.IsArrayType(TypeHelper.GetNodeType(propAccess.Expression));
            }
            return false;
        }

        private bool IsStringExtensionInvoke(CallExpression node)
        {
            if (node.Expression.Kind == NodeKind.PropertyAccessExpression)
            {
                PropertyAccessExpression propAccess = (PropertyAccessExpression)node.Expression;

                return STRING_EXTENSION_METHODS.Contains(propAccess.Name.Text);
                // TODO: TypeHelper.GetNodeType for primitive string type, such as toLowerCase
                //&& TypeHelper.IsStringType(TypeHelper.GetNodeType(propAccess.Expression));
            }
            return false;
        }

        private bool IsNumberExtensionInvoke(CallExpression node)
        {
            if (node.Expression.Kind == NodeKind.PropertyAccessExpression)
            {
                PropertyAccessExpression propAccess = (PropertyAccessExpression)node.Expression;
                return NUMBER_EXTENSION_METHODS.Contains(propAccess.Name.Text)
                    && TypeHelper.IsNumberType(TypeHelper.GetNodeType(propAccess.Expression)); ;
            }
            return false;
        }

        private bool IsStringStaticInvoke(CallExpression node)
        {
            if (node.Expression.Kind == NodeKind.PropertyAccessExpression)
            {
                PropertyAccessExpression propAccess = (PropertyAccessExpression)node.Expression;
                return propAccess.Expression.Text == "String" && STRING_STATIC_METHODS.Contains(propAccess.Name.Text);
            }
            return false;
        }
        private bool IsAsEnumInvoke(CallExpression node)
        {
            return (node.Arguments.Count >= 2) && (node.Expression is PropertyAccessExpression propAccess) && (propAccess.Name.Text == "asEnum");
        }

    }
}

