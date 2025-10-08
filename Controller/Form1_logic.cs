using Controller.ALU;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Controller
{
    public partial class Form1
    {
        //----------------------------
        //PC

        bool subTick()
        {
            if (Last_PC == 0)
                return false;

            if (StopPC.Contains(current_PC))
                return false;


            myComputer.proceed();
            current_PC = myComputer.getPC();
            current_A = myComputer.getA();
            current_D = myComputer.getD();
            current_I = myComputer.getI();
            return true;
        }
        bool Tick()
        {
            /*
            if (Last_PC == 0)
                return false;

            if(StopPC.Contains(Current_PC)) 
                return false;


            myComputer.proceed();
            Current_PC = myComputer.getPC();
            */
            bool ret = subTick();
            if (ret != true)
                return ret;

            if (speed < 2)
            {
                int A = myComputer.getA();
                int D = myComputer.getD();
                int I = myComputer.getI();
                int ALU = Tools.getALU(I);

                label_A.Text = "A=" + A + " ( 0x" + A.ToString("X") + " )";
                label_D.Text = "D=" + D + " ( 0x" + D.ToString("X") + " )";
                label_PC.Text = "PC=" + current_PC;
                label_I.Text = "I=" + I;
                label_ALU.Text = "ALU=" + ALU;
                dataGridView_Rom.ClearSelection();
                dataGridView_Rom.Rows[current_PC].Selected = true;
                updateI(I);
                refreshRam();
                UpdatePC(current_PC, false);
                UpdateStack();
                label_Nand_count.Text = "Nand count: " + myComputer.GetNandCount();
            }
            pictureBox1.Refresh();
            refreshMappedIO();

            if (current_PC >= Last_PC)
            {
                toolStripButtonTick.Enabled = false;
                return false;
            }
            return true;

        }


        void ColorRegister(string txt, int startpos, Dictionary<string, int> Csts, RichTextBox richTextBox)
        {
            txt = txt.ToUpper();
            foreach (string s in Csts.Keys)
            {
                if (txt == s)
                {
                    richTextBox.Select(startpos , txt.Length);
                    richTextBox.SelectionColor = Color.LightGreen;
                    return;
                }
            }
            if (txt.Length > 2)
            {
                int offset = 0;
                if (txt[0] == ' ')
                    offset = 1;
                if ((txt[offset] == '0') && (txt[offset+1] == 'X'))
                    return;
                if (txt[offset] == '\'')
                {
                    richTextBox.Select(startpos+1+ offset, 1);
                    richTextBox.SelectionColor = Color.Green;
                    return;
                }
            }
            for (int i = 0; i < txt.Length; i++)
            {
                if ((txt[i] == 'A') | (txt[i] == 'D') || (txt[i] == '*'))
                {
                    richTextBox.Select(startpos + i, 1);
                    richTextBox.SelectionColor = Color.LightGreen;
                }

            }
        }
        List<string> reservedWords = new List<string>();//= { "JMP", "init.stack" };
        void UpdateReserveredWords()
        {
            reservedWords = MacroList.Keys.ToList<string>();
            //reservedWords.Insert(0,"JMP");
            //reservedWords.Insert(1, "LABEL");
            reservedWords = reservedWords.ConvertAll(d => d.ToUpper());
        }


        void AnalyseLines(string txt, List<int> ListErrors, RichTextBox richTextBox)
        {
            int pos = 0;
            int nextpos = 0;
            bool shoudStop = false;
            int lineCpt = 0;
            richTextBox.SelectAll();
            richTextBox.SelectionColor = Color.White;
            UpdateReserveredWords();
            string Msg;
            Dictionary<string, int> Csts = GetConstants(out Msg);
            do
            {
                pos = txt.IndexOf("\n", nextpos);
                if (pos < 0)
                {
                    shoudStop = true;
                    pos = txt.Length;
                }
                string line = txt.Substring(nextpos, pos - nextpos);
                richTextBox.Select(nextpos, pos - nextpos);
                richTextBox.SelectionBackColor = Color.Black;


                if (line.TrimStart().StartsWith("#"))//comment
                {
                    richTextBox.Select(nextpos, pos - nextpos);
                    richTextBox.SelectionColor = Color.DarkGreen;
                }
                else if (line.TrimStart().ToUpper().StartsWith("JMP"))
                {
                    richTextBox.Select(nextpos, 3);
                    richTextBox.SelectionColor = Color.Yellow;
                    richTextBox.SelectionBackColor = Color.Black;
                }
                else if (line.TrimStart().ToUpper().StartsWith("LABEL"))
                {

                    int nbSpaces = line.Length - line.TrimStart().Length;
                    int posSpace = line.IndexOf(" ", nbSpaces);//here
                    richTextBox.Select(nextpos, posSpace);
                    richTextBox.SelectionColor = Color.Cyan;
                    richTextBox.SelectionBackColor = Color.Black;
                    if (posSpace > 0)
                    {
                        richTextBox.Select(nextpos + posSpace, line.Length - posSpace - 1);
                        richTextBox.SelectionColor = Color.Yellow;
                        richTextBox.SelectionBackColor = Color.Black;
                    }
                }
                else if (line.TrimStart().ToUpper().StartsWith("PRINT"))
                {
                    int nbSpaces = line.Length - line.TrimStart().Length;
                    int posSpace = line.IndexOf(" ", nbSpaces);//here
                    richTextBox.Select(nextpos, posSpace);
                    richTextBox.SelectionColor = Color.Cyan;
                    richTextBox.SelectionBackColor = Color.Black;
                    if (posSpace > 0)
                    {
                        ColorRegister(line.Substring(posSpace+1).ToUpper(), nextpos + posSpace+1,Csts, richTextBox);
                       
                    }
                }
                else if (line.TrimStart().ToUpper().StartsWith("GOTO") || (line.TrimStart().ToUpper().StartsWith("IF.GOTO")))
                {
                    int nbSpaces = line.Length - line.TrimStart().Length;
                    int posSpace = line.IndexOf(" ", nbSpaces);//here
                    richTextBox.Select(nextpos, posSpace);
                    richTextBox.SelectionColor = Color.Cyan;
                    richTextBox.SelectionBackColor = Color.Black;
                    if (posSpace > 0)
                    {
                        richTextBox.Select(nextpos + posSpace, line.Length - posSpace);
                        richTextBox.SelectionColor = Color.Yellow;
                        richTextBox.SelectionBackColor = Color.Black;
                    }
                }
                else
                {
                    int ReserverdWordIndex = -1;
                    for (int z = 0; z < reservedWords.Count; z++)
                        if (line.TrimStart().ToUpper().StartsWith(reservedWords[z]))
                            ReserverdWordIndex = z;

                    if (ReserverdWordIndex >= 0)
                    {
                        int posMacro = line.ToUpper().IndexOf(reservedWords[ReserverdWordIndex]);
                        richTextBox.Select(nextpos+ posMacro, reservedWords[ReserverdWordIndex].Length);
                        richTextBox.SelectionColor = Color.Cyan;
                        richTextBox.SelectionBackColor = Color.Black;
                    }
                    else//analize the line
                    {
                        for (int i = 0; i < dataGridView_Constant.Rows.Count; i++)
                        {
                            object Value = dataGridView_Constant.Rows[i].Cells[0].Value;
                            if (Value != null && Value != DBNull.Value && String.IsNullOrWhiteSpace(Value.ToString()))
                            {
                                string cst = (string)dataGridView_Constant.Rows[i].Cells[0].Value;
                                if ((cst != null) && (cst != ""))
                                {
                                    int posCst = line.ToUpper().IndexOf(cst.ToUpper());
                                    if (posCst >= 0)
                                    {
                                        richTextBox.Select(nextpos + posCst, cst.Length);
                                        richTextBox.SelectionColor = Color.Cyan;
                                        richTextBox.SelectionBackColor = Color.Black;
                                    }
                                }
                            }

                        }
                        int eqPos = line.IndexOf('=');
                        if (eqPos > 0)//we have a =
                        {
                            string sub1 = line.Substring(0, eqPos);
                            ColorRegister(sub1, nextpos,Csts, richTextBox);
                        }
                        int scPos = line.IndexOf(';');
                        if (scPos > 0)//we have a ;
                        {
                            richTextBox.Select(nextpos + scPos, pos - nextpos - scPos);
                            richTextBox.SelectionColor = Color.Yellow;
                        }

                        if (scPos < 0)
                            scPos = pos - nextpos;
                        string sub = line.Substring(eqPos + 1, scPos - eqPos - 1);
                        ColorRegister(sub, nextpos + eqPos + 1, Csts, richTextBox);
                    }

                    if (ListErrors != null)
                    {
                        for (int i = 0; i < ListErrors.Count; i++)
                        {
                            if (ListErrors[i] == lineCpt)
                            {
                                richTextBox.Select(nextpos, pos - nextpos);
                                richTextBox.SelectionBackColor = Color.LightCoral;
                            }
                            else
                            {
                                richTextBox.Select(nextpos, pos - nextpos);
                                richTextBox.SelectionBackColor = Color.Black;
                            }
                        }
                    }


                }
                lineCpt++;
                nextpos = pos + 1;
            } while (shoudStop == false);

        }

        Dictionary<string, int> GetConstants(out string Msg)
        {
            Msg = "";
            Dictionary<string, int> Cst = new Dictionary<string, int>();
            for (int i = 0; i < dataGridView_Constant.Rows.Count; i++)
            {
                object Value = dataGridView_Constant.Rows[i].Cells[0].Value;
                if (Value != null && Value != DBNull.Value && !String.IsNullOrWhiteSpace(Value.ToString()))
                {
                    string CstName = (string)Value;
                    string CstVal = (string)dataGridView_Constant.Rows[i].Cells[1].Value;
                    if ((CstName != "") && (CstName != null))
                    {
                        int val = Tools.ParseNumber(CstVal.ToUpper(), out Msg);
                        if (Msg == "")
                        {
                            if (Cst.ContainsKey(CstName))
                            {
                                Msg = "Conatant " + CstName + " exists more than 1 time";
                                return new Dictionary<string, int>();
                            }
                            else
                            {
                                Cst.Add(CstName.ToUpper(), val);
                            }
                        }
                        else
                        {
                            Msg = "Conatant " + CstName + "Value " + Msg;
                            return new Dictionary<string, int>();
                        }
                    }
                }
            }
            return Cst;
        }


        string  ManageParameters(List<string> paramList, string line)
        { 
            if (paramList.Count > 0)
            {
                if (line.ToUpper().Contains("VALUE"))
                {
                    line = line.ToUpper().Replace("VALUE", paramList[0]);
                }
                if (line.ToUpper().Contains("ADDRESS"))
                {
                    line = line.ToUpper().Replace("ADDRESS", paramList[0]);
                }
                if (line.ToUpper().Contains("LABEL"))
                {
                    line = line.ToUpper().Replace("LABEL", paramList[0]);
                }
                if (line.ToUpper().Contains("LABEL"))
                {
                    line = line.ToUpper().Replace("LABEL", paramList[0]);
                }
                if (line.ToUpper().Contains("INDEX"))
                {
                    line = line.ToUpper().Replace("INDEX", paramList[0]);
                }
                if (line.ToUpper().Contains("ARGUMENTCOUNT"))
                {
                    line = line.ToUpper().Replace("ARGUMENTCOUNT", paramList[1]);
                }
                if (line.ToUpper().Contains("LOCALSCOUNT"))
                {
                    line = line.ToUpper().Replace("LOCALSCOUNT", paramList[1]);
                }
                if (line.ToUpper().Contains("FUNCTIONNAME"))
                {
                    line = line.ToUpper().Replace("FUNCTIONNAME", paramList[0]);
                }
            }
            return line;
        }
        void ManageLine(string line, string MacroName, bool isMacro, List<string> paramList, Dictionary<string, int> _labels, out string Msg)
        {
            Msg = "";
            line = line.TrimStart();
            if (line == "")
            {
                if (isMacro == false)
                {
                    PC.Add(-1);
                    ASM.Add("");
                }
            }
            else if (line.StartsWith("#"))
            {
                if (isMacro == false)
                {
                    PC.Add(-1);
                    ASM.Add("");
                }
            }
            else if (line.ToUpper().StartsWith("LABEL"))
            {
                if (isMacro == false)
                {
                    PC.Add(-1);
                    ASM.Add("");
                }
              
                string lab = line.ToUpper().Substring(6, line.Length-6);
                string lab2 = lab.Replace(":", "");
                if (line.ToUpper().StartsWith("LABEL_G"))
                {
                    if (!labels.ContainsKey(paramList[0]))
                        labels.Add(paramList[0].ToUpper(), counter);
                    else
                        Msg = "Label " + paramList[0] + " is not unique in global scope";
                }
                else
                {
                    if (!_labels.ContainsKey(lab2))
                        _labels.Add(lab2, counter);
                    else
                        Msg = "Label " + lab2 + " is not unique in the scope of " + MacroName;
                }
            }
            else
            {
                if (isMacro == false)
                    PC.Add(counter);
                bool macroFound = false;
                foreach (KeyValuePair<string, List<string>> m in MacroList)
                {
                    if (line.ToUpper().StartsWith(m.Key))
                    {
                        macroFound = true;
                        line = ManageParameters(paramList, line);
                        AddMacro(line, m.Key, m.Value, out Msg);
                        if (Msg != "")
                            return;
                        if (m.Key=="END")
                        {
                            StopPC.Add(counter-1);
                        }
                    }

                }
                if (macroFound == false)
                {
                    line = ManageParameters(paramList, line);
                    counter++;
                    ASM.Add(line);
                }

            }
        }



        void AddMacro(string ListCode, string MacroName, List<string> macroValue, out string Msg)
        {
            Msg = "";
            int ASM_Start = ASM.Count();
            //ici rechercher les parametres de la macro et les passer a ManageLine
            string[] param = ListCode.Split(' ');
            Dictionary<string, int>  labelsInMacro = new Dictionary<string, int>();
           // List<string> p = param.ToList();
           // p.RemoveAt(0);
            for (int i = 0; i < macroValue.Count; i++)
            {
                //ici migrer plus haut
                List<string> p = param.ToList();
                p.RemoveAt(0);
                ManageLine(macroValue[i], MacroName,true, p, labelsInMacro,out Msg);
                if (Msg != "")
                    return;
            }
            //ici gerer labelsInMacro
            for (int i = ASM_Start; i < ASM.Count; i++)
            {
                if (ASM[i].StartsWith("A="))
                {
                    foreach (KeyValuePair<string, int> kv in labelsInMacro)
                    {
                        if (ASM[i].ToUpper().Contains(kv.Key))
                        {
                            ASM[i] = ASM[i].ToUpper().Replace(kv.Key, kv.Value.ToString());
                        }
                    }
                }
            }

        }
        private void Generate()
        {
            counter = 0;
            listBoxError.Items.Clear();
            string txt = richTextBoxCode.Text;
            string[] ListCode = txt.Split('\n');
            PC = new List<int>();
            ASM = new List<string>();
            ASMCode = new List<int>();
            string Msg2 = "";
            Dictionary<string, int> Csts = GetConstants(out Msg2);
            StopPC = new List<int>();
            labels = new Dictionary<string, int>();
            if (Msg2 != "")
            {
                listBoxError.Items.Add("Constant Error:" + Msg2);
                return;
            }
            //-------------------------------------------------------------------------------
            //find and add functions
            List<string> functionNameNeeded = new List<string>();
            for (int i = 0; i < ListCode.Length; i++)
            {
                if (ListCode[i].TrimStart().ToUpper().StartsWith("CALL"))
                {
                    int nbSpaces = ListCode[i].Length - ListCode[i].TrimStart().Length;
                    string functionName= ListCode[i].Split(' ')[nbSpaces+1].ToUpper();
                    if(!functionNameNeeded.Contains(functionName))
                        functionNameNeeded.Add(functionName);
                }
            }
            
            List<string> ListCodeL = ListCode.ToList();
            int lastManuallyEnteredLine = ListCode.Length;
            for (int i = 0; i < functionNameNeeded.Count; i++)
            {
                foreach(KeyValuePair<string,List<string>>a in  FunctionList)
                {
                    if(a.Key== functionNameNeeded[i])
                    {
                        ListCodeL.AddRange(a.Value);
                        /*
                        for(int j = 0;j < a.Value.Count();j++)
                            richTextBoxCode.Text+= a.Value[j]+"\n";
                        */
                    }
                }
            }
            
            string Msg = "";
            bool error = false;
            ListCode = ListCodeL.ToArray();
            
            
            //---------------------------------------------------------
            //generate ASM code
            List<int> listError = new List<int>();
            for (int i = 0; i < ListCode.Length; i++)
            {
                ManageLine(ListCode[i], "", false, new List<string>(), labels, out Msg);
                if (Msg != "")
                {
                    listBoxError.Items.Add("Line " + i + ": " + Msg);
                    error = true;
                }
            }

            for (int i = 0; i < ASM.Count; i++)
            {
                if (ASM[i].StartsWith("A="))
                {
                    foreach (KeyValuePair<string, int> kv in labels)
                    {
                        if (ASM[i].ToUpper().Contains(kv.Key))
                        {
                            ASM[i]=ASM[i].ToUpper().Replace(kv.Key, kv.Value.ToString());
                        }
                    }
                }
            }
            if (Msg != "")
            {
                listBoxError.Items.Add("Line " + ASMCode.Count + ": " + Msg);
                return;
            }
            //---------------------------------------------------
            //generate machine code
            myComputer.ClearRom();
            for (int i = 0; i < ASM.Count; i++)
            {
                Msg = "";
                if (ASM[i] != "")
                {
                    string codeS = Tools.Decode(ASM[i], Csts, out Msg).ToString();
                    if (Msg != "")
                    {
                        error = true;
                        listBoxError.Items.Add("Line " + ASMCode.Count + ": " + Msg);
                        listError.Add(ASMCode.Count);
                        ASMCode.Add(0);
                    }
                    else
                    {
                        int code = int.Parse(codeS);
                        ASMCode.Add(code);
                    }
                }
            }

            string PcTxt = "";
            int min= Math.Min(lastManuallyEnteredLine, PC.Count);
            for (int i = 0; i < min; i++)
            {
                string a = "";
                if (PC[i] == -1)
                    PcTxt +=  a.PadLeft(5)+"\n";
                else
                    PcTxt += PC[i].ToString().PadLeft(5) + "\n";
            }
            richTextBoxCode_PC.Text = PcTxt;

            if (error == true)
            {
                List<int> listLineError = new List<int>();
                for (int i = 0; i < listError.Count; i++)
                {
                    int line = UpdatePC(listError[i], true);
                    listLineError.Add(line);
                }
                SyntaxColorUpdate(listLineError);
                return;
            }
            for (int i = 0; i < ASMCode.Count; i++)
            {
                myComputer.setRom(i, ASMCode[i]);//A=48
            }
            Last_PC = ASMCode.Count;
            UpdateFromRom();
            SynchronizerichTextBox();
            Reset();
            toolStripButtonTick.Enabled = true;
            toolStripButtonRun.Enabled = true;
            toolStripButtonReset.Enabled = true;
            toolStripButtonStop.Enabled = true;
        }
    }
}
