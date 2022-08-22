using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TypeScript.Syntax;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;

namespace TypeScript.Converter.Java
{
    public class NewExpressionConverter : NodeConverter
    {
        public JCTree Convert(NewExpression node)
        {
            JCExpression clazz;
            if (node.TypeArguments.Count > 0)
            {
                clazz = TreeMaker.TypeApply(
                    node.Expression.ToJavaSyntaxTree<JCExpression>(),
                    node.TypeArguments.ToJavaSyntaxTrees<JCExpression>()
                 );
            }
            else
            {
                clazz = node.Expression.ToJavaSyntaxTree<JCExpression>();
            }

            List<JCTree> arguments = Nil<JCTree>();
            if (node.Arguments.Count > 0)
            {
                List<Node> parameters = TypeHelper.GetParameters(node);
                arguments = CreateArgumentTreesByParameters<JCTree>(node.Arguments, parameters);
            }

            //
            if (IsInitializeArrayWithSize(node))
            {
                JCExpression fn = TreeMaker.Select(TreeMaker.Ident(Names.fromString("ArrayExtension")), Names.fromString("initialize"));
                JCNewClass newTree = TreeMaker.NewClass(
                    null,
                    Nil<JCExpression>(),
                    clazz,
                    Nil<JCExpression>(),
                    null
                );
                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    fn,
                    new List<JCExpression>() {
                        newTree,
                        (JCExpression)arguments[0]
                    }
                );
            }
            else
            {
                return TreeMaker.NewClass(
                    null,
                    Nil<JCExpression>(),
                    clazz,
                    arguments.Where(treeNode => treeNode is JCExpression).Select(treeNode => (JCExpression)treeNode).ToList(),
                    arguments.Find(treeNode => treeNode is JCClassDecl) as JCClassDecl // arguments is object literal {a: xxx, b: xxx}
                );
            }
        }

        private bool IsInitializeArrayWithSize(NewExpression node)
        {
            if (node.Arguments.Count == 1 && TypeHelper.IsNumberType(TypeHelper.GetNodeType(node.Arguments[0])) && TypeHelper.IsArrayType(node.Expression.Text))
            {
                return true;
            }
            return false;
        }
    }
}

