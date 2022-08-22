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
    public class DecoratorConverter : NodeConverter
    {
        public JCTree Convert(Decorator node)
        {
            // @log
            if (node.Expression.Kind == NodeKind.Identifier)
            {
                return TreeMaker.Annotation(TreeMaker.Ident(Names.fromString(TypeHelper.GetTypeName(node.Expression))), Nil<JCExpression>());
            }

            // @log(xxx, xxx)
            if (node.Expression.Kind == NodeKind.CallExpression)
            {
                CallExpression callExpr = (CallExpression)node.Expression;

                bool hasObjectVariableParameter = false;
                List<Node> parameters = TypeHelper.GetParameters(callExpr);
                if (parameters.Count > 0)
                {
                    Parameter parameter = (Parameter)parameters[parameters.Count - 1];
                    if (parameter.IsVariable)
                    {
                        Node variableType = parameter.VariableType;
                        hasObjectVariableParameter = (variableType.Kind == NodeKind.AnyKeyword || variableType.Kind == NodeKind.ObjectKeyword);
                    }
                }
                //
                List<Node> arguments;
                List<Node> variableArguments;
                if (hasObjectVariableParameter)
                {
                    int splitIndex = parameters.Count - 1;
                    arguments = callExpr.Arguments.GetRange(0, splitIndex);
                    variableArguments = callExpr.Arguments.GetRange(splitIndex, callExpr.Arguments.Count - splitIndex);
                }
                else
                {
                    arguments = callExpr.Arguments;
                    variableArguments = new List<Node>();
                }
                //
                List<JCExpression> annnotationArguments = new List<JCExpression>();
                foreach (var argument in arguments)
                {
                    if (argument.Kind == NodeKind.Identifier)
                    {
                        annnotationArguments.Add(TreeMaker.Select(argument.ToJavaSyntaxTree<JCExpression>(), Names.fromString("class")));
                    }
                    else
                    {
                        annnotationArguments.Add(argument.ToJavaSyntaxTree<JCExpression>());
                    }
                }
                JCExpression annoVariableArgument = this.ConvertVariableParameters(variableArguments);
                if (annoVariableArgument != null)
                {
                    annnotationArguments.Add(annoVariableArgument);
                }

                return TreeMaker.Annotation(
                    TreeMaker.Ident(Names.fromString(TypeHelper.GetTypeName(callExpr.Expression))),
                    annnotationArguments
                );
            }

            return null;
        }

        private JCExpression ConvertVariableParameters(List<Node> arguments)
        {
            List<JCExpression> annnotationArguments = new List<JCExpression>();
            foreach (var arg in arguments)
            {
                switch (arg.Kind)
                {
                    case NodeKind.NullKeyword:
                        annnotationArguments.Add(TreeMaker.Annotation(TreeMaker.Ident(Names.fromString("AnnotationParameter")),
                            new List<JCExpression>() {
                                TreeMaker.Assign(TreeMaker.Ident(Names.fromString("type")), TreeMaker.Literal(TypeTag.NONE, "null"))
                            }
                         ));
                        break;
                    case NodeKind.NumericLiteral:
                        annnotationArguments.Add(TreeMaker.Annotation(TreeMaker.Ident(Names.fromString("AnnotationParameter")),
                           new List<JCExpression>() {
                                TreeMaker.Assign(TreeMaker.Ident(Names.fromString("type")), TreeMaker.Literal(TypeTag.NONE, "number")),
                                TreeMaker.Assign(TreeMaker.Ident(Names.fromString("numberValue")), arg.ToJavaSyntaxTree<JCExpression>())
                           }
                        ));
                        break;

                    case NodeKind.StringLiteral:
                        annnotationArguments.Add(TreeMaker.Annotation(TreeMaker.Ident(Names.fromString("AnnotationParameter")),
                            new List<JCExpression>() {
                                TreeMaker.Assign(TreeMaker.Ident(Names.fromString("type")), TreeMaker.Literal(TypeTag.NONE, "string")),
                                TreeMaker.Assign(TreeMaker.Ident(Names.fromString("stringValue")), arg.ToJavaSyntaxTree<JCExpression>())
                            }
                        ));
                        break;

                    case NodeKind.TrueKeyword:
                    case NodeKind.FalseKeyword:
                        annnotationArguments.Add(TreeMaker.Annotation(TreeMaker.Ident(Names.fromString("AnnotationParameter")),
                            new List<JCExpression>() {
                                TreeMaker.Assign(TreeMaker.Ident(Names.fromString("type")), TreeMaker.Literal(TypeTag.NONE, "boolean")),
                                TreeMaker.Assign(TreeMaker.Ident(Names.fromString("booleanValue")), arg.ToJavaSyntaxTree<JCExpression>())
                            }
                        ));
                        break;

                    case NodeKind.Identifier:
                        annnotationArguments.Add(TreeMaker.Annotation(TreeMaker.Ident(Names.fromString("AnnotationParameter")),
                           new List<JCExpression>() {
                                TreeMaker.Assign(TreeMaker.Ident(Names.fromString("type")), TreeMaker.Literal(TypeTag.NONE, "class")),
                                TreeMaker.Assign(TreeMaker.Ident(Names.fromString("classValue")), TreeMaker.Select(arg.ToJavaSyntaxTree<JCExpression>(), Names.fromString("class")))
                           }
                        ));
                        break;

                    case NodeKind.PropertyAccessExpression:
                        PropertyAccessExpression prop = (PropertyAccessExpression)arg;
                        string enumName = prop.Name.Text;
                        string enumType = TypeHelper.GetTypeName(prop.Expression);
                        annnotationArguments.Add(TreeMaker.Annotation(TreeMaker.Ident(Names.fromString("AnnotationParameter")),
                          new List<JCExpression>() {
                               TreeMaker.Assign(TreeMaker.Ident(Names.fromString("type")), TreeMaker.Literal(TypeTag.NONE, "enum")),
                               TreeMaker.Assign(TreeMaker.Ident(Names.fromString("enumValue")),
                                    TreeMaker.Annotation(TreeMaker.Ident(Names.fromString("AnnotationEnumParameter")), new List<JCExpression>()
                                    {
                                        TreeMaker.Assign(TreeMaker.Ident(Names.fromString("type")), TreeMaker.Select(TreeMaker.Ident(Names.fromString(enumType)), Names.fromString("class"))),
                                        TreeMaker.Assign(TreeMaker.Ident(Names.fromString("name")), TreeMaker.Literal(TypeTag.NONE, enumName))
                                    })
                               )
                          }
                       ));
                        break;

                    default:
                        break;
                }
            }

            return (annnotationArguments.Count > 0 ? TreeMaker.NewArray(null, Nil<JCExpression>(), annnotationArguments) : null);
        }
    }
}
