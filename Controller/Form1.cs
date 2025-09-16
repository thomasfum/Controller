using Controller.Processor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Controller
{
    public partial class Form1 : Form
    {
        Computer myComputer;
        DataTable dt_Rom = new DataTable();
        DataTable dt_Ram = new DataTable();
        public bool[] IGenerated = new bool[16];
        public Form1()
        {
            InitializeComponent();
            myComputer = new Computer(256,256);
            myComputer.setRom(0, 32912);
            myComputer.setRom(1, 2);
            myComputer.setRom(2, 34064);
            myComputer.setRom(3, 32775);


            dt_Ram.Columns.Add("No");
            dt_Ram.Columns.Add("Data");
            // and then add your rows
            for (int i = 0; i < 10; i++)
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


            // and then add your rows
            for (int i = 0; i < 10; i++)
            {
                var row = dt_Rom.NewRow();
                // Set values for columns with row[i] = xy
                int I=myComputer.GetRom(i);
                row[0] = i;
                row[1] = I;
                string Inst = "";
                if (Tools.get15(I) == true)
                    Inst = Tools.DstString[Tools.getTarget(I)] + Tools.CalString[Tools.getALU(I)] + Tools.JmpString[Tools.getJmp(I)];
                else
                    Inst = "A=" + I;
                row[2] = Inst;
                dt_Rom.Rows.Add(row);
            }

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
            dataGridView_Rom.Rows[0].Selected = true;
            //dataGridView1.SelectionMode= DataGridViewSelectionMode.FullRowSelect;
            label_Instruction_b.UseMnemonic = false;
            label_IGeneratedAsm.UseMnemonic = false;
        }
        void refreshRam()
        {
            for (int i = 0; i < 10; i++)
            {
                // Set values for columns with row[i] = xy
                int I = myComputer.GetRam(i);
                dataGridView_Ram.Rows[i].Cells[1].Value=I.ToString();
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            dataGridView_Rom.Rows[0].Selected = true;
            myComputer.reset();
        }
        private void button1_Click(object sender, EventArgs e)
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
            dataGridView_Rom.ClearSelection();
            dataGridView_Rom.Rows[PC].Selected = true;
            updateI(I);
            refreshRam();
        }
        void updateI(int I)
        { 
            // top
            label_Instruction_t1.Text = "|   " + " . " + " . " + "   " + " . ";
            label_Instruction_b.Text = "|";
            label_Instruction_t2.Text = "|ci ";
            if (Tools.get15(I) == true)
                label_Instruction_b.Text += " 1 ";
            else
                label_Instruction_b.Text += " 0 ";
            label_Instruction_t2.Text += " . ";
            label_Instruction_t2.Text += " . ";
            label_Instruction_b.Text += " 0  0 ";
            label_Instruction_t2.Text += " * ";
            if (Tools.get12(I) == true)
                label_Instruction_b.Text += " 1 ";
            else
                label_Instruction_b.Text += " 0 ";
            label_Instruction_t2.Text += " . ";
            label_Instruction_b.Text += " 0 ";
            label_Instruction_t1.Text += " | ";
            label_Instruction_t2.Text += " | ";
            label_Instruction_b.Text += " | ";
            //alu
            label_Instruction_t1.Text += "   operation   ";
            bool[] Alub = Tools.int2BoolArray(Tools.getALU(I));
            label_Instruction_t2.Text += " u ";
            label_Instruction_b.Text += " " + (Alub[4] ? '1' : '0') + " ";
            label_Instruction_t2.Text += "op1";
            label_Instruction_b.Text += " " + (Alub[3] ? '1' : '0') + " ";
            label_Instruction_t2.Text += "op2";
            label_Instruction_b.Text += " " + (Alub[2] ? '1' : '0') + " ";
            label_Instruction_t2.Text += "zx ";
            label_Instruction_b.Text += " " + (Alub[1] ? '1' : '0') + " ";
            label_Instruction_t2.Text += "sw ";
            label_Instruction_b.Text += " " + (Alub[0] ? '1' : '0') + " ";

            label_Instruction_t1.Text += " | ";
            label_Instruction_t2.Text += " | ";
            label_Instruction_b.Text += " | ";

            label_Instruction_t1.Text += " target  ";
            bool[] targetb = Tools.int2BoolArray(Tools.getTarget(I));
            label_Instruction_t2.Text += " a ";
            label_Instruction_b.Text += " " + (targetb[2] ? '1' : '0') + " ";
            label_Instruction_t2.Text += " d ";
            label_Instruction_b.Text += " " + (targetb[1] ? '1' : '0') + " ";
            label_Instruction_t2.Text += "a* ";
            label_Instruction_b.Text += " " + (targetb[0] ? '1' : '0') + " ";

            label_Instruction_t1.Text += " | ";
            label_Instruction_t2.Text += " | ";
            label_Instruction_b.Text += " | ";
            label_Instruction_t1.Text += "  jump   ";
            bool[] JMPb = Tools.int2BoolArray(Tools.getJmp(I));
            label_Instruction_t2.Text += "lt ";
            label_Instruction_b.Text += " " + (JMPb[2] ? '1' : '0') + " ";
            label_Instruction_t2.Text += "eq ";
            label_Instruction_b.Text += " " + (JMPb[1] ? '1' : '0') + " ";
            label_Instruction_t2.Text += "gt ";
            label_Instruction_b.Text += " " + (JMPb[0] ? '1' : '0') + " ";

            

            label_Instruction_t1.Text += " | decimal |";
            label_Instruction_t2.Text += " |         |";
            label_Instruction_b.Text += " | "+I.ToString("D").PadLeft(7)+ " |";

            label_Instruction_t1.Text += "  HEX |";
            label_Instruction_t2.Text += "      |";
            string txt = I.ToString("X");
            if(txt.Length >4 )
                label_Instruction_b.Text  += " "+I.ToString("X").Substring(4)+" |";
            else
                label_Instruction_b.Text += " " + txt.PadLeft(4,'0') + " |";

          
            string Inst = "";
            if (Tools.get15(I) == true)
                Inst = Tools.DstString[Tools.getTarget(I)] + Tools.CalString[Tools.getALU(I)] + Tools.JmpString[Tools.getJmp(I)];
            else
                Inst = "A=" + I;
            label_Instruction_t1.Text += "  Assembler  |";
            label_Instruction_t2.Text += "             |";
            label_Instruction_b.Text += Inst.PadRight(12) + " |";
            
            labelInstructionBottom.Text = "|                |                 |           |           |         |      |             |";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://nandgame.com");
        }

   
        //write ROM
        private void button2_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < dataGridView_Rom.RowCount; i++)
            {
                if((string)dataGridView_Rom.Rows[i].Cells[0].Value!="")
                { 
                    string s = (string)dataGridView_Rom.Rows[i].Cells[1].Value;
                    if(s!=null)
                        myComputer.setRom(i, int.Parse(s));
                }
            }
        }
        //clear ram
        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView_Ram.RowCount; i++)
            {
                myComputer.setRam(i, 0);
            }
            refreshRam();
        }
        //write RAM
        private void button3_Click(object sender, EventArgs e)
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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
           
            if (e.ColumnIndex == 1)
            {

                int row = e.RowIndex;
                string cell = (string)dgv?.Rows?[row]?.Cells?[e.ColumnIndex]?.Value;
                int I=int.Parse(cell);
                string Inst = "";
                if (Tools.get15(I) == true)
                    Inst = Tools.DstString[Tools.getTarget(I)] + Tools.CalString[Tools.getALU(I)] + Tools.JmpString[Tools.getJmp(I)];
                else
                    Inst = "A=" + I;
                dgv.Rows[row].Cells[2].Value = Inst;
                dgv.Rows[row].Cells[0].Value = row;
            }
            if (e.ColumnIndex == 2)
            {
                int row = e.RowIndex;
                dgv.Rows[row].Cells[0].Value = row;
            }
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
                Inst = Tools.DstString[Tools.getTarget(I)] + Tools.CalString[Tools.getALU(I)] + Tools.JmpString[Tools.getJmp(I)];
            else
                Inst = "A=" + I;
            label_IGeneratedAsm.Text= Inst;
        }

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

    }
}
