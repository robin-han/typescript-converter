using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GrapeCity.CodeAnalysis.TypeScript.Syntax;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
{
    public class Converter
    {
        #region Fields
        private ConverterContext _context;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public Converter() : this(new ConverterContext(null))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Converter(ConverterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._context = context;
        }
        #endregion


        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public ConverterContext Context
        {
            get
            {
                return this._context;
            }
            internal set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("context");
                }
                this._context = value;
            }
        }
        #endregion


        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argNodes"></param>
        /// <returns></returns>
        protected ArgumentSyntax[] ToArgumentList(List<Node> argNodes)
        {
            List<ArgumentSyntax> csArgumetns = new List<ArgumentSyntax>();
            foreach (Node node in argNodes)
            {
                csArgumetns.Add(SyntaxFactory.Argument(node.ToCsNode<ExpressionSyntax>()));
            }

            return csArgumetns.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string StripType(string type)
        {
            List<string> omittedNames = this.Context.Config.OmittedQualifiedNames;
            foreach (string omitted in omittedNames)
            {
                int index = type.IndexOf(omitted);
                if (index < 0)
                {
                    continue;
                }

                index = index + omitted.Length;
                if (index < type.Length && type[index] == '.')
                {
                    type = type.Substring(index + 1);
                }
            }
            return type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exprSyntax"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected CSharpSyntaxNode As(ExpressionSyntax exprSyntax, string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return exprSyntax;
            }
            else
            {
                typeName = this.StripType(typeName);
                BinaryExpressionSyntax csAs = SyntaxFactory.BinaryExpression(
                    SyntaxKind.AsExpression,
                    exprSyntax,
                    SyntaxFactory.ParseName(typeName));
                return SyntaxFactory.ParenthesizedExpression(csAs);
            }
        }

        /// <summary>
        /// Filter statements.
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        protected List<Node> FilterStatements(List<Node> statements)
        {
            List<string> excluteTypes = this.Context.Config.ExcludeTypes;
            if (excluteTypes.Count == 0)
            {
                return statements;
            }

            return statements.FindAll(statement =>
            {
                switch (statement.Kind)
                {
                    case NodeKind.ClassDeclaration:
                        return !excluteTypes.Contains((statement as ClassDeclaration).Name.Text);

                    case NodeKind.InterfaceDeclaration:
                        return !excluteTypes.Contains((statement as InterfaceDeclaration).Name.Text);

                    case NodeKind.EnumDeclaration:
                        return !excluteTypes.Contains((statement as EnumDeclaration).Name.Text);

                    case NodeKind.TypeAliasDeclaration:
                        return !excluteTypes.Contains((statement as TypeAliasDeclaration).Name.Text);

                    default:
                        return true;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected string CommentText(string text)
        {
            return "AAA___" + text + "___AAA";
        }
        #endregion

    }
}
