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
    public class TypeAliasDeclarationConverter : NodeConverter
    {
        public JCTree Convert(TypeAliasDeclaration node)
        {
            if (node.IsDelegate)
            {
                return CreateDelegateDeclaration(node);
            }

            //TODO: There is no import aliasing mechanism in Java, you can wrap it in a class and import static
            return null;
        }

        private JCTree CreateDelegateDeclaration(TypeAliasDeclaration node)
        {
            JCModifiers modifiers = TreeMaker.Modifiers(Flags.INTERFACE | node.Modifiers.ToFlags());
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
            List<JCExpression> implementing = Nil<JCExpression>();
            List<JCTree> defs = Nil<JCTree>();

            FunctionType fn = node.Type as FunctionType;
            JCExpression methodType = fn.Type.ToJavaSyntaxTree<JCExpression>();
            Name methodName = Names.fromString("invoke");
            List<JCVariableDecl> methodParams = fn.Parameters.ToJavaSyntaxTrees<JCVariableDecl>();
            JCMethodDecl methodDef = TreeMaker.MethodDef(TreeMaker.Modifiers(0), methodName, methodType, Nil<JCTypeParameter>(), methodParams, Nil<JCExpression>(), null, null);
            defs.Add(methodDef);

            var interfaceDef = TreeMaker.ClassDef(modifiers, name, typarams, null, implementing, defs);
            interfaceDef.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            return interfaceDef;
        }
    }
}

