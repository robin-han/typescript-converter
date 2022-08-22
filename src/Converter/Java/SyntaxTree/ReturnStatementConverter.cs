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
    public class ReturnStatementConverter : NodeConverter
    {
        public JCTree Convert(ReturnStatement node)
        {
            if (node.Expression == null)
            {
                return TreeMaker.Return(null);
            }

            #region Implicit Operator
            Node methodType = TypeHelper.GetReturnType(node);
            string fromType = TypeHelper.GetTypeName(TypeHelper.GetNodeType(node.Expression));
            string toType = TypeHelper.GetTypeName(methodType);
            var implicitOperator = OperatorConfig.ImplicitOperators.Find(@operator => @operator.From == fromType && @operator.To == toType);
            if (implicitOperator != null)
            {
                return TreeMaker.Return(CreateImplicitOperatorTree(implicitOperator, node.Expression));
            }
            #endregion

            //
            return TreeMaker.Return(node.Expression.ToJavaSyntaxTree<JCExpression>());
        }
    }
}
