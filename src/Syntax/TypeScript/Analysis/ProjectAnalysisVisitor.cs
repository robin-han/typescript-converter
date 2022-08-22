using TypeScript.Syntax;

namespace GrapeCity.Syntax.Converter.Source.TypeScript.Analysis
{
    public class ProjectAnalysisVisitor
    {
        public void Visit(Project project, INodeVisitor[] analysisVisitors)
        {
            if ((analysisVisitors == null) || (analysisVisitors.Length <= 0))
            {
                return;
            }

            foreach (var item in project.Documents)
            {
                if (item != null)
                {
                    this.Visit(item.Source, analysisVisitors);
                }
            }
        }

        protected void Visit(Node node, INodeVisitor[] analysisVisitors)
        {
            foreach (var item in node.Children)
            {
                this.Visit(item, analysisVisitors);
            }

            foreach (var analysisVisitor in analysisVisitors)
            {
                analysisVisitor.Visit(node);
            }
        }
    }
}
