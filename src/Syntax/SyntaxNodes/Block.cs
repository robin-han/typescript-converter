using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class Block : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.Block; }
        }

        public bool MultiLine
        {
            get;
            private set;
        }
        public List<Node> Statements
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);
            this.Statements = new List<Node>();

            JToken jsonMultiLine = jsonObj["multiLine"];
            this.MultiLine = jsonMultiLine == null ? false : jsonMultiLine.ToObject<bool>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "statements":
                    this.Statements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }


        public void AddStatement(Node statement)
        {
            statement.Parent = this;
            this.Statements.Add(statement);
        }

        public void InsertStatement(int index, Node statement)
        {
            statement.Parent = this;
            this.Statements.Insert(index, statement);
        }

        public void ClearStatements()
        {
            this.Statements.Clear();
        }

        public void RemoveStatement(Node statement)
        {
            this.Statements.Remove(statement);
        }

        public void RemoveStatementAt(int index)
        {
            this.Statements.RemoveAt(index);
        }
    }
}

