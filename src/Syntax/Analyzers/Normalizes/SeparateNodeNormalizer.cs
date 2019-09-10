using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class SeparateNodeNormalizer : Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.ForStatement:
                    this.SeparateForStatement(node as ForStatement);
                    break;

                case NodeKind.ClassDeclaration:
                    this.SeparateClassMembers(node as ClassDeclaration);
                    break;

                default:
                    break;
            }
        }

        #region ForStatement
        private void SeparateForStatement(ForStatement forStatement)
        {
            //separate by comma
            Node initializer = forStatement.Initializer;
            if (initializer != null)
            {
                if (initializer.Kind == NodeKind.BinaryExpression)
                {
                    forStatement.Initializers.AddRange(this.GetExpressions(initializer as BinaryExpression));
                }
                else
                {
                    forStatement.Initializers.Add(initializer);
                }
            }

            Node incrementor = forStatement.Incrementor;
            if (incrementor != null)
            {
                if (incrementor.Kind == NodeKind.BinaryExpression)
                {
                    forStatement.Incrementors.AddRange(this.GetExpressions(incrementor as BinaryExpression));
                }
                else
                {
                    forStatement.Incrementors.Add(incrementor);
                }
            }
        }

        private List<Node> GetExpressions(BinaryExpression exp)
        {
            List<Node> expressions = new List<Node>();

            Queue<BinaryExpression> queue = new Queue<BinaryExpression>();
            queue.Enqueue(exp);
            while (queue.Count > 0)
            {
                BinaryExpression binary = queue.Dequeue();
                if (binary.OperatorToken.Kind != NodeKind.CommaToken)
                {
                    expressions.Add(binary);
                    continue;
                }

                //
                Node left = binary.Left;
                Node right = binary.Right;

                if (left.Kind == NodeKind.BinaryExpression)
                {
                    queue.Enqueue(left as BinaryExpression);
                }
                else
                {
                    expressions.Add(left);
                }

                if (right.Kind == NodeKind.BinaryExpression)
                {
                    queue.Enqueue(right as BinaryExpression);
                }
                else
                {
                    expressions.Add(right);
                }
            }
            expressions.Reverse();

            return expressions;
        }
        #endregion

        #region ClassDeclaration
        //Separate getset method to two method
        private void SeparateClassMembers(ClassDeclaration classNode)
        {
            List<MethodDeclaration> changedMethods = new List<MethodDeclaration>();

            for (int i = 0; i < classNode.Members.Count; i++)
            {
                Node member = classNode.Members[i];

                if (member.Kind == NodeKind.MethodDeclaration)
                {
                    MethodDeclaration method = member as MethodDeclaration;
                    if (this.CanSeparate(method))
                    {
                        changedMethods.Add(method);
                        classNode.Members.RemoveAt(i);

                        List<Node> newMembers = this.Separate(method);
                        foreach (var mem in newMembers)
                        {
                            mem.Parent = classNode;
                        }
                        classNode.Members.InsertRange(i++, newMembers);
                    }
                }
            }

            //separate interface member
            if (changedMethods.Count > 0)
            {
                Project project = classNode.Project;
                foreach (InterfaceDeclaration @interface in project.GetInheritInterfaces(classNode))
                {
                    foreach (MethodDeclaration changedMethod in changedMethods)
                    {
                        this.SeparateInterfaceMember(@interface, changedMethod.Name.Text);
                    }
                }
            }
        }

        private void SeparateInterfaceMember(InterfaceDeclaration @interface, string memberName)
        {
            Node member = @interface.GetMember(memberName);
            if (member != null && member.Kind == NodeKind.MethodSignature)
            {
                MethodSignature method = member as MethodSignature;
                if (CanSeparate(method))
                {
                    int index = @interface.Members.IndexOf(method);
                    @interface.Members.RemoveAt(index);

                    List<Node> newMembers = this.Separate(method);
                    foreach (var mem in newMembers)
                    {
                        mem.Parent = @interface;
                    }
                    @interface.Members.InsertRange(index, newMembers);
                }
            }
        }

        private List<Node> Separate(MethodDeclaration method)
        {
            List<Node> newMembers = new List<Node>();
            if (method.IsAbstract)
            {
                (method.Parameters[0] as Parameter).QuestionToken = null;
                MethodDeclaration newGet = NodeHelper.CreateNode(NodeKind.MethodDeclaration) as MethodDeclaration;
                newGet.Modifiers.AddRange(method.Modifiers);
                newGet.Name = method.Name;
                newGet.Type = method.Type;
                newMembers.Add(newGet);

                MethodDeclaration newSet = NodeHelper.CreateNode(NodeKind.MethodDeclaration) as MethodDeclaration;
                newSet.Modifiers.AddRange(method.Modifiers);
                newSet.Name = method.Name;
                newSet.Parameters.AddRange(method.Parameters);
                newSet.Type = NodeHelper.CreateNode(NodeKind.VoidKeyword);
                newMembers.Add(newSet);
            }
            else
            {
                JObject getMethod = new JObject(method.TsNode);
                getMethod["parameters"][0].Remove();
                getMethod["body"]["statements"] = getMethod["body"]["statements"][0]["thenStatement"]["statements"];
                newMembers.Add(NodeHelper.CreateNode(getMethod));

                JObject setMethod = new JObject(method.TsNode);
                setMethod.Remove("type");
                (setMethod["parameters"][0] as JObject).Remove("questionToken");
                JToken elseStatement = setMethod["body"]["statements"][0]["elseStatement"];
                if (elseStatement["kind"].ToObject<string>() == "Block")
                {
                    setMethod["body"]["statements"] = elseStatement = elseStatement["statements"];
                }
                else
                {
                    setMethod["body"]["statements"][0] = elseStatement;
                }
                MethodDeclaration newSet = NodeHelper.CreateNode(setMethod) as MethodDeclaration;
                newSet.Type = NodeHelper.CreateNode(NodeKind.VoidKeyword);
                newMembers.Add(newSet);
            }

            return newMembers;
        }

        private List<Node> Separate(MethodSignature method)
        {
            List<Node> newMembers = new List<Node>();

            MethodSignature setMethod = NodeHelper.CreateNode(method.TsNode) as MethodSignature;
            (setMethod.Parameters[0] as Parameter).QuestionToken = null;
            setMethod.Type = NodeHelper.CreateNode(NodeKind.VoidKeyword);
            newMembers.Add(setMethod);

            MethodSignature getMethod = NodeHelper.CreateNode(method.TsNode) as MethodSignature;
            getMethod.Parameters = new List<Node>();
            newMembers.Add(getMethod);

            return newMembers;
        }

        private bool CanSeparate(MethodDeclaration method)
        {
            if (method.Parameters.Count != 1)
            {
                return false;
            }

            Parameter parameter = method.Parameters[0] as Parameter;
            Block body = method.Body as Block;

            if (method.IsAbstract && parameter.IsOptional)
            {
                return true;
            }

            if (!parameter.IsOptional || body == null || body.Statements.Count != 1 || body.Statements[0].Kind != NodeKind.IfStatement)
            {
                return false;
            }

            IfStatement ifStatement = body.Statements[0] as IfStatement;
            if (ifStatement.ElseStatement == null)
            {
                return false;
            }
            string exprText = ifStatement.Expression.Text.Replace(" ", "");
            return (exprText == "arguments.length<=0" || exprText == "arguments.length==0" || exprText == "arguments.length===0");
        }

        private bool CanSeparate(MethodSignature method)
        {
            if (method.Parameters.Count == 1)
            {
                Parameter parameter = method.Parameters[0] as Parameter;
                if (parameter.IsOptional && parameter.Type.Text == method.Type.Text)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
