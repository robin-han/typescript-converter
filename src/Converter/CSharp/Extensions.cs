using TypeScript.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace TypeScript.Converter.CSharp
{
    #region Document Extension
    internal static class DocumentExtension
    {
        internal static string GetPackageName(this Syntax.Document doc)
        {
            string ns1 = ConverterContext.Current.Config.Namespace;
            string ns2 = doc.RelativePath.Replace("\\", ".");
            if (!string.IsNullOrEmpty(ns1) && !string.IsNullOrEmpty(ns2))
            {
                return ns1 + "." + ns2;
            }
            else if (!string.IsNullOrEmpty(ns1))
            {
                return ns1;
            }
            else if (!string.IsNullOrEmpty(ns2))
            {
                return ns2;
            }
            else
            {
                return string.Empty;
            }
        }
    }
    #endregion

    #region SyntaxNode
    public static class SyntaxNodeExtension
    {
        /// <summary>
        /// Convert to csharp code.
        /// </summary>
        /// <param name="node">The typescipt node.</param>
        /// <returns>The csharp code.</returns>
        public static string ToCSharp(this Node node)
        {
            CSharpSyntaxNode csNode = node?.ToCsNode<CSharpSyntaxNode>();
            if (csNode != null)
            {
                return csNode.NormalizeWhitespace().ToFullString();
            }

            return string.Empty;
        }

        internal static T ToCsNode<T>(this Node tsNode)
        {
            Converter converter = ConverterContext.Current.CreateConverter(tsNode);
            if (converter == null)
            {
                Log(string.Format("Cannot find {0} Converter", tsNode.Kind));
                return default(T);
            }

            try
            {
                MethodInfo convertMethod = converter.GetType().GetMethod("Convert", new Type[] { tsNode.GetType() });
                object node = convertMethod.Invoke(converter, new object[] { tsNode });

                if (!CanConvert(tsNode))
                {
                    CannotConvert(tsNode);
                }

                if (node is T)
                {
                    return (T)node;
                }
                return default(T);
            }
            catch (TargetInvocationException ex)
            {
                PrintExecption(ex);
                FailToConvert(tsNode);

                return default(T);
            }
        }

        internal static T[] ToCsNodes<T>(this IEnumerable<Node> nodes)
        {
            List<T> ret = new List<T>();
            foreach (Node node in nodes)
            {
                if (IsIgnoreNode(node))
                {
                    continue;
                }

                T csNode = node.ToCsNode<T>();
                if (csNode != null)
                {
                    ret.Add(csNode);
                }
            }
            return ret.ToArray();
        }

        private static bool CanConvert(Node node)
        {
            switch (node.Kind)
            {
                //case NodeKind.UnionType:
                case NodeKind.LiteralType:
                case NodeKind.ForInStatement:
                case NodeKind.ArrayBindingPattern:
                case NodeKind.BindingElement:
                case NodeKind.CallSignature:
                case NodeKind.FunctionType:
                case NodeKind.InKeyword:
                    return false;

                case NodeKind.DeleteExpression:
                    return (node as DeleteExpression).Expression.Kind == NodeKind.ElementAccessExpression;

                case NodeKind.SpreadElement:
                    return (node.Parent != null && node.Parent.Kind == NodeKind.CallExpression);

                default:
                    return true;
            }
        }

        private static bool IsIgnoreNode(Node tsNode)
        {
            switch (tsNode.Kind)
            {
                case NodeKind.DefaultKeyword:
                    return true;

                default:
                    return false;
            }
        }

        private static void CannotConvert(Node node)
        {
            Log(string.Format("WARING: Cannot convert {0}: {1}", node.Kind, node.Text));
        }

        private static void FailToConvert(Node node)
        {
            Log(string.Format("ERROR: Fail to convert {0}: {1}", node.Kind, node.Text));
        }

        private static void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        [Conditional("DEBUG")]
        private static void PrintExecption(Exception ex)
        {
            Exception e = ex;
            while (e.InnerException != null)
            {
                e = e.InnerException;
            }
            Log(string.Format("ERROR: Exception {0}", e.ToString()));
        }

    }
    #endregion
}
