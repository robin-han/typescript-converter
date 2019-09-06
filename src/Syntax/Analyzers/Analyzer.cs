using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class Analyzer
    {
        public Analyzer()
        {
        }

        public void Analyze(Node node)
        {
            this.Visit(node);

            foreach (Node child in node.Children)
            {
                this.Analyze(child);
            }
        }

        public virtual int Priority
        {
            get { return int.MaxValue; }
        }

        protected virtual void Visit(Node node)
        {
        }


    }
}
