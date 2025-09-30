using Controller.Processor;
using System;

using System.Collections.Generic;

using System.Data;

using System.Drawing;
using System.IO;
using System.Linq;

using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;


//TODO
//--------------------
// Bugs
// - first macro alone
// - TODO in code
//
//-------------------- 
// Missing fcts
// - gosub
// - floating
// - multiplication
//
//--------------------
// Macro
// - input.s
// - s.to.n
// - input.n
//
//--------------------
// Program ideas:
//  - calculatrice
//  - labyrinthe
//  - pong



namespace Controller
{
    public partial class Form1 : Form
    {

        String AppPath=AppDomain.CurrentDomain.BaseDirectory + "../../";
        Computer myComputer;
        DataTable dt_Rom = new DataTable();
        DataTable dt_Ram = new DataTable();
        DataTable dt_Stack = new DataTable();
        DataTable dt_Constants = new DataTable();

        public bool[] IGenerated = new bool[16];
        public bool[] InOut = new bool[16];
        List<int> PC = new List<int>();
        List<string> ASM = new List<string>();
        List<int> ASMCode = new List<int>();
        Dictionary<string, List<string>> MacroList = new Dictionary<string, List<string>>();
        int RomSize = 256;
        int RamSize = 1024;
        string currentFileName = "";
        bool ScrollPC = true;
        int Last_PC=0;
        int Current_PC = -1;
        int counter = 0;
        bool StopRunningLoop = false;
        List<int> KeyPressed= new List<int>();
        int speed = 0;
        Dictionary<string, int> labels = new Dictionary<string, int>();


