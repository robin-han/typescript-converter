using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class TypeNormalizer : Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.MethodDeclaration:
                    this.CheckEnumerableType(node as MethodDeclaration);
                    break;

                case NodeKind.GetAccessor:
                    this.CheckEnumerableType(node as GetAccessor);
                    break;

                case NodeKind.GetSetAccessor:
                    this.CheckEnumerableType((node as GetSetAccessor).GetAccessor);
                    break;

                default:
                    break;
            }
        }

        private void CheckEnumerableType(MethodDeclaration method)
        {
            Node type = method.Type;
            if (type != null && type.Kind == NodeKind.ArrayType && Util.HasEnumerableFlag(method.Body.Text))
            {
                (type as ArrayType).IsEnumerable = true;
            }
        }

        private void CheckEnumerableType(GetAccessor method)
        {
            Node type = method.Type;
            if (type != null && type.Kind == NodeKind.ArrayType && Util.HasEnumerableFlag(method.Body.Text))
            {
                (type as ArrayType).IsEnumerable = true;
            }
        }
        
    }
}
