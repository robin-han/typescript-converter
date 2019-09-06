using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class Normalizer : Analyzer
    {
        public override int Priority
        {
            get { return 1; }
        }
    }
}