        public Form1()
        {
            InitializeComponent();
            myComputer = new Computer(RomSize, RamSize);
            //myComputer.setRom(0, 32912);//D=0

            /*
            int pc = 0;
            myComputer.setRom(pc++, 48);//A=48
            myComputer.setRom(pc++, 33168);
            myComputer.setRom(pc++, 2);
            myComputer.setRom(pc++, 34064);
            //myComputer.setRom(3, -27960);
            myComputer.setRom(pc++, 37576);
            myComputer.setRom(pc++, 1);
            myComputer.setRom(pc++, 32775);
            */

            dt_Ram.Columns.Add("No");
            dt_Ram.Columns.Add("Data");

            dt_Stack.Columns.Add("No");
            dt_Stack.Columns.Add("Data");

            // and then add your rows
            for (int i = 0; i < RamSize; i++)
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
            dataGridView_Ram.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_Ram.AllowUserToAddRows = false;

            dataGridView_Stack.DataSource = dt_Stack;
            dataGridView_Stack.Columns[0].Width = 35;
            dataGridView_Stack.RowHeadersVisible = false;
            dataGridView_Stack.Columns[0].ReadOnly = true;
            dataGridView_Stack.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_Stack.AllowUserToAddRows = false;

            dataGridView_Rom.DataSource = dt_Rom;
            dataGridView_Rom.Columns[0].Width = 35;
            dataGridView_Rom.RowHeadersVisible = false;
            dataGridView_Rom.Columns[0].ReadOnly = true;
            dataGridView_Rom.Columns[1].Width = 50;
            dataGridView_Rom.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            updateI(0);
            UpdateIGenerated();
            dataGridView_Rom.ClearSelection();
            dataGridView_Rom.Rows[0].Selected = true;

            labelKeyboard.UseMnemonic = false;//prevent issue with char &
            label_Data7.UseMnemonic = false;//prevent issue with char &
            label_IGeneratedAsm.UseMnemonic = false;//prevent issue with char &
            myComputer.SetDisplayImage(pictureBox1.Image as Bitmap);
            myComputer.setKeyboardCallBack(() => {
                /*
                 int tmp=currentKeypressed;
                 currentKeypressed = 0;
                 labelKeyboard.Text = "Keyboard key:  ";
                 //currentKeypressed = 0;
                */
                // return currentKeypressed; 
                if (KeyPressed.Count > 0)
                    return KeyPressed.First();
                else
                    return 0;
            }, () => {
                if (KeyPressed.Count > 0)
                    KeyPressed.RemoveAt(0);
                labelKeyboard.Text = "Keyboard key:  "; 
                return 0; });


            toolStripButtonSave.Enabled = false;
            toolStripButtonTick.Enabled = false;
            toolStripButtonRun.Enabled = false;
            toolStripButtonReset.Enabled = false;
            toolStripButtonStop.Enabled = false;

            //richTextBoxCode.SetInnerMargins(dist, dist, dist, 0);
            richTextBoxCode.SelectAll();
            richTextBoxCode.SelectionIndent += 5;//play with this values to match yours
            richTextBoxCode.SelectionLength = 0;

            richTextBoxCode_PC.SelectAll();
            richTextBoxCode_PC.SelectionAlignment = HorizontalAlignment.Right;

            label_Title1.Text = "";
            label_Title2.Text = "Operation";
            label_Title3.Text = "Target";
            label_Title4.Text = "Jump";
            label_Title5.Text = "Decimal";
            label_Title6.Text = "HEX";
            label_Title7.Text = "Assembler";



            label_SubTitle1.Text = "ci ";
            label_SubTitle1.Text += " . ";
            label_SubTitle1.Text += " . ";
            label_SubTitle1.Text += " * ";
            label_SubTitle1.Text += " . ";

            
            label_SubTitle2.Text = " u ";
            label_SubTitle2.Text += "op1";
            label_SubTitle2.Text += "op0 ";
            label_SubTitle2.Text += "zx ";
            label_SubTitle2.Text += "sw ";

            label_SubTitle3.Text = " a ";
            label_SubTitle3.Text += " d ";
            label_SubTitle3.Text += " a* ";
            
            label_SubTitle4.Text = "lt ";
            label_SubTitle4.Text += "eq ";
            label_SubTitle4.Text += "gt ";




            dt_Constants.Columns.Add("Name");
            dt_Constants.Columns.Add("Value");
            

            dataGridView_Constant.DataSource = dt_Constants;
            dataGridView_Constant.RowHeadersVisible = false;
            dataGridView_Constant.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            toolStripComboBox1.SelectedIndex = 0;

            dt_Constants.Rows.Clear();
            /*
            MacroList.Add("INIT.STACK", new List<string>() { "A=256", "D=A","A=0","*A=D" });
            MacroList.Add("PUSH.D", new List<string>() { "A=0", "A=*A", "*A=D","A=0","D=*A","D=D+1","*A=D" });
            MacroList.Add("POP.D", new List<string>() { "A=0", "D=*A", "D=D-1","*A=D","A=D","D=*A" });
            MacroList.Add("POP.A", new List<string>() { "A=0", "*A=*A-1", "A=*A", "A=*A" });
            MacroList.Add("PUSH.VALUE", new List<string>() { "A=value", "D=A", "A=0", "A=*A", "*A=D", "A=0", "D=*A", "D=D+1", "*A=D" });
            MacroList.Add("PUSH.STATIC", new List<string>() { "A=address", "D=*A", "A=0", "A=*A", "*A=D", "A=0", "D=*A", "D=D+1", "*A=D" });
            MacroList.Add("POP.STATIC", new List<string>() { "A=0", "A=*A-1", "D=*A", "A=address","*A=D","A=0", "*A=*A-1" });
            MacroList.Add("POP.MEMORY", new List<string>() { "pop.D", "pop.A", "*A=D" });
            MacroList.Add("PUSH.MEMORY", new List<string>() { "pop.A", "D=*A", "push.D" });
            MacroList.Add("GOTO", new List<string>() { "A=label", "JMP" });
            MacroList.Add("IF.GOTO", new List<string>() { "pop.D", "A=label", "D;JNE" });
            */
            UpdateMacroList();
            this.Closed += new EventHandler(MainWindow_Closed);

        }
        void UpdateMacroList()
        {
            MacroList.Clear();
            //DirectoryInfo d = new DirectoryInfo(AppPath+ @"../../asm/Macros/"); //Assuming Test is your Folder
            DirectoryInfo d = new DirectoryInfo(AppPath + @"../asm/Macros/"); //Assuming Test is your Folder

            FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
            string str = "";

            foreach (FileInfo file in Files)
            {
                str = str + ", " + file.Name;
                //string readText = readFile(AppPath + @"../../asm/Macros/"+file.Name);
                string readText = readFile(AppPath + @"../asm/Macros/" + file.Name);
                int CodePos = readText.IndexOf("##CODE##");
                if (CodePos != -1)
                {
                    string code=readText.Substring(CodePos + 9);
                    string []codeLine=code.Split('\n');
                    string macroName = Path.GetFileNameWithoutExtension(file.Name).ToUpper().Replace("_", ".");
                    MacroList.Add(macroName, codeLine.ToList());
                }
            }
        }






