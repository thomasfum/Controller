using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.LogicGates
{
    public class Xor_16
    {
        public bool[] a = new bool[16];
        public bool[] b = new bool[16];
        public bool[] s = new bool[16];
        Xor[] xor = new Xor[16];
        public Xor_16()
        {
            for (int i = 0; i < 16; i++)
                xor[i] = new Xor();
        }
        public int proceed()
        {
            int val = 0;
            for (int i = 0; i < 16; i++)
            {
                xor[i].a = a[i];
                xor[i].b = b[i];
                val += xor[i].proceed();
                s[i] = xor[i].output;
            }
            return val;
        }
    }
}
