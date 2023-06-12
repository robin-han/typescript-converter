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
using java.lang.common.api;

namespace TypeScript.Converter.Java
{
    public class ArrowFunctionConverter : NodeConverter
    {
        #region Fields
        // Array
        private const string EVERY_CALLBACK = "IEveryCallback";
        private const string FILTER_CALLBACK = "IFilterCallback";
        private const string FOREACH_CALLBACK = "IForEachCallback";
        private const string MAP_CALLBACK = "IMapCallback";
        private const string REDUCE_CALLBACK = "IReduceCallback";
        private const string SOME_CALLBACK = "ISomeCallback";
        private const string SORT_CALLBACK = "ISortCallback";

        // String
        private const string REPLACE_CALLBACK = "IReplaceCallback";

        private static readonly Dictionary<string, string> DEFAULT_CALLBACK_NAMES = new Dictionary<string, string>()
        {
            { "every",    EVERY_CALLBACK },
            { "filter",   FILTER_CALLBACK },
            { "forEach",  FOREACH_CALLBACK },
            { "map",      MAP_CALLBACK },
            { "reduce",   REDUCE_CALLBACK },
            { "some",     SOME_CALLBACK },
            { "sort",     SORT_CALLBACK },

            { "replace",  REPLACE_CALLBACK }
        };
        #endregion

        public JCTree Convert(ArrowFunction node)
        {
            return CreateAnonymousInnerClass(node);
        }

        /// <summary>
        /// Creates anonymous inner class.
        /// </summary>
        /// <param name="arrowFn"></param>
        /// <returns></returns>
        private JCTree CreateAnonymousInnerClass(ArrowFunction arrowFn)
        {
            JCExpression returnType = GetInvokeReturnType(arrowFn);
            JCBlock body;
            if (arrowFn.Body.Kind == NodeKind.Block)
            {
                body = arrowFn.Body.ToJavaSyntaxTree<JCBlock>();
            }
            else if (returnType is PrimitiveTypeTree primitiveType && primitiveType.getPrimitiveTypeKind() == TypeKind.VOID)
            {
                body = TreeMaker.Block(0, new List<JCStatement>() { TreeMaker.Exec(arrowFn.Body.ToJavaSyntaxTree<JCExpression>()) });
            }
            else
            {
                body = TreeMaker.Block(0, new List<JCStatement>() { TreeMaker.Return(arrowFn.Body.ToJavaSyntaxTree<JCExpression>()) });
            }

            // invoke method
            JCMethodDecl methodDef = TreeMaker.MethodDef(
                TreeMaker.Modifiers(Flags.PUBLIC),
                Names.fromString("invoke"),
                returnType,
                Nil<JCTypeParameter>(),
                GetInvokeParameters(arrowFn),
                Nil<JCExpression>(),
                body,
                null
           );

            // anonymous class def
            JCClassDecl classDef = TreeMaker.ClassDef(
               TreeMaker.Modifiers(0),
               null,
               Nil<JCTypeParameter>(),
               null,
               Nil<JCExpression>(),
               new List<JCTree>() { methodDef }
            );

            return TreeMaker.SpeculativeNewClass(
               null,
               Nil<JCExpression>(),
               GetNewClassExpression(arrowFn),
               Nil<JCExpression>(),
               classDef,
               true);
        }

