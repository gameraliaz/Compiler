// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
using Parser;

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

Console.WriteLine(grammer.ToString());
Console.WriteLine("\n");
Console.ReadLine();

var gs=slrpt.Run("E");

foreach(Grammer g in gs)
    Console.WriteLine(g.ToString());
Console.WriteLine("\n");
Console.ReadLine();