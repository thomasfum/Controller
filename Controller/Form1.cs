using Controller.Processor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controller
{
    public partial class Form1 : Form
    {
        Computer myComputer;
        public Form1()
        {
            InitializeComponent();
            myComputer = new Computer(256,256);
            myComputer.setRom(0, 32912);
            myComputer.setRom(1, 2);
            myComputer.setRom(2, 34064);
            myComputer.setRom(3, 32775);
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            myComputer.proceed();
            label1.Text = "A="+ myComputer.getA()+" D="+ myComputer.getD() + " PC=" + myComputer.getPC();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://nandgame.com");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
