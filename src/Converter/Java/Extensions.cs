using com.sun.tools.javac.code;
using com.sun.tools.javac.tree;
using TypeScript.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace TypeScript.Converter.Java
{
    #region Document Extension
    internal static class DocumentExtension
    {
        public static string GetPackageName(this Syntax.Document doc)
        {
            var convertContext = JavaConverter.Current.Context;
            string configNs = convertContext.Namespace;
            if (!string.IsNullOrEmpty(configNs))
            {
                return configNs;
            }

            string groupId = convertContext.Project?.GroupId;
            string path = doc.RelativePath.Replace(Path.DirectorySeparatorChar, '.');
            if (!string.IsNullOrEmpty(groupId))
            {
                return string.Join('.', groupId, path);
            }
            return path;
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
        public static string ToJava(this Node node)
        {
            JCTree csNode = node?.ToJavaSyntaxTree<JCTree>();
            if (csNode != null)
            {
                return csNode.ToString();
            }

            return string.Empty;
        }

        public static T ToJavaSyntaxTree<T>(this Node node)
        {
            object result = Convert(node);
            if (result != null)
            {
                if (result is T t)
                {
                    return t;
                }
                if (result is IList list)
                {
                    return (T)list[0];
                }
                return (T)result;
            }
            return default(T);
        }

        public static List<T> ToJavaSyntaxTrees<T>(this Node node)
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

        public static List<T> ToJavaSyntaxTrees<T>(this IEnumerable<Node> nodes)
        {
            List<T> trees = new List<T>();

            foreach (Node node in nodes)
            {
                if (IsIgnoreNode(node))
                {
                    continue;
                }

                object result = Convert(node);
                if (result is T t)
                {
                    trees.Add(t);
                }
                else if (result is IList list)
                {
                    foreach (var item in list)
                    {
                        trees.Add((T)item);
                    }
                }
            }

            return trees;
        }

        public static int ToFlags(this IEnumerable<Node> modifiers)
        {
            int flags = 0;

            foreach (Node modifer in modifiers)
            {
                switch (modifer.Kind)
                {
                    case NodeKind.PublicKeyword:
                        flags |= Flags.PUBLIC;
                        break;

                    case NodeKind.PrivateKeyword:
                        flags |= Flags.PRIVATE;
                        break;

                    case NodeKind.ProtectedKeyword:
                        flags |= Flags.PROTECTED;
                        break;

                    case NodeKind.StaticKeyword:
                        flags |= Flags.STATIC;
                        break;

                    case NodeKind.ReadonlyKeyword:
                        flags |= Flags.FINAL;
                        break;

                    case NodeKind.AbstractKeyword:
                        flags |= Flags.ABSTRACT;
                        break;

                    default:
                        break;
                }
            }
            return flags;
        }

        private static object Convert(Node node)
        {
            try
            {
                NodeConverter converter = JavaConverter.Current.CreateConverter(node);
                if (converter == null)
                {
                    LogWarning($"Cannot find {node.Kind} Converter");
                    return null;
                }

                MethodInfo convertMethod = converter.GetType().GetMethod("Convert", new System.Type[] { node.GetType() });
                return convertMethod.Invoke(converter, new object[] { node });
            }
            catch (TargetInvocationException ex)
            {
                LogError($"Cannot convert {node.Kind}: {node.Text}");
                PrintExecption(ex);
                return null;
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
            LogInfo($"Exception: {e.ToString()}");
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


    #region MethodName Extension
    internal static class MethodNameExtension
    {
        public static string ToGetMethodName(this string name)
        {
            return "get" + char.ToUpper(name[0]) + name.Substring(1);
        }

        public static string ToSetMethodName(this string name)
        {
            return "set" + char.ToUpper(name[0]) + name.Substring(1);
        }
    }
    #endregion
}
