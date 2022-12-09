using Scanner;

namespace Parser
{
    public class SLRParsingTable
    {
        private List<Tuple<Grammer, string, Grammer>> _actions;
        public List<Grammer> LR0Items;
        public Grammer _grammer;
        public SLRParsingTable(Grammer grammer)
        {
            _grammer = grammer;
            _actions = new List<Tuple<Grammer, string, Grammer>>();
            LR0Items = new List<Grammer>();
            Run(grammer.Rules[0].From);
        }

        public Action GetAction(int currentState, string currentInput)
        {
            foreach (var action in _actions)
            {
                if (action.Item1.NumOfGrammer == currentState && action.Item2 == currentInput)
                    return new Action() { Act = TypeOfAction.shift, State = action.Item3.NumOfGrammer };
            }
            foreach (var g in LR0Items)
            {
                if (g.NumOfGrammer == currentState)
                {
                    foreach (var item in g.Rules)
                    {
                        if (item.DotIndex == item.To.Count)
                        {
                            var follow = Follow(item.From);
                            if (follow.Contains(currentInput))
                            {
                                Action act = new Action();
                                act.Act = TypeOfAction.reduce;
                                foreach (var rg0 in LR0Items[0].Rules)
                                {
                                    if (rg0.ToString() == item.ToString())
                                    {
                                        act.State = -1;
                                        act.Act = TypeOfAction.accept;
                                        return act;
                                    }
                                }
                                for (int i = 0; i < _grammer.Rules.Count; i++)
                                {
                                    if (_grammer.Rules[i].ToString() == item.ToString())
                                    {
                                        act.State = i;
                                        return act;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new Action();
        }
        public int GetGoTo(int currentState, string currentInput)
        {
            foreach (var action in _actions)
            {
                if (action.Item1.NumOfGrammer == currentState && action.Item2 == currentInput)
                    return action.Item3.NumOfGrammer;
            }
            return -1;
        }
        private void Run(string S)
        {
            List<Grammer> result = new();
            //augmented
            Grammer grammer = new(0);
            Rule fr = new();
            string start = S + "0";
            bool check;
            do
            {
                check = false;
                foreach (Rule r in _grammer.Rules)
                {
                    if (r.From == start)
                    {
                        check = true;
                        int num = Convert.ToInt16(start[1..]) + 1;
                        start = S + Convert.ToString(num);
                        break;
                    }
                }
            } while (check);
            fr.addToList(S);
            fr.From = start;
            grammer.AddRule(fr);

            //item 0
            grammer = Closure(grammer);

            result.Add(grammer);

            List<Tuple<Grammer, string, Grammer>> Actions = new();
            //Goto section
            bool loop = true;
            while (loop)
            {
                loop = false;
                List<Grammer> temp = new List<Grammer>();
                foreach (Grammer g in result)
                {
                    List<string> items = new();
                    foreach (Rule r in g.Rules)
                    {
                        if (r.DotIndex < r.To.Count)
                        {
                            if (!items.Contains(r.To[r.DotIndex]))
                            {
                                items.Add(r.To[r.DotIndex]);
                                temp.Add(GoTo(g, items[items.Count - 1]));
                                Actions.Add(new Tuple<Grammer, string, Grammer>(g, items[items.Count - 1], temp[temp.Count - 1]));
                            }
                        }
                    }
                }

                foreach (Grammer g in temp)
                {
                    bool has = false;
                    foreach (Grammer g2 in result)
                    {
                        if (g2.Equals(g))
                        {
                            has = true;
                            break;
                        }
                    }
                    if (!has)
                    {
                        loop = true;
                        result.Add(g);
                    }
                }
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[i].NumOfGrammer = i;
            }
            for (int i = 0; i < Actions.Count; i++)
            {
                foreach (Grammer g in result)
                {
                    if (g.Equals(Actions[i].Item1))
                    {
                        Actions[i] = new(g, Actions[i].Item2, Actions[i].Item3);
                    }
                    if (g.Equals(Actions[i].Item3))
                    {
                        Actions[i] = new(Actions[i].Item1, Actions[i].Item2, g);
                    }
                }
            }
            _actions = Actions.Distinct().ToList();
            LR0Items = result;
        }
        private Grammer GoTo(Grammer I, string item)
        {
            Grammer grammer = new(I.NumOfGrammer);
            foreach (Rule r in I.Rules)
            {
                if (r.DotIndex < r.To.Count)
                {
                    if (r.To[r.DotIndex] == item)
                    {
                        var nr = r.NextDot();
                        grammer.AddRule(nr);
                    }
                }
            }
            return Closure(grammer);
        }
        private Grammer Closure(Grammer I)
        {
            Grammer grammer = new(I.NumOfGrammer);
            grammer.AddRules(I);

            Grammer temp = new(I.NumOfGrammer);
            temp.AddRules(I);

            bool loop = true;
            while (loop)
            {
                foreach (Rule r in grammer.Rules)
                {
                    foreach (Rule r2 in _grammer.Rules)
                    {
                        if (r.DotIndex < r.To.Count)
                        {
                            if (r2.From == r.To[r.DotIndex])
                            {
                                temp.AddRule(r2);
                            }
                        }
                    }
                }

                if (temp.Rules.Count == 0)
                    loop = false;
                else
                {
                    bool c = grammer.AddRules(temp);
                    if (!c)
                        loop = false;
                }
            }

            return grammer;
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

        public bool IsSLRGrammer()
        {
            foreach (var a in _actions)
            {
                foreach (var b in _actions)
                {
                    if (a.Item2 == b.Item2 && a.Item3.NumOfGrammer == b.Item3.NumOfGrammer)
                        return true;
                }
            }
            return false;
        }
        public List<string> ValidInputsForAState(int state)
        {
            List<string> result = new List<string>();
            foreach (var a in _actions)
            {
                if (a.Item1.NumOfGrammer == state)
                {
                    result.Add(a.Item2);
                }
            }
            return result;
        }
    }
}
