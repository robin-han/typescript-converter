using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class Normalizer
    {
        public Normalizer()
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
            get { return 0; }
        }

        protected virtual void Visit(Node node)
        {
        }
    }
}
