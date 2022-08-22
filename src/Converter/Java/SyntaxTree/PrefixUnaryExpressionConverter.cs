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
    public class PrefixUnaryExpressionConverter : NodeConverter
    {
        public JCTree Convert(PrefixUnaryExpression node)
        {
            //+XXX to number
            if (node.Operator == NodeKind.PlusToken && node.Operand.Kind != NodeKind.NumericLiteral)
            {
                return TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Ident(Names.fromString("toNumber")),
                    node.Operand.ToJavaSyntaxTrees<JCExpression>()
                );
            }

            //
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
                case NodeKind.PlusToken: // +
                    return Tag.POS;

                case NodeKind.MinusToken: // -
                    return Tag.NEG;

                case NodeKind.TildeToken: // ~
                    return Tag.COMPL;

                case NodeKind.ExclamationToken: // !
                    return Tag.NOT;

                case NodeKind.PlusPlusToken: // ++
                    return Tag.PREINC;

                case NodeKind.MinusMinusToken: // --
                    return Tag.PREDEC;

                default:
                    return null;
            }
        }
    }
}

