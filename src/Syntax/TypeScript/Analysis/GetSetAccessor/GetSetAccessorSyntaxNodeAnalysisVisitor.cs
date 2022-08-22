using System.Collections.Generic;
using System.Linq;

using TypeScript.Syntax;

namespace GrapeCity.Syntax.Converter.Source.TypeScript.Analysis
{
    public class GetSetAccessorSyntaxNodeAnalysisVisitor : INodeVisitor
    {
        private List<GetSetAccessorSyntaxNodeInformation> getSetAccessorSyntaxNodeInformations = new List<GetSetAccessorSyntaxNodeInformation>();

        public IEnumerable<GetSetAccessorSyntaxNodeInformation> GetSetAccessorSyntaxNodeInformations
        {
            get
            {
                return this.getSetAccessorSyntaxNodeInformations.ToArray();
            }
        }
        public void Visit(Node node)
        {
            if (node is GetAccessor getAccessor)
            {
                this.UpdateGetAccessorInformation(this.getSetAccessorSyntaxNodeInformations, getAccessor);
            }
        }

        protected void UpdateGetAccessorInformation(List<GetSetAccessorSyntaxNodeInformation> GetSetAccessorSyntaxNodeInformations, GetAccessor getAccessor)
        {
            var getSetAccessorSyntaxNodeInformation = this.getSetAccessorSyntaxNodeInformations.SingleOrDefault((information) => (information.Name == getAccessor.Name.GetName()) &&
                 (information.ClassDeclaration == getAccessor.Ancestor<ClassDeclaration>()));

            if (getSetAccessorSyntaxNodeInformation == null)
            {
                getSetAccessorSyntaxNodeInformation = new GetSetAccessorSyntaxNodeInformation(getAccessor);
            }
            else
            {
                getSetAccessorSyntaxNodeInformation.GetAccessorSyntaxNode = getAccessor;
            }
        }
        protected void UpdateSetAccessorInformation(List<GetSetAccessorSyntaxNodeInformation> GetSetAccessorSyntaxNodeInformations, SetAccessor setAccessor)
        {
            var getSetAccessorSyntaxNodeInformation = this.getSetAccessorSyntaxNodeInformations.SingleOrDefault((information) => (information.Name == setAccessor.Name.GetName()) &&
                 (information.ClassDeclaration == setAccessor.Ancestor<ClassDeclaration>()));

            if (getSetAccessorSyntaxNodeInformation == null)
            {
                getSetAccessorSyntaxNodeInformation = new GetSetAccessorSyntaxNodeInformation(setAccessor);
            }
            else
            {
                getSetAccessorSyntaxNodeInformation.SetAccessorSyntaxNode = setAccessor;
            }
        }
    }
}
