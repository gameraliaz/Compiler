using Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Pars
    {
        
        private List<SymbolTable> _symbolsTable;
        private IDictionary<int, string> _codes;
        public Pars(List<SymbolTable> SymbolsTable,IDictionary<int,string> Codes)
        { 
            _symbolsTable = SymbolsTable;
            _codes = Codes;
        }
        public void bottom_up()
        {
            Stack<string> stack = new Stack<string>();
            stack.Push("0");
            int state = 0;
            int[] LinesNumber = _codes.Keys.ToArray();
            Array.Sort(LinesNumber);

            string value = "";
            foreach (int Line in LinesNumber)
            {
                foreach (char c in _codes[Line])
                {
                    if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';' )
                    {

                    }
                    else if( c == '\t' || c == '\r' || c == '\n' || c == '\0' || c == ' ')
                    {
                        foreach(SymbolTable symbol in _symbolsTable)
                        {
                            if (symbol.Value == value)
                            {
                                var action=_LR(state, symbol.Token);
                                switch (action.Item1)
                                {
                                    case Action.shift:
                                        state=action.Item2;
                                        stack.Push(value);
                                        break;
                                    case Action.reduce:

                                        break;
                                    case Action.accept:
                                        break;
                                    case Action.error:
                                        break;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        value += c;
                    }
                }
            }
        }
        private Tuple<Action,int> _LR(int currentstate,Tokens input)
        {
            return new Tuple<Action, int>(Action.accept,1) ;
        }
    }
}
