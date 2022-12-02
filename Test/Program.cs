﻿// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
using Parser;
using Scanner;

static Grammer getGerammer()
{
    List<Rule> rules = new();
    Rule r1 = new();
    r1.From = "<exe-part>";
    r1.addToList(Tokens.KW_procedure);
    r1.addToList(Tokens.KW_division);
    r1.addToList(Tokens.OP_sim);
    r1.addToList("<stmt-List>");
    r1.addToList(Tokens.KW_end);
    r1.addToList(Tokens.OP_sim);
    rules.Add(r1);
    Rule r2 = new();
    r2.From = "<stmt-List>";
    r2.addToList("<stmt>");
    r2.addToList(Tokens.OP_sim);
    r2.addToList("<stmt-List>");
    rules.Add(r2);
    Rule r3 = new();
    r3.From = "<stmt-List>";
    r3.addToList("<stmt>");
    r3.addToList(Tokens.OP_sim);
    rules.Add(r3);

    Rule r4 = new();
    r4.From = "<stmt>";
    r4.addToList("<assgn-stmt>");
    rules.Add(r4);

    Rule r41 = new();
    r41.From = "<stmt>";
    r41.addToList("<in-stmt>");
    rules.Add(r41);
    Rule r42 = new();
    r42.From = "<stmt>";
    r42.addToList("<out-stmt>");
    rules.Add(r42);

    Rule r5 = new();
    r5.From = "<assgn-stmt>";
    r5.addToList(Tokens.KW_set);
    r5.addToList(Tokens.ID);
    r5.addToList(Tokens.KW_to);
    r5.addToList("<math-exp>");
    rules.Add(r5);

    Rule r51 = new();
    r51.From = "<assgn-stmt>";
    r51.addToList(Tokens.KW_set);
    r51.addToList(Tokens.ID);
    r51.addToList(Tokens.KW_to);
    r51.addToList(Tokens.Literal);
    rules.Add(r51);

    Rule r6 = new();
    r6.From = "<in-stmt>";
    r6.addToList(Tokens.KW_get);
    r6.addToList("<in-List>");
    r6.addToList(Tokens.OP_sim);
    rules.Add(r6);

    Rule r7 = new();
    r7.From = "<in-List>";
    r7.addToList(Tokens.ID);
    r7.addToList(Tokens.OP_cam);
    r7.addToList("<in-List>");
    rules.Add(r7);

    Rule r8 = new();
    r8.From = "<in-List>";
    r8.addToList(Tokens.ID);
    rules.Add(r8);

    Rule r61 = new();
    r61.From = "<out-stmt>";
    r61.addToList(Tokens.KW_put);
    r61.addToList("<out-List>");
    r61.addToList(Tokens.OP_sim);
    rules.Add(r61);

    Rule r71 = new();
    r71.From = "<out-List>";
    r71.addToList(Tokens.ID);
    r71.addToList(Tokens.OP_cam);
    r71.addToList("<out-List>");
    rules.Add(r71);

    Rule r81 = new();
    r81.From = "<out-List>";
    r81.addToList(Tokens.ID);
    rules.Add(r81);

    Rule r9 = new();
    r9.From = "<math-exp>";
    r9.addToList("<term>");
    r9.addToList("<math-exp2>");
    rules.Add(r9);
    Rule r91 = new();
    r91.From = "<math-exp2>";
    r91.addToList(Tokens.OP_sub);
    r91.addToList("<term>");
    r91.addToList("<math-exp2>");
    rules.Add(r91);
    Rule r912 = new();
    r912.From = "<math-exp2>";
    r912.addToList(Tokens.OP_add);
    r912.addToList("<term>");
    r912.addToList("<math-exp2>");
    rules.Add(r912);
    Rule r92 = new();
    r92.From = "<math-exp2>";
    r92.addToList("#");
    rules.Add(r92);

    Rule r99 = new();
    r99.From = "<term>";
    r99.addToList("<factor>");
    r99.addToList("<term2>");
    rules.Add(r99);
    Rule r991 = new();
    r991.From = "<term2>";
    r991.addToList(Tokens.OP_mul);
    r991.addToList("<factor>");
    r991.addToList("<term2>");
    rules.Add(r991);
    Rule r9912 = new();
    r9912.From = "<term2>";
    r9912.addToList(Tokens.OP_div);
    r9912.addToList("<factor>");
    r9912.addToList("<term2>");
    rules.Add(r9912);
    Rule r992 = new();
    r992.From = "<term2>";
    r992.addToList("#");
    rules.Add(r992);

    Rule r11 = new();
    r11.From = "<factor>";
    r11.addToList(Tokens.OP_sub);
    r11.addToList("<factor>");
    rules.Add(r11);
    Rule r112 = new();
    r112.From = "<factor>";
    r112.addToList("<element>");
    rules.Add(r112);

    Rule r12 = new();
    r12.From = "<element>";
    r12.addToList(Tokens.OP_lpa);
    r12.addToList("<math-exp>");
    r12.addToList(Tokens.OP_rpa);
    rules.Add(r12);
    Rule r121 = new();
    r121.From = "<element>";
    r121.addToList(Tokens.ID);
    rules.Add(r121);
    Rule r122 = new();
    r122.From = "<element>";
    r122.addToList(Tokens.Const);
    rules.Add(r122);
    Grammer grammer = new(-1);
    grammer.AddRules(rules);
    return grammer;
}


