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
        public Form1()
        {
            InitializeComponent();
        }

        private void hyperlinkLabelControl1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://nandgame.com");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Nand nand1 = new Nand();
            nand1.a = false;
            nand1.b = false;
            nand1.proceed();
        }
    }
}
