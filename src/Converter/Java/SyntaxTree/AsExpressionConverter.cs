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
    public class AsExpressionConverter : NodeConverter
    {
        private static readonly Name NORMAL_CAST_NAME = Names.fromString("cast");
        private static readonly Name ARRAY_CAST_NAME = Names.fromString("asArray");
        private static readonly Name CLASS_NAME = Names.fromString("class");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public JCTree Convert(AsExpression node)
        {
            if (TypeHelper.IsArrayType(node.Type))
            {
                // public <U> Array<U> asArray(Class<? extends U[]> newType)
                Node elementType = TypeHelper.GetArrayElementType(node.Type);

                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(TreeMaker.Ident(Names.fromString("ArrayExtension")), ARRAY_CAST_NAME),
                    new List<JCExpression>()
                    {
                        node.Expression.ToJavaSyntaxTree<JCExpression>(),
                        TreeMaker.Select(elementType.ToJavaSyntaxTree<JCExpression>(), CLASS_NAME)
                        //// New ArrayList<XXX>()
                        //TreeMaker.NewClass(
                        //    null,
                        //    Nil<JCExpression>(),
                        //    TreeMaker.TypeApply(
                        //        TreeMaker.Ident(Names.fromString("ArrayList")),
                        //        elementType.ToJavaSyntaxTrees<JCExpression>()
                        //     ),
                        //    Nil<JCExpression>(),
                        //    null)
                        //TreeMaker.Select(TreeMaker.TypeArray(elementType.ToJavaSyntaxTree<JCExpression>()), CLASS_NAME) //XXX[].class
                    }
                );
            }
            else if (TypeHelper.IsGenericType(node.Type))
            {
                return TreeMaker.TypeCast(
                    node.Type.ToJavaSyntaxTree<JCTree>(),
                    node.Expression.ToJavaSyntaxTree<JCExpression>()
                );
            }
            else
            {
                // public static <T> T cast(Object obj, Class<T> clss);
                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Ident(NORMAL_CAST_NAME),
                    new List<JCExpression>()
                    {
                        node.Expression.ToJavaSyntaxTree<JCExpression>(),
                        TreeMaker.Select(node.Type.ToJavaSyntaxTree<JCExpression>(), CLASS_NAME)
                    }
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exprSyntax"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        internal static JCExpression AsType(JCExpression exprSyntax, string typeName)
        {
            // cast(derive, Base.class);
            if (!string.IsNullOrEmpty(typeName))
            {
                return TreeMaker.Apply(
                   Nil<JCExpression>(),
                   TreeMaker.Ident(NORMAL_CAST_NAME),
                   new List<JCExpression>()
                   {
                       exprSyntax,
                       TreeMaker.Select(TreeMaker.Ident(Names.fromString(typeName)), CLASS_NAME)
                   }
                );
            }

            return exprSyntax;
        }
    }
}

