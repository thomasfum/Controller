using Controller.ALU;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controller
{
    public partial class Form1
    {
        //----------------------------
        //PC

        bool Tick()
        {
            if (Last_PC == 0)
                return false;
            myComputer.proceed();
            Current_PC = myComputer.getPC();
            

            if (speed < 2)
            {
                int A = myComputer.getA();
                int D = myComputer.getD();
                int I = myComputer.getI();
                int ALU = Tools.getALU(I);

                label_A.Text = "A=" + A;
                label_D.Text = "D=" + D;
                label_PC.Text = "PC=" + Current_PC;
                label_I.Text = "I=" + I;
                label_ALU.Text = "ALU=" + ALU;
                dataGridView_Rom.ClearSelection();
                dataGridView_Rom.Rows[Current_PC].Selected = true;
                updateI(I);
                refreshRam();
                UpdatePC(Current_PC, false);
                UpdateStack();
                label_Nand_count.Text = "Nand count: " + myComputer.GetNandCount();
            }
            pictureBox1.Refresh();
            refreshMappedIO();

            if (Current_PC >= Last_PC)
            {
                toolStripButtonTick.Enabled = false;
                return false;
            }
            return true;

        }


        void ColorRegister(string txt, int startpos)
        {
            txt = txt.ToUpper();
           
            if (txt.Length > 2)
            {
                int offset = 0;
                if (txt[0] == ' ')
                    offset = 1;
                if ((txt[offset] == '0') && (txt[offset+1] == 'X'))
                    return;
                if (txt[offset] == '\'')
                {
                    richTextBoxCode.Select(startpos+1+ offset, 1);
                    richTextBoxCode.SelectionColor = Color.Green;
                }
            }
            for (int i = 0; i < txt.Length; i++)
            {
                if ((txt[i] == 'A') | (txt[i] == 'D') || (txt[i] == '*'))
                {
                    richTextBoxCode.Select(startpos + i, 1);
                    richTextBoxCode.SelectionColor = Color.LightGreen;
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


        void AnalyseLines(string txt, List<int> ListErrors)
        {
            int pos = 0;
            int nextpos = 0;
            bool shoudStop = false;
            int lineCpt = 0;
            richTextBoxCode.SelectAll();
            richTextBoxCode.SelectionColor = Color.White;
            UpdateReserveredWords();
            do
            {
                pos = txt.IndexOf("\n", nextpos);
                if (pos < 0)
                {
                    shoudStop = true;
                    pos = txt.Length;
                }
                string line = txt.Substring(nextpos, pos - nextpos);
                richTextBoxCode.Select(nextpos, pos - nextpos);
                richTextBoxCode.SelectionBackColor = Color.Black;


                if (line.StartsWith("#"))//comment
                {
                    richTextBoxCode.Select(nextpos, pos - nextpos);
                    richTextBoxCode.SelectionColor = Color.DarkGreen;
                }
                else if (line.ToUpper().StartsWith("JMP"))
                {
                    richTextBoxCode.Select(nextpos, 3);
                    richTextBoxCode.SelectionColor = Color.Yellow;
                    richTextBoxCode.SelectionBackColor = Color.Black;
                }
                else if (line.ToUpper().StartsWith("LABEL"))
                {
                    int posSpace = line.IndexOf(" ");
                    richTextBoxCode.Select(nextpos, posSpace);
                    richTextBoxCode.SelectionColor = Color.Cyan;
                    richTextBoxCode.SelectionBackColor = Color.Black;
                    if (posSpace > 0)
                    {
                        richTextBoxCode.Select(nextpos + posSpace, line.Length - posSpace - 1);
                        richTextBoxCode.SelectionColor = Color.Yellow;
                        richTextBoxCode.SelectionBackColor = Color.Black;
                    }
                }
                else if (line.ToUpper().StartsWith("PRINT"))
                {
                    int posSpace = line.IndexOf(" ");
                    richTextBoxCode.Select(nextpos, posSpace);
                    richTextBoxCode.SelectionColor = Color.Cyan;
                    richTextBoxCode.SelectionBackColor = Color.Black;
                    if (posSpace > 0)
                    {
                        ColorRegister(line.Substring(posSpace+1).ToUpper(), nextpos + posSpace+1);
                       
                    }
                }
                else if (line.ToUpper().StartsWith("GOTO") || (line.ToUpper().StartsWith("IF.GOTO")))
                {
                    int posSpace = line.IndexOf(" ");
                    richTextBoxCode.Select(nextpos, posSpace);
                    richTextBoxCode.SelectionColor = Color.Cyan;
                    richTextBoxCode.SelectionBackColor = Color.Black;
                    if (posSpace > 0)
                    {
                        richTextBoxCode.Select(nextpos + posSpace, line.Length - posSpace);
                        richTextBoxCode.SelectionColor = Color.Yellow;
                        richTextBoxCode.SelectionBackColor = Color.Black;
                    }
                }
                else
                {
                    int ReserverdWordIndex = -1;
                    for (int z = 0; z < reservedWords.Count; z++)
                        if (line.ToUpper().StartsWith(reservedWords[z]))
                            ReserverdWordIndex = z;

                    if (ReserverdWordIndex >= 0)
                    {
                        richTextBoxCode.Select(nextpos, reservedWords[ReserverdWordIndex].Length);
                        richTextBoxCode.SelectionColor = Color.Cyan;
                        richTextBoxCode.SelectionBackColor = Color.Black;
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
                                        richTextBoxCode.Select(nextpos + posCst, cst.Length);
                                        richTextBoxCode.SelectionColor = Color.Cyan;
                                        richTextBoxCode.SelectionBackColor = Color.Black;
                                    }
                                }
                            }

                        }
                        int eqPos = line.IndexOf('=');
                        if (eqPos > 0)//we have a =
                        {
                            string sub1 = line.Substring(0, eqPos);
                            ColorRegister(sub1, nextpos);
                        }
                        int scPos = line.IndexOf(';');
                        if (scPos > 0)//we have a ;
                        {
                            richTextBoxCode.Select(nextpos + scPos, pos - nextpos - scPos);
                            richTextBoxCode.SelectionColor = Color.Yellow;
                        }

                        if (scPos < 0)
                            scPos = pos - nextpos;
                        string sub = line.Substring(eqPos + 1, scPos - eqPos - 1);
                        ColorRegister(sub, nextpos + eqPos + 1);
                    }

                    if (ListErrors != null)
                    {
                        for (int i = 0; i < ListErrors.Count; i++)
                        {
                            if (ListErrors[i] == lineCpt)
                            {
                                richTextBoxCode.Select(nextpos, pos - nextpos);
                                richTextBoxCode.SelectionBackColor = Color.LightCoral;
                            }
                            else
                            {
                                richTextBoxCode.Select(nextpos, pos - nextpos);
                                richTextBoxCode.SelectionBackColor = Color.Black;
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

        void ManageLine(string line, bool isMacro, List<string> paramList)
        {
            //line = line.ToUpper();
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
                labels.Add(lab.Replace(":", ""), counter);
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
                        AddMacro(line, m.Key, m.Value);
                    }

                }
                if (macroFound == false)
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
                        if (line.ToUpper().Contains("ARGUMENTCOUNT"))
                        {
                            line = line.ToUpper().Replace("ARGUMENTCOUNT", paramList[1]);
                        }
                        if (line.ToUpper().Contains("LOCALSCOUNT"))
                        {
                            line = line.ToUpper().Replace("LOCALSCOUNT", paramList[1]);
                        }

                    }
                    counter++;
                    ASM.Add(line);
                }

            }
        }



        void AddMacro(string ListCode, string MacroName, List<string> macroValue)
        {
            //ici rechercher les parametres de la macro et les passer a ManageLine
            string[] param = ListCode.Split(' ');
            for (int i = 0; i < macroValue.Count; i++)
            {
                List<string> p = param.ToList();
                p.RemoveAt(0);
                //if((macroValue[i]!="")&& (!macroValue[i].StartsWith("#")))
                    ManageLine(macroValue[i], true, p);
            }
        }
        private void Generate()
        {
            counter = 0;
            label_Error.Text = "";
            string txt = richTextBoxCode.Text;
            string[] ListCode = txt.Split('\n');
            PC = new List<int>();
            ASM = new List<string>();
            ASMCode = new List<int>();
            string Msg2 = "";
            Dictionary<string, int> Csts = GetConstants(out Msg2);
            labels = new Dictionary<string, int>();
            if (Msg2 != "")
            {
                label_Error.Text += "Constant Error:" + Msg2;
                return;
            }
            for (int i = 0; i < ListCode.Length; i++)
            {
                ManageLine(ListCode[i], false, new List<string>());
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


            bool error = false;
            myComputer.ClearRom();
            List<int> listError = new List<int>();
            for (int i = 0; i < ASM.Count; i++)
            {
                string Msg = "";
                if (ASM[i] != "")
                {
                    string codeS = Tools.Decode(ASM[i], Csts, out Msg).ToString();
                    if (Msg != "")
                    {
                        error = true;
                        label_Error.Text += "Line " + i + ": " + Msg + "\n";
                        listError.Add(i);
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
            for (int i = 0; i < PC.Count; i++)
            {
                if (PC[i] == -1)
                    PcTxt += "\n";
                else
                    PcTxt += PC[i] + "\n";
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
            Reset();

        }
    }
}
