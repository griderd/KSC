using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSC
{
    partial class Transpiler
    {
        List<string> output;
        List<string> errors;
        List<string> warnings;

        List<string> tokens;

        /// <summary>
        /// A list of files that make up the runtime.
        /// </summary>
        string[] runtime;

        public Transpiler(kOSVersions version)
        {
            output = new List<string>();
            errors = new List<string>();
            warnings = new List<string>();
            runtime = new string[0];
        }

        public Transpiler(kOSVersions version, string[] runtime)
            : this(version)
        {
            this.runtime = runtime;
        }

        public string[] Translate(string file, out string[] errors, out string[] warnings)
        {
            Tokenize(file);

            errors = this.errors.ToArray();
            warnings = this.warnings.ToArray();

            return output.ToArray();
        }

        void Tokenize(string inputCode)
        {
            string code = inputCode;
            StringBuilder token = new StringBuilder();

            for (int i = 0; i < code.Length; i++)
            {
                token.Append(code[i]);


            }
        }

    }
}
