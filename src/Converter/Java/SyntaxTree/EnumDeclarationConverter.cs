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
    internal class EnumNames
    {
        public static readonly Name ENUM_VALUE_NAME = NodeConverter.Names.fromString("value");
    }

    public class EnumDeclarationConverter : NodeConverter
    {
        public JCTree Convert(EnumDeclaration node)
        {
            if (IsAssignValueEnum(node))
            {
                return CreateAssignValueEnum(node);
            }

            JCModifiers modifiers = GetModifiers(node);
            Name name = Names.fromString(NormalizeTypeName(node.NameText));
            List<JCTree> defs = node.Members.ToJavaSyntaxTrees<JCTree>();

            var enumDef = TreeMaker.ClassDef(modifiers, name, Nil<JCTypeParameter>(), null, Nil<JCExpression>(), Nil<JCExpression>(), defs);
            enumDef.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            return enumDef;
        }

        private JCModifiers GetModifiers(EnumDeclaration node)
        {
            JCModifiers modifiers = TreeMaker.Modifiers(Flags.ENUM | node.Modifiers.ToFlags());
            if (node.IsExport)
            {
                modifiers.flags |= Flags.PUBLIC;
            }
            return modifiers;
        }

        private bool IsAssignValueEnum(EnumDeclaration node)
        {
            foreach (EnumMember member in node.Members)
            {
                if (member.Initializer != null)
                {
                    return true;
                }
            }
            return false;
        }

        /** Create enum declaration. May be need to implement a interface to get its value.
         * Such as:
            enum Color {
                RED(1), GREEN(2), BLUE(3);
        
                private final int value;

                private Color(int levelCode) {
                    this.value = levelCode;
                }

                public final int value() {
                    return this.value;
                }
            }
         */
        private JCClassDecl CreateAssignValueEnum(EnumDeclaration node)
        {
            JCModifiers modifiers = GetModifiers(node);
            Name name = Names.fromString(NormalizeTypeName(node.NameText));
            List<JCTree> defs = new List<JCTree>();

            for (int i = 0; i < node.Members.Count; i++)
            {
                EnumMember member = (EnumMember)node.Members[i];
                Name memberName = Names.fromString(member.Name.Text);

                JCExpression value = member.Initializer != null
                    ? member.Initializer.ToJavaSyntaxTree<JCExpression>()
                    : TreeMaker.Literal(TypeTag.INT, i);

                JCExpression init = TreeMaker.NewClass(
                    null,
                    Nil<JCExpression>(),
                    TreeMaker.Ident(memberName),
                    new List<JCExpression>() { value },
                    null
                 );

                JCVariableDecl memberDef = TreeMaker.VarDef(TreeMaker.Modifiers(Flags.ENUM), memberName, null, init);
                memberDef.docComments = member.JsDoc.Count > 0 ? member.JsDoc[0].Text : null;
                defs.Add(memberDef);
            }

            // private final int value
            JCVariableDecl property = TreeMaker.VarDef(
                TreeMaker.Modifiers(Flags.PRIVATE | Flags.FINAL),
                EnumNames.ENUM_VALUE_NAME,
                TreeMaker.TypeIdent(TypeTag.INT),
                null);
            defs.Add(property);

            // constructor
            JCMethodDecl ctorDef = TreeMaker.MethodDef(
                TreeMaker.Modifiers(Flags.PRIVATE),
                name,
                null,
                Nil<JCTypeParameter>(),
                new List<JCVariableDecl>() { TreeMaker.VarDef(TreeMaker.Modifiers(0), EnumNames.ENUM_VALUE_NAME, TreeMaker.TypeIdent(TypeTag.INT), null) },
                Nil<JCExpression>(),
                TreeMaker.Block(0, new List<JCStatement>() {
                    TreeMaker.Exec(TreeMaker.Assign(
                        TreeMaker.Select(TreeMaker.Ident(Names.fromString("this")), EnumNames.ENUM_VALUE_NAME),
                        TreeMaker.Ident(EnumNames.ENUM_VALUE_NAME)
                     ))
                }),
                null);
            defs.Add(ctorDef);

            // public int getValue
            JCMethodDecl geMethodDef = TreeMaker.MethodDef(
                TreeMaker.Modifiers(Flags.PUBLIC | Flags.FINAL),
                EnumNames.ENUM_VALUE_NAME,
                TreeMaker.TypeIdent(TypeTag.INT),
                Nil<JCTypeParameter>(),
                Nil<JCVariableDecl>(),
                Nil<JCExpression>(),
                TreeMaker.Block(0, new List<JCStatement> { TreeMaker.Return(TreeMaker.Select(TreeMaker.Ident(Names.fromString("this")), Names.fromString("value"))) }),
                null);
            defs.Add(geMethodDef);

            var enumDef = TreeMaker.ClassDef(modifiers, name, Nil<JCTypeParameter>(), null, Nil<JCExpression>(), Nil<JCExpression>(), defs);
            enumDef.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            return enumDef;
        }
    }

}