        /// <summary>
        /// Gets new class expression.
        /// </summary>
        /// <param name="arrowFn"></param>
        /// <returns></returns>
        private JCExpression GetNewClassExpression(ArrowFunction arrowFn)
        {
            List<Parameter> parameters = arrowFn.Parameters.Select(p => (Parameter)p).ToList();
            string fnName = GetAnonymousInnerClassName(arrowFn);
            JCExpression clazz = TreeMaker.Ident(Names.fromString(NormalizeTypeName(fnName)));
            JCExpression fnType = (arrowFn.Type == null ? TreeMaker.TypeIdent(TypeTag.VOID) : CreateGenericTypeParameter(arrowFn.Type));
            if (parameters.Count > 0)
            {
                if (fnName == EVERY_CALLBACK
                || fnName == FILTER_CALLBACK
                || fnName == FOREACH_CALLBACK
                || fnName == SOME_CALLBACK
                || fnName == SORT_CALLBACK)
                {
                    clazz = TreeMaker.TypeApply(
                        clazz,
                        new List<JCExpression>()
                        {
                            CreateGenericTypeParameter(parameters[0].Type)
                        }
                    );
                }
                else if (fnName == MAP_CALLBACK)
                {
                    clazz = TreeMaker.TypeApply(
                        clazz,
                        new List<JCExpression>()
                        {
                            CreateGenericTypeParameter(parameters[0].Type),
                            fnType
                        }
                    );
                }
                else if (fnName == REDUCE_CALLBACK)
                {
                    clazz = TreeMaker.TypeApply(
                        clazz,
                        new List<JCExpression>()
                        {
                            CreateGenericTypeParameter(parameters[1].Type),
                            CreateGenericTypeParameter(parameters[0].Type)
                        }
                    );
                }
                else
                {
                    Node typeDeclaration = arrowFn.Document.Project.GetTypeDeclaration(fnName);
                    if (IsGenericTypeDeclaraion(typeDeclaration))
                    {
                        clazz = TreeMaker.TypeApply(
                        clazz,
                        parameters.Select(p => CreateGenericTypeParameter(p.Type)).ToList()
                        );
                    }
                }
            }
            return clazz;
        }

        /// <summary>
        /// Gets invoke method return type.
        /// </summary>
        /// <param name="arrowFn"></param>
        /// <returns></returns>
        private JCExpression GetInvokeReturnType(ArrowFunction arrowFn)
        {
            JCExpression returnType;

            string fnName = GetAnonymousInnerClassName(arrowFn);
            if (arrowFn.Type == null)
            {
                if (fnName == SORT_CALLBACK)
                {
                    returnType = TreeMaker.TypeIdent(TypeTag.DOUBLE);
                }
                else if (fnName == SOME_CALLBACK || fnName == FILTER_CALLBACK || fnName == EVERY_CALLBACK)
                {
                    returnType = TreeMaker.TypeIdent(TypeTag.BOOLEAN);
                }
                else
                {
                    returnType = TreeMaker.TypeIdent(TypeTag.VOID);
                }
            }
            else if (fnName == MAP_CALLBACK || fnName == REDUCE_CALLBACK)
            {
                returnType = CreateGenericTypeParameter(arrowFn.Type);
            }
            else
            {
                returnType = arrowFn.Type.ToJavaSyntaxTree<JCExpression>();
            }

            return returnType;
        }

