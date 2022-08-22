using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class TypeNormalizer : Normalizer
    {
        public override int Priority
        {
            get { return int.MaxValue; }
        }

        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.ArrayLiteralExpression:
                    this.InferType(node as ArrayLiteralExpression);
                    break;

                case NodeKind.FunctionDeclaration:
                    this.InferType(node as FunctionDeclaration);
                    break;

                case NodeKind.GetAccessor:
                    this.InferType(node as GetAccessor);
                    break;

                case NodeKind.GetSetAccessor:
                    this.InferType(node as GetSetAccessor);
                    break;

                case NodeKind.IndexSignature:
                    this.InferType(node as IndexSignature);
                    break;

                case NodeKind.MethodDeclaration:
                    this.InferType(node as MethodDeclaration);
                    break;

                case NodeKind.MethodSignature:
                    this.InferType(node as MethodSignature);
                    break;

                case NodeKind.ObjectLiteralExpression:
                    this.InferType(node as ObjectLiteralExpression);
                    break;

                case NodeKind.PropertyDeclaration:
                    this.InferType(node as PropertyDeclaration);
                    break;

                case NodeKind.PropertySignature:
                    this.InferType(node as PropertySignature);
                    break;

                case NodeKind.VariableDeclarationList:
                    this.InferType(node as VariableDeclarationList);
                    break;

                case NodeKind.VariableDeclaration:
                    this.InferType(node as VariableDeclaration);
                    break;

                default:
                    break;
            }
        }

        private void InferType(ArrayLiteralExpression node)
        {
            if (node.Type == null)
            {
                Node type = TypeHelper.GetNodeType(node);
                node.SetType(type, type.Parent == null);
            }
        }

        private void InferType(FunctionDeclaration node)
        {
            if (node.Type == null)
            {
                node.SetType(NodeHelper.CreateNode(NodeKind.VoidKeyword));
            }
        }

        private void InferType(GetAccessor node)
        {
            if (node.Type == null)
            {
                node.SetType(NodeHelper.CreateNode(NodeKind.AnyKeyword));
            }
        }

        private void InferType(GetSetAccessor node)
        {
            if (node.Type == null)
            {
                Node type = node.GetAccessor.Type ?? ((Parameter)node.SetAccessor.Parameters[0]).Type;
                node.SetType(type, false);
            }
        }

        private void InferType(IndexSignature node)
        {
            if (node.Type == null)
            {
                node.SetType(NodeHelper.CreateNode(NodeKind.AnyKeyword));
            }
        }

        private void InferType(MethodDeclaration node)
        {
            if (node.Type == null)
            {
                node.SetType(NodeHelper.CreateNode(NodeKind.VoidKeyword));
            }
        }

        private void InferType(MethodSignature node)
        {
            if (node.Type == null)
            {
                node.SetType(NodeHelper.CreateNode(NodeKind.VoidKeyword));
            }
        }

        private void InferType(ObjectLiteralExpression node)
        {
            if (node.Type == null)
            {
                Node type = TypeHelper.GetNodeType(node);
                node.SetType(type, type.Parent == null);
            }
        }

        private void InferType(PropertyDeclaration node)
        {
            if (node.Type == null)
            {
                Node type = null;
                if (node.Initializer != null)
                {
                    type = TypeHelper.GetNodeType(node.Initializer);
                }
                type = type ?? NodeHelper.CreateNode(NodeKind.AnyKeyword);
                node.SetType(type, type.Parent == null);
            }
        }

        private void InferType(PropertySignature node)
        {
            if (node.Type == null)
            {
                node.SetType(NodeHelper.CreateNode(NodeKind.AnyKeyword));
            }
        }

        private void InferType(VariableDeclarationList node)
        {
            if (node.Type == null)
            {
                VariableDeclaration variableNode = (node.Declarations[0] as VariableDeclaration);
                if (variableNode.Type != null)
                {
                    node.SetType(variableNode.Type, false);
                }
                else
                {
                    node.SetType(NodeHelper.CreateNode(NodeKind.AnyKeyword));
                }
            }
        }

        private void InferType(VariableDeclaration node)
        {
            if (node.Type == null)
            {
                Node type = TypeHelper.GetNodeType(node);
                node.SetType(type, type.Parent == null);
            }
        }
    }
}
