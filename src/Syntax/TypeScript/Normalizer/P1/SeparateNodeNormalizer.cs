using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class SeparateNodeNormalizer : Normalizer
    {
        public override int Priority
        {
            get { return 10; }
        }

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
                        classNode.RemoveMemberAt(i);

                        List<Node> newMembers = this.Separate(method);
                        classNode.InsertMembers(i++, newMembers);
                    }
                }
            }

            //separate interface member
            if (changedMethods.Count > 0)
            {
                Project project = classNode.Document.Project;
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
                    @interface.RemoveMemberAt(index);

                    List<Node> newMembers = this.Separate(method);
                    @interface.InsertMembers(index, newMembers);
                }
            }
        }

        private List<Node> Separate(MethodDeclaration method)
        {
            List<Node> newMembers = new List<Node>();
            if (method.IsAbstract)
            {
                (method.Parameters[0] as Parameter).SetQuestionToken(null);
                MethodDeclaration newGet = NodeHelper.CreateNode(NodeKind.MethodDeclaration) as MethodDeclaration;
                newGet.Modifiers.AddRange(method.Modifiers);
                newGet.SetName(NodeHelper.CreateNode(method.Name.TsNode));
                newGet.SetType(NodeHelper.CreateNode(method.Type.TsNode));
                newMembers.Add(newGet);

                MethodDeclaration newSet = NodeHelper.CreateNode(NodeKind.MethodDeclaration) as MethodDeclaration;
                newSet.Modifiers.AddRange(method.Modifiers);
                newSet.SetName(NodeHelper.CreateNode(method.Name.TsNode));
                newSet.Parameters.AddRange(method.Parameters);
                newSet.SetType(NodeHelper.CreateNode(NodeKind.VoidKeyword));
                newMembers.Add(newSet);
            }
            else
            {
                if (method.Body.Statements.Count > 0 && method.Body.Statements[0].Kind == NodeKind.IfStatement) // If (arguments....)
                {
                    IfStatement ifStatement = method.Body.Statements[0] as IfStatement;
                    bool ifStatementIsGet = this.IfStatementIsGet(ifStatement);

                    // then
                    JObject thenTsNode = new JObject(method.TsNode);
                    if (ifStatementIsGet)
                    {
                        thenTsNode["parameters"][0].Remove();
                    }
                    else
                    {
                        thenTsNode.Remove("type");
                        (thenTsNode["parameters"][0] as JObject).Remove("questionToken");
                    }
                    thenTsNode["body"]["statements"] = thenTsNode["body"]["statements"][0]["thenStatement"]["statements"];
                    newMembers.Add(NodeHelper.CreateNode(thenTsNode));

                    // else
                    JObject elseTsNode = new JObject(method.TsNode);
                    if (ifStatementIsGet)
                    {
                        elseTsNode.Remove("type");
                        (elseTsNode["parameters"][0] as JObject).Remove("questionToken");
                    }
                    else
                    {
                        elseTsNode["parameters"][0].Remove();
                    }
                    JToken elseStatement = elseTsNode["body"]["statements"][0]["elseStatement"];
                    bool hasElseStatement = elseStatement != null;
                    if (hasElseStatement)
                    {
                        if (elseStatement["kind"].ToObject<string>() == "Block")
                        {
                            elseTsNode["body"]["statements"] = elseStatement = elseStatement["statements"];
                        }
                        else
                        {
                            elseTsNode["body"]["statements"][0] = elseStatement;
                        }
                    }
                    MethodDeclaration elseMethod = NodeHelper.CreateNode(elseTsNode) as MethodDeclaration;
                    elseMethod.SetType(NodeHelper.CreateNode(NodeKind.VoidKeyword));
                    if (!hasElseStatement)
                    {
                        elseMethod.Body.RemoveStatementAt(0);
                    }
                    newMembers.Add(elseMethod);
                }
                //else
                //{
                //    JObject getMethodTsNode = new JObject(method.TsNode);
                //    getMethodTsNode["parameters"][0].Remove();
                //    getMethodTsNode["body"]["statements"] = getMethodTsNode["body"]["statements"];
                //    newMembers.Add(NodeHelper.CreateNode(getMethodTsNode));

                //    JObject setMethodTsNode = new JObject(method.TsNode);
                //    setMethodTsNode.Remove("type");
                //    (setMethodTsNode["parameters"][0] as JObject).Remove("questionToken");
                //    MethodDeclaration setMethod = NodeHelper.CreateNode(setMethodTsNode) as MethodDeclaration;
                //    setMethod.Type = NodeHelper.CreateNode(NodeKind.VoidKeyword);
                //    setMethod.Body.ClearStatements();
                //    newMembers.Add(setMethod);
                //}
            }

            return newMembers;
        }

        private List<Node> Separate(MethodSignature method)
        {
            List<Node> newMembers = new List<Node>();

            MethodSignature setMethod = NodeHelper.CreateNode(method.TsNode) as MethodSignature;
            (setMethod.Parameters[0] as Parameter).SetQuestionToken(null);
            setMethod.SetType(NodeHelper.CreateNode(NodeKind.VoidKeyword));
            newMembers.Add(setMethod);

            MethodSignature getMethod = NodeHelper.CreateNode(method.TsNode) as MethodSignature;
            getMethod.Parameters.Clear();
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
            bool sameType = TypeHelper.IsSameType(parameter.Type, method.Type);
            if (method.IsAbstract && parameter.IsOptional && sameType)
            {
                return true;
            }

            Block body = method.Body;
            if (parameter.IsOptional && sameType && body.Statements.Count == 1 && body.Statements[0].Kind == NodeKind.IfStatement)
            {
                IfStatement ifStatement = body.Statements[0] as IfStatement;
                return (this.IfStatementIsGet(ifStatement) || this.IfStatementIsSet(ifStatement));
            }

            return false;
        }

        private bool CanSeparate(MethodSignature method)
        {
            if (method.Parameters.Count != 1)
            {
                return false;
            }

            Parameter parameter = method.Parameters[0] as Parameter;
            if (parameter.IsOptional && TypeHelper.IsSameType(parameter.Type, method.Type))
            {
                return true;
            }

            return false;
        }

        private bool IfStatementIsGet(IfStatement ifStatement)
        {
            string exprText = ifStatement.Expression.Text.Replace(" ", "");
            return (exprText == "arguments.length<=0" || exprText == "arguments.length==0" || exprText == "arguments.length===0");
        }

        private bool IfStatementIsSet(IfStatement ifStatement)
        {
            string exprText = ifStatement.Expression.Text.Replace(" ", "");
            return (exprText == "arguments.length>0");
        }
        #endregion
    }
}
