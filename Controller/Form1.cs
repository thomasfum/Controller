using Controller.Processor;
using System;

using System.Collections.Generic;

using System.Data;

using System.Drawing;
using System.IO;
using System.Linq;

using System.Runtime.InteropServices;

using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

//TODO
// stack debug
// keyboard
// macros predifined
// macro perso
// TODO in code
// bouton F5 F10 F11

//idee program:
//  - calculatrice
//  - labyrinthe



namespace Controller
{
    public partial class Form1 : Form
    {
        Computer myComputer;
        DataTable dt_Rom = new DataTable();
        DataTable dt_Ram = new DataTable();
        DataTable dt_Constants = new DataTable();
        public bool[] IGenerated = new bool[16];
        public bool[] InOut = new bool[16];
        List<int> PC = new List<int>();
        List<string> ASM = new List<string>();
        List<int> ASMCode = new List<int>();
        Dictionary<string, List<string>> MacroList = new Dictionary<string, List<string>>();
        int RomSize = 50;
        string currentFileName = "";
        bool ScrollPC = true;
        int Last_PC=0;
        public Form1()
        {
            InitializeComponent();
            myComputer = new Computer(256,256);
            //myComputer.setRom(0, 32912);//D=0
            int pc = 0;
            myComputer.setRom(pc++, 48);//A=48
            myComputer.setRom(pc++, 33168);
            myComputer.setRom(pc++, 2);
            myComputer.setRom(pc++, 34064);
            //myComputer.setRom(3, -27960);
            myComputer.setRom(pc++, 37576);
            myComputer.setRom(pc++, 1);
            myComputer.setRom(pc++, 32775);


            dt_Ram.Columns.Add("No");
            dt_Ram.Columns.Add("Data");
            // and then add your rows
            for (int i = 0; i < 30; i++)
            {
                var row = dt_Ram.NewRow();
                // Set values for columns with row[i] = xy
                int I = myComputer.GetRam(i);
                row[0] = i;
                row[1] = I;
                dt_Ram.Rows.Add(row);
            }
            dt_Rom.Columns.Add("No");
            dt_Rom.Columns.Add("Instruction");
            dt_Rom.Columns.Add("Assembly");

         
            UpdateFromRom();

            dataGridView_Ram.DataSource = dt_Ram;
            dataGridView_Ram.Columns[0].Width = 35;
            dataGridView_Ram.RowHeadersVisible = false;
            dataGridView_Ram.Columns[0].ReadOnly = true;
            dataGridView_Ram.Columns[1].Width = dataGridView_Ram.Width - 35 - 20;

            dataGridView_Rom.DataSource = dt_Rom;
            dataGridView_Rom.Columns[0].Width = 35;
            dataGridView_Rom.RowHeadersVisible = false;
            dataGridView_Rom.Columns[0].ReadOnly = true;

            dataGridView_Rom.Columns[1].Width = 50;
            dataGridView_Rom.Columns[2].Width = dataGridView_Rom.Width-35-50-20;
            updateI(0);
            UpdateIGenerated();
            dataGridView_Rom.ClearSelection();
            dataGridView_Rom.Rows[0].Selected = true;
            //dataGridView1.SelectionMode= DataGridViewSelectionMode.FullRowSelect;
            label_Instruction_b.UseMnemonic = false;
            label_IGeneratedAsm.UseMnemonic = false;
            myComputer.SetDisplayImage(pictureBox1.Image as Bitmap);

            button9.Enabled = false;
            
            //richTextBoxCode.SetInnerMargins(dist, dist, dist, 0);
            richTextBoxCode.SelectAll();
            richTextBoxCode.SelectionIndent += 5;//play with this values to match yours
            //richTextBoxCode.SelectionRightIndent += 15;//this too
            richTextBoxCode.SelectionLength = 0;

            richTextBoxCode_PC.SelectAll();
            richTextBoxCode_PC.SelectionAlignment = HorizontalAlignment.Right;


            label_Instruction_t1.Text = "|   " + " . " + " . " + "   " + " . ";
            label_Instruction_t1.Text += " | ";
            label_Instruction_t1.Text += "   operation   ";
            label_Instruction_t1.Text += " | ";
            label_Instruction_t1.Text += " target  ";
            label_Instruction_t1.Text += " | ";
            label_Instruction_t1.Text += "  jump   ";
            label_Instruction_t1.Text += " | decimal |";
            label_Instruction_t1.Text += "  HEX |";
            label_Instruction_t1.Text += "  Assembler  |";

            label_Instruction_t2.Text = "|ci ";
            label_Instruction_t2.Text += " . ";
            label_Instruction_t2.Text += " . ";
            label_Instruction_t2.Text += " * ";
            label_Instruction_t2.Text += " . ";
            label_Instruction_t2.Text += " | ";
            label_Instruction_t2.Text += " u ";
            label_Instruction_t2.Text += "op1";
            label_Instruction_t2.Text += "op0";
            label_Instruction_t2.Text += "zx ";
            label_Instruction_t2.Text += "sw ";
            label_Instruction_t2.Text += " | ";

            label_Instruction_t2.Text += " a ";
            label_Instruction_t2.Text += " d ";
            label_Instruction_t2.Text += "a* ";
            label_Instruction_t2.Text += " | ";
            label_Instruction_t2.Text += "lt ";
            label_Instruction_t2.Text += "eq ";
            label_Instruction_t2.Text += "gt ";
            label_Instruction_t2.Text += " |         |";
            label_Instruction_t2.Text += "      |";
            label_Instruction_t2.Text += "             |";

            labelInstructionBottom.Text = "|                |                 |           |           |         |      |             |";


            dt_Constants.Columns.Add("Name");
            dt_Constants.Columns.Add("Value");

            dataGridView_Constant.DataSource = dt_Constants;
            MacroList.Add("INIT.STACK", new List<string>() { "A=256", "D=A","A=0","*A=D" });
            MacroList.Add("PUSH.D", new List<string>() { "A=0", "A=*A", "*A=D","A=0","D=*A","D=D+1","*A=D" });
            MacroList.Add("POP.D", new List<string>() { "A=0", "A=*A-1", "D=*A", "A=0", "*A=*A-1" });
            MacroList.Add("POP.A", new List<string>() { "A=0", "*A=*A-1", "A=*A", "A=*A" });
            MacroList.Add("PUSH.VALUE", new List<string>() { "A=value", "D=A", "A=0", "A=*A", "*A=D", "A=0", "D=*A", "D=D+1", "*A=D" });
            MacroList.Add("PUSH.STATIC", new List<string>() { "A=address", "D=*A", "A=0", "A=*A", "*A=D", "A=0", "D=*A", "D=D+1", "*A=D" });
            MacroList.Add("POP.STATIC", new List<string>() { "A=0", "A=*A-1", "D=*A", "A=address","*A=D","A=0", "*A=*A-1" });
            MacroList.Add("POP.MEMORY", new List<string>() { "pop.D", "pop.A", "*A=D" });
            MacroList.Add("PUSH.MEMORY", new List<string>() { "pop.A", "D=*A", "push.D" });

        }


