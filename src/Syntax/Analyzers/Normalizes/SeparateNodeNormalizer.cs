using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
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
            for (int i = 0; i < classNode.Members.Count; i++)
            {
                Node member = classNode.Members[i];

                if (this.CanSeparate(member))
                {
                    classNode.Members.RemoveAt(i);
                    classNode.Members.InsertRange(i++, this.SeparateMember(member));
                }
            }
        }

        private List<Node> SeparateMember(Node member)
        {
            List<Node> newMembers = new List<Node>();

            JObject getMethod = new JObject(member.TsNode);
            getMethod["parameters"][0].Remove();
            getMethod["body"]["statements"] = getMethod["body"]["statements"][0]["thenStatement"]["statements"];
            newMembers.Add(member.CreateNode(getMethod));

            JObject setMethod = new JObject(member.TsNode);
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
            newMembers.Add(member.CreateNode(setMethod));

            return newMembers;
        }

        private bool CanSeparate(Node node)
        {
            if (node.Kind != NodeKind.MethodDeclaration || (node as MethodDeclaration).Parameters.Count != 1)
            {
                return false;
            }

            MethodDeclaration method = node as MethodDeclaration;
            Parameter parameter = method.Parameters[0] as Parameter;
            Block body = method.Body as Block;
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
        #endregion
    }
}
