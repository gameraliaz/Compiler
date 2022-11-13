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
        char nextchar;
        //int lexlen;
        //int token;
        //int nexttoken;
        StreamReader sr;
        int linenumber;
        #endregion

        public Scan(string File)
        {
            linenumber = 1;
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

            bool incomment = false;

            int l = linenumber;

            do
            {
                int h = sr.Read();
                nextchar = '\0';
                if (h == -1)
                    break;
                c = (char)h;
                nextchar = c;


                if (c == '\n')
                    linenumber++;
                else
                {
                    if (c=='/')
                    {
                        if ((char)sr.Peek()=='*')
                        {
                            if (!incomment)
                            {
                                l = linenumber;
                                sr.Read();
                                nextchar = '\0';
                                incomment = true;
                            }
                        }
                        else
                            if (!incomment)
                                break;
                    }
                    else if(c=='*')
                    {
                        if ((char)sr.Peek() == '/')
                        {
                            if(incomment)
                            {
                                incomment = false;
                                sr.Read();
                                nextchar = '\0';
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
        private bool _savefile()
        {/* /* */
            try
            {
                using (StreamWriter sw = new StreamWriter("out.txt"))
                {
                    sw.Write(nextchar);
                    char c;
                    int h = sr.Read();
                    while (h != -1)
                    {
                        c = Convert.ToChar(h);
                        sw.Write(c);
                        h = sr.Read();
                    }/*aaaa/*/
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
