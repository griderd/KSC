using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSC.Language;
using KSC.Types;

namespace KSC.SourceLanguage
{
    partial class C : Lang
    {
        KSFunction ParseFunction(StructureType returnType, string name)
        {
            KSFunction function = new KSFunction(name, returnType);

            tokens.NextToken();

            while (true)
            {
                StructureType t;
                if (!TryGetType(tokens.CurrentToken, out t))
                {
                    Error("Expected type.");
                    break;
                }

                tokens.NextToken();
                if ((tokens.CurrentToken == null) || (IsKeyword(tokens.CurrentToken)))
                {
                    Error("Expected identifier.");
                    break;
                }

                function.AddParameter(tokens.CurrentToken, t);

                if (tokens.GetNextToken() == ",")
                    continue;
                else if (tokens.CurrentToken == ")")
                    break;
                else
                {
                    Error("Expected ',' or ')'.");
                    break;
                }
            }

            return function;
        }

        KSField ParseArray(StructureType type, string name)
        {
            
        }
    }
}
