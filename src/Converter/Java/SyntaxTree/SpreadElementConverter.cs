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
    public class SpreadElementConverter : NodeConverter
    {
        public JCTree Convert(SpreadElement node)
        {
            if (IsCallExpressionParameter(node))
            {
                if (ShouldConvertToArray(node))
                {
                    //list.toArray(new T[0])
                    Node elementType = TypeHelper.GetArrayElementType(TypeHelper.GetNodeType(node.Expression));
                    if (elementType != null)
                    {
                        return ArrayTypeConverter.ToArrayMethodInvocation(node.Expression, elementType);
                    }
                }

                return node.Expression.ToJavaSyntaxTree<JCTree>();
            }

            //TODO: NOT SUPPORT
            return null;
        }

        private bool IsCallExpressionParameter(SpreadElement node)
        {
            return node.Parent.Kind == NodeKind.CallExpression;
        }

        /// <summary>
        /// Change to array for variable parameter.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ShouldConvertToArray(SpreadElement node)
        {
            if (node.Expression.Kind == NodeKind.Identifier)
            {
                string name = node.Expression.Text;

                // method
                MethodDeclaration methodParent = node.Ancestor<MethodDeclaration>();
                List<Node> paramters = methodParent == null ? new List<Node>() : methodParent.Parameters;
                foreach (Parameter param in paramters)
                {
                    if (param.Name.Text == name && param.IsVariable)
                    {
                        return false;
                    }
                }

                // constructor
                Constructor ctorParent = node.Ancestor<Constructor>();
                paramters = ctorParent == null ? new List<Node>() : ctorParent.Parameters;
                foreach (Parameter param in paramters)
                {
                    if (param.Name.Text == name && param.IsVariable)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

