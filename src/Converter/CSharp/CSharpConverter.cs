using TypeScript.Syntax;
using TypeScript.Syntax.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Project = TypeScript.Syntax.Project;

namespace TypeScript.Converter.CSharp
{
    public class CSharpConverter
    {
        #region Fields
        private readonly ConverterContext _context;
        #endregion

        #region Constructor
        public CSharpConverter(ConverterContext context)
        {
            this._context = context;
        }
        #endregion

        #region Properties
        public ConverterContext Context
        {
            get
            {
                return this._context;
            }
        }

        public static ConverterContext CurrentContext
        {
            get;
            internal set;
        }
        #endregion

        #region Public Methods
        public void Analyze(List<Node> nodes)
        {
            foreach (Type type in this.Context.Project.GetAnalyzerTypes())
            {
                foreach (Node node in nodes)
                {
                    Analyzer analyzer = type.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes) as Analyzer;
                    analyzer.Analyze(node);
                }
            }
        }

        public string Convert(Node tsNode)
        {
            CurrentContext = this.Context;
            CSharpSyntaxNode csNode = tsNode?.ToCsNode<CSharpSyntaxNode>();
            CurrentContext = null;

            if (csNode != null)
            {
                return csNode.NormalizeWhitespace().ToFullString();
            }
            return string.Empty;
        }

        public static Type GetConverter(Node tsNode)
        {
            Type nodeType = tsNode.GetType();
            if (Converters.ContainsKey(nodeType))
            {
                return Converters[nodeType];
            }
            return null;
        }
        #endregion

        #region Static Members
        private static Dictionary<Type, Type> _converters;
        private static Dictionary<Type, Type> Converters
        {
            get
            {
                if (_converters != null)
                {
                    return _converters;
                }

                //
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
        #endregion
    }

}
