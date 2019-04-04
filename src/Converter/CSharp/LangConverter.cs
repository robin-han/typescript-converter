using GrapeCity.CodeAnalysis.TypeScript.Syntax;
using GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
{
    public class LangConverter
    {
        #region Fields
        private readonly ConverterContext _context;
        internal const string CONVERTER_CONTEXT_KEY = "ConverterContext";
        #endregion

        #region Constructor
        public LangConverter(ConverterContext context)
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
       
        internal static ConverterContext CurrentContext
        {
            get;
            set;
        }
        #endregion

        #region Public Methods
        public void Analyze(Node tsNode)
        {
            foreach (Type type in Analyzers)
            {
                Analyzer analyzer = type.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes) as Analyzer;
                analyzer.Analyze(tsNode);
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


        private static List<Type> _analyzers;
        public static List<Type> Analyzers
        {
            get
            {
                if (_analyzers != null)
                {
                    return _analyzers;
                }

                //
                _analyzers = new List<Type>();
                Type baseType = typeof(Analyzer);
                Type normaizerType = typeof(Normalizer);

                Type[] types = typeof(Analyzer).Assembly.GetExportedTypes();
                foreach (Type type in types)
                {
                    if (type.IsSubclassOf(normaizerType))
                    {
                        _analyzers.Insert(0, type);
                    }
                    else if (type.IsSubclassOf(baseType))
                    {
                        _analyzers.Add(type);
                    }
                }
                return _analyzers;
            }
        }

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
    }

}
