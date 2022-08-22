using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeScript.Syntax;
using TypeScript.Syntax.Converter;

namespace TypeScript.Converter.CSharp
{
    public class NodeConverter
    {
        #region Fields
        private IConvertContext _context;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public NodeConverter() : this(new DefaultConvertContext())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public NodeConverter(IConvertContext context)
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
        /// Gets or sets the converter context
        /// </summary>
        public IConvertContext Context
        {
            get
            {
                return this._context;
            }
            set
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
                csArgumetns.Add(SyntaxFactory.Argument(node.ToCsSyntaxTree<ExpressionSyntax>()));
            }

            return csArgumetns.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string TrimTypeName(string typeName)
        {
            return this.Context.TrimTypeName(typeName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected bool IsQualifiedName(string name)
        {
            return this.Context.QualifiedNames.Contains(name);
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
                typeName = this.TrimTypeName(typeName);
                BinaryExpressionSyntax csAs = SyntaxFactory.BinaryExpression(
                    SyntaxKind.AsExpression,
                    exprSyntax,
                    SyntaxFactory.ParseName(typeName));
                return SyntaxFactory.ParenthesizedExpression(csAs);
            }
        }

        /// <summary>
        /// Filter types.
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        protected List<Node> FilterTypes(List<Node> statements)
        {
            List<string> excluteTypes = this.Context.ExcludeTypes;
            if (excluteTypes.Count == 0)
            {
                return statements;
            }

            return statements.FindAll(statement =>
            {
                switch (statement.Kind)
                {
                    case NodeKind.ClassDeclaration:
                        return !excluteTypes.Contains((statement as ClassDeclaration).NameText);

                    case NodeKind.InterfaceDeclaration:
                        return !excluteTypes.Contains((statement as InterfaceDeclaration).NameText);

                    case NodeKind.EnumDeclaration:
                        return !excluteTypes.Contains((statement as EnumDeclaration).NameText);

                    case NodeKind.TypeAliasDeclaration:
                        return !excluteTypes.Contains((statement as TypeAliasDeclaration).NameText);

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

        private class DefaultConvertContext : IConvertContext
        {
            public DefaultConvertContext()
            {
                this.Project = null;
                this.Namespace = "";
                this.TypeScriptType = false;
                this.Usings = new List<string>();
                this.QualifiedNames = new List<string>();
                this.ExcludeTypes = new List<string>();
            }
            public IProject Project { get; private set; }
            public string Namespace { get; set; }
            public bool TypeScriptType { get; set; }
            public List<string> Usings { get; set; }
            public List<string> QualifiedNames { get; set; }
            public List<string> ExcludeTypes { get; set; }

            public IOutput GetOutput(Syntax.Document doc)
            {
                return null;
            }

            public string TrimTypeName(string typeName)
            {
                return typeName;
            }
        }
    }


}
