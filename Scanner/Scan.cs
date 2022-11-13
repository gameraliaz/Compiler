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
        //int charclass;
        //char[] lexem;
        //char nextchar;
        //int lexlen;
        //int token;
        //int nexttoken;
        StreamReader sr;
        int linenumber;
        #endregion

        public Scan(string File)
        {
            linenumber = 0;
            _file = File;
            Result = "";
        }
        public void Run()
        {
            try
            {
                sr = new StreamReader(_file);
                bool a = _getValuableCode();
                Result += Convert.ToString(linenumber);
                Result += '\n';
                if (a)
                    _savefile();
            }
            catch (Exception e)
            {
                Result += "The file could not be read:";
                Result += e.Message;
                Result += '\n';
            }
        }
        private bool _getValuableCode()
        {
            char c = '\0';
            bool comenti = false;
            bool incomment = false;
            bool comento = false;
            int l = 0;
            int lt = 0;
            do
            {
                bool check = (c != ' ' && c!='\r'&& c != '\n' && !incomment && comenti);
                int h = sr.Peek();
                if (h == -1)
                    break;
                c = (char)h;
                if (c == '\n')
                    linenumber++;
                if (c == '/')
                {
                    if (comento)
                    {
                        // set incoment to false if incoment was false error=> (current line) closed not opend comment oprator(*/)
                        if (incomment)
                        {
                            incomment = false;
                            //c = Convert.ToChar(sr.Read());
                            comento = false;
                            comenti = false;
                        }
                        else
                        {
                            Result += "in " + Convert.ToString(linenumber) + " closed not opend commend oprator (*/)";
                            Result += '\n';
                            return false;
                        }
                    }
                    else
                    {
                        if (!incomment)
                        {
                            lt = linenumber;
                            comenti = true;
                        }
                    }
                    check = true;
                }
                else if (c == '*')
                {
                    if (comenti && !incomment)
                    {
                        l = lt;
                        incomment = true;
                        comenti = false;
                        comento = false;
                    }
                    else
                    {
                        comento = true;
                    }
                    check = true;
                }
                else
                {
                    check = !check;
                    comenti = false;
                    comento = false;
                }
                if (check)
                    c = Convert.ToChar(sr.Read());
                
            }
            while (c == ' ' ||c=='\r'|| c == '\n' || incomment || comenti );

            //nextchar = Convert.ToChar(sr.Read());
            if (incomment)
            {
                Result += "in line " + Convert.ToString(l) + " opend and not closed commend oprator (/*)";
                Result += '\n';
                return false;
            }
            //if (nextchar == '*' && Convert.ToChar(sr.Peek()) == '/')
            //{
            //    Result += "in line " + Convert.ToString(linenumber) + " closed not opend commend oprator (*/)";
            //    Result += '\n';
            //    return false;
            //}
            else return true;
        }
        private bool _savefile()
        {/* /* */
            try
            {
                using (StreamWriter sw = new StreamWriter("out.txt"))
                {
                    char c;
                    int h = sr.Read();
                    while (h != -1)
                    {
                        c = Convert.ToChar(h);
                        sw.Write(c);
                        h = sr.Read();
                    }
                }
            }catch (Exception ex)
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
