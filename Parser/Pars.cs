﻿using Scanner;

namespace Parser
{
    public class Pars
    {
        //prints output (exeptions)
        public string Result { get; set; }

        private List<SymbolTable> _symbolsTable;
        private IDictionary<int, string> _codes;
        public Pars(List<SymbolTable> SymbolsTable, IDictionary<int, string> Codes)
        {
            _symbolsTable = SymbolsTable;
            _codes = Codes;
            Result = "";
        }


        public bool bottom_up(SLRParsingTable slrpt)
        {
            if (!slrpt.IsSLRGrammer())
            {
                Result = "This isn't SLRGrammer!";
                return false;
            }
            int lastl = 0;
            SymbolTable lastsymbol = new();
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
                    if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';')
                    {
                        foreach (SymbolTable symbol in _symbolsTable)
                        {
                            if (symbol.Value == value)
                            {
                                lastl = Line;
                                lastsymbol=symbol;
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
                                            _SLRErrorHandeler(state, symbol, slrpt, Line);
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
                                lastl = Line;
                                lastsymbol = symbol;
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
                                            _SLRErrorHandeler(state, symbol, slrpt, Line);
                                            return false;
                                    }
                                }
                                break;
                            }
                        }
                        value = "";
                    }
                    else if (c == '\t' || c == '\r' || c == '\n' || c == '\0' || c == ' ')
                    {
                        foreach (SymbolTable symbol in _symbolsTable)
                        {
                            if (symbol.Value == value)
                            {
                                lastl = Line;
                                lastsymbol = symbol;
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
                                            _SLRErrorHandeler(state, symbol, slrpt, Line);
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
            var l = slrpt.GetAction(state, "$");
            if (l.Act == TypeOfAction.accept)
            {
                Result = "Syntax is ok!";
                return true;
            }
            _SLRErrorHandeler(state, lastsymbol, slrpt, lastl);
            return false;
        }

        private void _SLRErrorHandeler(int state, SymbolTable input, SLRParsingTable slrpt, int linenum)
        {
            Result += "Error in line " + linenum.ToString() + ": Syntax error ,expected (";
            var expecteds = slrpt.ValidInputsForAState(state);
            bool nothing = true;
            foreach (var expected in expecteds)
            {
                Result += expected + " or ";
                nothing = false;
            }
            if (!nothing)
                Result = Result[..^4] + ")";
            else
                Result = Result[..^10];
            Result += " near the (" + input.Value + "(" + input.Token.ToString() + ")" + ").";
        }

        public bool top_down(PredictiveParsingTable ppt)
        {
            if (!ppt.IsLL1Grammer())
            {
                Result = "This isn't LL(1) Grammer!";
                return false;
            }
            int lastl=0;
            SymbolTable lastsymbol=new();
            Stack<string> stack = new Stack<string>();
            stack.Push("$");
            stack.Push(ppt._grammer.Rules[0].From);
            string nonterminal = ppt._grammer.Rules[0].From;
            int[] LinesNumber = _codes.Keys.ToArray();
            Array.Sort(LinesNumber);

            string value = "";
            foreach (int Line in LinesNumber)
            {
                foreach (char c in _codes[Line])
                {
                    if (c == ')' || c == '(' || c == '+' || c == '-' || c == '/' || c == '*' || c == ',' || c == ';')
                    {
                        foreach (SymbolTable symbol in _symbolsTable)
                        {
                            if (symbol.Value == value)
                            {
                                lastsymbol = symbol;
                                lastl = Line;
                                bool ch = true;
                                while (ch)
                                {
                                    if (symbol.Token.ToString() == nonterminal)
                                    {
                                        stack.Pop();
                                        nonterminal = stack.Peek();
                                        ch = false;
                                        continue;
                                    }
                                    var action = ppt.GetAction(nonterminal, symbol.Token.ToString());
                                    switch (action.Act)
                                    {
                                        case TypeOfAction.accept:
                                            stack.Pop();
                                            nonterminal = stack.Peek();
                                            for (int i = ppt._actions[action.State].Item3.To.Count - 1; i >= 0; i--)//string s in ppt._actions[action.State].Item3.To
                                            {
                                                if (ppt._actions[action.State].Item3.To[i] != "#")
                                                {
                                                    stack.Push(ppt._actions[action.State].Item3.To[i]);
                                                    nonterminal = ppt._actions[action.State].Item3.To[i];
                                                }
                                            }
                                            break;
                                        case TypeOfAction.error:
                                            _PredictiveErrorHandeler(nonterminal, symbol, ppt, Line);
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
                                lastsymbol = symbol;
                                lastl = Line;
                                bool ch = true;
                                while (ch)
                                {
                                    if (symbol.Token.ToString() == nonterminal)
                                    {
                                        stack.Pop();
                                        nonterminal = stack.Peek();
                                        ch = false;
                                        continue;
                                    }
                                    var action = ppt.GetAction(nonterminal, symbol.Token.ToString());
                                    switch (action.Act)
                                    {
                                        case TypeOfAction.accept:
                                            stack.Pop();
                                            nonterminal = stack.Peek();
                                            for (int i = ppt._actions[action.State].Item3.To.Count - 1; i >= 0; i--)//string s in ppt._actions[action.State].Item3.To
                                            {
                                                if (ppt._actions[action.State].Item3.To[i] != "#")
                                                {
                                                    stack.Push(ppt._actions[action.State].Item3.To[i]);
                                                    nonterminal = ppt._actions[action.State].Item3.To[i];
                                                }
                                            }
                                            break;
                                        case TypeOfAction.error:
                                            _PredictiveErrorHandeler(nonterminal, symbol, ppt, Line);
                                            return false;
                                    }
                                }
                                break;
                            }
                        }
                        value = "";
                    }
                    else if (c == '\t' || c == '\r' || c == '\n' || c == '\0' || c == ' ')
                    {
                        foreach (SymbolTable symbol in _symbolsTable)
                        {
                            if (symbol.Value == value)
                            {
                                lastsymbol = symbol;
                                lastl = Line;
                                bool ch = true;
                                while (ch)
                                {
                                    if (symbol.Token.ToString() == nonterminal)
                                    {
                                        stack.Pop();
                                        nonterminal = stack.Peek();
                                        ch = false;
                                        continue;
                                    }
                                    var action = ppt.GetAction(nonterminal, symbol.Token.ToString());
                                    switch (action.Act)
                                    {
                                        case TypeOfAction.accept:
                                            stack.Pop();
                                            nonterminal = stack.Peek();
                                            for (int i = ppt._actions[action.State].Item3.To.Count - 1; i >= 0; i--)//string s in ppt._actions[action.State].Item3.To
                                            {
                                                if (ppt._actions[action.State].Item3.To[i] != "#")
                                                {
                                                    stack.Push(ppt._actions[action.State].Item3.To[i]);
                                                    nonterminal = ppt._actions[action.State].Item3.To[i];
                                                }
                                            }
                                            break;
                                        case TypeOfAction.error:
                                            _PredictiveErrorHandeler(nonterminal, symbol, ppt, Line);
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
            while (stack.Count > 1)
            {
                nonterminal = stack.Pop();
                var action = ppt.GetAction(nonterminal, "$");
                switch (action.Act)
                {
                    case TypeOfAction.accept:
                        stack.Pop();
                        for (int i = ppt._actions[action.State].Item3.To.Count - 1; i >= 0; i--)//string s in ppt._actions[action.State].Item3.To
                        {
                            if (ppt._actions[action.State].Item3.To[i] != "#")
                            {
                                stack.Push(ppt._actions[action.State].Item3.To[i]);
                                nonterminal = ppt._actions[action.State].Item3.To[i];
                            }
                        }
                        break;
                    case TypeOfAction.error:
                        _PredictiveErrorHandeler(nonterminal, lastsymbol, ppt, lastl);
                        return false;
                }
            }
            Result = "Syntax is ok!";
            return true;
        }

        private void _PredictiveErrorHandeler(string nonterminal, SymbolTable input, PredictiveParsingTable ppt, int linenum)
        {
            Result += "Error in line " + linenum.ToString() + ": Syntax error ,expected (";
            var expecteds = ppt.ValidInputsForAState(nonterminal);

            bool nothing = true;
            foreach (var expected in expecteds)
            {
                Result += expected + " or ";
                nothing = false;
            }
            if (!nothing)
                Result = Result[..^4] + ")";
            else
                Result = Result[..^10];
            Result += " near the (" + input.Value + "(" + input.Token.ToString() + ")" + ").";
        }

    }
}
