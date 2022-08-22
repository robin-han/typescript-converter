using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Converter
{
    /// <summary>
    /// Define languate convert interface.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Gets the convert context
        /// </summary>
        IConvertContext Context { get; }

        /// <summary>
        /// Convert syntax tree node to code
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        string Convert(Node node);
    }
}