        /// <summary>
        /// Gets invoke method parameters.
        /// </summary>
        /// <param name="arrowFn"></param>
        /// <returns></returns>
        private List<JCVariableDecl> GetInvokeParameters(ArrowFunction arrowFn)
        {
            List<Parameter> parameters = arrowFn.Parameters.Select(p => (Parameter)p).ToList();
            string fnName = GetAnonymousInnerClassName(arrowFn);

            List<JCVariableDecl> @params = parameters.ToJavaSyntaxTrees<JCVariableDecl>();

            // public void invoke(E e, int index);
            if (fnName == EVERY_CALLBACK
                || fnName == FILTER_CALLBACK
                || fnName == FOREACH_CALLBACK
                || fnName == MAP_CALLBACK
                || fnName == SOME_CALLBACK)
            {
                if (parameters.Count == 1)
                {
                    if (IsPrimitiveType(parameters[0].Type))
                    {
                        @params[0].vartype = CreateGenericTypeParameter(parameters[0].Type);
                    }
                    //autocomplete seconds parameter or change its type to int
                    @params.Add(TreeMaker.VarDef(
                        TreeMaker.Modifiers(0),
                        Names.fromString("index"),
                        TreeMaker.TypeIdent(TypeTag.INT),
                        null)
                    );
                }
                else if (parameters.Count == 2)
                {
                    @params[1].vartype = TreeMaker.TypeIdent(TypeTag.INT);
                }
            }
            // public U invoke(U previousValue, E currentValue, int index);
            else if (fnName == REDUCE_CALLBACK)
            {
                if (IsPrimitiveType(parameters[0].Type))
                {
                    @params[0].vartype = CreateGenericTypeParameter(parameters[0].Type);
                }
                if (IsPrimitiveType(parameters[1].Type))
                {
                    @params[1].vartype = CreateGenericTypeParameter(parameters[1].Type);
                }

                //autocomplete seconds parameter or change its type to int
                if (parameters.Count == 2)
                {
                    @params.Add(TreeMaker.VarDef(
                        TreeMaker.Modifiers(0),
                        Names.fromString("index"),
                        TreeMaker.TypeIdent(TypeTag.INT),
                        null)
                    );
                }
                else if (parameters.Count == 3)
                {
                    @params[2].vartype = TreeMaker.TypeIdent(TypeTag.INT);
                }
            }
            // public String invoke(String subString, String arg1, String arg2);
            else if (fnName == REPLACE_CALLBACK)
            {
                if (parameters.Count == 2)
                {
                    @params.Add(TreeMaker.VarDef(
                        TreeMaker.Modifiers(0),
                        Names.fromString("arg2"),
                        TreeMaker.Ident(Names.fromString("String")),
                        null)
                    );
                }
            }
            // public double invoke(E e1, E e2);
            else if (fnName == SORT_CALLBACK)
            {
                if (IsPrimitiveType(parameters[0].Type))
                {
                    @params[0].vartype = CreateGenericTypeParameter(parameters[0].Type);
                }
                if (IsPrimitiveType(parameters[1].Type))
                {
                    @params[1].vartype = CreateGenericTypeParameter(parameters[1].Type);
                }
            }

            return @params;
        }

        private bool IsGenericTypeDeclaraion(Node typeDeclaration)
        {
            if (typeDeclaration != null && typeDeclaration.GetValue("TypeParameters") is List<Node> typeParams)
            {
                return typeParams.Count > 0;
            }
            return false;
        }

        private bool IsPrimitiveType(Node type)
        {
            return TypeHelper.IsNumberType(type) || TypeHelper.IsBoolType(type);
        }

        private JCExpression CreateGenericTypeParameter(Node type)
        {
            if (TypeHelper.IsIntType(type))
            {
                return TreeMaker.Ident(Names.fromString("Integer"));
            }
            if (TypeHelper.IsNumberType(type))
            {
                return TreeMaker.Ident(Names.fromString("Double"));
            }
            if (TypeHelper.IsBoolType(type))
            {
                return TreeMaker.Ident(Names.fromString("Boolean"));
            }
            return type.ToJavaSyntaxTree<JCExpression>();
        }

        /// <summary>
        /// Gets arrow function name.
        /// </summary>
        /// <param name="arrowFn"></param>
        /// <returns></returns>
        private string GetAnonymousInnerClassName(ArrowFunction arrowFn)
        {
            Node type = TypeHelper.GetDeclarationType(arrowFn);
            if (type != null)
            {
                return TypeHelper.TrimType(type).Text;
            }

            Node parent = arrowFn.Parent;
            while (parent.Kind == NodeKind.ParenthesizedExpression)
            {
                parent = parent.Parent;
            }

            if (parent.Kind == NodeKind.CallExpression)
            {
                CallExpression callExpr = (CallExpression)parent;
                if (callExpr.Expression is PropertyAccessExpression propAccess && DEFAULT_CALLBACK_NAMES.ContainsKey(propAccess.Name.Text))
                {
                    return DEFAULT_CALLBACK_NAMES[propAccess.Name.Text];
                }
            }

            return "ArrowFnXXXXXX";
        }
    }
}

