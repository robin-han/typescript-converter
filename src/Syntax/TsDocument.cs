using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class TsDocument
    {
        #region Properties
        public TsDocument()
        {
            this.RootNode = null;
            this.Path = string.Empty;
        }

        public Node RootNode
        {
            get;
            internal set;
        }

        public string Path
        {
            get;
            internal set;
        }

        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(this.Path);
            }
        }
        #endregion


        public List<string> GetNodeKinds()
        {
            List<string> kinds = new List<string>();
            List<Node> nodes = this.RootNode.DescendantsAndSelf();

            foreach (Node node in nodes)
            {
                string kind = node.Kind.ToString();
                if (!kinds.Contains(kind))
                {
                    kinds.Add(kind);
                }
            }

            return kinds;
        }

        public Dictionary<string, string> GetLostNodes()
        {
            Dictionary<string, string> lost = new Dictionary<string, string>();
            List<Node> nodes = this.RootNode.DescendantsAndSelf();

            foreach (Node node in nodes)
            {
                string key = node.Kind.ToString();
                if (lost.ContainsKey(key))
                {
                    continue;
                }

                List<string> ignoredProperties = new List<string>();
                List<string> definedProperties = new List<string>();
                PropertyInfo[] properties = node.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (PropertyInfo p in properties)
                {
                    definedProperties.Add(p.Name.ToLower());
                }

                foreach (var p in node.TsNode.Properties())
                {
                    if (!definedProperties.Contains(p.Name.ToLower()))
                    {
                        ignoredProperties.Add(p.Name);
                    }
                }

                if (ignoredProperties.Count > 0)
                {
                    lost[key] = string.Join(',', ignoredProperties);
                }
            }

            return lost;
        }

    }
}
