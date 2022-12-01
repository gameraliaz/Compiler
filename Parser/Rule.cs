using Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Rule
    {
        public string From { get; set; } 
        public List<string> To { get; set; }
        public int DotIndex { get; set; }
        public Rule()
        {
            From = "";
            To = new List<string>();
            DotIndex = 0;
        }
        public void addToList(Tokens tkn)
        {
            To.Add(tkn.ToString());
        }
        public void addToList(string nonterminate)
        {
            To.Add(nonterminate);
        }
        public Rule NextDot()
        {
            Rule rule = new();
            rule.DotIndex = DotIndex+1;
            rule.From = From;
            rule.To = To;
            return rule;
        }
        public override string ToString()
        {
            string Res= From+"->";
            foreach(string t in To)
            {
                Res += t;
            }
            return Res;
        }
        public override bool Equals(object? obj)
        {
            return obj?.ToString() == ToString() && DotIndex== ((Rule)obj).DotIndex;
        }
    }
}
