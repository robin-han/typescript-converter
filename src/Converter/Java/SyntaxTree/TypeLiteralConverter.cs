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
    public class TypeLiteralConverter : NodeConverter
    {
        private readonly static List<string> TupleNames = new List<string>()
        {
            "Unit",    // Unit<A>
            "Pair",    // Pair<A,B>
            "Triplet", // Triplet<A,B,C>
            "Quartet", // Quartet<A,B,C,D>
            "Quintet", // Quintet<A,B,C,D,E>
            "Sextet",  // Sextet<A,B,C,D,E,F>
            "Septet",  // Septet<A,B,C,D,E,F,G>
            "Octet",   // Octet<A,B,C,D,E,F,G,H>
            "Ennead",  // Ennead<A,B,C,D,E,F,G,H,I>
            "Decade",  // Decade<A,B,C,D,E,F,G,H,I,J>
        };

        internal static string GetTupleName(int memberCount)
        {
            return TupleNames[memberCount - 1];
        }

        public JCTree Convert(TypeLiteral node)
        {
            List<Node> members = node.Members;
            int memberCount = members.Count;

            if (node.IsIndexSignature)
            {
                return members[0].ToJavaSyntaxTree<JCTree>();
            }
            else
            {
                JCExpression clazz = TreeMaker.Ident(Names.fromString(GetTupleName(memberCount)));
                List<JCExpression> arguments = new List<JCExpression>();
                foreach (PropertySignature member in members)
                {
                    arguments.Add(member.Type.ToJavaSyntaxTree<JCExpression>());
                }
                return TreeMaker.TypeApply(clazz, arguments);
            }
        }
    }
}

