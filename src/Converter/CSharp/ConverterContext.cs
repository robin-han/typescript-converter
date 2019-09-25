using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TypeScript.Syntax;
using TypeScript.Syntax.Analysis;

namespace TypeScript.Converter.CSharp
{
    public class ConverterContext
    {
        #region Fields
        private readonly Syntax.Project _project;
        private readonly ConverterConfig _config;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        public ConverterContext(Syntax.Project project) : this(project, new ConverterConfig())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="config"></param>
        public ConverterContext(Syntax.Project project, ConverterConfig config)
        {
            this._project = project;
            this._config = config;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets current converter context
        /// </summary>
        public static ConverterContext Current
        {
            get;
            set;
        }

        /// <summary>
        /// Get the config
        /// </summary>
        public ConverterConfig Config
        {
            get { return this._config; }
        }

        /// <summary>
        /// Get the project.
        /// </summary>
        public Syntax.Project Project
        {
            get { return this._project; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create a converter by syntax node.
        /// </summary>
        /// <param name="node">The syntax node.</param>
        /// <returns>The node's converter.</returns>
        public Converter CreateConverter(Node node)
        {
            Type nodeType = node.GetType();
            if (ConverterTypes.ContainsKey(nodeType))
            {
                Type converterType = ConverterTypes[nodeType];
                Converter converter = (Converter)converterType.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes);
                converter.Context = this;
                return converter;
            }
            return null;
        }

        /// <summary>
        /// Analyze syntax nodes before convert.
        /// </summary>
        /// <param name="nodes"></param>
        public void Analyze(List<Node> nodes)
        {
            foreach (Type type in this.Project.GetAnalyzerTypes())
            {
                foreach (Node node in nodes)
                {
                    Analyzer analyzer = type.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes) as Analyzer;
                    analyzer.Analyze(node);
                }
            }
        }
        #endregion

        #region Static Members
        private static Dictionary<Type, Type> _converterTypes;
        private static Dictionary<Type, Type> ConverterTypes
        {
            get
            {
                if (_converterTypes != null)
                {
                    return _converterTypes;
                }

                //
                _converterTypes = new Dictionary<Type, Type>();
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
                        _converterTypes[paramType] = type;
                    }
                }
                return _converterTypes;
            }
        }
        #endregion
    }
}
