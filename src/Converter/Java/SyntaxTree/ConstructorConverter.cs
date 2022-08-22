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
    public class ConstructorConverter : NodeConverter
    {
        public List<JCTree> Convert(Constructor node)
        {
            ClassDeclaration @class = (ClassDeclaration)(node.Parent);
            JCModifiers mods = @class.IsExport ? TreeMaker.Modifiers(Flags.PUBLIC) : TreeMaker.Modifiers(0);
            Name name = Names.fromString(NormalizeTypeName(@class.NameText));

            List<JCTree> methods = new List<JCTree>();
            List<Parameter> parameters = node.Parameters.Select(n => (Parameter)n).ToList();
            // Overriding
            int startIndex = parameters.FindIndex(p => p.IsOptional || p.Initializer != null);
            int endIndex = parameters.FindLastIndex(p => p.IsOptional || p.Initializer != null);
            if (startIndex >= 0) // Has optional parameter
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    Parameter parameter = parameters[i];
                    List<Parameter> methodParams = parameters.GetRange(0, i);

                    JCExpression invokeFn = TreeMaker.Ident(Names.fromString("this"));
                    List<JCExpression> overloadingInvokeParameters = methodParams.Select(p => p.Name.ToJavaSyntaxTree<JCExpression>()).ToList();
                    Node initializer = parameter.Initializer != null ? parameter.Initializer : Parameter.CreateInitializer(parameter.Type);
                    overloadingInvokeParameters.Add(initializer.ToJavaSyntaxTree<JCExpression>());
                    JCMethodInvocation invokeExpression = TreeMaker.Apply(
                        Nil<JCExpression>(),
                        invokeFn,
                        overloadingInvokeParameters
                    );

                    List<JCVariableDecl> overloadingMethodParameters = methodParams.ToJavaSyntaxTrees<JCVariableDecl>();
                    JCBlock overloadingMethodBody = TreeMaker.Block(0, new List<JCStatement>() { TreeMaker.Exec(invokeExpression) });
                    JCMethodDecl overloadingMethodDef = TreeMaker.MethodDef(
                        mods,
                        name,
                        null,
                        Nil<JCTypeParameter>(),
                        overloadingMethodParameters,
                        Nil<JCExpression>(),
                        overloadingMethodBody,
                        null);
                    methods.Add(overloadingMethodDef);
                }
            }

            // Constructor
            List<JCVariableDecl> @params = parameters.ToJavaSyntaxTrees<JCVariableDecl>();
            JCBlock body = node.Body?.ToJavaSyntaxTree<JCBlock>();

            JCMethodDecl methodDef = TreeMaker.MethodDef(mods, name, null, Nil<JCTypeParameter>(), @params, Nil<JCExpression>(), body, null);
            methodDef.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            methods.Add(methodDef);

            return methods;
        }
    }
}

