using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class SLRParsingTable
    {
        private Grammer _grammer;
        public SLRParsingTable(Grammer grammer)
        {
            _grammer = grammer;
        }
        public List<Grammer> Run(string S)
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

            for(int i = 0; i < result.Count; i++)
            {
                result[i].NumOfGrammer = i;
            }
            return result;

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
    }
}
