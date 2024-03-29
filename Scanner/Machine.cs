﻿namespace Scanner
{
    internal class Machine
    {
        private char[] lexem;

        public char[] Lexem
        {
            get { return lexem; }
            set { lexem = value; }
        }
        private Tokens token;

        public Tokens Token
        {
            get { return token; }
            set { token = value; }
        }
        public int State;
        public Machine()
        {
            lexem = new char[100];
            token = Tokens.ID;
            State = 0;
            DFA_Token = _id;
        }
        private delegate ClassLex DDFA_Token(char c);
        private DDFA_Token DFA_Token;
        public ClassLex Read(char c)
        {
            if (State == 0)
            {
                if (!_setToken(c))
                    return ClassLex.Accept;
                else
                    return ClassLex.OnWork;
            }
            else
            {
                return DFA_Token(c);
            }

        }

        private bool _setToken(char c)
        {
            lexem[0] = c;
            lexem[1] = '\0';
            State = 1;
            switch (c)
            {
                case 'p':
                    Token = Tokens.KW_procedure;
                    DFA_Token = _procedure;
                    return true;
                case 'd':
                    Token = Tokens.KW_division;
                    DFA_Token = _division;
                    return true;
                case 'e':
                    Token = Tokens.KW_end;
                    DFA_Token = _end;
                    return true;
                case 's':
                    Token = Tokens.KW_set;
                    DFA_Token = _set;
                    return true;
                case 't':
                    Token = Tokens.KW_to;
                    DFA_Token = _to;
                    return true;
                case 'g':
                    Token = Tokens.KW_get;
                    DFA_Token = _get;
                    return true;
                default:
                    if (c > '0' && c <= '9')
                    {
                        Token = Tokens.Const;
                        DFA_Token = _const;
                    }
                    else if (c == '\'')
                    {
                        Token = Tokens.Literal;
                        DFA_Token = _literal;
                    }
                    else if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')
                    {
                        Token = Tokens.ID;
                        DFA_Token = _id;
                    }
                    else
                    {
                        if (c == ';')
                            Token = Tokens.OP_sim;
                        else if (c == ',')
                            Token = Tokens.OP_cam;
                        else if (c == '+')
                            Token = Tokens.OP_add;
                        else if (c == '-')
                            Token = Tokens.OP_sub;
                        else if (c == '*')
                            Token = Tokens.OP_mul;
                        else if (c == '/')
                            Token = Tokens.OP_div;
                        else if (c == '(')
                            Token = Tokens.OP_lpa;
                        else if (c == ')')
                            Token = Tokens.OP_rpa;
                        return false;
                    }
                    return true;
            }
        }

        // dfa of all tokens
        private ClassLex _literal(char c)
        {
            switch (State)
            {
                case 1:
                    if (c == '\'')
                    {
                        State = -1;
                        return ClassLex.Error;
                    }
                    else
                    {
                        lexem[State++] = c;
                        lexem[State] = '\0';
                        return ClassLex.OnWork;
                    }
                default:
                    if (State >= 2)
                    {
                        if (c == '\'')
                        {
                            lexem[State] = c;
                            lexem[State + 1] = '\0';
                            State = -2;
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }

                    }
                    else
                    {
                        State = -3;
                        return ClassLex.AcceptStar;
                    }
            }
        }

        private ClassLex _const(char c)
        {
            if (c >= '0' && c <= '9')
            {
                lexem[State++] = c;
                lexem[State] = '\0';
                return ClassLex.OnWork;
            }
            else if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';' || c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0')
            {
                State = -1;
                return ClassLex.AcceptStar;
            }
            else
            {
                State = -2;
                return ClassLex.Error;
            }
        }
        private ClassLex _id(char c)
        {
            if (State > 8)
            {
                State = -1;
                return ClassLex.Error;
            }
            else if (State == 8)
            {
                if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';' || c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0')
                {
                    State = 9;
                    return ClassLex.AcceptStar;
                }
                else
                {
                    State = -1;
                    return ClassLex.Error;
                }
            }
            else
            {
                if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';' || c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0')
                {
                    State = -3;
                    return ClassLex.AcceptStar;
                }
                else
                {
                    if (c >= '0' && c <= '9' || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')
                    {
                        lexem[State++] = c;
                        lexem[State] = '\0';
                        return ClassLex.OnWork;
                    }
                    else
                    {
                        State = -2;
                        return ClassLex.Error;
                    }
                }
            }
        }
        private ClassLex _procedure(char c)
        {
            switch (State)
            {
                case 1:
                    {
                        if (c == 'r')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else if (c == 'u')
                        {
                            Token = Tokens.KW_put;
                            DFA_Token = _put;
                            return DFA_Token(c);
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 2:
                    {
                        if (c == 'o')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 3:
                    {
                        if (c == 'c')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 4:
                    {
                        if (c == 'e')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 5:
                    {
                        if (c == 'd')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 6:
                    {
                        if (c == 'u')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 7:
                    {
                        if (c == 'r')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 8:
                    {
                        if (c == 'e')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 9:
                    {
                        if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';' || c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0')
                        {
                            State = -1;
                            return ClassLex.AcceptStar;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
            }
            return ClassLex.AcceptStar;
        }
        private ClassLex _division(char c)
        {
            switch (State)
            {
                case 1:
                    {
                        if (c == 'i')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 2:
                    {
                        if (c == 'v')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 3:
                    {
                        if (c == 'i')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 4:
                    {
                        if (c == 's')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 5:
                    {
                        if (c == 'i')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 6:
                    {
                        if (c == 'o')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 7:
                    {
                        if (c == 'n')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 8:
                    {
                        if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';' || c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0')
                        {
                            State = -1;
                            return ClassLex.AcceptStar;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
            }
            return ClassLex.AcceptStar;
        }
        private ClassLex _end(char c)
        {
            switch (State)
            {
                case 1:
                    {
                        if (c == 'n')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 2:
                    {
                        if (c == 'd')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 3:
                    {
                        if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';' || c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0')
                        {
                            State = -1;
                            return ClassLex.AcceptStar;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
            }
            return ClassLex.AcceptStar;
        }
        private ClassLex _get(char c)
        {
            switch (State)
            {
                case 1:
                    {
                        if (c == 'e')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 2:
                    {
                        if (c == 't')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 3:
                    {
                        if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';' || c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0')
                        {
                            State = -1;
                            return ClassLex.AcceptStar;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
            }
            return ClassLex.AcceptStar;
        }
        private ClassLex _set(char c)
        {
            return _get(c);
        }
        private ClassLex _to(char c)
        {
            switch (State)
            {
                case 1:
                    {
                        if (c == 'o')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 2:
                    {
                        if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';' || c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0')
                        {
                            State = -1;
                            return ClassLex.AcceptStar;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
            }
            return ClassLex.AcceptStar;
        }
        private ClassLex _put(char c)
        {
            switch (State)
            {
                case 2:
                    {
                        if (c == 't')
                        {
                            lexem[State++] = c;
                            lexem[State] = '\0';
                            return ClassLex.OnWork;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
                case 3:
                    {
                        if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';' || c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0')
                        {
                            State = -1;
                            return ClassLex.AcceptStar;
                        }
                        else
                        {
                            Token = Tokens.ID;
                            DFA_Token = _id;
                            return DFA_Token(c);
                        }
                    }
            }
            return ClassLex.AcceptStar;
        }
    }
}