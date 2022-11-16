using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Scanner
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
        int State;
        public Machine()
        {
            lexem = new char[100];
            token = Tokens.ID;
            State = 0;
            DFA_Token = _id;
        }
        private delegate void DDFA_Token(char c);
        private DDFA_Token DFA_Token;
        public bool Read(char c)
        {
            if (State == 0)
            {
                if (!_setToken(c))
                    return true;
                else
                    return false;
            }
            else
            {
                DFA_Token(c);
                return false;
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
                    if (c >= '0' && c <= '9')
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
        private void _literal(char c)
        {

        }
        private void _const(char c)
        {

        }
        private void _id(char c)
        {
            
        }
        private void _procedure(char c)
        {

        }
        private void _division(char c)
        {

        }
        private void _end(char c)
        {

        }
        private void _get(char c)
        {

        }
        private void _set(char c)
        {

        }
        private void _to(char c)
        {

        }
    }
}