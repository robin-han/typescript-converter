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
    public class UnionTypeConverter : NodeConverter
    {
        public JCTree Convert(UnionType node)
        {
            if (node.Types.Count == 2 && node.HasNullType)
            {
                Node type = TypeHelper.TrimType(node);
                switch (type.Kind)
                {
                    case NodeKind.NumberKeyword:
                        return TreeMaker.Ident(Names.fromString("Double"));

                    case NodeKind.BooleanKeyword:
                        return TreeMaker.Ident(Names.fromString("Boolean"));

                    case NodeKind.StringKeyword:
                        return TreeMaker.Ident(Names.fromString("String"));

                    default:
                        string typeName = TypeHelper.ToShortName(type.Text);
                        switch (typeName)
                        {
                            case NativeTypes.Int:
                                return TreeMaker.Ident(Names.fromString("Integer"));

                            case NativeTypes.Long:
                                return TreeMaker.Ident(Names.fromString("Long"));

                            case NativeTypes.Double:
                                return TreeMaker.Ident(Names.fromString("Double"));

                            case NativeTypes.Bool:
                                return TreeMaker.Ident(Names.fromString("Boolean"));

                            default:
                                return type.ToJavaSyntaxTree<JCTree>();
                        }
                }
            }

            // TODO:  UnionType: since 1.7
            return null;
        }
    }
}