        //----------------------------
        //PC
        private void Reset_Click(object sender, EventArgs e)
        {
            dataGridView_Rom.ClearSelection();
            dataGridView_Rom.Rows[0].Selected = true;
            richTextBoxCode_PC.SelectAll();
            richTextBoxCode_PC.SelectionBackColor = Color.Black;
            myComputer.reset();
        }
        private void Tick_Click(object sender, EventArgs e)
        {
            myComputer.proceed();
            int A = myComputer.getA();
            int D = myComputer.getD();
            int I = myComputer.getI();
            int ALU = Tools.getALU(I);
            int PC = myComputer.getPC();
            label_A.Text = "A=" + A;
            label_D.Text = "D=" + D;
            label_PC.Text = "PC=" + PC;
            label_I.Text = "I=" + I;
            label_ALU.Text = "ALU=" + ALU;
            int ram0=myComputer.GetRam(0);
            label_SP.Text = "SP=" + ram0;
            dataGridView_Rom.ClearSelection();
            dataGridView_Rom.Rows[PC].Selected = true;

            
            updateI(I);
            refreshRam();
            refreshMappedIO();
            label_Nand_count.Text= "Nand count: "+myComputer.GetNandCount();
            pictureBox1.Refresh();
            UpdatePC(PC,false);
            
        }
     

