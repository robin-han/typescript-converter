using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;

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

                case NodeKind.ClassDeclaration:
                    this.AddLostConstructor(node as ClassDeclaration);
                    break;

                default:
                    break;
            }
        }

        private void AddLostConstructor(ClassDeclaration classNode)
        {
            Constructor constructor = classNode.GetConstructor();
            List<ClassDeclaration> baseClasses = classNode.Project.GetInheritClasses(classNode);
            if (constructor == null && baseClasses.Count > 0)
            {
                var baseClass = baseClasses.Find(c =>
                {
                    var ctor = c.GetConstructor();
                    return (ctor != null && ctor.Parameters.Count > 0);
                });

                if (baseClass != null)
                {
                    Constructor baseCtor = baseClass.GetConstructor();
                    Constructor newCtor = (Constructor)NodeHelper.CreateNode((JObject)baseCtor.TsNode.DeepClone());

                    CallExpression baseNode = (CallExpression)NodeHelper.CreateNode(NodeKind.CallExpression);
                    baseNode.Expression = NodeHelper.CreateNode(NodeKind.SuperKeyword);
                    foreach (Parameter parameter in baseCtor.Parameters)
                    {
                        baseNode.AddArgument(NodeHelper.CreateNode(NodeKind.Identifier, parameter.Name.Text));
                    }

                    newCtor.Base = baseNode;
                    newCtor.Body.ClearStatements();
                    ModifierNormalizer.NormalizeModify(newCtor);
                    classNode.InsertMember(0, newCtor);
                }
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
                ExpressionStatement statement = (ExpressionStatement)NodeHelper.CreateNode(this.GetInitPropertyStatementString(prop.Name.Text));
                (statement.Expression as BinaryExpression).Right = prop.Initializer;
                ctorBlock.InsertStatement(0, statement);
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
            Node baseInvokeStatement = ctorNode.Body.Statements.Find(s => this.IsBaseConstructor(s));
            if (baseInvokeStatement != null)
            {
                ctorNode.Body.RemoveStatement(baseInvokeStatement);
                Node baseNode = NodeHelper.CreateNode(baseInvokeStatement.TsNode);
                ctorNode.Base = baseNode;
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
