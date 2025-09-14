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

        public int proceed()
        {
            int val = 0;
            for (int i = 0; i < 16; i++)
            {
                Xor xor = new Xor();
                xor.a = a[i];
                xor.b = b[i];
                val += xor.proceed();
                s[i] = xor.output;
            }
            return val;
        }
    }
}
