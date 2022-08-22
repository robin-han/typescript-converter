using System.Collections.Generic;

namespace TypeScript.Syntax
{
    public class AnalysisVisitor : INodeVisitor
    {
        public List<TypeDeclaration> Types = new List<TypeDeclaration>();
        public List<TypeAliasDeclaration> Aliases = new List<TypeAliasDeclaration>();
        public List<ClassDeclaration> Classes = new List<ClassDeclaration>();
        public List<Identifier> Identifiers = new List<Identifier>();

        public void Visit(Node node)
        {
            if (node is ClassDeclaration classDeclaration)
            {
                this.Classes.Add(classDeclaration);
                this.Types.Add(classDeclaration);
            }
            if (node is EnumDeclaration enumDeclaration)
            {
                this.Types.Add(enumDeclaration);
            }
            if (node is InterfaceDeclaration interfaceDeclaration)
            {
                this.Types.Add(interfaceDeclaration);
            }
            if (node is Identifier identifier)
            {
                this.Identifiers.Add(identifier);
            }
            if (node is TypeAliasDeclaration typeAliasDeclaration)
            {
                this.Aliases.Add(typeAliasDeclaration);
            }
        }
    }

}
