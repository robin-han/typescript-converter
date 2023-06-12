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
    public class ArrayLiteralExpressionConverter : NodeConverter
    {
        public JCTree Convert(ArrayLiteralExpression node)
        {
            if (IsSpreadItem(node))
            {
                // TODO: [...arg]
                // throw new NotSupportedException("Does not support spread item");
                return TreeMaker.Literal(TypeTag.UNINITIALIZED_OBJECT, "XXX" + node.Text + "XXX");
            }

            if (IsEmptyArray(node))
            {
                return CreateArrayWithoutInitializer(node);
            }

            if (IsOneDimensionArray(node))
            {
                return CreateOneDimensionArrayWithInitializer(node);

            }
            if (IsTwoDimensionsArray(node))
            {
                return CreateTwoDimensionsArrayWithInitializer(node);
            }

            throw new NotSupportedException("Does not support multi-dimensions array");
        }

        private bool IsSpreadItem(ArrayLiteralExpression node)
        {
            return node.Elements.Find(e => e.Kind == NodeKind.SpreadElement) != null;
        }

        private bool IsEmptyArray(ArrayLiteralExpression node)
        {
            return node.Elements.Count == 0;
        }

        private bool IsOneDimensionArray(ArrayLiteralExpression node)
        {
            Node elemType = TypeHelper.GetArrayElementType(TypeHelper.TrimType(node.Type));
            return !TypeHelper.IsArrayType(elemType);
        }

        private bool IsTwoDimensionsArray(ArrayLiteralExpression node)
        {
            Node elemType = TypeHelper.GetArrayElementType(TypeHelper.TrimType(node.Type));
            if (TypeHelper.IsArrayType(elemType))
            {
                return !TypeHelper.IsArrayType(TypeHelper.GetArrayElementType(elemType));
            }
            return false;
        }

        private JCTree CreateArrayWithoutInitializer(ArrayLiteralExpression node)
        {
            Node type = TypeHelper.TrimType(node.Type);
            return TreeMaker.NewClass(
                null,
                Nil<JCExpression>(),
                type.ToJavaSyntaxTree<JCExpression>(),
                Nil<JCExpression>(),
                null
             );
        }

        private JCTree CreateOneDimensionArrayWithInitializer(ArrayLiteralExpression node)
        {
            Node type = TypeHelper.TrimType(node.Type);
            Node elemType = TypeHelper.GetArrayElementType(type);

            // ArrayList<Integer> obj = new ArrayList<Integer>(Array.asList(new Integer[] { 10, 20, 30, 40 } ));
            JCNewArray arg = TreeMaker.NewArray(
                elemType.ToJavaSyntaxTree<JCExpression>(),
                Nil<JCExpression>(),
                node.Elements.ToJavaSyntaxTrees<JCExpression>()
            );
            JCFieldAccess fn = TreeMaker.Select(
                TreeMaker.Ident(Names.fromString("Array")),
                Names.fromString("asList")
            );
            List<JCExpression> args = new List<JCExpression>(){
                    TreeMaker.Apply(
                    Nil<JCExpression>(),
                    fn,
                    new List<JCExpression>() { arg })
                };

            return TreeMaker.NewClass(
                null,
                Nil<JCExpression>(),
                type.ToJavaSyntaxTree<JCExpression>(),
                args,
                null
            );
        }

        private JCTree CreateTwoDimensionsArrayWithInitializer(ArrayLiteralExpression node)
        {
            Node type = TypeHelper.TrimType(node.Type);
            Node elemType = TypeHelper.GetArrayElementType(type);
            Node elementElementType = TypeHelper.GetArrayElementType(elemType);

            List<JCStatement> initStats = new List<JCStatement>();
            foreach (Node element in node.Elements)
            {
                // add(new ArrayList<EET>(Arrays.asList(xx.ToArray(new EET[0]));
                //xx.ToArray(new EET[0])
                JCMethodInvocation toArray = ArrayTypeConverter.ToArrayMethodInvocation(element, elementElementType);
                //Arrays.asList(XXX)
                JCMethodInvocation asList = TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Select(
                        TreeMaker.Ident(Names.fromString("Array")),
                        Names.fromString("asList")
                    ),
                    new List<JCExpression>() { toArray }
                );
                // new ArrayList<EET>(XXX)
                JCNewClass addParam = TreeMaker.NewClass(
                    null,
                    Nil<JCExpression>(),
                    TreeMaker.TypeApply(
                        TreeMaker.Ident(Names.fromString("ArrayList")),
                        elementElementType.ToJavaSyntaxTrees<JCExpression>()
                    ),
                    new List<JCExpression>() { asList },
                    null
                );
                //add(XXX)
                JCMethodInvocation add = TreeMaker.Apply(
                    Nil<JCExpression>(),
                    TreeMaker.Ident(Names.fromString("add")),
                    new List<JCExpression>() { addParam }
                 );

                initStats.Add(TreeMaker.Exec(add));
            }

            JCClassDecl def = TreeMaker.ClassDef(
                TreeMaker.Modifiers(0),
                null,
                Nil<JCTypeParameter>(),
                null,
                Nil<JCExpression>(),
                new List<JCTree>() {
                    CreateSerialVersionUID(),
                    TreeMaker.Block(0, initStats)
                }
            );

            return TreeMaker.NewClass(
                null,
                Nil<JCExpression>(),
                type.ToJavaSyntaxTree<JCExpression>(),
                Nil<JCExpression>(),
                def
            );
        }
    }
}
