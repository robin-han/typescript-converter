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
    public class GetAccessorConverter : NodeConverter
    {
        public JCTree Convert(GetAccessor node)
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
            JCExpression type = node.Type.ToJavaSyntaxTree<JCExpression>();
            Name name = Names.fromString(node.Name.Text.ToGetMethodName());
            JCBlock body = node.Body?.ToJavaSyntaxTree<JCBlock>();

            JCMethodDecl getDef = TreeMaker.MethodDef(mods, name, type, Nil<JCTypeParameter>(), Nil<JCVariableDecl>(), Nil<JCExpression>(), body, null);
            getDef.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            return getDef;
        }
    }
}

