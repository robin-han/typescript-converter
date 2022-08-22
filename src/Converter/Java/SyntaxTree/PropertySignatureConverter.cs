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
    public class PropertySignatureConverter : NodeConverter
    {
        public List<JCTree> Convert(PropertySignature node)
        {
            // only public and abstract alowed: IDonutShape, interfance modifier public????
            List<JCTree> methods = new List<JCTree>();

            // get
            string comments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            // implicitly public and abstract or static final field
            JCModifiers mods = node.HasModify(NodeKind.StaticKeyword) ? TreeMaker.Modifiers(node.Modifiers.ToFlags()) : TreeMaker.Modifiers(0);
            JCExpression type = node.Type.ToJavaSyntaxTree<JCExpression>();
            Name getName = Names.fromString(node.Name.Text.ToGetMethodName());
            JCMethodDecl geMethodDef = TreeMaker.MethodDef(mods, getName, type, Nil<JCTypeParameter>(), Nil<JCVariableDecl>(), Nil<JCExpression>(), null, null);
            geMethodDef.docComments = comments;
            methods.Add(geMethodDef);

            // set
            if (!node.IsReadonly)
            {
                Name setName = Names.fromString(node.Name.Text.ToSetMethodName());
                JCVariableDecl paramDef = TreeMaker.VarDef(TreeMaker.Modifiers(0), Names.fromString("value"), type, null);
                JCMethodDecl setMethodDef = TreeMaker.MethodDef(mods, setName, TreeMaker.TypeIdent(TypeTag.VOID), Nil<JCTypeParameter>(), new List<JCVariableDecl>() { paramDef }, Nil<JCExpression>(), null, null);
                setMethodDef.docComments = comments;
                methods.Add(setMethodDef);
            }

            return methods;
        }
    }
}

