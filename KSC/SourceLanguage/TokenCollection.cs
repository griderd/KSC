using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSC.SourceLanguage
{
    class TokenCollection
    {
        List<string> tokens;
        int tokenId = 0;

        public string CurrentToken
        {
            get
            {
                if ((tokenId >= tokens.Count) | (tokenId < 0))
                    return null;
                else
                    return tokens[tokenId];
            }
        }

        public int Length { get { return tokens.Count; } }

        public TokenCollection(string[] tokens)
        {
            this.tokens = new List<string>(tokens);
        }

        public bool NextToken()
        {
            tokenId++;
            return ((tokenId < tokens.Count) & (tokenId >= 0));
        }

        public string GetNextToken()
        {
            NextToken();
            return CurrentToken;
        }
    }
}
