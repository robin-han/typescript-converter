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
    public class ClassDeclarationConverter : NodeConverter
    {
        public JCTree Convert(ClassDeclaration node)
        {
            JCModifiers modifiers = TreeMaker.Modifiers(node.Modifiers.ToFlags(), node.Decorators.ToJavaSyntaxTrees<JCAnnotation>());
            if (node.IsExport)
            {
                modifiers.flags |= Flags.PUBLIC;
            }
            Name name = Names.fromString(NormalizeTypeName(node.NameText));

            List<JCTypeParameter> typarams = Nil<JCTypeParameter>();
            if (node.TypeParameters.Count > 0)
            {
                typarams = node.TypeParameters.ToJavaSyntaxTrees<JCTypeParameter>();
            }
            JCExpression extending = node.Extending?.ToJavaSyntaxTree<JCExpression>();
            List<JCExpression> implementing = Nil<JCExpression>();
            if (node.Implementing.Count > 0)
            {
                implementing = node.Implementing.ToJavaSyntaxTrees<JCExpression>();
            }
            List<JCTree> defs = node.Members.ToJavaSyntaxTrees<JCTree>();

            var classDef = TreeMaker.ClassDef(modifiers, name, typarams, extending, implementing, defs);
            classDef.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            return classDef;
        }
    }
}

