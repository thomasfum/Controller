using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Switching
{
    public class Selector_16
    {
        public bool[] a = new bool[16];
        public bool[] b = new bool[16];
        public bool[] output = new bool[16];
        public bool s;
        public int proceed()
        {
            int val = 0;
            for (int i = 0; i < 16; i++)
            {
                Selector selector = new Selector();
                selector.d0 = a[i];
                selector.d1 = b[i];
                selector.s = s;
                val += selector.proceed();
                output[i] = selector.c;
            }
            return val;
        }
    }
}
