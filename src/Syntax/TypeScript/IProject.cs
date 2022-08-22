using TypeScript.Syntax.Converter;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax
{
    /// <summary>
    /// Defines IProject interface.
    /// </summary>
    public interface IProject
    {
        /// <summary>
        /// Gets the group id.
        /// </summary>
        string GroupId { get; }

        /// <summary>
        /// Gets the project path.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets all documents.
        /// </summary>
        List<Document> Documents { get; }

        /// <summary>
        /// Gets all types in the project
        /// </summary>
        List<string> TypeDeclarationNames { get; }

        /// <summary>
        /// Gets the project's converter.
        /// </summary>
        IConverter Converter { get; }

        /// <summary>
        /// Normalize documents
        /// </summary>
        /// <param name="docs">The documents</param>
        void Normalize(List<Document> docs);

        /// <summary>
        /// Gets the document by its path.
        /// </summary>
        /// <param name="path">The import/export path.</param>
        /// <returns>The document.</returns>
        Document GetDocument(string path);

        /// <summary>
        /// Gets the document which the class, interface, enum or typeAlias within.
        /// </summary>
        /// <param name="typeName">The type name.</param>
        /// <returns>The document where the type in</returns>
        Document GetTypeDeclarationDocument(string typeName);

        /// <summary>
        /// Gets the type declaration by type name.
        /// </summary>
        /// <param name="typeName">The type name</param>
        /// <returns>The type node</returns>
        Node GetTypeDeclaration(string typeName);
    }
}