SLRParsingTable slrpt=new SLRParsingTable(getGerammer());


List<string> list = new List<string>();
list.Add(Tokens.ID.ToString());
list.Add(Tokens.OP_add.ToString());
list.Add(Tokens.OP_mul.ToString());
list.Add(Tokens.OP_lpa.ToString());
list.Add(Tokens.OP_rpa.ToString());
list.Add(Tokens.OP_cam.ToString());
list.Add(Tokens.OP_sim.ToString());
list.Add(Tokens.OP_sub.ToString());
list.Add(Tokens.OP_div.ToString());
list.Add(Tokens.Const.ToString());
list.Add(Tokens.Literal.ToString());
list.Add(Tokens.KW_put.ToString());
list.Add(Tokens.KW_get.ToString());
list.Add(Tokens.KW_procedure.ToString());
list.Add(Tokens.KW_division.ToString());
list.Add(Tokens.KW_end.ToString());
list.Add(Tokens.KW_set.ToString());
list.Add(Tokens.KW_to.ToString());
list.Add("$");
Console.Write(" ");
foreach (var i in list)
{
    Console.Write(i.ToString()+" ");
}
Console.WriteLine();
for (int j = 0; j < slrpt.LR0Items.Count; j++)
{
    Console.Write(j.ToString()+" ");
    foreach(var i in list)
    {
        Console.Write(slrpt.GetAction(j, i).ToString() + " "); 
    }
    Console.WriteLine();
}
Console.WriteLine("\n");
Console.ReadLine();

List<string> list2 = new List<string>();
list2.Add("<exe-part>");
list2.Add("<stmt-List>");
list2.Add("<assgn-stmt>");
list2.Add("<in-stmt>");
list2.Add("<in-List>");
list2.Add("<out-stmt>");
list2.Add("<out-List>");
list2.Add("<math-exp>");
list2.Add("<term>");
list2.Add("<factor>");
list2.Add("<element>");
Console.Write(" ");
foreach (var i in list2)
{
    Console.Write(i.ToString() + " ");
}
Console.WriteLine();
for (int j = 0; j < slrpt.LR0Items.Count; j++)
{
    Console.Write(j.ToString() + " ");
    foreach (var i in list2)
    {
        Console.Write(slrpt.GetGoTo(j, i).ToString() + " ");
    }
    Console.WriteLine();
}
Console.WriteLine("\n");
Console.ReadLine();



