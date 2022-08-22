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
    public class EnumMemberConverter : NodeConverter
    {
        public JCTree Convert(EnumMember node)
        {
            Name name = Names.fromString(node.Name.Text);

            var enumMemberDef = TreeMaker.VarDef(TreeMaker.Modifiers(Flags.ENUM), name, null, null);
            enumMemberDef.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            return enumMemberDef;
        }
    }
}

