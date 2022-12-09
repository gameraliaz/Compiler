namespace Parser
{
    public class Grammer
    {
        public List<Rule> Rules { get; set; }
        public int NumOfGrammer { get; set; }
        public Grammer(int Number)
        {
            Rules = new List<Rule>();
            NumOfGrammer = Number;
        }
        //exist => false else true
        public bool AddRule(Rule rule)
        {
            Rule r = new();
            r.From = rule.From;
            r.To = rule.To;
            r.DotIndex = rule.DotIndex;
            bool exist = false;
            foreach (Rule i in Rules)
            {
                if (i.Equals(r))
                {
                    exist = true; break;
                }
            }
            if (!exist) Rules.Add(r);
            else return false;
            return true;
        }
        public bool AddRules(List<Rule> rules)
        {
            bool res = false;
            foreach (Rule rule in rules)
            {
                bool r = AddRule(rule);
                if (r)
                    res = true;
            }
            return res;
        }
        public bool AddRules(Grammer grammer)
        {
            return AddRules(grammer.Rules);
        }
        public override string ToString()
        {
            var res = "Grammer " + Convert.ToString(NumOfGrammer) + "\n";
            foreach (Rule rule in Rules)
            {
                res += rule.ToString() + "\n";
            }
            return res;
        }
        public override bool Equals(object? obj)
        {
            if (((Grammer)obj).Rules.Count == Rules.Count)
            {
                foreach (Rule rule in ((Grammer)obj).Rules)
                {
                    bool same = false;
                    foreach (Rule rule2 in Rules)
                    {
                        if (rule.Equals(rule2))
                        {
                            same = true;
                            break;
                        }
                    }
                    if (!same) return false;
                }
                return true;
            }
            return false;
        }
    }
}
