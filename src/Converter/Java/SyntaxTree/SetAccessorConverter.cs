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
    public class SetAccessorConverter : NodeConverter
    {
        public JCTree Convert(SetAccessor node)
        {
            List<Node> modifiers = node.Modifiers.FindAll(m => m.Kind != NodeKind.OverrideKeyword);
            List<JCAnnotation> @annotations = new List<JCAnnotation>();
            Node overKeyword = node.Modifiers.Find(m => m.Kind == NodeKind.OverrideKeyword);
            if (overKeyword != null)
            {
                @annotations.Add(overKeyword.ToJavaSyntaxTree<JCAnnotation>());
            }
            @annotations.AddRange(node.Decorators.ToJavaSyntaxTrees<JCAnnotation>());
            JCModifiers mods = TreeMaker.Modifiers(modifiers.ToFlags(), @annotations);

            //
            JCExpression type = TreeMaker.TypeIdent(TypeTag.VOID);
            Name name = Names.fromString(node.Name.Text.ToSetMethodName());
            List<JCVariableDecl> @params = node.Parameters.ToJavaSyntaxTrees<JCVariableDecl>();
            JCBlock body = node.Body?.ToJavaSyntaxTree<JCBlock>();

            JCMethodDecl setDef = TreeMaker.MethodDef(mods, name, type, Nil<JCTypeParameter>(), @params, Nil<JCExpression>(), body, null);
            setDef.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            return setDef;
        }
    }
}

