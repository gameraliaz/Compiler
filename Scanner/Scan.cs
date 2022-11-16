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
                    Result += "All " + Convert.ToString(LineNumber) + " lines scanned!\n" +
                        "Tokens\tValues\n";
                    foreach (Machine m in machine)
                    {
                        Result += Convert.ToString(m.Token) + "\t";
                        int nl = 0;
                        char clex = '\0';
                        do
                        {
                            clex = m.Lexem[nl];
                            Result += clex.ToString();
                            nl++;
                        } while (clex != '\0');
                        Result += "\n";
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
