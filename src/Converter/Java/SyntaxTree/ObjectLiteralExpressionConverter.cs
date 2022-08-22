using System;
using System.Linq;
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
    public class ObjectLiteralExpressionConverter : NodeConverter
    {
        public JCTree Convert(ObjectLiteralExpression node)
        {
            if (ShouldConvertToTuple(node))
            {
                return ConvertToTuple(node);
            }

            if (ShouldConvertToDictionary(node))
            {
                return ConvertToDictionary(node);
            }

            return ConvertToAnonymousInnerClass(node);
        }

        /// <summary>
        /// Indicate should convert to to tuple type.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ShouldConvertToTuple(ObjectLiteralExpression node)
        {
            return node.Properties.Count >= 2
                && node.Type.Kind == NodeKind.TypeLiteral
                && !((TypeLiteral)node.Type).IsIndexSignature;
        }
        private JCTree ConvertToTuple(ObjectLiteralExpression node)
        {
            TypeLiteral typeLiteral = (TypeLiteral)node.Type;
            List<Node> typeLiteralMembers = typeLiteral.Members;
            int memberCount = typeLiteralMembers.Count;

            JCExpression clazz = TreeMaker.Ident(Names.fromString(TypeLiteralConverter.GetTupleName(memberCount)));
            List<JCExpression> typeArgs = new List<JCExpression>();
            JCExpression[] args = new JCExpression[memberCount];

            List<string> memberNames = new List<string>();
            foreach (PropertySignature member in typeLiteralMembers)
            {
                typeArgs.Add(member.Type.ToJavaSyntaxTree<JCExpression>());
                memberNames.Add(member.Name.Text);
            }

            foreach (PropertyAssignment prop in node.Properties)
            {
                int index = memberNames.IndexOf(prop.Name.Text);
                args[index] = prop.Initializer.ToJavaSyntaxTree<JCExpression>();
            }

            return TreeMaker.NewClass(null, Nil<JCExpression>(), TreeMaker.TypeApply(clazz, typeArgs), args.ToList(), null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ShouldConvertToDictionary(ObjectLiteralExpression node)
        {
            return TypeHelper.IsDictionaryType(node.Type);
        }
        private JCTree ConvertToDictionary(ObjectLiteralExpression node)
        {
            JCClassDecl def = null;
            if (node.Properties.Count > 0)
            {
                List<JCStatement> initStatements = new List<JCStatement>();
                foreach (PropertyAssignment prop in node.Properties)
                {
                    // put(Key, Value);
                    JCMethodInvocation add = TreeMaker.Apply(
                        Nil<JCExpression>(),
                        TreeMaker.Ident(Names.fromString("put")),
                        new List<JCExpression>()
                        {
                            prop.Name.ToJavaSyntaxTree<JCExpression>(),
                            prop.Initializer.ToJavaSyntaxTree<JCExpression>()
                        }
                     );
                    initStatements.Add(TreeMaker.Exec(add));
                }

                def = TreeMaker.ClassDef(
                    TreeMaker.Modifiers(0),
                    null,
                    Nil<JCTypeParameter>(),
                    null,
                    Nil<JCExpression>(),
                    new List<JCTree>() {
                        CreateSerialVersionUID(),
                        TreeMaker.Block(0, initStatements)
                    }
                );
            }

            return TreeMaker.NewClass(
                null,
                Nil<JCExpression>(),
                node.Type.ToJavaSyntaxTree<JCExpression>(),
                Nil<JCExpression>(),
                def
            );
        }

        private JCTree ConvertToAnonymousInnerClass(ObjectLiteralExpression node)
        {
            JCClassDecl def = null;
            if (node.Properties.Count > 0)
            {
                List<JCStatement> initStats = new List<JCStatement>();
                foreach (PropertyAssignment prop in node.Properties)
                {
                    // setXXX(prop.Initializer);
                    JCMethodInvocation add = TreeMaker.Apply(
                        Nil<JCExpression>(),
                        TreeMaker.Ident(Names.fromString(prop.Name.Text.ToSetMethodName())),
                        new List<JCExpression>()
                        {
                            prop.Initializer.ToJavaSyntaxTree<JCExpression>()
                        }
                     );
                    initStats.Add(TreeMaker.Exec(add));
                }

                def = TreeMaker.ClassDef(
                    TreeMaker.Modifiers(0),
                    null,
                    Nil<JCTypeParameter>(),
                    null,
                    Nil<JCExpression>(),
                    new List<JCTree>() {
                        TreeMaker.Block(0, initStats)
                    }
                );
            }

            //
            if (node.Type.Kind == NodeKind.AnyKeyword || TypeHelper.GetTypeName(node.Type) == NativeTypes.JsonElement)
            {
                return def;
            }
            else
            {
                return TreeMaker.NewClass(
                    null,
                    Nil<JCExpression>(),
                    node.Type.ToJavaSyntaxTree<JCExpression>(),
                    Nil<JCExpression>(),
                    def
                );
            }
        }

        internal static bool ShouldConvertToAnonymousInnerClass(ObjectLiteralExpression node)
        {
            return node.Type == null || node.Type.Kind == NodeKind.AnyKeyword || TypeHelper.GetTypeName(node.Type) == NativeTypes.JsonElement;
        }
    }
}