        private void NandGame_TextChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://nandgame.com");
        }



        //----------------------------
        //RAM
        void refreshRam()
        {
            for (int i = 0; i < RamSize; i++)
            {
                // Set values for columns with row[i] = xy
                int I = myComputer.GetRam(i);
                dataGridView_Ram.Rows[i].Cells[1].Value = I.ToString();
            }
        }
        private void RAM_Clear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView_Ram.RowCount-1; i++)
            {
                myComputer.setRam(i, 0);
            }
            refreshRam();
            UpdateStack();
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
            if (Tools.get15(I) == true)
                label_Data1.Text = " 1 ";
            else
                label_Data1.Text = " 0 ";
            label_Data1.Text += " 0  0 ";
            if (Tools.get12(I) == true)
                label_Data1.Text += " 1 ";
            else
                label_Data1.Text += " 0 ";
            label_Data1.Text += " 0 ";

            //alu
            bool[] Alub = Tools.int2BoolArray(Tools.getALU(I));
            label_Data2.Text = " " + (Alub[4] ? '1' : '0') + " ";
            label_Data2.Text += " " + (Alub[3] ? '1' : '0') + " ";
            label_Data2.Text += " " + (Alub[2] ? '1' : '0') + " ";
            label_Data2.Text += " " + (Alub[1] ? '1' : '0') + " ";
            label_Data2.Text += " " + (Alub[0] ? '1' : '0') + " ";
            
            bool[] targetb = Tools.int2BoolArray(Tools.getTarget(I));
            label_Data3.Text = " " + (targetb[2] ? '1' : '0') + " ";
            label_Data3.Text += " " + (targetb[1] ? '1' : '0') + " ";
            label_Data3.Text += " " + (targetb[0] ? '1' : '0') + " ";
            
            bool[] JMPb = Tools.int2BoolArray(Tools.getJmp(I));
            label_Data4.Text = " " + (JMPb[2] ? '1' : '0') + " ";
            label_Data4.Text += " " + (JMPb[1] ? '1' : '0') + " ";
            label_Data4.Text += " " + (JMPb[0] ? '1' : '0') + " ";

            label_Data5.Text =  I.ToString("D");



            string txt = I.ToString("X");
            if (txt.Length > 4)
                label_Data6.Text =  I.ToString("X").Substring(4);
            else
                label_Data6.Text = txt.PadLeft(4, '0');


            string Inst = "";
            if (Tools.get15(I) == true)
                Inst = Tools.GetInstruction(I);
            else
                Inst = "A=" + I;
            label_Data7.Text = Inst;

        }
        void UpdateIGenerated()
        {
            int I= Tools.boolArray2Int(IGenerated);
            textBox_IGeneratedD.Text = I.ToString();
            string txt = I.ToString("X");
            if (txt.Length > 4)
                label_IGeneratedX.Text =  I.ToString("X").Substring(4);
            else
                label_IGeneratedX.Text =  txt.PadLeft(4, '0');
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


        private void textBox_IGeneratedD_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
        private void richTextBoxCode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SyntaxColorUpdate();
                //e.Handled = true;
                //e.SuppressKeyPress = true;
                NeedGenerate();
            }
            if ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.PageUp) || (e.KeyCode == Keys.Down) || (e.KeyCode == Keys.PageDown))
                SyntaxColorUpdate();
            if ((e.KeyCode != Keys.Up) && (e.KeyCode != Keys.PageUp) && (e.KeyCode != Keys.Down) && (e.KeyCode != Keys.PageDown) && (e.KeyCode != Keys.Right) && (e.KeyCode != Keys.Left) && (e.KeyCode != Keys.Home) && (e.KeyCode != Keys.End))
                NeedGenerate();
        }

     
      

        void UpdateStack()
        {
            int ram0 = myComputer.GetRam(0);
            label_SP.Text = "SP=" + ram0;
            dt_Stack.Clear();
            for (int i = ram0-1; i >= 256; i--)
            {
                dt_Stack.Rows.Add(i, myComputer.GetRam(i));
            }
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
                    if (_PC<=PC[i]  )
                    {
                        string s = "";
                        if (_PC== PC[i])
                             s = "\n" + PC[i] + "\n";
                        else
                            s = "\n" + PC[lastPCneq] + "\n";
                        //string s = "\n" + PC[lastPCneq] + "\n";
                        //string s = "\n" + PC[i] + "\n";
                        if (lastPCneq == 0)
                            s = PC[lastPCneq] + "\n";
                        pos = richTextBoxCode_PC.Text.IndexOf(s);
                        if (pos >= 0)
                        {
                            if (lastPCneq == 0)
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
                        if (start >= 0)
                        {
                            richTextBoxCode.Select(start, 0);
                            richTextBoxCode.ScrollToCaret();
                        }
                    }
                }
            }
            return richTextBoxCode_PC.GetLineFromCharIndex(pos + 1); ;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, ref Point lp);
        void SynchronizerichTextBox()
        {
            //https://www.codeproject.com/articles/Synchronized-Scrolling-of-Multiple-RichTextBoxes
            int WM_USER = 0x400;
            int EM_GETSCROLLPOS = WM_USER + 221;
            int EM_SETSCROLLPOS = WM_USER + 222;
            Point pt = new Point(0, 0);
            SendMessage(richTextBoxCode.Handle, EM_GETSCROLLPOS, new IntPtr(0), ref pt);
            SendMessage(richTextBoxCode_PC.Handle, EM_SETSCROLLPOS, new IntPtr(0), ref pt);
        }

        private void richTextBoxCode_VScroll(object sender, EventArgs e)
        {
            if (ScrollPC == true)
            {
                SynchronizerichTextBox();
            }
        }
        
      
      
        private void Save()
        {
            string createText = "##CONSTANTS##\n";
            for (int i = 0; i < dataGridView_Constant.Rows.Count; i++)
            {
                object Value = dataGridView_Constant.Rows[i].Cells[0].Value;
                if (Value != null && Value != DBNull.Value && !String.IsNullOrWhiteSpace(Value.ToString()))
                {
                    string cst = (string)Value;
                    if ((cst != null) && (cst != ""))
                    {
                        createText += cst;
                        createText += "=";
                        createText += (string)dataGridView_Constant.Rows[i].Cells[1].Value;
                        createText += "\n";
                    }
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

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            open();
        }
        void open()
        { 
            var dlg = new OpenFileDialog();
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            dlg.InitialDirectory = AppPath;
            dlg.RestoreDirectory = false;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            currentFileName = dlg.FileNames[0];
            toolStripButtonSave.Enabled = true;
           
            this.Text = "Controller - "+Path.GetFileNameWithoutExtension(currentFileName);

            string readText=readFile(currentFileName);
            int CodePos = readText.IndexOf("##CODE##");
            if (CodePos != -1)
            {
                richTextBoxCode.Text = readText.Substring(CodePos + 9);
                SyntaxColorUpdate();
                richTextBoxCode_PC.Text = "";
            }
        }

        string readFile(string filename)
        {
            string readText = File.ReadAllText(filename).Replace("\r", "");
            int CstPos = readText.IndexOf("##CONSTANTS##");
            int CodePos = readText.IndexOf("##CODE##");
            if (CstPos != -1)
            {
               
                string Csts = readText.Substring(CstPos + 14, CodePos - CstPos - 14);
                string[] CstsList = Csts.Split('\n');
                for (int i = 0; i<CstsList.Length; i++)
                {
                    if (CstsList[i] != "")
                    {
                        string[] cst = CstsList[i].Split('=');
                        int foundAtPos = -1;
                        for (int j = 0; j < dt_Constants.Rows.Count; j++)
                        {
                            if ((string)dt_Constants.Rows[j][0] == cst[0])
                                foundAtPos = j;
                        }
                        if (foundAtPos == -1)
                        {
                            dt_Constants.Rows.Add(cst);
                        }
                        else 
                        {
                            string val =(string) dt_Constants.Rows[foundAtPos][1];
                            if (val != cst[1])
                            {
                                dt_Constants.Rows[foundAtPos][1] = "?";
                            }
                        }
                    }
                }
            }
            return readText;
        }
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {
            SaveAs();
        }
        void SaveAs()
        { 
        var dlg = new SaveFileDialog();
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            dlg.InitialDirectory = AppPath;
            dlg.RestoreDirectory = false;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            currentFileName = dlg.FileNames[0];
            toolStripButtonSave.Enabled = true;
            this.Text = "Controller - " + Path.GetFileNameWithoutExtension(currentFileName);
            Save();
        }

        private void toolStripButtonGenerate_Click(object sender, EventArgs e)
        {
            Generate();
        }

        private void toolStripButtonTick_Click(object sender, EventArgs e)
        {
            Tick();
        }

        private void toolStripButtonReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        void Reset()
        { 
            toolStripButtonTick.Enabled = true;
            toolStripButtonRun.Enabled = true;
            dataGridView_Rom.ClearSelection();
            dataGridView_Rom.Rows[0].Selected = true;
            richTextBoxCode_PC.SelectAll();
            richTextBoxCode_PC.SelectionBackColor = Color.Black;
            richTextBoxCode_PC.SelectionColor = Color.White;
            myComputer.reset();
            Current_PC = 0;
            KeyPressed.Clear();
        }

        private async void toolStripButtonRun_Click(object sender, EventArgs e)
        {
            await run(Last_PC);
        }
        async Task run(int toPC)
        {
            StopRunningLoop = false;
            toolStripButtonRun.Enabled = false;
            toolStripButtonTick.Enabled = false;
            toolStripButtonReset.Enabled = false;
            bool go = false;
            int cpt = 0;
            do
            {
                cpt++;
                if(speed==0)
                    await Task.Delay(50);
                if (speed != 0)
                    if (cpt > 2)
                    {
                        cpt = 0;
                        await Task.Delay(1);
                    }
                if (StopRunningLoop == true)
                    break;
                go = Tick();
                if (Current_PC >= toPC)
                    break;
            } while (go);
            if (Current_PC == Last_PC)
                toolStripButtonReset.Enabled = true;
            else 
            {
                toolStripButtonRun.Enabled = true;
                toolStripButtonTick.Enabled = true;
                toolStripButtonReset.Enabled = true;
            }
        }

        private void toolStripButtonStepOver_Click(object sender, EventArgs e)
        {
            stepOver();
        }

        private void toolStripButtonStepOut_Click(object sender, EventArgs e)
        {
            stepOver();
        }


        void stepIn()
        {
            if (Last_PC == 0)
                return;
            if (Current_PC >= Last_PC - 1)
                return;
            Tick();
        }
      
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.O))
            {
                open();
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                Save();
                return true;
            }
            if (keyData == (Keys.Control | Keys.B))
            {
                Generate();
                return true;
            }
            if (keyData == (Keys.F5))
            {
                run(Last_PC);
                return true;
            }
            if (keyData == (Keys.F10))
            {
                stepOver();
                return true;
            }
            if (keyData == (Keys.F11))
            {
                stepIn();
                return true;
            }
            if (keyData == (Keys.Shift | Keys.F11))
            {
                stepOver();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.S))
            {
                SaveAs();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

     
        void stepOver()
        {
            int next = -1;
            for (int i = 0; i < PC.Count; i++)
            {
                if  (PC[i]> Current_PC)
                {
                    next = PC[i];
                    break;
                }
            }
            if (next >= 0)
                run(next);
            else
                run(Last_PC);

        }

        private void richTextBoxCode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SyntaxColorUpdate();
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            StopRunningLoop = true;
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            toolStripButtonSave.Enabled = false;
            currentFileName = "";
            richTextBoxCode.Clear();
            richTextBoxCode_PC.Clear();
        }

       

        public static class KeyEventUtility
        {
            // ReSharper disable InconsistentNaming
            public enum MapType : uint
            {
                MAPVK_VK_TO_VSC = 0x0,
                MAPVK_VSC_TO_VK = 0x1,
                MAPVK_VK_TO_CHAR = 0x2,
                MAPVK_VSC_TO_VK_EX = 0x3,
            }
            // ReSharper restore InconsistentNaming

            [DllImport("user32.dll")]
            public static extern int ToUnicode(
                uint wVirtKey,
                uint wScanCode,
                byte[] lpKeyState,
                [Out, MarshalAs( UnmanagedType.LPWStr, SizeParamIndex = 4 )]
                StringBuilder pwszBuff,
                int cchBuff,
                uint wFlags);

            [DllImport("user32.dll")]
            public static extern bool GetKeyboardState(byte[] lpKeyState);

            [DllImport("user32.dll")]
            public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

            public static char GetCharFromKey(Key key)
            {
                char ch = ' ';

                int virtualKey = KeyInterop.VirtualKeyFromKey(key);
                var keyboardState = new byte[256];
                GetKeyboardState(keyboardState);

                uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
                var stringBuilder = new StringBuilder(2);

                int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
                switch (result)
                {
                    case -1:
                        break;
                    case 0:
                        break;
                    case 1:
                        {
                            ch = stringBuilder[0];
                            break;
                        }
                    default:
                        {
                            ch = stringBuilder[0];
                            break;
                        }
                }
                return ch;
            }
        }

     
        private void richTextBoxCode_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                SyntaxColorUpdate();//apply color after paste that can come with colors
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            speed = toolStripComboBox1.SelectedIndex;
        }
        void MainWindow_Closed(object sender, EventArgs e)
        {
            StopRunningLoop = true;
        }

        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Key wpfKey = KeyInterop.KeyFromVirtualKey((int)e.KeyCode);
            int key = KeyEventUtility.GetCharFromKey(wpfKey);
            KeyPressed.Add(key);

            //labelKeyboard.Text = "Keyboard key: " + key + " => " + (char)key;
            string txt = "";
            for (int i = 0; i < KeyPressed.Count; i++)
            {
                if (KeyPressed[i] == 13)
                    txt += "#13";
                else if (KeyPressed[i] == 8)
                    txt += "#8";
                else
                    txt += (char)KeyPressed[i];
            }
            labelKeyboard.Text = "Keyboard key: " + txt;
            textBox_Keyboard.Text = "";
        }
        //keyboard clear
        private void button1_Click(object sender, EventArgs e)
        {
            KeyPressed.Clear();
            labelKeyboard.Text = "Keyboard key: ";
            textBox_Keyboard.Text = "";
        }
        void NeedGenerate()
        {
            toolStripButtonTick.Enabled = false;
            toolStripButtonRun.Enabled = false;
            toolStripButtonReset.Enabled = false;
            toolStripButtonStop.Enabled = false;
        }
    
        
    }
}
