// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
using Parser;
using Scanner;

List<Rule> rules = new();
Rule r1 = new();
r1.From = "E";
r1.addToList("E");
r1.addToList(Scanner.Tokens.OP_add);
r1.addToList("T");
rules.Add(r1);
Rule r2 = new();
r2.From = "E";
r2.addToList("T");
rules.Add(r2);
Rule r3 = new();
r3.From = "T";
r3.addToList("T");
r3.addToList(Scanner.Tokens.OP_mul);
r3.addToList("F");
rules.Add(r3);
Rule r4 = new();
r4.From = "T";
r4.addToList("F");
rules.Add(r4);
Rule r5 = new();
r5.From = "F";
r5.addToList(Scanner.Tokens.OP_lpa);
r5.addToList("E");
r5.addToList(Scanner.Tokens.OP_rpa);
rules.Add(r5);
Rule r6 = new();
r6.From = "F";
r6.addToList(Scanner.Tokens.ID);
rules.Add(r6);
foreach(Rule r in rules)
    Console.WriteLine(r.ToString());
Console.WriteLine("\n");
Console.ReadLine();

Grammer grammer = new(-1);
grammer.AddRules(rules);

SLRParsingTable slrpt=new SLRParsingTable(grammer);

List<string> list = new List<string>();
list.Add(Tokens.ID.ToString());
list.Add(Tokens.OP_add.ToString());
list.Add(Tokens.OP_mul.ToString());
list.Add(Tokens.OP_lpa.ToString());
list.Add(Tokens.OP_rpa.ToString());
Console.Write(" \t\t");
foreach (var i in list)
{
    Console.Write(i.ToString()+"\t\t\t");
}
Console.WriteLine();
for (int j = 0; j < 12; j++)
{
    Console.Write(j.ToString()+" \t\t");
    foreach(var i in list)
    {
        Console.Write(slrpt.GetAction(j, i).ToString() + " \t\t"); 
    }
    Console.WriteLine();
}
Console.WriteLine("\n");
Console.ReadLine();

List<string> list2 = new List<string>();
list2.Add("E");
list2.Add("T");
list2.Add("F");
Console.Write("\t\t");
foreach (var i in list2)
{
    Console.Write(i.ToString() + "\t\t");
}
Console.WriteLine();
for (int j = 0; j < 12; j++)
{
    Console.Write(j.ToString() + " \t\t");
    foreach (var i in list2)
    {
        Console.Write(slrpt.GetGoTo(j, i).ToString() + " \t\t");
    }
    Console.WriteLine();
}
Console.WriteLine("\n");
Console.ReadLine();