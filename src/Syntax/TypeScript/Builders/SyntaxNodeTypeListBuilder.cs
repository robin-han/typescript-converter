using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TypeScript.Syntax;

namespace GrapeCity.Syntax.Converter.Source.TypeScript.Builders
{
    public class SyntaxNodeNameAndTypeDictionaryBuilder
    {
        public static SyntaxNodeNameAndTypeDictionaryBuilder DefaultSyntaxNodeTypeListBuilder = new SyntaxNodeNameAndTypeDictionaryBuilder();

        public Dictionary<string, Type> Build()
        {
            var syntaxNodeTypes = new Dictionary<string, Type>();
            Type[] types = Assembly.GetExecutingAssembly().GetExportedTypes();
            Type nodeType = typeof(Node);
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(nodeType))
                {
                    var nodeKindAttribute = type.GetCustomAttribute<NodeKindAttribute>(false);
                    if (nodeKindAttribute != null)
                    {
                        var nodeKind = nodeKindAttribute.Kind;
                        var key = nodeKind.ToString();
                        if (syntaxNodeTypes.Keys.Contains(key))
                        {
                            throw new RepeatSyntaxNodeTypeException();
                        }
                        syntaxNodeTypes[key] = type;
                    }
                }
            }
            return syntaxNodeTypes;
        }
    }

}
