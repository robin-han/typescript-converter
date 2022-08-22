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
    public class ArrayTypeConverter : NodeConverter
    {
        public JCTree Convert(ArrayType node)
        {
            //if (Context.TypeScriptType)
            //{
            //    return TreeMaker.TypeApply(
            //        TreeMaker.Ident(Names.fromString("Array")),
            //        node.ElementType.ToJavaSyntaxTrees<JCExpression>());
            //}
            //else
            //{
            //    return TreeMaker.TypeApply(
            //        TreeMaker.Ident(Names.fromString("ArrayList")),
            //        node.ElementType.ToJavaSyntaxTrees<JCExpression>());
            //}
            return TreeMaker.TypeApply(
                TreeMaker.Ident(Names.fromString("ArrayList")),
                node.ElementType.ToJavaSyntaxTrees<JCExpression>());
        }

        /// <summary>
        /// Convert to xxx.ToArray(new ElementType[0]).
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        internal static JCMethodInvocation ToArrayMethodInvocation(Node expression, Node elementType)
        {
            List<JCExpression> args = new List<JCExpression>()
            {
                TreeMaker.NewArray(
                elementType.ToJavaSyntaxTree<JCExpression>(),
                new List<JCExpression>() { TreeMaker.Literal(TypeTag.INT, 0) },
                null)
            };

            return TreeMaker.Apply(
                Nil<JCExpression>(),
                TreeMaker.Select(expression.ToJavaSyntaxTree<JCExpression>(), Names.fromString("toArray")),
                args
            );
        }


    }
}

