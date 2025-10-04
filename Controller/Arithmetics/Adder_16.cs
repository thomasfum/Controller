using Controller.Arithmetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    // adder 16 bits
    public class Adder_16
    {
        public bool[] a = new bool[16];
        public bool[] b = new bool[16];
        public bool[] s = new bool[16];
        public bool c;
        public bool cOut;
        FullAdder[] add = new FullAdder[16];
        public Adder_16()
        {
            for (int i = 0; i < 16; i++)
                add[i] = new FullAdder();
        }
        public int proceed()
        {
            int val = 0;
            bool lastC = c;
            for (int i = 0; i < 16; i++) 
            {
                add[i].a = a[i];
                add[i].b = b[i];
                add[i].c = lastC;
                val+=add[i].proceed();
                s[i] = add[i].l;
                lastC = add[i].h;
            }
            cOut = lastC;
            return val;
        }
    }
}
