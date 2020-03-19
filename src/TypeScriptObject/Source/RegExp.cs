using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TypeScript.CSharp
{
    public class RegExp : Regex
    {
        #region Constructor
        public RegExp(string pattern, string flags = "") : base(pattern,
            (flags.Contains("i") ? RegexOptions.IgnoreCase : RegexOptions.None) |
            (flags.Contains("m") ? RegexOptions.Multiline : RegexOptions.None))
        {
            this.flags = flags;
            this.lastIndex = 0;
            foreach (var f in flags)
            {
                switch (f)
                {
                    case 'i':
                        this.ignoreCase = true;
                        break;

                    case 'g':
                        this.global = true;
                        break;

                    case 'm':
                        this.multiline = true;
                        break;

                    default:
                        throw new ArgumentException(string.Format("Invaid flag {0}", f));
                }
            }
        }

        #endregion

        #region Properties
        public string flags
        {
            get;
            private set;
        }

        public bool ignoreCase
        {
            get;
            private set;
        }

        public bool multiline
        {
            get;
            private set;
        }

        public bool global
        {
            get;
            private set;
        }

        public string source
        {
            get { return this.pattern; }
        }

        public int lastIndex
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public bool test(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            string value = this.lastIndex > 0 ? input.Substring(this.lastIndex) : input;
            Match match = this.Match(value);

            if (this.global)
            {
                this.lastIndex = match.Success ? match.Index + match.Length : 0;
            }

            return match.Success;
        }

        public RegExpArray exec(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            string value = this.lastIndex > 0 ? input.Substring(this.lastIndex) : input;
            Match match = this.Match(value);

            if (this.global)
            {
                this.lastIndex = match.Success ? match.Index + match.Length : 0;
            }
            if (match.Success)
            {
                RegExpArray ret = new RegExpArray();
                foreach (Group g in match.Groups)
                {
                    if (g.Success)
                    {
                        ret.Add(g.Value);
                    }
                    else
                    {
                        ret.Add(null);
                    }
                }
                ret.input = input;
                ret.index = match.Index;
                return ret;
            }

            return null;
        }
        #endregion
    }
}
