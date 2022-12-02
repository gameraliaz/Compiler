// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
using Parser;
using Scanner;



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