        private void NandGame_TextChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://nandgame.com");
        }



        //----------------------------
        //RAM
        void refreshRam()
        {
            for (int i = 0; i < 10; i++)
            {
                // Set values for columns with row[i] = xy
                int I = myComputer.GetRam(i);
                dataGridView_Ram.Rows[i].Cells[1].Value = I.ToString();
            }
        }
        private void RAM_Clear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView_Ram.RowCount; i++)
            {
                myComputer.setRam(i, 0);
            }
            refreshRam();
        }
        private void RAM_Write_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView_Ram.RowCount; i++)
            {
                if ((string)dataGridView_Ram.Rows[i].Cells[0].Value != "")
                {
                    string s = (string)dataGridView_Ram.Rows[i].Cells[1].Value;
                    if (s != null)
                        myComputer.setRam(i, int.Parse(s));
                }
            }
        }
        //----------------------------
        //ROM
        private void ROM_GridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
           
            if (e.ColumnIndex == 1)
            {

                int row = e.RowIndex;
                string cell = (string)dgv?.Rows?[row]?.Cells?[e.ColumnIndex]?.Value;
                string cell2=cell.Replace(" ", "");
                char a= Convert.ToChar(160);
                cell2 = cell.Replace(""+a, "");
                int I=int.Parse(cell2);
                string Inst = "";
                if (Tools.get15(I) == true)
                    Inst = Tools.GetInstruction(I);
                else
                    Inst = "A=" + I;
                dgv.Rows[row].Cells[2].Value = Inst;
                dgv.Rows[row].Cells[0].Value = row;
            }
            if (e.ColumnIndex == 2)
            {
             
                int row = e.RowIndex;
                dgv.Rows[row].Cells[0].Value = row;
                string val = (string)dgv.Rows[row].Cells[2].Value;
                val = val.Replace(" ", "");
                string Msg = "";
                Dictionary<string, int> Csts = GetConstants(out Msg);
                if (Msg != "")
                    MessageBox.Show(Msg);
                else
                {
                    string rep = Tools.Decode(val, Csts, out Msg).ToString();

                    if (Msg != "")
                        MessageBox.Show(Msg);
                    else
                        dgv.Rows[row].Cells[1].Value = rep;
                }
            }
        }
        private void ROM_Write_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < dataGridView_Rom.RowCount; i++)
            {
                if ((string)dataGridView_Rom.Rows[i].Cells[0].Value != "")
                {
                    string s = (string)dataGridView_Rom.Rows[i].Cells[1].Value;
                    if (s != null)
                        myComputer.setRom(i, int.Parse(s));
                }
            }
        }
        private void UpdateFromRom()
        {
            dt_Rom.Clear();
            for (int i = 0; i < RomSize; i++)
            {
                var row = dt_Rom.NewRow();
                // Set values for columns with row[i] = xy
                int I = myComputer.GetRom(i);
                row[0] = i;
                row[1] = I;
                string Inst = "";
                if (Tools.get15(I) == true)
                    Inst = Tools.GetInstruction(I);
                else
                    Inst = "A=" + I;
                row[2] = Inst;
                dt_Rom.Rows.Add(row);
            }
        }
        //----------------------------
        //Instruction Registry 
        void updateI(int I)
        {
            // top
           
            label_Instruction_b.Text = "|";
          
            if (Tools.get15(I) == true)
                label_Instruction_b.Text += " 1 ";
            else
                label_Instruction_b.Text += " 0 ";
          
            label_Instruction_b.Text += " 0  0 ";
           
            if (Tools.get12(I) == true)
                label_Instruction_b.Text += " 1 ";
            else
                label_Instruction_b.Text += " 0 ";
           
            label_Instruction_b.Text += " 0 ";
          
       
            label_Instruction_b.Text += " | ";
            //alu
            
            bool[] Alub = Tools.int2BoolArray(Tools.getALU(I));
            
            label_Instruction_b.Text += " " + (Alub[4] ? '1' : '0') + " ";
            label_Instruction_b.Text += " " + (Alub[3] ? '1' : '0') + " ";
            label_Instruction_b.Text += " " + (Alub[2] ? '1' : '0') + " ";
            label_Instruction_b.Text += " " + (Alub[1] ? '1' : '0') + " ";
            label_Instruction_b.Text += " " + (Alub[0] ? '1' : '0') + " ";
            label_Instruction_b.Text += " | ";

           

            bool[] targetb = Tools.int2BoolArray(Tools.getTarget(I));
            label_Instruction_b.Text += " " + (targetb[2] ? '1' : '0') + " ";
            label_Instruction_b.Text += " " + (targetb[1] ? '1' : '0') + " ";
            label_Instruction_b.Text += " " + (targetb[0] ? '1' : '0') + " ";
            label_Instruction_b.Text += " | ";
            
            bool[] JMPb = Tools.int2BoolArray(Tools.getJmp(I));
            label_Instruction_b.Text += " " + (JMPb[2] ? '1' : '0') + " ";
            label_Instruction_b.Text += " " + (JMPb[1] ? '1' : '0') + " ";
            label_Instruction_b.Text += " " + (JMPb[0] ? '1' : '0') + " ";

            label_Instruction_b.Text += " | " + I.ToString("D").PadLeft(7) + " |";

            
            
            string txt = I.ToString("X");
            if (txt.Length > 4)
                label_Instruction_b.Text += " " + I.ToString("X").Substring(4) + " |";
            else
                label_Instruction_b.Text += " " + txt.PadLeft(4, '0') + " |";


            string Inst = "";
            if (Tools.get15(I) == true)
                Inst = Tools.GetInstruction(I);
            else
                Inst = "A=" + I;
            
            label_Instruction_b.Text += Inst.PadRight(12) + " |";

           
        }
        void UpdateIGenerated()
        {
            int I= Tools.boolArray2Int(IGenerated);
            textBox_IGeneratedD.Text = I.ToString();
            string txt = I.ToString("X");
            if (txt.Length > 4)
                label_IGeneratedX.Text = " " + I.ToString("X").Substring(4);
            else
                label_IGeneratedX.Text = " " + txt.PadLeft(4, '0');
            string Inst = "";
            if (Tools.get15(I) == true)
                Inst = Tools.GetInstruction(I);
            else
                Inst = "A=" + I;
            label_IGeneratedAsm.Text= Inst;
        }

        private void textBox_IGeneratedD_Validated(object sender, EventArgs e)
        {
            UpdateCheckBoxed(sender);
        }
        void UpdateCheckBoxed(object sender)
        {
            TextBox txt = (TextBox)sender;
            string a = txt.Text;
            int val = int.Parse(a);
            bool[] arr = new bool[16];
            arr = Tools.int2BoolArray(val);
            checkBox15.Checked = arr[0];
            checkBox16.Checked = arr[1];
            checkBox14.Checked = arr[3];
            checkBox13.Checked = arr[2];
            checkBox12.Checked = arr[5];
            checkBox11.Checked = arr[4];
            checkBox10.Checked = arr[7];
            checkBox9.Checked = arr[6];
            checkBox8.Checked = arr[9];
            checkBox7.Checked = arr[8];
            checkBox6.Checked = arr[11];
            checkBox5.Checked = arr[10];
            checkBox4.Checked = arr[13];
            checkBox3.Checked = arr[12];
            checkBox2.Checked = arr[14];
            checkBox1.Checked = arr[15];
        }


        private void textBox_IGeneratedD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                UpdateCheckBoxed(sender);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        //----------------------------
        //Display
        private void Display_Clear_Click(object sender, EventArgs e)
        {
            myComputer.ClearDisplay();
            pictureBox1.Refresh();
        }

        //----------------------------
        //inout
        void refreshMappedIO()
        {
            int val = myComputer.GetInOut();
            bool[] inout = Tools.int2BoolArray(val);
            checkBox17.Checked = inout[0];
            checkBox18.Checked = inout[1];
            checkBox19.Checked = inout[2];
            checkBox20.Checked = inout[3];
            checkBox21.Checked = inout[4];
            checkBox22.Checked = inout[5];
            checkBox23.Checked = inout[6];
            checkBox24.Checked = inout[7];

        }

        private void InOut_Clear_Click(object sender, EventArgs e)
        {
            checkBox17.Checked = false;
            checkBox18.Checked = false;
            checkBox19.Checked = false;
            checkBox20.Checked = false;
            checkBox21.Checked = false;
            checkBox22.Checked = false;
            checkBox23.Checked = false;
            checkBox24.Checked = false;
            myComputer.ResetInOut();
        }

        //----------------------------
        //check boxes

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[15] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[14] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[13] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[12] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[11] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[10] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[9] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[8] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[7] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[6] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[5] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[4] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[3] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[2] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[1] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            IGenerated[0] = ((CheckBox)sender).Checked;
            UpdateIGenerated();
        }

      

    
        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            InOut[7]= ((CheckBox)sender).Checked;
            myComputer.SetInOut(InOut);
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            InOut[6] = ((CheckBox)sender).Checked;
            myComputer.SetInOut(InOut);
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            InOut[5] = ((CheckBox)sender).Checked;
            myComputer.SetInOut(InOut);
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            InOut[4] = ((CheckBox)sender).Checked;
            myComputer.SetInOut(InOut);
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            InOut[3] = ((CheckBox)sender).Checked;
            myComputer.SetInOut(InOut);
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            InOut[2] = ((CheckBox)sender).Checked;
            myComputer.SetInOut(InOut);
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            InOut[1] = ((CheckBox)sender).Checked;
            myComputer.SetInOut(InOut);
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            InOut[0] = ((CheckBox)sender).Checked;
            myComputer.SetInOut(InOut);
        }


        void ColorRegister(string txt, int startpos)
        {
            for (int i=0;i<txt.Length;i++)
            {
                if ((txt[i]=='A')| (txt[i] == 'D')||(txt[i] == '*'))
                {
                    richTextBoxCode.Select(startpos+i,1);
                    richTextBoxCode.SelectionColor = Color.LightGreen;
                }

            }
        }
        List<string> reservedWords = new List<string>();//= { "JMP", "init.stack" };
        void UpdateReserveredWords()
        {
            reservedWords=MacroList.Keys.ToList<string>();
            reservedWords.Add("JMP");
            reservedWords = reservedWords.ConvertAll(d => d.ToUpper());
        }


        void AnalyseLines(string txt, List<int> ListErrors)
        {
            int pos = 0;
            int nextpos = 0;
            bool shoudStop = false;
            int lineCpt = 0;
            UpdateReserveredWords();
            do
            {
                pos = txt.IndexOf("\n", nextpos);
                if (pos < 0)
                {
                    shoudStop = true;
                    pos= txt.Length;
                }
                string line = txt.Substring(nextpos, pos - nextpos);
                richTextBoxCode.Select(nextpos, pos - nextpos);
                richTextBoxCode.SelectionBackColor = Color.Black;

                if (line.StartsWith("#"))//comment
                {
                    richTextBoxCode.Select(nextpos, pos- nextpos);
                    richTextBoxCode.SelectionColor = Color.DarkGreen;
                }
                else 
                {
                    int ReserverdWordIndex = -1;
                    for (int z = 0; z < reservedWords.Count; z++)
                        if (line.ToUpper().StartsWith(reservedWords[z]))
                            ReserverdWordIndex = z;
                    if(ReserverdWordIndex>=0)
                    {
                        richTextBoxCode.Select(nextpos, reservedWords[ReserverdWordIndex].Length);
                        richTextBoxCode.SelectionColor = Color.Cyan;
                        richTextBoxCode.SelectionBackColor = Color.Black;
                    }
                    else//analize the line
                    {
                        for (int i = 0; i < dataGridView_Constant.Rows.Count; i++)
                        {
                            string cst=(string)dataGridView_Constant.Rows[i].Cells[0].Value;
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
                        int eqPos=line.IndexOf('=');
                        if (eqPos > 0)//we have a =
                        {
                            string sub1 = line.Substring(0, eqPos);
                            ColorRegister(sub1, nextpos);
                        }
                        int scPos = line.IndexOf(';');
                        if (scPos > 0)//we have a ;
                        {
                            richTextBoxCode.Select(nextpos+ scPos, pos - nextpos- scPos);
                            richTextBoxCode.SelectionColor = Color.Yellow;
                        }
                       
                        if (scPos < 0)
                            scPos = pos - nextpos;
                        string sub=line.Substring(eqPos +1, scPos-eqPos-1);
                        ColorRegister(sub, nextpos +eqPos + 1);
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
            } while (shoudStop ==false);
           
        }
      

        const int WM_SETREDRAW = 11;
        void SyntaxColorUpdate(List<int> ListErrors=null)
        {

            
            int SelectionStart = richTextBoxCode.SelectionStart;
            int SelectionLength = richTextBoxCode.SelectionLength;
            string txt = richTextBoxCode.Text;
            int topIndex = richTextBoxCode.GetCharIndexFromPosition(new Point(1, 1));
                    
           
            Point toto = new Point();
            SendMessage(richTextBoxCode.Handle, WM_SETREDRAW, new IntPtr(0), ref toto);
            ScrollPC = false;
            
            AnalyseLines(txt, ListErrors);
            
            richTextBoxCode.Select(topIndex, 1);
            richTextBoxCode.ScrollToCaret();
            richTextBoxCode.Select(SelectionStart, SelectionLength);
            richTextBoxCode.SelectionColor = Color.White;
           
            
            SendMessage(richTextBoxCode.Handle, WM_SETREDRAW, new IntPtr(1), ref toto);
                       
            richTextBoxCode.Refresh();
            richTextBoxCode.Invalidate();
            richTextBoxCode.Update();
            ScrollPC = true;
        }
        private void richTextBoxCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SyntaxColorUpdate();
                //e.Handled = true;
                //e.SuppressKeyPress = true;
            }
            if ((e.KeyCode == Keys.Up)|| (e.KeyCode == Keys.PageUp)|| (e.KeyCode == Keys.Down) || (e.KeyCode == Keys.PageDown))
                SyntaxColorUpdate();
            
        }

     
        Dictionary<string,int> GetConstants(out string Msg)
        {
            Msg = "";
            Dictionary<string, int> Cst = new Dictionary<string, int>();
            for (int i = 0; i < dataGridView_Constant.Rows.Count; i++)
            {
                string CstName=(string)dataGridView_Constant.Rows[i].Cells[0].Value;
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
            return Cst;
        }

        void ManageLine(string line, List<string>  paramList)
        {
            if (line == "")
            {
                PC.Add(-1);
                ASM.Add("");
            }
            else if (line.StartsWith("#"))
            {
                PC.Add(-1);
                ASM.Add("");
            }
            else if (line == "")
            {
                PC.Add(-1);
                ASM.Add("");
            }
            else
            {
                PC.Add(ASM.Count);
                bool macroFound = false;
                foreach (KeyValuePair<string, List<string>> m in MacroList)
                {
                    if (line.StartsWith(m.Key))
                    {
                        macroFound = true;
                        AddMacro(line, m.Key, m.Value);
                    }

                }
                if (macroFound == false)
                {
                    if (paramList.Count > 0)
                    {
                        if (line.Contains("VALUE"))
                        {
                            line = line.Replace("VALUE", paramList[0]);
                        }
                        if (line.Contains("value"))
                        {
                            line = line.Replace("value", paramList[0]);
                        }
                        if (line.Contains("ADDRESS"))
                        {
                            line = line.Replace("ADDRESS", paramList[0]);
                        }
                        if (line.Contains("address"))
                        {
                            line = line.Replace("address", paramList[0]);
                        }
                    }
                    ASM.Add(line);
                }
             
            }
        }



        void AddMacro(string ListCode, string MacroName, List<string> macroValue)
        {
            //ici rechercher les parametres de la macro et les passer a ManageLine
            string [] param=ListCode.Split(' ');
            for (int i = 0; i < macroValue.Count; i++)
            {
                List<string> p=param.ToList();
                p.RemoveAt(0);
                //ASM.Add(macroValue[i]);
                ManageLine(macroValue[i], p);
            }
        }
        private void Generate_Click(object sender, EventArgs e)
        {
            label_Error.Text = "";
            string txt = richTextBoxCode.Text;
            //string txt1= txt.Replace(" ", "").ToUpper();
            string txt1= txt.ToUpper();
            string[] ListCode=txt1.Split('\n');
            PC= new List<int>();
            ASM = new List<string>();
            ASMCode = new List<int>();
            string Msg2 = "";
            Dictionary<string, int>  Csts =GetConstants(out Msg2);
            if (Msg2 != "")
            {
                label_Error.Text += "Constant Error:" + Msg2;
                return;
            }
            for (int i=0;i<ListCode.Length;i++)
            {
                ManageLine(ListCode[i], new List<string>());
                /*
                if (ListCode[i]=="")
                {
                    PC.Add(-1);
                    ASM.Add("");
                }
                else if (ListCode[i].StartsWith("#"))
                {
                    PC.Add(-1);
                    ASM.Add("");
                }
                else if (ListCode[i]=="")
                {
                    PC.Add(-1);
                    ASM.Add("");
                }
                else
                {
                    PC.Add(ASM.Count);
                    bool macroFound = false;
                    foreach(KeyValuePair<string,List<string>> m in MacroList)
                    {
                        if (ListCode[i].StartsWith(m.Key))
                        {
                            macroFound = true;
                            AddMacro(ListCode[i], m.Key, m.Value);
                           
                        }

                    }
                    if(macroFound==false)
                        ASM.Add(ListCode[i]);
                  
                }
                */
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
                    int line=UpdatePC(listError[i], true);
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
          
           
        }
        int UpdatePC(int _PC,bool error)
        {
            Color back = Color.LightGreen;
            if(error)
                back = Color.LightCoral;
            //----
            //rom
            if (error == false)
            {
                int top = dataGridView_Rom.FirstDisplayedScrollingRowIndex;
                int nbVisible = dataGridView_Rom.Rows.GetRowCount(DataGridViewElementStates.Displayed);
                int bottom = top + nbVisible;

                if (_PC < top)
                {
                    dataGridView_Rom.FirstDisplayedScrollingRowIndex = dataGridView_Rom.SelectedRows[0].Index;
                }
                else if (_PC >= bottom)
                {
                    dataGridView_Rom.FirstDisplayedScrollingRowIndex = dataGridView_Rom.SelectedRows[0].Index;
                }
            }

            //----
            //code and PC
            if (error == false)
            {
                richTextBoxCode_PC.SelectAll();
                richTextBoxCode_PC.SelectionBackColor = Color.Black;
                richTextBoxCode_PC.SelectionColor = Color.White;
            }
            int lastPCneq = 0;
            int max=PC.Max();
            int pos = -1;
            if (_PC == 0)
            {
                for (int i = 0; i < PC.Count; i++)
                {
                    if (PC[i] >= 0)
                    {
                        lastPCneq = i;
                        string s = PC[i] + "\n";
                        pos = richTextBoxCode_PC.Text.IndexOf(s);
                        richTextBoxCode_PC.Select(pos, s.Length - 1);
                        richTextBoxCode_PC.SelectionBackColor = back;
                        richTextBoxCode_PC.SelectionColor = Color.Black;
                        break;
                    }
                }
            }
            else if (_PC>=max)
            {
                for (int i = 0; i < PC.Count; i++)
                {
                    if (PC[i] == max)
                    {
                        lastPCneq = i;
                        string s = "\n"+ PC[i] + "\n";
                        pos = richTextBoxCode_PC.Text.IndexOf(s);
                        richTextBoxCode_PC.Select(pos, s.Length - 1);
                        richTextBoxCode_PC.SelectionBackColor = back;
                        richTextBoxCode_PC.SelectionColor = Color.Black;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < PC.Count; i++)
                {
                    if (PC[i] > _PC)
                    {
                        string s = "\n" + PC[lastPCneq] + "\n";
                        if (i == 0)
                            s = PC[lastPCneq] + "\n";
                        pos = richTextBoxCode_PC.Text.IndexOf(s);
                        if (pos >= 0)
                        {
                            if (i == 0)
                                richTextBoxCode_PC.Select(pos, s.Length - 1);
                            else
                                richTextBoxCode_PC.Select(pos + 1, s.Length - 2);
                            richTextBoxCode_PC.SelectionBackColor = back;
                            richTextBoxCode_PC.SelectionColor = Color.Black;
                        }

                        break;
                    }
                    if (PC[i] >= 0)
                        lastPCneq = i;
                }
            }
            if (error == false)
            {
                if (pos >= 0)
                {
                    int totalLine = PC.Count;
                    int Line = richTextBoxCode_PC.GetLineFromCharIndex(pos + 1);
                    int topIndex = richTextBoxCode_PC.GetCharIndexFromPosition(new Point(1, 1));
                    int bottomIndex = richTextBoxCode_PC.GetCharIndexFromPosition(new Point(1, richTextBoxCode_PC.Height - 1));
                    int topLine = richTextBoxCode_PC.GetLineFromCharIndex(topIndex);
                    int bottomLine = richTextBoxCode_PC.GetLineFromCharIndex(bottomIndex);
                    if (Line < topLine)
                    {
                        int start = richTextBoxCode.GetFirstCharIndexFromLine(Line);
                        richTextBoxCode.Select(start, 0);
                        richTextBoxCode.ScrollToCaret();
                    }
                    else if (Line >= bottomLine)
                    {
                        int start = richTextBoxCode.GetFirstCharIndexFromLine(Line);
                        richTextBoxCode.Select(start, 0);
                        richTextBoxCode.ScrollToCaret();
                    }
                }
            }
            return richTextBoxCode_PC.GetLineFromCharIndex(pos + 1); ;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, ref Point lp);
        private void richTextBoxCode_VScroll(object sender, EventArgs e)
        {

            //https://www.codeproject.com/articles/Synchronized-Scrolling-of-Multiple-RichTextBoxes
            if (ScrollPC == true)
            {
                int WM_USER = 0x400;
                int EM_GETSCROLLPOS = WM_USER + 221;
                int EM_SETSCROLLPOS = WM_USER + 222;
                Point pt = new Point(0, 0);
                SendMessage(richTextBoxCode.Handle, EM_GETSCROLLPOS, new IntPtr(0), ref pt);
                SendMessage(richTextBoxCode_PC.Handle, EM_SETSCROLLPOS, new IntPtr(0), ref pt);
            }
        }

      
        //load
        private void button8_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory+"../../";
            dlg.RestoreDirectory = false;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            currentFileName= dlg.FileNames[0];
            button9.Enabled = true;
            string readText = File.ReadAllText(currentFileName);


            int CstPos = readText.IndexOf("##CONSTANTS##");
            int CodePos = readText.IndexOf("##CODE##");
            if (CstPos != -1)
            {
                dt_Constants.Rows.Clear();
                string Csts = readText.Substring(CstPos+14, CodePos - CstPos-14);
                string[] CstsList = Csts.Split('\n');
                for (int i = 0; i < CstsList.Length; i++)
                {
                    if (CstsList[i] != "")
                    {
                        string[] cst = CstsList[i].Split('=');
                        dt_Constants.Rows.Add(cst);
                    }

                }
            }
            if (CodePos != -1)
            {
                richTextBoxCode.Text = readText.Substring(CodePos + 9);
                SyntaxColorUpdate();
                richTextBoxCode_PC.Text = "";
            }

        }
        //save
        private void button9_Click(object sender, EventArgs e)
        {
            Save();
        }
        //saveas
        private void button10_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "../../";
            dlg.RestoreDirectory = false;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            currentFileName = dlg.FileNames[0];
            button9.Enabled = true;
            Save();


        }
        private void Save()
        {
            string createText = "##CONSTANTS##\n";
            for (int i = 0; i < dataGridView_Constant.Rows.Count; i++)
            {
                string cst = (string)dataGridView_Constant.Rows[i].Cells[0].Value;
                if ((cst != null) && (cst != ""))
                {
                    createText += cst;
                    createText += "=";
                    createText += (string)dataGridView_Constant.Rows[i].Cells[1].Value;
                    createText += "\n";
                }
            }
            createText += "##CODE##\n";
            createText += richTextBoxCode.Text;
            File.WriteAllText(currentFileName, createText);
        }

        private void richTextBoxCode_Leave(object sender, EventArgs e)
        {
            SyntaxColorUpdate();
        }

        private void richTextBoxCode_Click(object sender, EventArgs e)
        {
            SyntaxColorUpdate();
        }
    }
}
