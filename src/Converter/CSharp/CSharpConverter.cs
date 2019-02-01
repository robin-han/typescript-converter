using GrapeCity.CodeAnalysis.TypeScript.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
{
    public class CSharpConverter
    {
        public string Convert(Node tsNode)
        {
            var csNode = tsNode?.ToCsNode<CSharpSyntaxNode>();
            if (csNode != null)
            {
                return csNode.NormalizeWhitespace().ToFullString();
            }
            return string.Empty;
        }


        internal static Type GetConverter(Node tsNode)
        {
            Type nodeType = tsNode.GetType();
            var converters = GetConverters();
            if (converters.ContainsKey(nodeType))
            {
                return converters[nodeType];
            }
            return null;
        }

        private static Dictionary<Type, Type> _converters;
        private static Dictionary<Type, Type> GetConverters()
        {
            if (_converters != null)
            {
                return _converters;
            }

            _converters = new Dictionary<Type, Type>();
            Type baseType = typeof(Converter);
            Type[] types = Assembly.GetExecutingAssembly().GetExportedTypes();

            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(baseType))
                {
                    continue;
                }

                MethodInfo convererMethod = type.GetMethod("Convert");
                if (convererMethod == null)
                {
                    continue;
                }

                ParameterInfo[] parameters = convererMethod.GetParameters();
                if (parameters != null && parameters.Length == 1)
                {
                    Type paramType = parameters[0].ParameterType;
                    _converters[paramType] = type;
                }
            }
            return _converters;
        }
    }

}
