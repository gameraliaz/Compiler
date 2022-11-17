using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner
{

    public class Scan
    {
        #region Main Porperties
        //prints output (exeptions)
        public string Result { get; set; }
        //address of file
        string _file;
        //Symbol Table
        public List<SymbolTable> SymbolsTable;
        #endregion

        #region Scannner variables
        List<Machine> machine;
        char NextChar;
        StreamReader? Sr;
        int LineNumber;
        #endregion

        public Scan(string File)
        {
            LineNumber = 1;
            _file = File;
            Result = "";
            NextChar = '\0';
            machine = new List<Machine>();
            machine.Add(new Machine());
            SymbolsTable = new List<SymbolTable>();
            _initST();
        }
        private void _initST()
        {
            SymbolTable KW_Procedure=new SymbolTable();
            KW_Procedure.Value = "procedure";
            KW_Procedure.Token = Tokens.KW_procedure;
            SymbolsTable.Add(KW_Procedure);

            SymbolTable KW_division = new SymbolTable();
            KW_division.Value = "division";
            KW_division.Token = Tokens.KW_division;
            SymbolsTable.Add(KW_division);

            SymbolTable KW_end = new SymbolTable();
            KW_end.Value = "end";
            KW_end.Token = Tokens.KW_end;
            SymbolsTable.Add(KW_end);

            SymbolTable KW_set = new SymbolTable();
            KW_set.Value = "set";
            KW_set.Token = Tokens.KW_set;
            SymbolsTable.Add(KW_set);

            SymbolTable KW_to = new SymbolTable();
            KW_to.Value = "to";
            KW_to.Token = Tokens.KW_to;
            SymbolsTable.Add(KW_to);

            SymbolTable KW_get = new SymbolTable();
            KW_get.Value = "get";
            KW_get.Token = Tokens.KW_get;
            SymbolsTable.Add(KW_get);

            SymbolTable KW_put = new SymbolTable();
            KW_put.Value = "put";
            KW_put.Token = Tokens.KW_put;
            SymbolsTable.Add(KW_put);
        }
        private bool _addToST(SymbolTable st)
        {
            foreach(SymbolTable item in SymbolsTable)
            {
                if(item.Value == st.Value)
                {
                    return false;
                }
            }
            SymbolsTable.Add(st);
            return true;
        }
        public void Run()
        {
            Dictionary<int, string> Code = new Dictionary<int, string>();
            // pass 1 removing comments and labeling lines
            try
            {
                Sr = new StreamReader(_file);
                int linenumber = 0;
                do
                {
                    if (_getValuableCode())
                    {
                        if (LineNumber != linenumber)
                        {
                            Code.Add(LineNumber, "");
                            linenumber = LineNumber;
                        }
                        if (NextChar != '\n')
                            Code[LineNumber] += NextChar;
                        else
                        {
                            Code[LineNumber] += " ";
                        }
                    }
                    else
                        break;
                } while (NextChar != '\0');
                Sr.Close();
            }
            catch (Exception e)
            {
                Result += "The file could not be read:";
                Result += e.Message;
                Result += '\n';
            }

            _savefile(Code);

            // pass 2 lex
            while (true)
            {
                // get noneblank
                Dictionary<int, string> tCode = new Dictionary<int, string>(Code);
                foreach (int line in Code.Keys)
                {
                    bool outloop = false;
                    foreach (char c in Code[line])
                    {
                        if (!(c == ' ' || c == '\r' || c == '\t' || c=='\0'))
                        {
                            outloop = true;
                            NextChar = c;
                            break;
                        }
                        else
                            tCode[line] = tCode[line].Remove(0,1);
                    }
                    if (tCode[line] == "")
                        tCode.Remove(line);
                    if (outloop)
                        break;
                }

                Code.Clear();
                Code = new Dictionary<int, string>(tCode);

                bool outloopERR = false;
                //lex
                foreach (int line in Code.Keys)
                {
                    bool outloop = false;
                    foreach (char c in Code[line])
                    {
                        NextChar = c;
                        ClassLex status;
                        status = machine[machine.Count - 1].Read(NextChar);
                        if (status == ClassLex.Accept) //Reading
                        {
                            machine.Add(new Machine());
                            tCode[line] = tCode[line].Remove(0,1);
                            outloop = true;
                            Code = new Dictionary<int, string>(tCode);
                            break;
                        }
                        else if (status == ClassLex.AcceptStar)
                        {
                            machine.Add(new Machine());
                            outloop = true;
                            Code = new Dictionary<int, string>(tCode);
                            break;
                        }
                        else if (status == ClassLex.OnWork)
                        {
                            tCode[line] = tCode[line].Remove(0,1);
                        }
                        else
                        {
                            Result += "Error in line " + Convert.ToString(line) + ":\n" + ErrorMassage();
                            Result += "\n";
                            outloopERR = true;
                            Code = new Dictionary<int, string>(tCode);
                            break;
                        }
                    }
                    if (tCode[line] == "")
                        tCode.Remove(line);
                    if (outloop || outloopERR)
                        break;
                }
                if (outloopERR)
                    break;
                if (tCode.Count == 0)
                {
                    Result += "All " + Convert.ToString(LineNumber) + " lines scanned!\n";
                    foreach (Machine m in machine)
                    {
                        SymbolTable st=new SymbolTable();
                        st.Token = m.Token;
                        int nl = 0;
                        char clex = '\0';
                        do
                        {
                            clex = m.Lexem[nl];
                            st.Value += clex.ToString();
                            nl++;
                        } while (clex != '\0');
                    }
                    break;
                }
            }
        }
        private bool _getValuableCode()
        {
            char c = '\0';

            bool incomment = false;

            int l = LineNumber;

            do
            {
                int h = Sr.Read();
                NextChar = '\0';
                if (h == -1)
                    break;
                c = (char)h;
                NextChar = c;


                if (c == '\n')
                    LineNumber++;
                if (c == '/')
                {
                    if ((char)Sr.Peek() == '*')
                    {
                        if (!incomment)
                        {
                            l = LineNumber;
                            Sr.Read();
                            NextChar = '\0';
                            incomment = true;
                        }
                    }
                    else
                        if (!incomment)
                        break;
                }
                else if (c == '*')
                {
                    if ((char)Sr.Peek() == '/')
                    {
                        if (incomment)
                        {
                            incomment = false;
                            Sr.Read();
                            NextChar = '\0';
                        }
                        else
                        {
                            Result += "in line " + Convert.ToString(l) + " closed not opend comment oprator (*/)";
                            Result += '\n';
                            return false;
                        }
                    }
                    else
                        if (!incomment)
                        break;
                }
                else
                    if (!incomment)
                    break;

            }
            while (true);
            if (incomment)
            {
                Result += "in line " + Convert.ToString(l) + " opend and not closed comment oprator (/*)";
                Result += '\n';
                return false;
            }
            else return true;
        }
        private string ErrorMassage()
        {
            string err = "";
            switch (machine[machine.Count - 1].Token)
            {
                case Tokens.ID:
                    {
                        if (machine[machine.Count - 1].State == -1)
                        {
                            err = "Variables must be less than or equal to 8 characters.";
                        }
                        else if (machine[machine.Count - 1].State == -2)
                        {
                            err = "Variables can only consist of uppercase and lowercase English letters \n" +
                                "or numbers (numbers cannot be at the beginning).\n" +
                                "You used other characters for your variable!";
                        }
                        break;
                    }
                case Tokens.Const:
                    err = "Const only can have digit (if you want make variable,in variable numbers cannot be at the beginning)!";
                    break;
            }
            return err;
        }
        private bool _savefile(Dictionary<int, string> wts)
        {/* /* */
            try
            {
                using (StreamWriter sw = new StreamWriter("out.txt"))
                {
                    sw.Write(NextChar);
                    foreach (int line in wts.Keys)
                        sw.WriteLine(Convert.ToString(line) + ":" + wts[line]);
                }
            }
            catch (Exception ex)
            {
                Result += "The file could not be save:";
                Result += ex.Message;
                Result += '\n';
                return false;
            }
            return true;
        }

    }
}
