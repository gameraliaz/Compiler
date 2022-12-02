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
        
        
        public bool bottom_up(SLRParsingTable slrpt)
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
                        foreach (SymbolTable symbol in _symbolsTable)
                        {
                            if (symbol.Value == value)
                            {
                                bool ch = true;
                                while (ch)
                                {
                                    var action = slrpt.GetAction(state, symbol.Token.ToString());
                                    switch (action.Act)
                                    {
                                        case TypeOfAction.shift:
                                            state = action.State;
                                            stack.Push(value);
                                            stack.Push(state.ToString());
                                            ch = false;
                                            break;
                                        case TypeOfAction.reduce:
                                            for (int i = 0; i < 2 * slrpt._grammer.Rules[action.State].To.Count; i++)
                                                stack.Pop();
                                            state = Convert.ToInt32(stack.Peek());
                                            stack.Push(slrpt._grammer.Rules[action.State].From);
                                            state = slrpt.GetGoTo(state, slrpt._grammer.Rules[action.State].From);
                                            stack.Push(state.ToString());
                                            break;
                                        case TypeOfAction.error:
                                            return false;
                                    }
                                }
                                break;
                            }
                        }
                        value = c.ToString();
                        foreach (SymbolTable symbol in _symbolsTable)
                        {
                            if (symbol.Value == value)
                            {
                                bool ch = true;
                                while (ch)
                                {
                                    var action = slrpt.GetAction(state, symbol.Token.ToString());
                                    switch (action.Act)
                                    {
                                        case TypeOfAction.shift:
                                            state = action.State;
                                            stack.Push(value);
                                            stack.Push(state.ToString());
                                            ch = false;
                                            break;
                                        case TypeOfAction.reduce:
                                            for (int i = 0; i < 2 * slrpt._grammer.Rules[action.State].To.Count; i++)
                                                stack.Pop();
                                            state = Convert.ToInt32(stack.Peek());
                                            stack.Push(slrpt._grammer.Rules[action.State].From);
                                            state = slrpt.GetGoTo(state, slrpt._grammer.Rules[action.State].From);
                                            stack.Push(state.ToString());
                                            break;
                                        case TypeOfAction.error:
                                            return false;
                                    }
                                }
                                break;
                            }
                        }
                        value = "";
                    }
                    else if( c == '\t' || c == '\r' || c == '\n' || c == '\0' || c == ' ')
                    {
                        foreach(SymbolTable symbol in _symbolsTable)
                        {
                            if (symbol.Value == value)
                            {
                                bool ch=true;
                                while (ch)
                                {
                                    var action = slrpt.GetAction(state, symbol.Token.ToString());
                                    switch (action.Act)
                                    {
                                        case TypeOfAction.shift:
                                            state = action.State;
                                            stack.Push(value);
                                            stack.Push(state.ToString());
                                            ch= false;
                                            break;
                                        case TypeOfAction.reduce:
                                            for (int i = 0; i < 2 * slrpt._grammer.Rules[action.State].To.Count; i++)
                                                stack.Pop();
                                            state = Convert.ToInt32(stack.Peek());
                                            stack.Push(slrpt._grammer.Rules[action.State].From);
                                            state = slrpt.GetGoTo(state, slrpt._grammer.Rules[action.State].From);
                                            stack.Push(state.ToString());
                                            break;
                                        case TypeOfAction.error:
                                            return false;
                                    }
                                }
                                break;
                            }
                        }
                        value = "";
                    }
                    else
                    {
                        value += c;
                    }
                }
            }
            var l=slrpt.GetAction(state, "$");
            if (l.Act == TypeOfAction.accept)
                return true;
            return false;
        }
    }
}
