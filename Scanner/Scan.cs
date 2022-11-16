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
                        if(LineNumber !=linenumber)
                        {
                            Code.Add(LineNumber, "");
                            linenumber = LineNumber;
                        }
                        if (NextChar !='\n')    
                            Code[LineNumber] += NextChar;
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
            foreach(int line in Code.Keys)
            {
                foreach(char c in Code[line])
                {
                    if (!(c == ' ' || c== '\r' || c=='\t' ))
                    {
                        NextChar = c;
                        if (machine[machine.Count - 1].Read(c)) //Reading
                            machine.Add(new Machine());
                    }
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
        private bool _savefile(Dictionary<int,string> wts)
        {/* /* */
            try
            {
                using (StreamWriter sw = new StreamWriter("out.txt"))
                {
                    sw.Write(NextChar);
                    foreach (int line in wts.Keys)
                        sw.WriteLine(Convert.ToString(line)+":"+wts[line]);
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
