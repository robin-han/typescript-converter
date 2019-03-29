using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class InferTypeAnalyzer : Analyzer
    {
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

                case NodeKind.Parameter:
                    this.InferType(node as Parameter);
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
                    this.InferType(node as VariableDeclarationNode);
                    break;

                default:
                    break;
            }
        }

        private void InferType(ArrayLiteralExpression note)
        {
            if (note.Type == null)
            {
                note.Type = note.GetNodeType();
            }
        }

        private void InferType(FunctionDeclaration node)
        {
            if (node.Type == null)
            {
                node.Type = node.CreateNode(NodeKind.VoidKeyword);
            }
        }

        private void InferType(GetAccessor node)
        {
            if (node.Type == null)
            {
                node.Type = node.CreateNode(NodeKind.ObjectKeyword);
            }
        }

        private void InferType(GetSetAccessor node)
        {
            if (node.Type == null)
            {
                Node type = node.GetAccessor.Type ?? node.SetAccessor.Parameters[0].Type;
                node.Type = type;
            }
        }

        private void InferType(IndexSignature node)
        {
            if (node.Type == null)
            {
                node.Type = node.CreateNode(NodeKind.ObjectKeyword);
            }
        }

        private void InferType(MethodDeclaration node)
        {
            if (node.Type == null)
            {
                node.Type = node.CreateNode(NodeKind.VoidKeyword);
            }
        }

        private void InferType(MethodSignature node)
        {
            if (node.Type == null)
            {
                node.Type = node.CreateNode(NodeKind.VoidKeyword);
            }
        }

        private void InferType(ObjectLiteralExpression node)
        {
            if (node.Type == null)
            {
                node.Type = node.GetNodeType();
            }
        }

        private void InferType(Parameter node)
        {
            if (node.Type == null)
            {
                node.Type = node.CreateNode(NodeKind.AnyKeyword);
            }
        }

        private void InferType(PropertyDeclaration node)
        {
            if (node.Type == null)
            {
                Node type = null;
                if (node.Initializer != null)
                {
                    type = node.GetNodeType(node.Initializer);
                }
                type = type ?? node.CreateNode(NodeKind.AnyKeyword);
                node.Type = type;
            }
        }

        private void InferType(PropertySignature node)
        {
            if (node.Type == null)
            {
                node.Type = node.CreateNode(NodeKind.AnyKeyword);
            }
        }

        private void InferType(VariableDeclarationList node)
        {
            if (node.Type == null)
            {
                if (node.Declarations.Count > 0)
                {
                    VariableDeclarationNode variableNode = (node.Declarations[0] as VariableDeclarationNode);
                    if (variableNode.Type == null)
                    {
                        this.Visit(variableNode);
                    }
                    node.Type = variableNode.Type;
                }
                else
                {
                    node.Type = node.CreateNode(NodeKind.AnyKeyword);
                }
            }
        }

        private void InferType(VariableDeclarationNode node)
        {
            if (node.Type == null)
            {
                if (node.Parent.Kind == NodeKind.CatchClause)
                {
                    node.Type = node.CreateNode(NodeKind.Identifier, "Exception");
                }
                else
                {
                    Node type = null;
                    if (node.Initializer != null)
                    {
                        type = node.GetNodeType(node.Initializer);
                    }
                    type = type ?? node.CreateNode(NodeKind.AnyKeyword);

                    node.Type = type;
                }
            }
        }

    }
}
