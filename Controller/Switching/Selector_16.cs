using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Switching
{
    public class Selector_16
    {
        public bool[] D0 = new bool[16];
        public bool[] D1 = new bool[16];
        public bool[] output = new bool[16];
        public bool s;
        Selector[] selector = new Selector[16];
        public Selector_16()
        {
            for (int i = 0; i < 16; i++)
                selector[i] = new Selector();
        }
        public int proceed()
        {
            int val = 0;
            for (int i = 0; i < 16; i++)
            {
                selector[i].d0 = D0[i];
                selector[i].d1 = D1[i];
                selector[i].s = s;
                val += selector[i].proceed();
                output[i] = selector[i].outout;
            }
            return val;
        }
    }
}
