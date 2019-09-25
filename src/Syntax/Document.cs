using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;


namespace TypeScript.Syntax
{
    public class Document
    {
        #region Fields
        private Node _root;
        private string _path;
        private Project _project;
        #endregion

        #region Constructor
        public Document(string path, Node root)
        {
            this._path = path;
            this._root = root;
            this._project = null;

            root.Document = this;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the document's full path.
        /// </summary>
        public string Path
        {
            get
            {
                return this._path;
            }
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
                    string dir = System.IO.Path.GetDirectoryName(this.Path);
                    string relativePath = System.IO.Path.GetRelativePath(this.Project.Path, dir);
                    return (relativePath == "." ? string.Empty : relativePath);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the document's root node.
        /// </summary>
        public Node Root
        {
            get
            {
                return this._root;
            }
        }

        /// <summary>
        /// Gets the document's project.
        /// </summary>
        public Project Project
        {
            get
            {
                return this._project;
            }
            set
            {
                this._project = value;
            }
        }

        /// <summary>
        /// Gets all type nodes(class, interfact, enum, type alias, etc.) in the document.
        /// </summary>
        public List<Node> TypeNodes
        {
            get
            {
                return this.Root.DescendantsOnce((n) =>
                {
                    return (
                       n.Kind == NodeKind.ClassDeclaration
                    || n.Kind == NodeKind.InterfaceDeclaration
                    || n.Kind == NodeKind.EnumDeclaration
                    || n.Kind == NodeKind.TypeAliasDeclaration);
                });
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the export default node.
        /// </summary>
        public string GetExportDefaultName()
        {
            if (this.Root == null || this.Root.Kind != NodeKind.SourceFile)
            {
                return string.Empty;
            }

            SourceFile source = this.Root as SourceFile;
            foreach (Node statement in source.Statements)
            {
                switch (statement.Kind)
                {
                    case NodeKind.ClassDeclaration:
                        ClassDeclaration classNode = statement as ClassDeclaration;
                        if (classNode.HasModify(NodeKind.ExportKeyword) && classNode.HasModify(NodeKind.DefaultKeyword))
                        {
                            return classNode.Name.Text;
                        }
                        break;

                    case NodeKind.InterfaceDeclaration:
                        InterfaceDeclaration interfaceNode = statement as InterfaceDeclaration;
                        if (interfaceNode.HasModify(NodeKind.ExportKeyword) && interfaceNode.HasModify(NodeKind.DefaultKeyword))
                        {
                            return interfaceNode.Name.Text;
                        }
                        break;

                    case NodeKind.EnumDeclaration:
                        EnumDeclaration enumNode = statement as EnumDeclaration;
                        if (enumNode.HasModify(NodeKind.ExportKeyword) && enumNode.HasModify(NodeKind.DefaultKeyword))
                        {
                            return enumNode.Name.Text;
                        }
                        break;

                    case NodeKind.ExportDeclaration:
                        ExportDeclaration exportNode = statement as ExportDeclaration;
                        foreach (ExportSpecifier specifier in ((NamedExports)exportNode.ExportClause).Elements)
                        {
                            if (specifier.Name.Text == "default")
                            {
                                string propertyName = specifier.PropertyName.Text;
                                if (exportNode.ModuleSpecifier != null)
                                {
                                    Document fromDoc = source.Project.GetDocumentByPath(exportNode.ModulePath);
                                    return (fromDoc != null ? fromDoc.GetExportActualName(propertyName): string.Empty);
                                }
                                else
                                {
                                    return propertyName;
                                }
                            }
                        }
                        break;

                    case NodeKind.ExportAssignment:
                        ExportAssignment assignmentNode = statement as ExportAssignment;
                        return assignmentNode.Expression.Text;

                    default:
                        break;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the actual class, interface, enum name of export.
        /// </summary>
        /// <param name="declarationName">The declaration name</param>
        /// <returns>The export actual model name.</returns>
        public string GetExportActualName(string declarationName)
        {
            if (this.Root == null || this.Root.Kind != NodeKind.SourceFile)
            {
                return string.Empty;
            }

            SourceFile source = this.Root as SourceFile;
            foreach (Node statement in source.Statements)
            {
                switch (statement.Kind)
                {
                    case NodeKind.ClassDeclaration:
                        ClassDeclaration classNode = statement as ClassDeclaration;
                        if (classNode.Name.Text == declarationName && classNode.HasModify(NodeKind.ExportKeyword))
                        {
                            return declarationName;
                        }
                        break;

                    case NodeKind.InterfaceDeclaration:
                        InterfaceDeclaration interfaceNode = statement as InterfaceDeclaration;
                        if (interfaceNode.Name.Text == declarationName && interfaceNode.HasModify(NodeKind.ExportKeyword))
                        {
                            return declarationName;
                        }
                        break;

                    case NodeKind.EnumDeclaration:
                        EnumDeclaration enumNode = statement as EnumDeclaration;
                        if (enumNode.Name.Text == declarationName && enumNode.HasModify(NodeKind.ExportKeyword))
                        {
                            return declarationName;
                        }
                        break;

                    case NodeKind.ExportDeclaration:
                        ExportDeclaration exportNode = statement as ExportDeclaration;
                        string actualName = this.GetExportDeclarationActualName(exportNode, declarationName);
                        if (!string.IsNullOrEmpty(actualName))
                        {
                            return actualName;
                        }
                        break;

                    case NodeKind.ExportAssignment:
                        ExportAssignment assignmentNode = statement as ExportAssignment;
                        if (assignmentNode.Expression.Text == declarationName)
                        {
                            return declarationName;
                        }
                        break;

                    default:
                        break;
                }
            }

            return String.Empty;
        }

        /// <summary>
        /// Gets export declaration actual name.
        /// </summary>
        /// <param name="export">The export declaration.</param>
        /// <param name="name">The export name</param>
        /// <returns>The export actual model name.</returns>
        private string GetExportDeclarationActualName(ExportDeclaration export, string name)
        {
            Document fromDoc = null;
            if (export.ModuleSpecifier != null)
            {
                fromDoc = export.Project.GetDocumentByPath(export.ModulePath);
            }

            if (export.ExportClause == null) // export * from './abc'
            {
                return (fromDoc != null ? fromDoc.GetExportActualName(name) : string.Empty);
            }
            else
            {
                NamedExports namedExports = export.ExportClause as NamedExports;
                foreach (ExportSpecifier specifier in namedExports.Elements)
                {
                    if (specifier.Name.Text == name)
                    {
                        string actualName = (specifier.PropertyName != null ? specifier.PropertyName.Text : specifier.Name.Text);
                        return (fromDoc != null ? fromDoc.GetExportActualName(actualName) : actualName);
                    }
                }
            }
            return string.Empty;
        }
        #endregion
    }
}
