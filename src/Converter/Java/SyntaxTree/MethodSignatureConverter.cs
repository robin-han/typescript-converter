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
    public class MethodSignatureConverter : NodeConverter
    {
        public List<JCTree> Convert(MethodSignature node)
        {
            JCModifiers mods = TreeMaker.Modifiers(0);
            JCExpression type = node.Type.ToJavaSyntaxTree<JCExpression>();
            Name name = Names.fromString(node.Name.Text);
            List<JCTypeParameter> typeParams = node.TypeParameters.ToJavaSyntaxTrees<JCTypeParameter>();

            List<JCTree> methods = new List<JCTree>();
            List<Parameter> parameters = node.Parameters.Select(n => (Parameter)n).ToList();
            // Overriding
            int startIndex = parameters.FindIndex(p => p.IsOptional || p.Initializer != null);
            int endIndex = parameters.FindLastIndex(p => p.IsOptional || p.Initializer != null);
            if (startIndex >= 0) // Has optional parameter
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    List<Parameter> methodParams = parameters.GetRange(0, i);

                    List<JCVariableDecl> overloadingMethodParameters = methodParams.ToJavaSyntaxTrees<JCVariableDecl>();
                    JCMethodDecl overloadingMethodDef = TreeMaker.MethodDef(
                        mods,
                        name,
                        type,
                        typeParams,
                        overloadingMethodParameters,
                        Nil<JCExpression>(),
                        null,
                        null);
                    methods.Add(overloadingMethodDef);
                }
            }

            // Methods
            List<JCVariableDecl> @params = parameters.ToJavaSyntaxTrees<JCVariableDecl>();
            JCMethodDecl methodDef = TreeMaker.MethodDef(mods, name, type, typeParams, @params, Nil<JCExpression>(), null, null);
            methodDef.docComments = node.JsDoc.Count > 0 ? node.JsDoc[0].Text : null;
            methods.Add(methodDef);

            return methods;
        }
    }
}

