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
        public C() : base(new string[] {
            "auto", "break", "case", "char",
            "const", "continue", "default", "do",
            "int", "long", "register", "return",
            "short", "signed", "sizeof", "static",
            "struct", "switch", "typedef", "union",
            "unsigned", "void", "volatile", "while",
            "double", "else", "enum", "extern", 
            "float", "for", "goto", "if" })
        {
        }

        TokenCollection tokens;
        StructureTypeCollection declaredTypes;

        StructureType GetType(string name)
        {
            StructureType t;
            if ((declaredTypes != null) && (declaredTypes.TryGetType(name, out t)))
                return t;
            else if (BuiltIns.TryGetBuiltInType(name, out t))
                return t;
            else
                return null;
        }

        bool TryGetType(string name, out StructureType t)
        {
            t = GetType(name);
            return t != null;
        }

        public string[] Tokenize(string str)
        {
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            TokenizerState state = TokenizerState.None;

            Action AddToken = new Action(() =>
            {
                if (sb.Length > 0)
                {
                    tokens.Add(sb.ToString());
                    sb.Clear();
                }
            });

            for (int i = 0; i < str.Length; i++)
            {
                char currentChar = str[i];
                char nextChar = (i + 1 < str.Length) ? str[i + 1] : '\0';
                char thirdChar = (i + 2 < str.Length) ? str[i + 2] : '\0';

                // If new line or whitespace
                if (((currentChar == '\r') & (nextChar == '\n')) | 
                    (currentChar == '\n') | 
                    (currentChar == ' ') |
                    (currentChar == '\t'))
                {
                    AddToken();
                    continue;
                }
                else if (currentChar == '\"')
                {
                    if (state == TokenizerState.InComment)
                    {
                        continue;
                    }
                    else if (state == TokenizerState.InString)
                    {
                        state = TokenizerState.None;
                        tokens.Add("\"");
                        continue;
                    }
                    else if (state == TokenizerState.None)
                    {
                        state = TokenizerState.InString;
                        tokens.Add("\"");
                        continue;
                    }
                }
                else if ((currentChar == '/') & (nextChar == '*'))
                {
                    state = TokenizerState.InComment;
                    i++;
                    continue;
                }
                else if ((currentChar == '*') & (nextChar == '/'))
                {
                    if (state == TokenizerState.InComment)
                    {
                        state = TokenizerState.None;
                        i++;
                        continue;
                    }
                }

                int opGroup = IsOperator(currentChar, nextChar, thirdChar);

                if (opGroup > 0)
                {
                    AddToken();
                    tokens.Add(str.Substring(i, opGroup));
                    i += opGroup;
                }
                else
                {
                    sb.Append(tokens[i]);
                }

                string s = sb.ToString();
                if (IsKeyword(s))
                {
                    tokens.Add(s);
                    sb.Clear();
                }
            }

            return tokens.ToArray();
        }

        int IsOperator(char currentChar, char nextChar, char thirdChar)
        {
            switch (currentChar)
            {
                // Punctuation Tokens
                case '?':
                case ':':
                case '(':
                case ')':
                case '[':
                case ']':
                case '.':
                case ',':
                    return 1;

                // +, ++, +=
                // &, &&, &=
                // |, ||, |=
                case '+':
                case '&':
                case '|':
                    if ((nextChar == currentChar) | (nextChar == '='))
                        return 2;
                    else
                        return 1;
                    
                // -, --, -=, ->
                case '-':
                    switch (nextChar)
                    {
                        case '-':
                        case '=':
                        case '>':
                            return 2;

                        default:
                            return 1;
                    }

                // <, <<, <=, <<=
                // >, >>, >=, >>=
                case '<':
                case '>':
                    if (nextChar == currentChar)
                    {
                        if (thirdChar == '=')
                            return 3;
                        else
                            return 2;
                    }
                    else
                    {
                        return 1;
                    }

                // Assignment Operators
                case '*':
                case '/':
                case '!':
                case '%':
                case '=':
                case '^':
                    if (nextChar == '=')
                        return 2;
                    else
                        return 1;

                default:
                    return 0;
            }
        }

        public bool IsKeyword(string str)
        {
            return keywords.Contains(str);
        }

        public override KSGlobal[] Parse(string str)
        {
            KSGlobal global = new KSGlobal();
            IBlockChild currentLevel = global;

            declaredTypes = new StructureTypeCollection();

            // Setup the initial data
            tokens = new TokenCollection(Tokenize(str));

            // Check that there are tokens
            if (tokens.Length == 0) throw new Exception("Token length is zero. File empty.");

            #region Internal Functions

            Action<string> GetNextTokenExpect = new Action<string>(expected=>
                {
                    if (!tokens.NextToken())
                        Error("Expected " + expected + ".");
                });

            Func<bool> GoUp = new Func<bool>(() =>
            {
                if (currentLevel.Parent != null)
                {
                    currentLevel = currentLevel.Parent;
                    return true;
                }
                return false;
            });
            #endregion 

            // TODO: Parse C language here
            while (tokens.CurrentToken != null)
            {
                // Check that the token is a type
                StructureType type = null;
                if (!declaredTypes.TryGetType(tokens.CurrentToken, out type))
                    type = BuiltIns.GetBuiltInType(tokens.CurrentToken);
                
                if (type != null)
                {
                    // The token is a type, which means that the following tokens indicate either a variable definition or a function.
                    
                    // The next token should be a name.                    
                    string name = "";
                    GetNextTokenExpect("identifier");

                    if (IsKeyword(tokens.CurrentToken))
                    {
                        Error("Keyword cannot be used as an identifier.");
                        break;
                    }
                    else
                    {
                        name = tokens.CurrentToken;
                    }

                    // Next, it needs to be known if this is a function or not. If there's an opening parenthesis, it's a function.
                    GetNextTokenExpect("'(', '[', '=', or ';'");

                    switch (tokens.CurrentToken)
                    {
                        case "(":
                            global.AddChild(ParseFunction(type, name));
                            break;

                        case "[":
                            global.AddChild(ParseArray(type, name));
                            break;

                        case ";":
                            global.AddChild(new KSField(name, true, type));
                            break;

                        case "=":
                            global.AddChild(new KSField(name, true, type));
                            // TODO: Figure out how to add an assignment here
                            break;
                    }
                }
            }

            return new KSGlobal[] { global };
        }

        public override KSGlobal[] Parse(string[] lines)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < lines.Length; i++)
            {
                sb.AppendLine(lines[i]);
            }

            return Parse(sb.ToString());
        }
    }
}
