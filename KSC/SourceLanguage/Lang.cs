using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSC.Language;

namespace KSC.SourceLanguage
{
    abstract class Lang
    {
        public enum TokenizerState
        {
            InComment,
            InString,
            None
        }

        List<string> errors, warnings;

        protected string[] keywords;

        public string[] Errors { get { return errors.ToArray(); } }
        public string[] Warnings { get { return warnings.ToArray(); } }

        public Lang(string[] keywords)
        {
            if (keywords == null)
                throw new ArgumentNullException();
            this.keywords = keywords;

            errors = new List<string>();
            warnings = new List<string>();
        }

        public abstract string[] Tokenize(string str);

        public abstract KSGlobal[] Parse(string str);

        public abstract KSGlobal[] Parse(string[] lines);

        public bool IsKeyword(string str)
        {
            return keywords.Contains(str);
        }

        protected void Error(string error)
        {
            errors.Add("Error: " + error);
        }

        protected void Warning(string warning)
        {
            warnings.Add("Warning: " + warning);
        }
    }
}
