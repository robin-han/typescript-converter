using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax.Analysis
{
    /// <summary>
    /// Normalizer
    /// </summary>
    public class ConstructorNormalizer : Normalizer
    {
        public override int Priority
        {
            get { return 200; }
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
            if (constructor == null)
            {
                Document doc = classNode.Document;
                List<ClassDeclaration> baseClasses = doc.Project.GetInheritClasses(classNode);

                var baseCtorClass = baseClasses.Find(c =>
                {
                    var ctor = c.GetConstructor();
                    return (ctor != null && ctor.Parameters.Count > 0);
                });

                if (baseCtorClass != null)
                {
                    Constructor baseCtor = baseCtorClass.GetConstructor();
                    Constructor newCtor = (Constructor)NodeHelper.CreateNode((JObject)baseCtor.TsNode.DeepClone());

                    ExpressionStatement baseStatement = (ExpressionStatement)NodeHelper.CreateNode(NodeKind.ExpressionStatement);
                    CallExpression baseNode = (CallExpression)NodeHelper.CreateNode(NodeKind.CallExpression);
                    baseNode.SetExpression(NodeHelper.CreateNode(NodeKind.SuperKeyword));
                    foreach (Parameter parameter in baseCtor.Parameters)
                    {
                        baseNode.AddArgument(NodeHelper.CreateNode(NodeKind.Identifier, parameter.Name.Text));
                    }

                    baseStatement.SetExpression(baseNode);
                    newCtor.JsDoc.Clear();
                    newCtor.Body.ClearStatements();
                    newCtor.Body.InsertStatement(0, baseStatement);
                    ModifierNormalizer.NormalizeModify(newCtor);
                    classNode.InsertMember(0, newCtor);
                }
                else if (this.GetInitProperties(classNode).Count > 0)
                {
                    Constructor newCtor = (Constructor)NodeHelper.CreateNode(BuildConstructorString());
                    ModifierNormalizer.NormalizeModify(newCtor);
                    classNode.InsertMember(0, newCtor);
                }
            }
        }

        private void NormalizeConstructor(Constructor ctorNode)
        {
            this.AddPropertyInitStatements(ctorNode);
        }

        private void AddPropertyInitStatements(Constructor ctorNode)
        {
            ClassDeclaration classNode = ctorNode.Parent as ClassDeclaration;
            if (classNode == null)
            {
                return;
            }

            int insertIndex = (ctorNode.Base == null ? 0 : 1);
            Block ctorBlock = ctorNode.Body as Block;
            List<PropertyDeclaration> props = this.GetInitProperties(classNode);
            props.Reverse();
            foreach (PropertyDeclaration prop in props)
            {
                ExpressionStatement statement = (ExpressionStatement)NodeHelper.CreateNode(this.BuildPropertyStatementString(prop.Name.Text));
                (statement.Expression as BinaryExpression).SetRight(prop.Initializer, false);
                ctorBlock.InsertStatement(insertIndex, statement);
            }
        }

        private string BuildPropertyStatementString(string propName)
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

        private string BuildConstructorString()
        {
            return
            "{" +
                "kind: \"Constructor\", " +
                "body: {" +
                    "kind: \"Block\"" +
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
                    if (prop.Initializer != null && prop.IsPublic && !prop.IsStatic)
                    {
                        ret.Add(prop);
                    }
                }
            }
            return ret;
        }
    }
}
