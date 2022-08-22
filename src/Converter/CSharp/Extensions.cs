using TypeScript.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace TypeScript.Converter.CSharp
{
    #region Document Extension
    internal static class DocumentExtension
    {
        internal static string GetPackageName(this Syntax.Document doc)
        {
            string configNs = CSharpConverter.Current.Context.Namespace;
            if (!string.IsNullOrEmpty(configNs))
            {
                return configNs;
            }

            return doc.RelativePath.Replace(Path.DirectorySeparatorChar, '.');
        }
    }
    #endregion


    #region Syntax Tree Node Extension
    internal static class SyntaxTreeNodeExtension
    {
        /// <summary>
        /// Convert to csharp code.
        /// </summary>
        /// <param name="node">The typescipt node.</param>
        /// <returns>The csharp code.</returns>
        public static string ToCSharp(this Node node)
        {
            CSharpSyntaxNode csNode = node?.ToCsSyntaxTree<CSharpSyntaxNode>();
            if (csNode != null)
            {
                return csNode.NormalizeWhitespace("    ", Environment.NewLine,  false).ToFullString();
            }

            return string.Empty;
        }

        public static T ToCsSyntaxTree<T>(this Node tsNode)
        {
            object result = Convert(tsNode);
            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }

        public static List<T> ToCsSyntaxTrees<T>(this Node node)
        {
            List<T> trees = new List<T>();

            object result = Convert(node);
            if (result is IList list)
            {
                foreach (var item in list)
                {
                    trees.Add((T)item);
                }
            }
            else if (result is T t)
            {
                trees.Add(t);
            }

            return trees;
        }

        public static T[] ToCsSyntaxTrees<T>(this IEnumerable<Node> nodes)
        {
            List<T> ret = new List<T>();
            foreach (Node node in nodes)
            {
                if (IsIgnoreNode(node))
                {
                    continue;
                }

                T csNode = node.ToCsSyntaxTree<T>();
                if (csNode != null)
                {
                    ret.Add(csNode);
                }
            }
            return ret.ToArray();
        }

        private static object Convert(Node node)
        {
            try
            {
                NodeConverter converter = CSharpConverter.Current.CreateConverter(node);
                if (converter == null)
                {
                    LogInfo($"Cannot find {node.Kind} Converter");
                    return null;
                }
                MethodInfo convertMethod = converter.GetType().GetMethod("Convert", new Type[] { node.GetType() });
                object result = convertMethod.Invoke(converter, new object[] { node });

                if (!CanConvert(node))
                {
                    LogWarning($"Cannot convert {node.Kind}: {node.Text}");
                }

                return result;
            }
            catch (TargetInvocationException ex)
            {
                LogError($"Fail to convert {node.Kind}: { node.Text}");
                PrintExecption(ex);
                return null;
            }
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

        private static void PrintExecption(Exception ex)
        {
            Exception e = ex;
            while (e.InnerException != null)
            {
                e = e.InnerException;
            }
            LogInfo($"ERROR: Exception {e.ToString()}");
        }

        private static void LogInfo(string msg)
        {
            Console.WriteLine(msg);
        }
        private static void LogWarning(string msg)
        {
            Console.WriteLine($"WARNING: {msg}");
        }
        private static void LogError(string msg)
        {
            Console.WriteLine($"ERROR: {msg}");
        }
    }
    #endregion
}
