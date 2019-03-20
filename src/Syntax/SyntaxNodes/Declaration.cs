using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class Declaration : Node
    {

        #region Ignored Properties
        protected int ModifierFlagsCache { get; set; }
        #endregion


        internal bool HasJsDocTag(string tagName)
        {
            List<Node> jsDoc = this.GetValue("JsDoc") as List<Node>;
            if (jsDoc != null && jsDoc.Count > 0)
            {
                JSDocComment docComment = jsDoc[0] as JSDocComment;
                if (docComment != null)
                {
                    return docComment.Tags.Find(tag => tag.Kind == NodeKind.JSDocTag && (tag as JSDocTag).TagName.Text == tagName) != null;
                }
            }
            return false;
        }
    }
}

