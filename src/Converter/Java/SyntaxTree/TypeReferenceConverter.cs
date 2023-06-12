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
    public class TypeReferenceConverter : NodeConverter
    {
        public JCTree Convert(TypeReference node)
        {
            string typeText = node.TypeName.Text;

            switch (typeText)
            {
                case NativeTypes.Int:
                    if (IsGenericTypeArgument(node))
                    {
                        return TreeMaker.Ident(Names.fromString("Integer"));
                    }
                    else
                    {
                        return TreeMaker.TypeIdent(TypeTag.INT);
                    }

                case NativeTypes.Long:
                    if (IsGenericTypeArgument(node))
                    {
                        return TreeMaker.Ident(Names.fromString("Long"));
                    }
                    else
                    {
                        return TreeMaker.TypeIdent(TypeTag.LONG);
                    }

                case NativeTypes.AnyObject:
                    return TreeMaker.Ident(Names.fromString("Object"));

                case NativeTypes.Bool:
                    return TreeMaker.TypeIdent(TypeTag.BOOLEAN);

                case NativeTypes.Double:
                    return TreeMaker.TypeIdent(TypeTag.DOUBLE);

                case NativeTypes.String:
                    return TreeMaker.Ident(Names.fromString("String"));

                case NativeTypes.Type:
                    return TreeMaker.Ident(Names.fromString("Class<?>"));

                case "Array":
                case "ReadonlyArray":
                    if (node.TypeArguments.Count > 0)
                    {
                        return TreeMaker.TypeApply(
                            TreeMaker.Ident(Names.fromString("ArrayList")),
                            node.TypeArguments[0].ToJavaSyntaxTrees<JCExpression>());
                    }
                    else
                    {
                        return TreeMaker.Ident(Names.fromString("ArrayList"));
                    }

                case "RegExpMatchArray":
                case "RegExpExecArray":
                    return TreeMaker.Ident(Names.fromString("RegExpArray"));

                default:
                    if (node.TypeArguments.Count > 0)
                    {
                        return TreeMaker.TypeApply(
                            node.TypeName.ToJavaSyntaxTree<JCExpression>(),
                            node.TypeArguments.ToJavaSyntaxTrees<JCExpression>()
                        );
                    }
                    else
                    {
                        return node.TypeName.ToJavaSyntaxTree<JCTree>();
                    }
            }
        }
    }
}

