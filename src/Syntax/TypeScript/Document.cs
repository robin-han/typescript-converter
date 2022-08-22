using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace TypeScript.Syntax
{
    public class Document
    {
        #region Constructor
        public Document(string path, SourceFile root)
        {
            this.Path = path;
            this.Source = root;

            this.Source.Document = this;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the document's full path.
        /// </summary>
        public string Path
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the document's relative path.
        /// </summary>
        public string RelativePath
        {
            get
            {
                if (this.Project != null && !string.IsNullOrEmpty(this.Project.Path))
                {
                    string docDirectory = System.IO.Path.GetDirectoryName(this.Path);
                    string relativePath = System.IO.Path.GetRelativePath(this.Project.Path, docDirectory);
                    return (relativePath == "." ? string.Empty : relativePath);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the document's source file.
        /// </summary>
        public SourceFile Source
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the document's project.
        /// </summary>
        public Project Project
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets all type nodes(class, interfact, enum, type alias, etc.) in the document.
        /// </summary>
        public List<Node> TypeDeclarations
        {
            get
            {
                return this.Source.GetTypeDeclarations(true);
            }
        }

        public List<string> TypeDeclarationNames
        {
            get
            {
                return TypeDeclarations.Select(n => n.GetName()).ToList();
            }
        }

        /// <summary>
        /// Gets all global functions in the document
        /// </summary>
        public List<Node> Functions
        {
            get
            {
                return this.Source.GetFunctionDeclarations(true);
            }
        }

        public bool IsEnumFile
        {
            get
            {
                List<Node> declarations = this.TypeDeclarations;
                return (declarations.Count == 1 && declarations[0].Kind == NodeKind.EnumDeclaration);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets type definition.
        /// </summary>
        /// <param name="typeName">The type name</param>
        /// <returns>The type definition</returns>
        public Node GetTypeDeclaration(string typeName)
        {
            SourceFile sourceFile = this.Source;

            Node definition = sourceFile.GetOwnModuleTypeDeclaration(typeName);
            if (definition != null)
            {
                return definition;
            }

            definition = sourceFile.GetImportModuleTypeDeclaration(typeName);
            if (definition != null)
            {
                return definition;
            }

            int index = this.Project == null ? -1 : this.Project.TypeDeclarationNames.IndexOf(typeName);
            if (index >= 0)
            {
                return this.Project.TypeDeclarations[index];
            }

            return null;
        }

        /// <summary>
        /// Gets export type definition.
        /// </summary>
        /// <param name="typeName">The export type name.</param>
        /// <returns>The definition.</returns>
        public Node GetExportTypeDeclaration(string typeName)
        {
            return this.Source.GetExportModuleTypeDeclaration(typeName);
        }

        /// <summary>
        /// Gets the export default type definition.
        /// </summary>
        /// <returns></returns>
        public Node GetExportDefaultTypeDeclaration()
        {
            return this.Source.GetExportDefaultModuleTypeDeclaration();
        }

        /// <summary>
        /// Gets the first public type declaration in the document.
        /// </summary>
        /// <returns></returns>
        public Node GetPublicTypeDeclaration()
        {
            foreach (var typeDeclaration in this.TypeDeclarations)
            {
                if (typeDeclaration.HasModify(NodeKind.ExportKeyword))
                {
                    return typeDeclaration;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets reference types
        /// </summary>
        /// <returns></returns>
        public List<string> GetReferenceTypes()
        {
            List<string> declarationNames = this.Project.TypeDeclarationNames;
            List<Node> types = new List<Node>();
            this.Source.Descendants(n =>
            {
                if (n.GetValue("Type") is Node t)
                {
                    types.Add(t);
                }

                switch (n.Kind)
                {
                    case NodeKind.HeritageClause:
                    case NodeKind.IntersectionType:
                    case NodeKind.UnionType:
                        if (n.GetValue("Types") is List<Node> ts)
                        {
                            types.AddRange(ts);
                        }
                        break;

                    case NodeKind.CallExpression:
                    case NodeKind.ExpressionWithTypeArguments:
                    case NodeKind.NewExpression:
                    case NodeKind.TypeReference:
                        if (n.GetValue("TypeArguments") is List<Node> tas)
                        {
                            types.AddRange(tas);
                        }
                        if (n.Kind == NodeKind.CallExpression)
                        {
                            CallExpression call = (CallExpression)n;
                            if (call.IsTypePredicate(out FunctionDeclaration predicateFunc))
                            {
                                types.Add(((TypePredicate)predicateFunc.Type).Type);
                            }
                        }
                        break;

                    case NodeKind.TypeParameter:
                        TypeParameter typeParameter = (TypeParameter)n;
                        if (typeParameter.Constraint != null)
                        {
                            types.Add(typeParameter.Constraint);
                        }
                        else
                        {
                            types.Add(typeParameter.Name);
                        }
                        break;

                    case NodeKind.PropertyAccessExpression:
                        PropertyAccessExpression access = (PropertyAccessExpression)n;

                        if (access.Expression.Kind == NodeKind.Identifier && declarationNames.Contains(access.Expression.Text))
                        {
                            types.Add(access.Expression);
                        }
                        else if (access.Name.Kind == NodeKind.Identifier && TypeHelper.IsMatchTypeName(access.Name.Text) && declarationNames.Contains(access.Name.Text))
                        {
                            types.Add(access.Name);
                        }
                        break;

                    case NodeKind.BinaryExpression:
                        BinaryExpression binary = (BinaryExpression)n;
                        if (binary.OperatorToken.Kind == NodeKind.InstanceOfKeyword)
                        {
                            types.Add(binary.Right);
                        }
                        break;

                    case NodeKind.ArrowFunction:
                        Node arrowDeclarationType = TypeHelper.GetDeclarationType(n);
                        if (arrowDeclarationType != null)
                        {
                            types.Add(arrowDeclarationType);
                        }
                        break;

                    default:
                        break;
                }
                return false;
            });

            List<string> result = new List<string>();
            foreach (Node typeNode in types)
            {
                Node type = typeNode;
                if (type.Kind == NodeKind.ArrayType)
                {
                    type = ((ArrayType)type).ElementType;
                }

                string name = TypeHelper.GetTypeDeclarationName(type);
                if (!result.Contains(name) && declarationNames.Contains(name))
                {
                    result.Add(name);
                }
            }
            return result;
        }
        #endregion
    }
}
