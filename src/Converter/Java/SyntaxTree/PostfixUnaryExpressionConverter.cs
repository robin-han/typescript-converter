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
    public class PostfixUnaryExpressionConverter : NodeConverter
    {
        public JCTree Convert(PostfixUnaryExpression node)
        {
            Tag opcode = KindToTag(node.Operator);
            if (opcode != null)
            {
                return TreeMaker.Unary(opcode, node.Operand.ToJavaSyntaxTree<JCExpression>());
            }
            return null;
        }

        private Tag KindToTag(NodeKind @operator)
        {
            switch (@operator)
            {
                case NodeKind.PlusPlusToken:
                    return Tag.POSTINC;

                case NodeKind.MinusMinusToken:
                    return Tag.POSTDEC;

                default:
                    return null;
            }
        }
    }
}

