using Scanner;

namespace Parser
{
    public class PredictiveParsingTable
    {
        public Grammer _grammer;
        public List<Tuple<string, string, Rule>> _actions;
        public PredictiveParsingTable(Grammer Grammer)
        {
            _actions = new List<Tuple<string, string, Rule>>();
            _grammer = Grammer;
            Run();
        }
        public Action GetAction(string Nonterminal, string input)
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                if (_actions[i].Item1 == Nonterminal && _actions[i].Item2 == input)
                    return new Action() { Act = TypeOfAction.accept, State = i };
            }
            return new();
        }
        private void Run()
        {
            foreach (var rule in _grammer.Rules)
            {
                var a = First(rule.To);
                foreach (var item in a)
                {
                    if (item == "#")
                    {
                        var b = Follow(rule.From);
                        foreach (var item2 in b)
                        {
                            _actions.Add(new Tuple<string, string, Rule>(rule.From, item2, rule));
                        }
                    }
                    _actions.Add(new Tuple<string, string, Rule>(rule.From, item, rule));
                }
            }
            _actions = _actions.Distinct().ToList();
        }
        private List<string> Follow(string NonTerminal, List<string>? starts = null)
        {
            starts ??= new List<string>();
            List<string> result = new List<string>();
            starts.Add(NonTerminal);
            if (NonTerminal == _grammer.Rules[0].From)
                result.Add("$");
            foreach (Rule r in _grammer.Rules)
            {
                for (int i = 0; i < r.To.Count; i++)
                {
                    if (r.To[i] == NonTerminal)
                    {
                        if (i == r.To.Count - 1)
                        {
                            if (!starts.Contains(r.From))
                                result.AddRange(Follow(r.From, starts));
                            else break;
                        }
                        else
                        {
                            bool landa = true;
                            for (int j = i + 1; j < r.To.Count; j++)
                            {
                                var ni = First(r.To[j]);
                                if (!ni.Contains("#"))
                                {
                                    result.AddRange(ni);
                                    landa = false;
                                    break;
                                }
                                else
                                {
                                    foreach (var k in ni)
                                    {
                                        if (k != "#")
                                            result.Add(k);
                                    }
                                }
                            }
                            if (landa)
                            {
                                if (!starts.Contains(r.From))
                                    result.AddRange(Follow(r.From, starts));
                                else break;
                            }
                        }
                    }
                }
            }
            result = result.Distinct().ToList();
            return result;
        }
        private List<string> First(string item)
        {
            List<string> result = new List<string>();
            if (item == Tokens.Const.ToString() || item == Tokens.Literal.ToString() || item == Tokens.ID.ToString() || item == Tokens.OP_mul.ToString() || item == Tokens.OP_add.ToString() || item == Tokens.OP_sub.ToString() || item == Tokens.OP_div.ToString() || item == Tokens.OP_sim.ToString() || item == Tokens.OP_cam.ToString() || item == Tokens.OP_rpa.ToString() || item == Tokens.OP_lpa.ToString() || item == Tokens.KW_procedure.ToString() || item == Tokens.KW_division.ToString() || item == Tokens.KW_end.ToString() || item == Tokens.KW_put.ToString() || item == Tokens.KW_get.ToString() || item == Tokens.KW_set.ToString() || item == Tokens.KW_to.ToString() || item == "#")
                result.Add(item);
            else
            {
                foreach (var r in _grammer.Rules)
                {
                    if (r.From == item)
                    {
                        bool tohi = true;
                        foreach (var i in r.To)
                        {
                            if (i == "#")
                            {
                                result.Add(i);
                                continue;
                            }
                            if (i == item)
                                continue;
                            var nf = First(i);
                            if (!nf.Contains("#"))
                            {
                                result.AddRange(nf);
                                tohi = false;
                                break;
                            }
                            else
                            {
                                foreach (var j in nf)
                                {
                                    if (j != "#")
                                        result.Add(j);
                                }
                            }
                        }
                        if (tohi) result.Add("#");
                    }
                }
            }
            result = result.Distinct().ToList();
            return result;
        }
        private List<string> First(List<string> items)
        {
            var result = new List<string>();
            bool tohi = true;
            foreach (var item in items)
            {
                var temp = First(item);
                if (!temp.Contains("#"))
                {
                    result.AddRange(temp);
                    tohi = false;
                    break;
                }
                foreach (var i in temp)
                {
                    if (i == "#")
                        continue;
                    result.Add(i);
                }
            }
            if (tohi) result.Add("#");
            result = result.Distinct().ToList();
            return result;
        }

        public List<string> ValidInputsForAState(string nonterminal)
        {
            List<string> result = new();
            foreach (var action in _actions)
            {
                if (action.Item1 == nonterminal)
                    result.Add(action.Item2);
            }
            return result;
        }
        public bool IsLL1Grammer()
        {
            foreach (var item1 in _actions)
            {
                foreach (var item2 in _actions)
                {
                    if (item1 != item2)
                        if (item1.Item1 == item2.Item1 && item1.Item2 == item2.Item2)
                            return false;
                }
            }
            return true;
        }
    }
}
