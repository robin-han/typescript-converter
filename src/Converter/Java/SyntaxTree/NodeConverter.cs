using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TypeScript.Syntax;
using TypeScript.Syntax.Converter;
using com.sun.tools.javac.util;
using com.sun.tools.javac.tree;
using com.sun.tools.javac.code;
using static com.sun.tools.javac.tree.JCTree;

namespace TypeScript.Converter.Java
{
    public class NodeConverter
    {
        #region Static Fields
        /// <summary>
        /// The java syntax tree context
        /// </summary>
        private static readonly Context javaSyntaxTreeContext = new Context();

        /// <summary>
        /// Gets the java syntax tree maker.
        /// </summary>
        public static readonly TreeMaker TreeMaker = TreeMaker.instance(javaSyntaxTreeContext);

        /// <summary>
        /// Gets the java syntax tree node names.
        /// </summary>
        public static readonly Names Names = Names.instance(javaSyntaxTreeContext);
        #endregion

        #region Fields
        /// <summary>
        /// The converter context.
        /// </summary>
        private IConvertContext _context;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public NodeConverter() : this(new DefaultConvertContext())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public NodeConverter(IConvertContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._context = context;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the converter context
        /// </summary>
        public IConvertContext Context
        {
            get
            {
                return this._context;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("context");
                }
                this._context = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Filter types.
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        protected List<Node> FilterTypes(List<Node> statements)
        {
            List<string> excluteTypes = this.Context.ExcludeTypes;
            if (excluteTypes.Count == 0)
            {
                return statements;
            }

            return statements.FindAll(statement =>
            {
                switch (statement.Kind)
                {
                    case NodeKind.ClassDeclaration:
                        return !excluteTypes.Contains((statement as ClassDeclaration).NameText);

                    case NodeKind.InterfaceDeclaration:
                        return !excluteTypes.Contains((statement as InterfaceDeclaration).NameText);

                    case NodeKind.EnumDeclaration:
                        return !excluteTypes.Contains((statement as EnumDeclaration).NameText);

                    case NodeKind.TypeAliasDeclaration:
                        return !excluteTypes.Contains((statement as TypeAliasDeclaration).NameText);

                    default:
                        return true;
                }
            });
        }

        /// <summary>
        /// Trim type name's start '_'.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string NormalizeTypeName(string name)
        {
            if (TypeHelper.IsMatchTypeName(name) && Context.Project.TypeDeclarationNames.Contains(name))
            {
                name = name.TrimStart('_');
            }
            return name;
        }

        protected string TrimTypeName(string typeName)
        {
            return this.Context.TrimTypeName(typeName);
        }

        protected bool IsQualifiedName(string name)
        {
            return this.Context.QualifiedNames.Contains(name);
        }
        #endregion

        #region Static Method
        internal static List<T> Nil<T>()
        {
            return new List<T>();
        }

        internal static bool IsGenericTypeArgument(Node typeArg)
        {
            //A[]
            Node parent = typeArg.Parent;
            if (parent != null && parent.Kind == NodeKind.ArrayType)
            {
                return true;
            }

            //Hashtable<K,V>
            Node grandpa = parent?.Parent;
            if (grandpa != null && (grandpa.Kind == NodeKind.IndexSignature || grandpa.Kind == NodeKind.TypeLiteral))
            {
                return true;
            }

            //AAA<T>
            List<Node> typeArguments = parent?.GetValue("TypeArguments") as List<Node>;
            if (typeArguments != null && typeArguments.Contains(typeArg))
            {
                return true;
            }

            return false;
        }

        internal static JCVariableDecl CreateSerialVersionUID()
        {
            return TreeMaker.VarDef(
                TreeMaker.Modifiers(Flags.PRIVATE | Flags.STATIC | Flags.FINAL),
                Names.fromString("serialVersionUID"),
                TreeMaker.TypeIdent(TypeTag.LONG),
                TreeMaker.Literal(TypeTag.LONG, 1)
            );
        }

        internal static JCMethodInvocation CreateImplicitOperatorTree(ImplicitOperator @operator, Node type)
        {
            return CreateImplicitOperatorTree(@operator, type.ToJavaSyntaxTree<JCExpression>());
        }
        internal static JCMethodInvocation CreateImplicitOperatorTree(ImplicitOperator @operator, JCExpression typeExpr)
        {
            string[] convertingMethod = @operator.ConvertingMethod.Split('.');
            return TreeMaker.Apply(
                Nil<JCExpression>(),
                TreeMaker.Select(TreeMaker.Ident(Names.fromString(convertingMethod[0])), Names.fromString(convertingMethod[1])),
                new List<JCExpression>()
                {
                    typeExpr
                }
            );
        }

        internal static List<T> CreateArgumentTreesByParameters<T>(List<Node> arguments, List<Node> parameters) where T : JCTree
        {
            if (arguments.Count == 0)
            {
                return Nil<T>();
            }

            if (parameters.Count >= arguments.Count)
            {
                List<T> args = new List<T>();
                for (int i = 0; i < arguments.Count; i++)
                {
                    Node argument = arguments[i];
                    string fromType = TypeHelper.GetTypeName(TypeHelper.GetNodeType(argument));
                    string toType = TypeHelper.GetTypeName(((Parameter)parameters[i]).Type);
                    var implicitOperator = OperatorConfig.ImplicitOperators.Find(@operator => @operator.From == fromType && @operator.To == toType);
                    if (implicitOperator != null)
                    {
                        args.Add((T)(JCTree)CreateImplicitOperatorTree(implicitOperator, argument));
                    }
                    else
                    {
                        args.Add((T)argument.ToJavaSyntaxTree<JCTree>());
                    }
                }
                return args;
            }

            return arguments.ToJavaSyntaxTrees<T>();
        }

        #endregion

        #region Class DefaultConvertContext
        private class DefaultConvertContext : IConvertContext
        {
            public DefaultConvertContext()
            {
                this.Project = null;
                this.Namespace = "";
                this.TypeScriptType = false;
                this.TypeScriptAdvancedType = false;
                this.Usings = new List<string>();
                this.QualifiedNames = new List<string>();
                this.ExcludeTypes = new List<string>();
            }

            public IProject Project { get; private set; }
            public string Namespace { get; set; }
            public bool TypeScriptType { get; set; }
            public bool TypeScriptAdvancedType { get; set; }
            public List<string> Usings { get; set; }
            public List<string> QualifiedNames { get; set; }
            public List<string> ExcludeTypes { get; set; }

            public IOutput GetOutput(Document doc)
            {
                return null;
            }

            public string TrimTypeName(string typeName)
            {
                return typeName;
            }
        }
        #endregion
    }
}
