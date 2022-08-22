using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Converter.Java
{
    /// <summary>
    /// TODO: add this config to application config
    /// </summary>
    class OperatorConfig
    {
        public static readonly List<TypeOperator> BinaryOperators = new List<TypeOperator>
        {
            new TypeOperator()
            {
                Name = "DataValueType",
                ConvertingOperators = new List<string>(){ ">", "<", ">=", "<=", "==", "===", "!=", "!==" },
                ConvertingMethod = "DataValueTypeOperator.logicalCompare"
            },
            new TypeOperator()
            {
                Name = "DataValueType",
                ConvertingOperators = new List<string>(){ "+", "-", "*", "/"},
                ConvertingMethod ="DataValueTypeOperator.arithmeticalOperate"
            },
            new TypeOperator()
            {
                Name = "DataValueType",
                ConvertingOperators = new List<string>(){ "="},
                ConvertingMethod ="DataValueTypeOperator.cast"
            },
            new TypeOperator()
            {
                Name = "Date",
                ConvertingOperators = new List<string>(){ ">", "<", ">=", "<=", "==", "===", "!=", "!==" },
                ConvertingMethod = "DateOperator.logicalCompare"
            },
            new TypeOperator()
            {
                Name = "string",
                ConvertingOperators = new List<string>(){ ">", "<", "==", "===", "!=", "!==" },
                ConvertingMethod = "StringOperator.logicalCompare"
            },
            new TypeOperator()
            {
                Name = "number?",
                ConvertingOperators = new List<string>(){ "==", "===", "!=", "!==" },
                ConvertingMethod = "NumberOperator.logicalCompare"
            }
        };

        public static readonly List<ImplicitOperator> ImplicitOperators = new List<ImplicitOperator>()
        {
            new ImplicitOperator()
            {
                From = "DataValueType",
                To = "string",
                ConvertingMethod = "DataValueTypeOperator.toString"
            },
            new ImplicitOperator()
            {
                From = "DataValueType",
                To = "number",
                ConvertingMethod = "DataValueTypeOperator.toNumber"
            },
            new ImplicitOperator()
            {
                From = "DataValueType",
                To = "boolean",
                ConvertingMethod = "DataValueTypeOperator.toBoolean"
            },
            new ImplicitOperator()
            {
                From = "DataValueType",
                To = "Date",
                ConvertingMethod = "DataValueTypeOperator.toDate"
            },
            new ImplicitOperator()
            {
                From = "string",
                To = "DataValueType",
                ConvertingMethod = "DataValueTypeOperator.fromString"
            },
            new ImplicitOperator()
            {
                From = "number",
                To = "DataValueType",
                ConvertingMethod = "DataValueTypeOperator.fromNumber"
            },
            new ImplicitOperator()
            {
                From = "boolean",
                To = "DataValueType",
                ConvertingMethod = "DataValueTypeOperator.fromBoolean"
            },
            new ImplicitOperator()
            {
                From = "Date",
                To = "DataValueType",
                ConvertingMethod = "DataValueTypeOperator.fromDate"
            },
        };
    }

    class TypeOperator
    {
        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the converting operators
        /// </summary>
        public List<string> ConvertingOperators { get; set; }

        /// <summary>
        /// Gets or sets the converting method
        /// </summary>
        public string ConvertingMethod { get; set; }
    }

    class ImplicitOperator
    {
        /// <summary>
        /// Gets or sets the from type name.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the to type name.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the converting method name
        /// </summary>
        public string ConvertingMethod { get; set; }
    }
}
