using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Converter
{
    /// <summary>
    /// Defines convert context interface.
    /// </summary>
    public interface IConvertContext
    {
        /// <summary>
        /// Gets the typescript syntax project
        /// </summary>
        IProject Project { get; }

        /// <summary>
        /// Gets or sets the namespace or package.
        /// </summary>
        string Namespace { get; }

        /// <summary>
        /// Gets or sets whether convert to typescript type.
        /// </summary>
        bool TypeScriptType { get; }

        /// <summary>
        /// Gets or sets current usings or imported package.
        /// </summary>
        List<string> Usings { get; }

        /// <summary>
        /// Gets or sets the omitted qualified names.
        /// </summary>
        List<string> QualifiedNames { get; }

        /// <summary>
        /// Gets or set excluded types
        /// </summary>
        List<string> ExcludeTypes { get; }

        #region Methods
        /// <summary>
        /// Gets the document's output.
        /// </summary>
        /// <param name="doc">The document</param>
        /// <returns></returns>
        IOutput GetOutput(Document doc);

        /// <summary>
        /// Trim type name by qualified names. (such as: dv.moduels.PlotView -> PlotView).
        /// </summary>
        /// <param name="typeName">The type name</param>
        /// <returns></returns>
        string TrimTypeName(string typeName);
        #endregion
    }
}
