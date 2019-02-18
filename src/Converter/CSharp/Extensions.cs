using GrapeCity.CodeAnalysis.TypeScript.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
{
    internal static class SyntaxNodeExtensions
    {
        public static T ToCsNode<T>(this Node tsNode)
        {
            Type converter = LangConverter.GetConverter(tsNode);
            if (converter == null)
            {
                Log(string.Format("Cannot find {0} Converter", tsNode.Kind));
                return default(T);
            }

            ConstructorInfo constructorInfo = converter.GetConstructor(Type.EmptyTypes);
            MethodInfo convertMethod = converter.GetMethod("Convert");
            try
            {
                object node = convertMethod.Invoke(constructorInfo.Invoke(Type.EmptyTypes), new object[] { tsNode });

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

        public static List<T> ToCsNodeList<T>(this IEnumerable<Node> nodes)
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
            return ret;
        }

        public static T[] ToCsNodes<T>(this IEnumerable<Node> nodes)
        {
            return nodes.ToCsNodeList<T>().ToArray();
        }


        private static bool CanConvert(Node tsNode)
        {
            switch (tsNode.Kind)
            {
                case NodeKind.LiteralType:
                case NodeKind.DeleteExpression:
                case NodeKind.ForInStatement:
                case NodeKind.ArrayBindingPattern:
                case NodeKind.BindingElement:
                case NodeKind.UnionType:
                case NodeKind.CallSignature:
                case NodeKind.FunctionType:
                case NodeKind.SpreadElement:
                case NodeKind.InKeyword:
                    return false;

                default:
                    return true;
            }
        }

        private static bool IsIgnoreNode(Node tsNode)
        {
            switch (tsNode.Kind)
            {
                case NodeKind.ExportKeyword:
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
}
