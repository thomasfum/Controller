using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Arithmetics
{
    public class Inverter_16
    {
        public bool[] a = new bool[16];
        public bool[] s = new bool[16];
        Invert[] inv = new Invert[16];
        public Inverter_16()
        {
            for (int i = 0; i < 16; i++)
                inv[i] = new Invert();
        }
        public int proceed()
        {
            int val = 0;
            for (int i = 0; i < 16; i++)
            {
                inv[i].input = a[i];
                val+=inv[i].proceed();
                s[i] = inv[i].output;
            }
            return val;
        }
    }
}
