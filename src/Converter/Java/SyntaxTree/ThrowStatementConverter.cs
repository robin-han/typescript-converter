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
    public class ThrowStatementConverter : NodeConverter
    {
        public JCTree Convert(ThrowStatement node)
        {
            return TreeMaker.Throw(node.Expression.ToJavaSyntaxTree<JCExpression>());

            //// TODO: its method should add 'throws BaseError' and all caller also should add 'throws BaseError'
            //MethodDeclaration method = node.Ancestor<MethodDeclaration>(NodeKind.MethodDeclaration);
            //if (method != null && method.Type != null && method.Type.Kind != NodeKind.VoidKeyword)
            //{
            //    JCExpression expr = TreeMaker.Literal(TypeTag.BOT, null);
            //    if (TypeHelper.IsNumberType(method.Type))
            //    {
            //        expr = TreeMaker.Literal(TypeTag.DOUBLE, "0.0");
            //    }
            //    if (TypeHelper.IsBoolType(method.Type))
            //    {
            //        expr = TreeMaker.Literal(TypeTag.BOOLEAN, false);
            //    }
            //    return TreeMaker.Return(expr);
            //}
            //return null;
            ////~
        }
    }
}

