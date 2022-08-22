using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TypeScript.Syntax;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;
using System.Linq;

namespace TypeScript.Converter.Java
{
    public class MethodDeclarationConverter : NodeConverter
    {
        public List<JCTree> Convert(MethodDeclaration node)
        {
            List<JCTree> methods = new List<JCTree>();
            methods.AddRange(CreateOverrideMethods(node));

            List<Node> modifiers = node.Modifiers.FindAll(m => m.Kind != NodeKind.OverrideKeyword);
            Node @override = node.Modifiers.Find(m => m.Kind == NodeKind.OverrideKeyword);

            //
            JCModifiers mods = TreeMaker.Modifiers(modifiers.ToFlags(), @override == null ? Nil<JCAnnotation>() : @override.ToJavaSyntaxTrees<JCAnnotation>());
            JCExpression type = node.Type.ToJavaSyntaxTree<JCExpression>();
            Name name = Names.fromString(node.Name.Text);
            List<JCTypeParameter> typeParams = node.TypeParameters.ToJavaSyntaxTrees<JCTypeParameter>();
            List<JCVariableDecl> @params = node.Parameters.ToJavaSyntaxTrees<JCVariableDecl>();
            JCBlock body = node.Body?.ToJavaSyntaxTree<JCBlock>();

            JCMethodDecl methodDef = TreeMaker.MethodDef(mods, name, type, typeParams, @params, Nil<JCExpression>(), body, null);
            methodDef.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            methods.Add(methodDef);

            return methods;
        }

        private List<JCTree> CreateOverrideMethods(MethodDeclaration node)
        {
            List<JCTree> methods = new List<JCTree>();

            List<Parameter> parameters = node.Parameters.Select(n => (Parameter)n).ToList();
            int startIndex = parameters.FindIndex(p => p.IsOptional || p.Initializer != null);
            int endIndex = parameters.FindLastIndex(p => p.IsOptional || p.Initializer != null);
            // Has optional parameter and not abstract
            if (startIndex >= 0 && !node.IsAbstract)
            {
                List<Node> modifiers = node.Modifiers.FindAll(m => m.Kind != NodeKind.OverrideKeyword);
                JCModifiers mods = TreeMaker.Modifiers(modifiers.ToFlags());
                JCExpression type = node.Type.ToJavaSyntaxTree<JCExpression>();
                Name name = Names.fromString(node.Name.Text);
                List<JCTypeParameter> typeParams = node.TypeParameters.ToJavaSyntaxTrees<JCTypeParameter>();

                for (int i = startIndex; i <= endIndex; i++)
                {
                    Parameter parameter = parameters[i];
                    List<Parameter> methodParams = parameters.GetRange(0, i);

                    //
                    JCExpression invokeFn = node.Name.ToJavaSyntaxTree<JCExpression>();
                    List<JCExpression> overloadingInvokeParameters = methodParams.Select(p => p.Name.ToJavaSyntaxTree<JCExpression>()).ToList();
                    Node initializer = parameter.Initializer != null ? parameter.Initializer : Parameter.CreateInitializer(parameter.Type);
                    overloadingInvokeParameters.Add(initializer.ToJavaSyntaxTree<JCExpression>());
                    JCExpression invokeExpression = TreeMaker.Apply(
                        Nil<JCExpression>(),
                        invokeFn,
                        overloadingInvokeParameters
                    );
                    JCStatement invokeStatement =
                          (node.Type.Kind == NodeKind.VoidKeyword)
                        ? (JCStatement)TreeMaker.Exec(invokeExpression)
                        : (JCStatement)TreeMaker.Return(invokeExpression);
                    //
                    List<JCVariableDecl> overloadingMethodParameters = methodParams.ToJavaSyntaxTrees<JCVariableDecl>();
                    JCBlock overloadingMethodBody = TreeMaker.Block(0, new List<JCStatement>() { invokeStatement });
                    //
                    JCMethodDecl overloadingMethodDef = TreeMaker.MethodDef(
                        mods,
                        name,
                        type,
                        typeParams,
                        overloadingMethodParameters,
                        Nil<JCExpression>(),
                        overloadingMethodBody,
                        null);
                    methods.Add(overloadingMethodDef);
                }
            }

            return methods;
        }
    }
}

