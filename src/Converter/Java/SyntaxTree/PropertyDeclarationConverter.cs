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
using System.Linq;

namespace TypeScript.Converter.Java
{
    public class PropertyDeclarationConverter : NodeConverter
    {
        public List<JCTree> Convert(PropertyDeclaration node)
        {
            if (node.IsPublic && !node.IsStatic)
            {
                return this.ToGetSet(node);
            }

            JCModifiers mods = TreeMaker.Modifiers(node.Modifiers.ToFlags(), node.Decorators.ToJavaSyntaxTrees<JCAnnotation>());
            JCExpression type = node.Type.ToJavaSyntaxTree<JCExpression>();
            Name name = Names.fromString(node.Name.Text);
            JCExpression init = node.Initializer?.ToJavaSyntaxTree<JCExpression>();
            JCVariableDecl property = TreeMaker.VarDef(mods, name, type, init);
            property.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null; ;
            return new List<JCTree>() { property };
        }

        private List<JCTree> ToGetSet(PropertyDeclaration node)
        {
            List<JCTree> trees = new List<JCTree>();

            Name name = Names.fromString(node.Name.Text);
            JCExpression type = node.Type.ToJavaSyntaxTree<JCExpression>();

            // field
            if (!node.IsAbstract)
            {
                JCVariableDecl fieldDef = TreeMaker.VarDef(TreeMaker.Modifiers(Flags.PRIVATE), name, type, null);
                trees.Add(fieldDef);
            }

            // get
            string comments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            List<Node> modifiers = node.Modifiers;
            if (node.IsAbstract)
            {
                modifiers = modifiers.Where(n => n.Kind != NodeKind.ReadonlyKeyword).ToList();
            }
            JCModifiers mods = TreeMaker.Modifiers(modifiers.ToFlags());
            Name getName = Names.fromString(node.Name.Text.ToGetMethodName());
            JCReturn returnStat = TreeMaker.Return(TreeMaker.Ident(name));
            JCBlock getBody = node.IsAbstract ? null : TreeMaker.Block(0, new List<JCStatement> { returnStat });
            JCMethodDecl geMethodDef = TreeMaker.MethodDef(mods, getName, type, Nil<JCTypeParameter>(), Nil<JCVariableDecl>(), Nil<JCExpression>(), getBody, null);
            geMethodDef.docComments = comments;
            trees.Add(geMethodDef);

            // set
            if ((node.IsReadonly && !node.IsAbstract) || (!node.IsReadonly))
            {
                Name setName = Names.fromString(node.Name.Text.ToSetMethodName());
                Name paramName = Names.fromString("value");
                JCVariableDecl paramDef = TreeMaker.VarDef(TreeMaker.Modifiers(0), paramName, type, null);
                JCExpressionStatement assignStat = TreeMaker.Exec(TreeMaker.Assign(TreeMaker.Select(TreeMaker.Ident(Names.fromString("this")), name), TreeMaker.Ident(paramName)));
                JCBlock setBody = node.IsAbstract ? null : TreeMaker.Block(0, new List<JCStatement> { assignStat });
                if (node.IsReadonly)
                {
                    mods = TreeMaker.Modifiers(Flags.PRIVATE);
                }
                JCMethodDecl setMethodDef = TreeMaker.MethodDef(mods, setName, TreeMaker.TypeIdent(TypeTag.VOID), Nil<JCTypeParameter>(), new List<JCVariableDecl>() { paramDef }, Nil<JCExpression>(), setBody, null);
                setMethodDef.docComments = comments;
                trees.Add(setMethodDef);
            }

            return trees;
        }
    }
}

