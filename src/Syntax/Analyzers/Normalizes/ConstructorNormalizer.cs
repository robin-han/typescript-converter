using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class ConstructorNormalizer : Normalizer
    {
        public override int Priority
        {
            get { return 2; }
        }

        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.Constructor:
                    this.NormalizeConstructor(node as Constructor);
                    break;

                default:
                    break;
            }

        }

        private void NormalizeConstructor(Constructor ctorNode)
        {
            this.RemoveBaseStatement(ctorNode);
            this.AddPropertyInitStatements(ctorNode);
        }

        private void AddPropertyInitStatements(Constructor ctorNode)
        {
            ClassDeclaration classNode = ctorNode.Parent as ClassDeclaration;
            if (classNode == null)
            {
                return;
            }

            Block ctorBlock = ctorNode.Body as Block;
            List<PropertyDeclaration> props = this.GetInitProperties(classNode);
            props.Reverse();
            foreach (PropertyDeclaration prop in props)
            {
                ExpressionStatement statement = NodeHelper.CreateNode(this.GetInitPropertyStatementString(prop.Name.Text)) as ExpressionStatement;
                (statement.Expression as BinaryExpression).Right = prop.Initializer;
                ctorBlock.Statements.Insert(0, statement);
            }
        }

        private string GetInitPropertyStatementString(string propName)
        {
            return
             "{" +
                "kind: \"ExpressionStatement\", " +
                "expression: { " +
                    "kind: \"BinaryExpression\", " +
                    "left: { " +
                        "kind: \"PropertyAccessExpression\", " +
                        "expression: { " +
                            "kind: \"ThisKeyword\" " +
                        "}," +
                        "name: { " +
                            "text: \"" + propName + "\", " +
                            "kind: \"Identifier\" " +
                        "}" +
                    "}," +
                    "operatorToken: {" +
                        "kind: \"FirstAssignment\" " +
                    "}" +
                 "}" +
             "}";
        }

        private List<PropertyDeclaration> GetInitProperties(ClassDeclaration classNode)
        {
            List<PropertyDeclaration> ret = new List<PropertyDeclaration>();
            foreach (Node member in classNode.Members)
            {
                if (member.Kind == NodeKind.PropertyDeclaration)
                {
                    PropertyDeclaration prop = member as PropertyDeclaration;
                    if (prop.Initializer != null && prop.IsPublic && !prop.IsStatic && !prop.IsReadonly)
                    {
                        ret.Add(prop);
                    }
                }
            }
            return ret;
        }

        private void RemoveBaseStatement(Constructor ctorNode)
        {
            List<Node> statements = (ctorNode.Body as Block).Statements;
            for (int i = 0; i < statements.Count; i++)
            {
                Node statement = statements[i];
                if (this.IsBaseConstructor(statement))
                {
                    ctorNode.Body.RemoveChild(statement);
                    Node baseNode = NodeHelper.CreateNode(statement.TsNode);
                    baseNode.Parent = ctorNode;
                    ctorNode.Base = baseNode;
                    break;
                }
            }
        }

        private bool IsBaseConstructor(Node node)
        {
            if (node.Kind != NodeKind.ExpressionStatement)
            {
                return false;
            }

            ExpressionStatement expStatement = node as ExpressionStatement;
            if (expStatement.Expression.Kind != NodeKind.CallExpression)
            {
                return false;
            }

            CallExpression callExp = expStatement.Expression as CallExpression;
            if (callExp.Expression.Kind != NodeKind.SuperKeyword)
            {
                return false;
            }

            return true;
        }

    }
}
